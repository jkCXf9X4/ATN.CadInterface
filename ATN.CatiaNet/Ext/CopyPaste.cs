using Dassault.Catia.R24.INFITF;
using System;

using ATN.Catia.R24.COM;


namespace ATN.Catia.R24.Ext
{
	/// <summary>
	/// Description of CopyPaste.
	/// </summary>
	public static class CopyPaste
	{
		public static void Paste2(this Selection sel)
		{
			if (sel.Count2 == 0)
			{
				throw new ArgumentException("Nothing in selection");
			}
			
			sel.Paste();
		}
		
		public static void Copy2(this Selection sel)
		{
			if (sel.Count2 == 0)
			{
				throw new ArgumentException("Nothing in selection");
			}
			
			sel.Copy();
		}
		
		
		//In all the containers
		//"CATIA_LINK_FORMAT" to paste "Catia Link Source",
		//"OLE_LINK_FORMAT" to paste "Ole Link Source",
		//"OLE_EMBED_FORMAT" to paste "Ole Embed Source".
		//
		//In a Part container
		//"CATPrtCont" to paste "As Specified In Part Document",
		//"CATPrtResultWithOutLink" to paste "AsResult",
		//"CATPrtResult" to paste "AsResultWithLink",
		//"CATMaterialCont" to paste "As material",
		//"AsMaterialLink" to paste "As material link",
		//"CATMechProdCont" to paste "As specified in Assembly",
		//"CATProdCont" to paste "As specified in Product Structure",
		//"CATIA_SPEC" to paste "CATIA_SPEC",
		//"CATIA_RESULT" to paste "CATIA_RESULT".
		//
		//In a Product container
		//"CATProdCont" to paste "As specified in Product Structure",
		//"CATSpecBreakLink" to paste "Break Link".


		public static void PasteBreakLink(this Selection sel)
		{
			sel.PasteSpecial("CATSpecBreakLink");
		}
		
		public static void PasteCATProdCont(this Selection sel)
		{
			sel.PasteSpecial("CATProdCont");
		}
		
		
		public static bool CopyPasteBreakLink(string part, string destination)
		{
			var sel = SelectionExt.SelectParts(part);
			
			sel.Copy();
			
			sel = SelectionExt.SelectPart(destination);
			
			sel.PasteBreakLink();
			
			sel.Clear();
			
			return true;
		
		}
		
		public static bool PasteSelection(this Selection sel, string destination)
		{
			sel.Copy();
			
			sel = SelectionExt.SelectPart(destination);
			
			sel.Paste2();
			
			sel.Clear();
			
			return true;
		
		}
	}
	
	public static class TreeCopyPaste
	{
		
		public static Selection CopyPaste(DocumentNode node, DocumentNode toNodem, bool clear)
		{
			return CopyPasteBase(new DocumentNode[]{node}, toNodem, clear, false, "");
		}
		
		public static Selection CopyPaste(DocumentNode[] nodes, DocumentNode toNode, bool clear)
		{
			return CopyPasteBase(nodes,toNode, clear, false, "");
		}
		
		public static Selection CopyPasteBreakLink(DocumentNode node, DocumentNode toNode, bool clear)
		{
			return CopyPasteBase(new DocumentNode[]{node},toNode, clear, true, "CATSpecBreakLink");

		}

		public static Selection CopyPasteBreakLink(DocumentNode[] nodes, DocumentNode toNode, bool clear)
		{
			return CopyPasteBase(nodes,toNode, clear, true, "CATSpecBreakLink");

		}
		
		public static Selection CopyPasteCATProdCont(DocumentNode node, DocumentNode toNode, bool clear)
		{
			return CopyPasteBase(new DocumentNode[]{node},toNode, clear, true, "CATProdContk");

		}
	
		public static Selection CopyPasteCATProdCont(DocumentNode[] nodes, DocumentNode toNode, bool clear)
		{
			return CopyPasteBase(nodes,toNode, clear, true, "CATProdContk");

		}
		
		static Selection CopyPasteBase(DocumentNode[] nodes, DocumentNode toNode, bool clear, bool special, string command)
		{
			var aDoc = CatiaApplication.Instance.ActiveDocument;
			var sel = aDoc.Selection;
			
			sel.Clear();
			foreach(var i in nodes)
			{
				sel.Add(i.product);
			}

			sel.Copy();
			sel.Clear();
				
			sel.Add(toNode.product);
			
			if (special)
			{
				sel.PasteSpecial(command);
			}
			else{
				sel.Paste();
			}
			
			if (clear)
				sel.Clear();
			
			return sel;
		}
	
	}
}
