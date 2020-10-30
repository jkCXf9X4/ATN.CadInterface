using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

using ATN.Utils;
using ATN.Utils.NativeExt.StringExt;

using Dassault.Catia.R24.INFITF;
using Dassault.Catia.R24.ProductStructureTypeLib;
using Dassault.Catia.R24.MECMOD;
using Dassault.Catia.R24.PARTITF;
using Dassault.Catia.R24.HybridShapeTypeLib;
using Dassault.Catia.R24.KnowledgewareTypeLib;
using Dassault.Catia.R24.SPATypeLib;

namespace ATN.Catia.R24.Ext
{
	public static class SelectionExt
	{
		
		//		Everywhere: shortcut “all”
		//		InWorkbench: shortcut “in”
		//		FromWorkbench: shortcut “from”
		//		FromSelection: shortcut “sel”
		//		VisibleOnScreen: shortcut “scr”
		

		
		public static Selection SelectAll()
		{
			return Search.NewSearchActiveDoc(@"'Product Structure'.Assembly+'Product Structure'.Part+'Product Structure'.Product,all");
		}
		
		public static Selection SelectUnderActive()
		{
			var sel = Search.NewSearchActiveDoc(@"'Product Structure'.Assembly+'Product Structure'.Part+'Product Structure'.Product,in");
			sel.Remove2(1); // remove top node
			return sel;
		}
		
		public static Selection SelectUnderSelection()
		{
			var sel = Search.NewSearchActiveDoc(@"'Product Structure'.Assembly+'Product Structure'.Part+'Product Structure'.Product,sel");
			if (sel.Count2 > 0)
			{
				sel.Remove2(1); // remove top node
				return sel;
			}
			return null;
		}
		
		public static Selection SelectPart(string name)
		{
			var sel = SelectParts(name);
			
			if (sel.Count2 > 1)
				throw new Exception("More than one part was found");
			
			return sel;
		}
		
		public static Selection SelectParts(string name)
		{
			return Search.NewSearchActiveDoc(@"'Product Structure'.Assembly.'Part Number'=" + name + "+'Product Structure'.Part.'Part Number'=" + name + ",all");
		}
		
		public static Selection SelectFromGraph(string text)
		{
			return Search.NewSearchActiveDoc(@"'Product Structure'.Part.'Name In Graph'=" + text + "+'Product Structure'.Assembly.'Name In Graph'=" + text + ",all");
		}
		
		public static Selection SelectInstances(string text)
		{
			return Search.NewSearchActiveDoc(@"'Product Structure'.Product.Name=" + text + ",all");
		}
		
		public static Selection SelectInvisible()
		{
			 return Search.NewSearchActiveDoc(@"'Assembly Design'.Product.Visibilty=Invisible,all");
		}
		
		
		public static void SelectPoints(this Selection sel)
		{
			sel.Search("CATPrtSearch.Point,sel");       
		}
		
		
		public static List<SelectedElement> GetElements(this Selection sel)
		{
			var list = new List<SelectedElement>();
			for (int i = 1; i < sel.Count2 + 1; i++) {
				list.Add(sel.Item2(i));
			}
			return list;
		}
		
		public static List<AnyObject> GetValues(this Selection sel)
		{
			return sel.GetElements().Select(x => x.GetValue()).ToList();
		}
		
		public static List<Product> GetProducts(this Selection sel)
		{
			return sel.GetValues().Select(x => (Product)x).ToList();
		}
		
		public static List<Document> GetDocuments(this Selection sel)
		{
			return sel.GetProducts().Select(x => (Document)x.ReferenceProduct.Parent).ToList();
		}
		
		public static List<AnyObject> GetLeafProducts(this Selection sel)
		{
			var leaf = sel.GetElements().Where(x=> x.HasLeaf());
			return leaf.Select(x => x.LeafProduct).ToList();
		}

		public static List<string> GetPaths(this Selection sel)
		{
			return sel.GetDocuments().Select(x => x.GetName()).ToList();
		}
		
		public static List<string> GetInstanceNames(this Selection sel)
		{
			return sel.GetLeafProducts().Select(x => x.GetName()).ToList();
		}
		
		public static bool HasLeaf(this SelectedElement element)
		{
			if(element.LeafProduct.GetName() == "InvalidLeafProduct")
			{
				return false;
			}
			return true;
		}
		
		public static AnyObject GetValue(this SelectedElement element)
		{
			return  (AnyObject)element.Value;
		}
		
		public static Product GetReferenceProduct(this SelectedElement element)
		{
			return  ((Product)element.LeafProduct).ReferenceProduct;
		}
		
		public static Selection SelectPoints(this Selection sel, string name)
		{
			sel.Search("CATPrtSearch.Point,sel");
			int nrPoints = sel.Count2;
			int found = 0;
			
			for (int i = 1; i < nrPoints + 1; i++) {
				Debug.WriteLine("Index " + i.ToString());
				var Point = (Point)sel.Item2(i).Value;
				string PointName = Point.get_Name();
				
				Debug.WriteLine("Index " + Point.get_Name());
				if (PointName.CompareWithWildcard(name)) {
					Debug.WriteLine("Found match " + i.ToString());
					found += 1;
				} else {
					Debug.WriteLine("removing " + i.ToString());
					sel.Remove2(i);
					i--;
					nrPoints--;
				}
				
			}
			return sel;
		}
		
		public static void DebugSelection(this Selection sel)
		{
			Debug.WriteLine("Nr of objects: " + sel.Count2);
			
			
			sel.GetElements().ForEach(x => {
			    Debug.WriteLine("Value: " + ((AnyObject)x.Value).GetName());
				Debug.WriteLine( x.GetValue().GetType2());
				
				Debug.WriteLine("Leaf: " + x.LeafProduct.GetName());
			});
		}
		
		
		
	}

}
