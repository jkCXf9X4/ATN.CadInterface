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
	/// Description of Measure.
	/// </summary>
	public static class Measure
	{

//Set partDocument1 = CATIA.ActiveDocument
//Set part1 = partDocument1.Part
//Set bodies1 = part1.Bodies
//Set body1 = bodies1.Item("Original_Body")
//Set shapes1 = body1.Shapes
//Set pad1 = shapes1.Item("Master_Pad")
//Set selection1 = partDocument1.Selection
//
//Set TheSpaWorkbench = CATIA.ActiveDocument.GetWorkbench("SPAWorkbench")
//selection1.Clear
//selection1.Add pad1
//selection1.Search "Topology.Face,sel"
//Dim i As Integer
//For i = 1 To selection1.Count2
//Set reference1 = hybridShapes1.Item("XY_Plane")
//Set Facename = selection1.Item2(i).Value
//Set reference2 = part1.CreateReferenceFromObject(Facename)
//Set TheMeasurable = TheSpaWorkbench.GetMeasurable(reference1)
//MinimumDistance = TheMeasurable.GetAngleBetween(reference2)
//Next
//End Sub
	}
}
