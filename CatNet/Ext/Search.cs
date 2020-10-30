/*
 * Created by SharpDevelop.
 * User: erxzr5
 * Date: 2017-03-31
 * Time: 09:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
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


namespace ATN.Catia.R24.Ext
{
	public static class Search
	{
				
		public static Selection SearchDoc(Document doc, string query, bool visible = false)
		{
			if (!visible)
				CatiaApp.Instance.HSOSynchronized = false;
			doc.Selection.Search(query);
			if (!visible)
				CatiaApp.Instance.HSOSynchronized = true;
			return doc.Selection;
		}
		
		public static Selection NewSearchActiveDoc(string query)
		{
			return SearchDoc(CatiaApp.Instance.ActiveDocument, query);
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