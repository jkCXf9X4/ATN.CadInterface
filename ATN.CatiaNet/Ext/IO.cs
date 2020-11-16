using System;
using System.Diagnostics;
using System.IO;

using System.Windows.Automation;

using ATN.Utils.WinAutomation;

using Dassault.Catia.R24.INFITF;
using Dassault.Catia.R24.ProductStructureTypeLib;
using Dassault.Catia.R24.MECMOD;

using ATN.Catia.R24.COM;


namespace ATN.Catia.R24.Ext
{
	public static class Export
	{
		public static void To3dPdf(Document doc, string path)
		{
			throw new NotImplementedException();
			
			//if(doc.IsProductDocument())
			//{
			//	doc = (Document)Convert.CreatePartFromProduct(doc.AsProductDocument());
			//}
			
			//if(doc.IsPartDocument())
			//{
			//	STL(doc.AsPartDocument(), path + ".stl");

				
			//	//Debug.WriteLine("Output: " + pdf3d.output.FullName);
			//}
		}
		
		public static bool Step(Document doc, string path)
		{
			if(!License.IsStepLicenceAvailible()) {
				return false;
			}
			
			if (doc.IsPartDocument()){
				return Step(doc.AsPartDocument(), path);
			}
			else if (doc.IsProductDocument()){
				return Step(doc.AsProductDocument(),path);
			}
			return false;
		}
		
		public static bool Step(PartDocument part, string path)
		{
			part.ExportData(path, "stp");
			return true;
		}
		
		public static bool Step(ProductDocument prod, string path)
		{
			prod.ExportData(path, "stp");
			return true;
		}
		
		public static bool STL(Document doc, string path)
		{
			if (doc.IsPartDocument()){
				return STL(doc.AsPartDocument(), path);
			}
			else if (doc.IsProductDocument()){
				return STL(doc.AsProductDocument(),path);
			}
			return false;
		}
		
		public static bool STL(PartDocument part, string path)
		{
			part.ExportData(path, "stl");
			return true;
		}
		
		public static bool STL(ProductDocument prod, string path)
		{
			prod.ExportData(path, "stl");
			return true;
		}
	}
	
	public static class Convert
	{
		
		public static PartDocument CreatePartFromProduct(ProductDocument productDoc)
		{

			Application Catia = CatiaApplication.Instance;
			
			Selection mySelect = Catia.ActiveDocument.Selection;
			mySelect.Clear();
			mySelect.Add(productDoc.Product);
			
			CatiaApplication.ToFront();
			
			var bench = Workbench.GetWorkbenchId();
			Debug.WriteLine("Current workbench: " + bench);
			
			Workbench.SwitchWorkbench(Workbench.Workbenches.Assembly);
			
			System.Threading.Thread.Sleep(200);
			
			Catia.RefreshDisplay = true;
			Catia.StartCommand("c:Generate CATPart from Product...");
			Catia.RefreshDisplay = true;
			
			var dialog = ATN.Utils.WinAutomation.Window.FindWindowFromCaptation("Generate CATPart from Product", 5000);
			
			if (dialog == IntPtr.Zero) {
				throw new Exception("No window found");
			}
			
			AutomationElement dialogElement = AutomationElement.FromHandle(dialog);

			_ = dialogElement.FindByNameExt("OK").PressButtonExt();
  			
			System.Threading.Thread.Sleep(1000);
			
			ATN.Utils.WinAutomation.Window.WaitForWindow(dialog);
  				
			System.Threading.Thread.Sleep(1000);
  			
			return (PartDocument)Catia.ActiveDocument;
		}
	}
	
	public static class Import
	{
		
	}
}
