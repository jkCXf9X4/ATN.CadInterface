using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

using Dassault.Catia.R24.INFITF;
using Dassault.Catia.R24.ProductStructureTypeLib;
using Dassault.Catia.R24.MECMOD;
using Dassault.Catia.R24.PARTITF;
using Dassault.Catia.R24.HybridShapeTypeLib;
using Dassault.Catia.R24.KnowledgewareTypeLib;
using Dassault.Catia.R24.SPATypeLib;





namespace ATN.Catia.R24.Ext
{
	/// <summary>
	/// Window managment in catia
	/// </summary>
	public static class Window 
	{	
		public static void SetActive(object i) {
			throw new NotImplementedException();
			
			//new CatiaCom().Instance.Windows.Item(i).Activate();
		}

		public static void SetActive(Document i) {
			throw new NotImplementedException();
			
			//new CatiaCom().Instance.Windows.Item(i.FullName).Activate();
		}
		
		// VB for finding the right window		
// Function FindParent(Element, ParentTyp)
// set Puffer = Element
// fertig = false
// do
//    if TypeName(Puffer) = ParentTyp then 
//       set FindParent = Puffer
//       fertig = true
//    elseif TypeName(Puffer) = "CNEXT" then
//       fertig = true
//    end if
// set Puffer = Puffer.Parent
// loop until fertig = true
//End Function
		
	}

}
