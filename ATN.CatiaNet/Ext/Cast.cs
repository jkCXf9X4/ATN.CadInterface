using System.Collections.Generic;
using System.Linq;

using Dassault.Catia.R24.INFITF;
using Dassault.Catia.R24.MECMOD;
using Dassault.Catia.R24.ProductStructureTypeLib;
using Dassault.Catia.R24.KnowledgewareTypeLib;

namespace ATN.Catia.R24.Ext
{
    public static class CastExtension
	{
		public static Document AsDocument(this CATBaseDispatch i)
		{
			return (Document)i;
		}	
		
		public static List<Document> AsDocument(this  List<CATBaseDispatch> i)
		{
			return i.Select(x=> x.AsDocument()).ToList();
		}	

		
		public static ProductDocument AsProductDocument(this AnyObject i)
		{
			return (ProductDocument)i;
		}
		
		public static List<ProductDocument> AsProductDocument(this List<AnyObject> i)
		{
			return i.Select(x=> x.AsProductDocument()).ToList();
		}

		
		public static Product AsProduct(this AnyObject i)
		{
			return (Product)i;
		}
		
		public static List<Product> AsProduct(this List<AnyObject> i)
		{
			return i.Select(x=> x.AsProduct()).ToList();
		}
		
		
		public static Part AsPart(this AnyObject i)
		{
			return (Part)i;
		}
		
		public static List<Part> AsPart(this List<AnyObject> i)
		{
			return i.Select(x=> x.AsPart()).ToList();
		}
		
		
		public static PartDocument AsPartDocument(this CATBaseDispatch i)
		{
			return (PartDocument)i;
		}
		
		public static List<PartDocument> AsPartDocument(this List<CATBaseDispatch> i)
		{
			return i.Select(x=> x.AsPartDocument()).ToList();
		}
		
		
		public static Parameter AsParameter(this CATBaseDispatch i)
		{
			return (Parameter)i;
		}
		
		public static List<Parameter> AsParameter(this List<CATBaseDispatch> i)
		{
			return i.Select(x=> x.AsParameter()).ToList();
		}
		
	}
	
}