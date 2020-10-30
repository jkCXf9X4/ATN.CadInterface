using System;
using System.Collections.Generic;
using System.Diagnostics;

using Dassault.Catia.R24.INFITF;
using Dassault.Catia.R24.ProductStructureTypeLib;
using Dassault.Catia.R24.MECMOD;
using Dassault.Catia.R24.PARTITF;
using Dassault.Catia.R24.HybridShapeTypeLib;
using Dassault.Catia.R24.KnowledgewareTypeLib;
using Dassault.Catia.R24.SPATypeLib;

using ATN.Utils.NativeExt.DictExt;

namespace ATN.Catia.R24.Ext
{
    /// <summary>
    /// Description of Workbench.
    /// </summary>
    public static class Workbench
    {
        public enum Workbenches
        {
            Part,
            Assembly,
            SheetMetal,
            Step,
            _3DCS
        }

        static readonly Dictionary<string, Workbenches> dict = new Dictionary<string, Workbenches>
         {
            { "SXT", Workbenches.Step },
            { "PrtCfg", Workbenches.Part },  // "Part Design"
			{ "Assembly", Workbenches.Assembly},
            { "", Workbenches.SheetMetal},
            { "DVTDCSWorkbench", Workbenches._3DCS }
         };


        public static string GetWorkbenchId()
        {
            var Catia = CatiaApp.Instance;
            return Catia.GetWorkbenchId();
        }

        public static bool SwitchWorkbench(Workbenches i)
        {
            var Catia = CatiaApp.Instance;
            var wbID = GetWorkbenchId();
            if (Workbench.dict[wbID] != i)
            {
                Catia.StartWorkbench(dict.KeyByValue(i));
                System.Threading.Thread.Sleep(1000);
            }
            return true;
        }

    }

}
