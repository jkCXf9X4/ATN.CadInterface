using ATN.Catia.R24.Ext;
using ATN.WinApi;
using Dassault.Catia.R24.CATStk;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace ATN.Catia.R24._3DCS
{
    public static class List_GUI
    {
        enum menuType
        {
            Tolerance,
            Measurement,
            GDAT, // GD&T
            Move
        }


        public static void CheckPrerequsites()
        {
            CatiaApp.ToFront();

            if (!CatiaApp.IsForemost())
            {
                throw new Exception("Not all prerequsites are fullfilled to run this command");
            }

            //var bench = Workbench.GetWorkbenchId();
            //Debug.WriteLine("Current workbench: " + bench);

            Workbench.SwitchWorkbench(Workbench.Workbenches._3DCS);
        }

        private static void OpenMenu(string id)
        {
            var handle = CatiaApp.GetHandle();

            AutomationElement root = AutomationElement.FromHandle(handle);

            root.FindByIdExt(id).PressButtonExt();

            Thread.Sleep(200);
        }

        private static AutomationElement OpenAndGetDialog(string instanceName, string id, string captationSearchString)
        {
            SelectionExt.SelectInstances(instanceName);

            OpenMenu(id);

            var handle = ATN.WinApi.Window.FindWindowFromCaptation(captationSearchString, 5000);

            AutomationElement dialog = AutomationElement.FromHandle(handle);
            return dialog;
        }

        public static AutomationElement OpenAndGetMoveDialog(string instanceName)
        {
            List_GUI.CheckPrerequsites();

            var dialogSearchString = $"Moves In {instanceName.Replace('.', '_')}";

            var moveDialog = OpenAndGetDialog(instanceName, "572", dialogSearchString);

            return moveDialog;
        }

        public static AutomationElement OpenAndGetToleranceDialog(string instanceName)
        {
            List_GUI.CheckPrerequsites();

            var dialogSearchString = $"Tolerances In {instanceName.Replace('.', '_')}";

            var moveDialog = OpenAndGetDialog(instanceName, "580", dialogSearchString);

            return moveDialog;
        }

        public static AutomationElement OpenAndGetGdatDialog(string instanceName)
        {
            List_GUI.CheckPrerequsites();

            var dialogSearchString = $"GD&Ts In {instanceName.Replace('.', '_')}";

            var moveDialog = OpenAndGetDialog(instanceName, "588", dialogSearchString);

            return moveDialog;
        }

        public static AutomationElement OpenAndGetMeasureDialog(string instanceName)
        {
            List_GUI.CheckPrerequsites();

            var dialogSearchString = $"Measurements In {instanceName.Replace('.', '_')}";

            var moveDialog = OpenAndGetDialog(instanceName, "589", dialogSearchString);

            return moveDialog;
        }

        public static List<string> GetAppributeList(AutomationElement ae)
        {
            var datagrids = ae.FindByTypeExt(ControlType.DataGrid);

            GridPattern gridPattern = datagrids[0].GetCurrentPattern (GridPattern.Pattern) as GridPattern;

            int rowCount = gridPattern.Current.RowCount;

            List<string> names = new List<string>();
            foreach( int i in Enumerable.Range(0, rowCount))
            {
                var item = gridPattern.GetItem(i, 1);

                string name = item.Current.Name.Split(' ').First();

                names.Add(name);
            }

            return names;
        }

        public static AutomationElement OpenAppributeFromList(AutomationElement ae, string name)
        {
            var datagrid = ae.FindByTypeExt(ControlType.DataGrid)[0];


            GridPattern gridPattern = datagrid.GetCurrentPattern(GridPattern.Pattern) as GridPattern;

            int rowCount = gridPattern.Current.RowCount;

            List<string> names = new List<string>();
            foreach (int i in Enumerable.Range(0, rowCount))
            {
                var item = gridPattern.GetItem(i, 1);
                if (Regex.IsMatch(item.Current.Name, name))
                {
                    AutomationElement row = datagrid.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, (i+1).ToString()));

                    SelectionItemPattern itemPattern = row.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
                    itemPattern.Select();
                }
            }

            ae.FindByNameExt("Modify").PressButtonExt();

            var handle = WinApi.Window.FindWindowFromCaptationRegex($".*GD&T.*{name}.*");

            AutomationElement newDialog = AutomationElement.FromHandle(handle);

            return newDialog;
        }
    }
}
