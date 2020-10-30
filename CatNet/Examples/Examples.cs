/*
 * Created by SharpDevelop.
 * User: erxzr5
 * Date: 2018-02-07
 * Time: 11:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;

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
	/// <summary>
	/// Description of Examples.
	/// </summary>
	public static class Examples
	{
		public static void Example_CopyPaste()
		{
			Tree.ApplyWorkMode();
			
			var aDoc = CatiaApp.Instance.ActiveDocument;
			var sel =  aDoc.Selection;
			
			var tree = new NodeTree(aDoc);
			
			var docNode = tree.list.Where(x => x.product.GetName().CompareWithWildcard("52714606.1")).FirstOrDefault();
			
			Debug.WriteLine(docNode.document.GetName());
			Debug.WriteLine(docNode.product.GetName());
			
			sel.Clear();
			
			sel.Add(docNode.product);
			
			sel.Copy();
			
			sel.Clear();
			
			docNode = tree.list.Where(x => x.product.GetName().CompareWithWildcard("52714607.1")).FirstOrDefault();
			
			sel.Add(docNode.product);
			
			sel.Paste();
		}

		public static void PrintTreeNames()
        {
			Application catia = CatiaApp.Instance;

			var doc = catia.ActiveDocument;
			var productDocument = doc.AsProductDocument();
			var product = productDocument.Product.AsProduct();

			Console.WriteLine(doc.GetName());

			Console.WriteLine(productDocument.GetName());

			Console.WriteLine(product.GetName());

			Console.WriteLine(Feature.GetType2(doc));

			var tree = new NodeTree(productDocument);

			//var names = tree.list.Select(x => x);

			var names = tree.list.Select(x => x.GetProductName());

			Console.WriteLine(names.Join2(" "));

			var sel = SelectionExt.SelectAll();

			var names_sel = sel.GetInstanceNames();

			Console.WriteLine(names_sel.Join2(" "));


			tree.list[0].SelectNode();

			System.Threading.Thread.Sleep(1000);

			tree.list[2].SelectNode();

			System.Threading.Thread.Sleep(1000);
		}
		
		public static void Example_Transform()
		{
			var aDoc = CatiaApp.Instance.ActiveDocument;
			var sel =  aDoc.Selection;
			
			var tree = new NodeTree(aDoc);
			
			var docNode = tree.list.Where(x => x.product.GetName().CompareWithWildcard("52726609.1")).FirstOrDefault();
			
			for(int i= 0;i< 360;i+=5)
			{
				Ext.Transform.PositionLeaf(docNode.product, new Transformation(0,0,0,i,0,0));
			}
			for(int i= 0;i< 360;i+=5)
			{
				Ext.Transform.PositionLeaf(docNode.product, new Transformation(0,0,0,0,i,0));
			}
			for(int i= 0;i< 360;i+=5)
			{
				Ext.Transform.PositionLeaf(docNode.product, new Transformation(0,0,0,0,0,i));
			}
		}
		
				
		public static void Example__Tree2()
		{
            Application catia;
            try {
				catia = CatiaApp.Instance;
			} catch {
				Debug.WriteLine("Start catia");
				throw new ArgumentNullException();
			}
			
			var activeDoc = catia.ActiveDocument;
			_ = activeDoc.Selection;

			Tree.ApplyWorkMode();
			Debug.WriteLine("start indexing tree");
			_ = new NodeTree(activeDoc);
			

			
			return ;
		}
		
		
		static void Example_Tree3()
		{
            Application catia;
            try {
				catia = CatiaApp.Instance;
			} catch {
				Debug.WriteLine("Start catia");
				throw new ArgumentNullException();
			}
			
			var activeDoc = catia.ActiveDocument;
			_ = activeDoc.Selection;

			Tree.ApplyWorkMode();
			Debug.WriteLine("start indexing tree");
			_ = new NodeTree(activeDoc);

			
			// or 
			

			
			return ;
		}
		
		private static DocumentNode Example__lamda(DocumentNode dn)
		{
			dn.doc_name = "hej";
			return dn;
		}
		
		public static void Test1()
		{
            Application catia;
            try
			{
				catia = CatiaApp.Instance;
			}
			catch
			{
				Debug.WriteLine("Start catia");
				throw new ArgumentNullException();
			}

			var activeDoc = catia.ActiveDocument;
            _ = activeDoc.Selection;

            Tree.ApplyWorkMode();
			var tree = new NodeTree(activeDoc);
			
			foreach (DocumentNode node1 in tree.GetNodesFromProductName("2069162.1")) {
				
				var stringList = node1.GetRootRelationship();
		
				foreach (var a in stringList) {
					Debug.WriteLine(a);
				}
			}
			
			
			var node = tree.GetNodesFromProductName("52744322.1")[0];
            _ = node.SelectNode();

            //				var t1 = node.leafProduct;
            //				
            //				Debug.WriteLine("T2:" + t1.TypeName());
            //				Debug.WriteLine(t1.GetName());
            //				
            //				var t3 = (ProductStructureTypeLib.Product) t1.Parent;
            //				Debug.WriteLine("T3:" +t3.TypeName());
            //				
            //				Debug.WriteLine(t3.GetName());
            //				var t4 = (ProductStructureTypeLib.Product) t3.ReferenceProduct;
            //				Debug.WriteLine("T4:" + t4.TypeName());
            //				Debug.WriteLine(t4.GetName());
            //				
            //				var t5 = t4.GetParent();
            //				Debug.WriteLine("T5:" + t5.TypeName());  
            //				Debug.WriteLine(t5.GetName());
            //				            
            //				var t6 = t5.GetProduct().Products;
            //				Debug.WriteLine("T6:" + t6.TypeName());  
            //				Debug.WriteLine(t6.Item(1).GetName());
            //				
            //				var t5 = (ProductStructureTypeLib.Product) .;
            //				Debug.WriteLine("T5:" + t5.TypeName());
            //				Debug.WriteLine(t5.NameExt());
        }
		
		public static void Example_Selection()
		{
			var activeDoc =  CatiaApp.Instance.ActiveDocument;
			
			Debug.WriteLine( activeDoc.GetName());
			
			var selection = activeDoc.Selection;
			
			_ =  CatiaApp.Instance.Documents;
			
			
			selection.DebugSelection();
			
			Tree.ApplyWorkMode();
			
			var tree = new NodeTree(selection.GetDocuments().FirstOrDefault());
			
			tree.DebugTree();
			
			Debug.WriteLine("----------------------------------");
			
			var sel =SelectionExt.SelectUnderSelection();
			
			foreach(var doc in sel.GetDocuments())
			{
				Debug.WriteLine(doc.GetName());
			}
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}


	}
}
