using System;
using System.Collections.Generic;
using System.Linq;

using Dassault.Catia.R24.INFITF;
using Dassault.Catia.R24.MECMOD;



namespace ATN.Catia.R24.Ext
{	
	public static class DocumentExt
	{
		public static string GetDocumentName(this Document i)
		{
			string[] name = i.FullName.Split('.');
			string n1 = name[name.Count() - 2];
			n1 = n1.Split('\\').Last();
			return n1;
		}

		public static string GetDocumentExtension(this Document i)
		{
			string[] split = i.FullName.Split('.', '\\');
			string extension = split.Last();
			return extension;
		}
		
		public static List<String> GetDocumentInfo(Document i)
		{
			return new List<string>(new String[] {"Enter DocumentInfo:getDocumentType",
				"fullname: " + i.FullName,
				"Path: " + i.Path,
				"Type: " + Feature.GetType2(i),
				"Extension: " + GetDocumentExtension(i)
			});
		}

		public static string GetPartNumber(this Document document)
		{
			Feature.FeatureType type = document.GetType2();
			
			switch (type) {
				case Feature.FeatureType.Part:
					Part partBase = document.GetPartBase();
					return partBase.GetName();

				case Feature.FeatureType.Product:
					return document.GetProduct().GetPartNumber();

			}
			return document.GetName();
		}

		public static string GetDescription(Document document)
		{
			Feature.FeatureType type = document.GetType2();
			string decsription;
			
			switch (type) {
				case Feature.FeatureType.Part:
					Part partBase = document.GetPartBase();
					var prop = partBase.Parameters;
					decsription = prop.Item(@"Definition").ValueAsString();
					return decsription;
				case Feature.FeatureType.Product:
					return document.GetProduct().GetDefinition();

			}
			return null;
		}
		
		public static bool SetVisibilityOn(this Document doc)
		{
			var sel = SelectionExt.SelectInvisible();
			sel.VisProperties.SetShow(CatVisPropertyShow.catVisPropertyShowAttr);
			sel.Clear();
			return true;
		}
		
		public static Document GetActiveDocument()
		{
			var doc = CatiaApp.Instance.ActiveDocument;
			return doc;
		}

		public static string GetActiveDocumentName()
		{
			var doc = CatiaApp.Instance.ActiveDocument;
			var sel = doc.Selection;
			sel.Clear();
			sel.Search("Name=*,in");
			var tmpobj = sel.Item(1);
			sel.Clear();
			
			return tmpobj.Reference.GetName();
		}

		public static Document GetRootDocument()
		{
			ActivateRootDocument();
			return GetActiveDocument();
		}

		public static void ActivateRootDocument()
		{
			var active_wndow = CatiaApp.Instance.ActiveWindow;
			var parent = active_wndow.Parent;
			active_wndow.Activate();
		}


		public static bool IsActiveDocument(string fileName)
		{
			string name = GetActiveDocumentName();
			if (name.Substring(0, 8) == fileName) {
				return true;
			}
			return false;
		}
		
		public static void GetLinks(Document document)
		{
			// TODO:get links
			//		    Set CATIA = GetObject(, "CATIA.Application")
			//    Dim oStiEngine ' As StiEngine
			//    Set oStiEngine = CATIA.GetItem("CAIEngine")
			//    Dim oStiDBItem ' As StiDBItem
			//    Set oStiDBItem = oStiEngine.GetStiDBItemFromAnyObject(CATIA.ActiveDocument)
		}
	}
}