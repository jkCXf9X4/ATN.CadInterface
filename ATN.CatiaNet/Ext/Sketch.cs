using System;
using System.Linq;
using System.Diagnostics;

using Dassault.Catia.R24.INFITF;
using Dassault.Catia.R24.ProductStructureTypeLib;
using Dassault.Catia.R24.MECMOD;
using Dassault.Catia.R24.PARTITF;
using Dassault.Catia.R24.HybridShapeTypeLib;
using Dassault.Catia.R24.KnowledgewareTypeLib;
using Dassault.Catia.R24.SPATypeLib;

namespace ATN.Catia.R24.Ext
{ 

    public static class SketchExt
	{
		
		//public static Sketch GetCurrentSketch() {
			//return partDocument.Part.InWorkObject as Sketch;
		//}
		
		public static void EditSketch(this Sketch S, Action<Factory2D> A) {
			if (S == null)
				throw new ArgumentException("Sketch is null", "S");
			Factory2D F = S.OpenEdition();
			if (F == null)
				throw new Exception("Could not open edition");
			try {
				A(F);
			} finally {
				S.CloseEdition();

			}
		}
	}
}
