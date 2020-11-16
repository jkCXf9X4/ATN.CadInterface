using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Automation;

using CATIA = Dassault.Catia.R24;

using Dassault.Catia.R24.INFITF;
using Dassault.Catia.R24.ProductStructureTypeLib;
using Dassault.Catia.R24.MECMOD;
using Dassault.Catia.R24.PARTITF;
using Dassault.Catia.R24.HybridShapeTypeLib;
using Dassault.Catia.R24.KnowledgewareTypeLib;
using Dassault.Catia.R24.SPATypeLib;

using ATN.Catia.R24.COM;


namespace ATN.Catia.R24.Ext
{
	public static class PartExt
	{
		public static Part GetPartBase(this Document i)
		{
			var partDocBase = i.AsPartDocument();
			return GetPartBase(partDocBase);
		}
		
		public static Part GetPartBase(this PartDocument i)
		{
			var partBase = i.get_Part();
			return partBase;
		}
		
		public static void HideAllGeometry(this Part part)
		{

			foreach (HybridBody body in part.HybridBodies) {
				
				var sel = CatiaApplication.Instance.ActiveDocument.Selection;
				
				sel.Clear();
				
				sel.Add(body);
				
				sel.VisProperties.SetShow(CatVisPropertyShow.catVisPropertyNoShowAttr);
				
				sel.Clear();
			}
			part.Update2();
		}
		
		public static bool Update2(this Part part)
		{
			
			try {
				part.Update();
			} catch {
				return false;
			}
			return true;
		}
	}
}
