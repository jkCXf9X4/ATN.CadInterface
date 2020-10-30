
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Dassault.Catia.R24.INFITF;

using ATN.Utils.WinAutomation;

namespace ATN.Catia.R24.Ext
{
    public class CatiaApp
    {
        private static Lazy<CatiaApp> lazy = null;

        public static Application Instance
        {
            get
            {
                if (lazy == null)
                {
                    lazy = new Lazy<CatiaApp>(() => new CatiaApp());
                }
                return lazy.Value.catia;
            }
        }

        private Application catia = null;

        private CatiaApp()
        {
            try
            {
                catia = (Application)Marshal.GetActiveObject("CATIA.Application");
                catia.Visible = true;
            }
            catch
            {
                try
                {
                    catia = (Application)Activator.CreateInstance(Type.GetTypeFromProgID("CATIA.Application"));
                    catia.Visible = true;
                }
                catch
                {
                    throw new CatiaException("No application found");
                }
            }
        }

        ~CatiaApp()
        {
            Marshal.ReleaseComObject(catia);
            catia = null;
        }

        public static IntPtr GetHandle()
        {
            Process[] p = Process.GetProcessesByName("CNEXT");
            return p.FirstOrDefault().MainWindowHandle;
        }

        public static void ToFront()
        {
            ATN.Utils.WinAutomation.Window.SetForegroundWindow(GetHandle());
            ATN.Utils.WinAutomation.Window.MaximizeWindow(GetHandle());
        }

        public static bool IsForemost()
        {
            if (ATN.Utils.WinAutomation.Window.GetForegroundWindow() == GetHandle())
            {
                return true;
            }
            return false;
        }

        public static void StartCommand(string msg)
        {
            var catia = CatiaApp.Instance;
            catia.StartCommand(msg);
            catia.RefreshDisplay = true;
            System.Threading.Thread.Sleep(100);
        }
    }
}
