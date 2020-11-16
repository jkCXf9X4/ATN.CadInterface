
using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

using ATN.Utils;

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
	public static class Search
	{
				
		public static Selection SearchDoc(Document doc, string query, bool visible = false)
		{
			if (!visible)
				CatiaApplication.Instance.HSOSynchronized = false;
			doc.Selection.Search(query);
			if (!visible)
				CatiaApplication.Instance.HSOSynchronized = true;
			return doc.Selection;
		}
		
		public static Selection NewSearchActiveDoc(string query)
		{
			return SearchDoc(CatiaApplication.Instance.ActiveDocument, query);
		}

		public static Document GetDocument(string instance)
		{
			var sel = SelectionExt.SelectInstances(instance);
			return sel.GetDocuments().FirstOrDefault();
		}
		public static Product GetProduct(string instance)
		{
			var sel = SelectionExt.SelectInstances(instance);
			return sel.GetProducts().FirstOrDefault();
		}
		
		public static Product GetLeafProduct(string instance)
		{
			var sel = SelectionExt.SelectInstances(instance);
			return (Product)sel.GetLeafProducts().FirstOrDefault();
		}
		
	}
}