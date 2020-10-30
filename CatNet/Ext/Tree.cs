using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Automation;



using Dassault.Catia.R24.INFITF;
using Dassault.Catia.R24.ProductStructureTypeLib;
using Dassault.Catia.R24.MECMOD;
using Dassault.Catia.R24.PARTITF;
using Dassault.Catia.R24.HybridShapeTypeLib;
using Dassault.Catia.R24.KnowledgewareTypeLib;
using Dassault.Catia.R24.SPATypeLib;

using ATN.Utils.NativeExt.EnumExt;
using ATN.Utils.NativeExt.ICollectionExt;
using ATN.Utils.NativeExt.StringExt;

using ATN.Utils.WinAutomation;

namespace ATN.Catia.R24.Ext
{
	
	public static class Tree
	{
		public static bool ExportTree(Document doc)
		{
			throw new NotImplementedException();
			//TODO: Skapa och sedan eventuellt använda som produkt?
			
			//Sub CATMain()
			//Dim productDocument1 As Document
			//Set productDocument1 = CATIA.ActiveDocument
			//'Input box to select txt or xls
			//Dim exportFormat As StringexportFormat = Inputbox ("Please choose format to export the tree as._      Type either 'xls' or 'txt'")
			//IF exportFormat <> "xls" THEN
			//IF exportFormat <> "txt" THEN
			//MsgBox "Did not enter txt or xls. Program cancelled please retry macro."
			//
			//Else
			//
			//'Input box to enter name of file
			//Dim partName As StringpartName = Inputbox ("Please enter the file name.")
			//'Input box to enter file location
			//Dim oLocation As StringoLocation = "C:\Macro Files\"productDocument1.ExportData oLocation & partName & "." & _exportFormat,"txt"
			//
			//End If
			//End If
			//End Sub
			//return true;
		}
		
		
		static List<string> GetRootRelationship(Product i)
		{
			try {
				return (new List<string>(new []{ i.GetName()  })).Concat(GetRootRelationship(i.Parent as Product)).ToList();
			} catch {
				try {
					return new List<string>(new []{ i.GetName() });
				} catch {
				}
			}
			return null;
		}
		
		public static bool ApplyWorkMode(CatWorkModeType type = CatWorkModeType.DESIGN_MODE)
		{
			// insert test to see if only one is selected
			
			var doc = CatiaApp.Instance.ActiveDocument;
			
			if (doc.IsProductDocument()) {
				var prod = (ProductDocument)doc;
				prod.Product.ApplyWorkMode(type);
				return true;
			}
			return false;
				
		}
		
		public static bool ApplyWorkMode(Document doc, CatWorkModeType type = CatWorkModeType.DESIGN_MODE)
		{
			// insert test to see if only one is selected
			if (doc.IsProductDocument()) {
				var prod = (ProductDocument)doc;
				prod.Product.ApplyWorkMode(type);
				return true;
			}
			return false;
				
		}
		

		
		
		public static bool ActivateInstance(string instance)
		{
			var sel = SelectionExt.SelectInstances(instance);
			
			if (sel.Count2 > 1)
				throw new MultipleSelectionException("More than one node selected");
			
			Activate();
			
			return true;
		}

		/// <summary>
		/// Activate selected node in tree
		/// </summary>
		/// <returns></returns>
		public static bool ActivateSelection()
		{
			// Check if one is selected
			if ( CatiaApp.Instance.ActiveDocument.Selection.Count2 != 1)
				return false;
			
			Activate();
			
			return true;
		}
		
		/// <summary>
		/// Expands selected entity one step
		/// </summary>
		/// <returns>True if expanded correctly, false otherwise</returns>
		public static bool ExpandSelection()
		{
			var Catia = CatiaApp.Instance;
			//TODO: Implement deaper expansion
			
			// Prerequsites 
			if (Catia.ActiveDocument.Selection.Count2 == 0)
				return false;
			
			// Main
			// - Open Dialog
			Catia.StartCommand("Expand Selection");
			
			// - Find window
			IntPtr depthDialog = ATN.Utils.WinAutomation.Window.FindWindowFromCaptation("Specification depth", 5000); //find hWnd of dialog window
			if (depthDialog == IntPtr.Zero) {
				return false;
			}
			
			// - press OK
			AutomationElement dialog = AutomationElement.FromHandle(depthDialog);
			var addBtn = dialog.FindByNameExt("OK");
			addBtn.PressButtonExt();
			
			return true;
		}
		
		private static void Activate()
		{
			CatiaApp.Instance.StartCommand("FrmActivate");
		}
	}

	
	
	public class DocumentNode
	{
		public string hash;
		public string doc_name;
		public string product_name;
		public Feature.FeatureType type;
		public DocumentNode parent = null;
		public List<DocumentNode> children = new List<DocumentNode>();
		public Document document;
		/// <summary>
		/// Catia part/product Instance
		/// </summary>
		public Product product = null;
		public int level;
		public int index;
		
		public DocumentNode()
		{
			hash = new DateTimeOffset(DateTime.UtcNow).Millisecond.ToString(); //  ATN.Utils.Cryptology.Hash.GetTimestampHash();
		}
		
		public DocumentNode(Document doc, Product product, DocumentNode parent)
			: base()
		{
			document = doc;
			this.product = (Product)product;
			this.parent = parent;
			type = doc.GetType2();
			doc_name = doc.GetName();
			product_name = GetProductName();
			if (parent == null) {
				level = 0;
			} else {
				level = parent.level + 1;
			}
		}
		
		public bool ActivateNode()
		{
			Tree.ActivateInstance(product.GetName());
			
			return true;
		}
		
		public bool SetDesignMode(CatWorkModeType type = CatWorkModeType.DESIGN_MODE)
		{
			Tree.ApplyWorkMode(this.product.AsDocument(), type);
			
			return true;
		}
		
		public Selection SelectUnderNode()
		{
			if (!SelectNode())
				throw new NotFoundException();
			
			return SelectionExt.SelectUnderSelection();
		}
		
		
		public bool SelectNode()
		{
			string tempName = GetProductName();
			
			var sel = SelectionExt.SelectInstances(tempName);
			
			if(sel.Count2 == 1)
				return true;
			
			// problems if you dont load(design mode) in the entire tree to find the correct one
			
			bool found = false;
			var lim = sel.Count2 + 1;
			
			for (int i = 1; i < lim; i++) {
				var tempLeaf = (Product)sel.Item2(i).LeafProduct;
                var t1 = tempLeaf.GetRootRelationship().RemoveAtWithNegative(-1).ToList();
				var t2 = this.GetRootRelationship().RemoveAtWithNegative(-1).ToList();

                for ( int j = t2.Count; j <= t1.Count;j++)
				{
					t1.RemoveAtWithNegative(-1).ToList();
				}
				
				if (t1.Compare1(t2))
					found = true;
				else {
					sel.Remove2(i);
					i--;
					lim--;
				}
			}
			return found;
		}
		
		public Selection AddNodeToSelection(Selection sel)
		{
			throw new NotImplementedException();
			
		}
		
		
		public void CopyContentTo(DocumentNode toNode)
		{
			var sel = SelectUnderNode();
				
			sel.Copy();
				
			toNode.SelectNode();
				
			sel.Paste();
				
			sel.Clear();
		}

		private DocumentNode AddNode(Document doc, Product leaf, DocumentNode parent)
		{
			var no = new DocumentNode(doc, leaf, parent);
			
			no.UpdateChildren();
			
			return no;
		}
		
		public void UpdateChildren()
		{
			children.Clear();
			
			if (type == Feature.FeatureType.ProductDocument || type ==  Feature.FeatureType.Product) {
				
				var productBase = document.GetProduct();

				foreach (Product iPrd in  productBase.Products) {
					children.Add(AddNode(iPrd.ReferenceProduct.Parent.AsDocument()  , iPrd, this));
				}
			}
		}
		
		public string GetIndent()
		{
			return new String('-', level);
		}

		public string GetProductName()
		{
			if (this.product != null)
			{
				return product.GetName();
			}
			else
            {
				return document.AsProductDocument().Product.GetName();
            }
		}

		public string GetProductNameWithIndent()
		{
			return GetIndent() + GetProductName();
		}
		
		public void DebugNode(bool indention = true)
		{
			if (indention)
            {
				Debug.WriteLine(GetProductNameWithIndent());
			}
            else
            {
				Debug.WriteLine(GetProductName());
			}
		}
		
		public List<DocumentNode> Where(Func<DocumentNode, bool> predicate, bool children = true)
		{
			var i = new List<DocumentNode>();
			
			this.IfPredicateDoAction(predicate, x=> i.Add(x),children);
			
			return i;
		}

		public void Do(Action<DocumentNode> action, bool children = true)
		{	
			this.IfPredicateDoAction(x=>true, action, children);
		}
		
		void IfPredicateDoAction(Func<DocumentNode, bool> predicate, Action<DocumentNode> action, bool applyToChildren)
		{
			if (predicate(this)) {
				action(this);
			}
			if (applyToChildren) {
				this.children.ForEach(x=>x.IfPredicateDoAction(predicate ,action, applyToChildren));
			}
		}
		
		public List<string> GetRootRelationship()
		{
			try {
				return (new List<string>(new []{ this.product.GetName()})).Concat(this.parent.GetRootRelationship()).ToList();
			} catch {
				try {
					return new List<string>(new []{ this.product.GetName() });
				} catch {
				}
			}
			return null;
		}
		
	}
	

	
	public class NodeTree
	{
		public List<DocumentNode> list;
        readonly Document rootDoc;
		public int depth = 0;
		public int index = -1;
		DocumentNode topNode;
		
		public NodeTree(Document doc)
		{
			rootDoc = doc;

			if (!doc.IsProductOrProductDocument()) {
				return;
			}

			ResetTree();
		}
		
		public void ResetTree()
		{
			topNode = new DocumentNode(rootDoc, null, null);
			
			topNode.UpdateChildren();
			
			ResetList();
		}
		
			
		public void ResetList()
		{
			list = new List<DocumentNode>();
			index = -1;
			
			AddToList(topNode);
		}
		
		private void AddToList(DocumentNode node)
		{
			index += 1;
			node.index = index;
			
			list.Add(node);

			if (depth < node.level) {
				depth = node.level;
			}
			
			foreach (var i in node.children) {
				AddToList(i);
			}
		}
		
		public void UpdateNode(DocumentNode node)
		{
			var found = list.Where(x => x.hash == node.hash).ToList();
			 
			if (found.Count > 1)
				throw new MultipleFoundException();
			if (found.Count == 0)
				throw new NotFoundException();

			found.First().UpdateChildren();
			ResetList();
		}
		
		public DocumentNode GetTopNode()
		{
			if( topNode == null)
				throw new NullReferenceException("Tree is not instanciated");
			return topNode;
		}
		
		public List<DocumentNode> GetPartNodes()
		{
			return list.Where(x => x.type == Feature.FeatureType.Part).ToList();
		}
		
		public List<DocumentNode> GetNodesFromDocumentName(string name)
		{
			return list.Where(x => x.document.GetName().CompareWithWildcard(name)).ToList();
		}
		
		public List<DocumentNode> GetNodesFromProductName(string name)
		{
			var i = list.Where(x => x.GetProductName().CompareWithWildcard(name)).ToList();
			return i;
		}
		
		public List<DocumentNode> GetProductNodes()
		{
			return list.Where(x => x.type == Feature.FeatureType.Product).ToList();
		}
		
		public List<DocumentNode> GetNodesFromLeafDescription(string name)
		{
			return list.Where(x => x.product.GetDefinition().CompareWithWildcard(name)).ToList();
		}
				
		public void DebugTree()
		{
			foreach (var docNode in list) {
				docNode.DebugNode();
			}
		}

	}
	
	public static class NodeTreeExt
	{

	}
}
