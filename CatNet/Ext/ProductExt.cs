using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Automation;

//using ATN.WinApi;
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
	public static class ProductExt
	{
		//Document->product->products
		
		//					get_PartNumber(); // works
		//					get_DescriptionInst(); // nothing
		//					get_DescriptionRef(); // strange
		//					get_Definition(); // Description!
		//					get_Name(); // same as partumber
		//					get_Nomenclature(); // nothing
		
		public static Product GetProduct(this Document i)
		{
			var productDocBase = i.AsProductDocument();
			return GetProduct(productDocBase);

		}


		public static Product GetProduct(this ProductDocument i)
		{
			var productBase = i.Product;
			return productBase;
		}

		
		public static List<Product> GetSubProduct(this ProductDocument i)
		{
			// TODO: FIX 
			throw new NotImplementedException();
			
			// var products = i.GetProduct().Products;
			// return products;
		}

		public static string GetPath(this Product inst)
		{
			return String.Join(@"\",GetRootRelationship(inst) );
		}
		
		public static List<string> GetRootRelationship(this Product i)
		{
			// might need to add max level counter, to be seen
			//Debug.WriteLine(i.GetType2().ToString());
			
			if (!i.IsType2(
				Feature.FeatureType.Application) &&
			    (i.IsType2(Feature.FeatureType.Product) || i.IsType2(Feature.FeatureType.Products))
			   ) {
			
				var name = i.GetName();
				try {
					var l = GetRootRelationship(i.Parent as Product);
					return (new List<string>(new []{ name })).Concat(l).ToList();
				} catch {
					try {
						return new List<string>(new []{ name });
					} catch {
					}
				}
				return null;
			}
			return null;
		}
		
		public static Part GetPart(this Product productDoc)
		{
			var doc = (Document)productDoc;
			if (doc.IsPart()) {
				PartDocument oShapeRepresentation;
				if (productDoc.HasAMasterShapeRepresentation()) {
					oShapeRepresentation = productDoc.GetMasterShapeRepresentation(false).AsPartDocument();
					string name = oShapeRepresentation.GetName();// .get_Name();
					if (name.Contains("CATPart")) {
						return oShapeRepresentation.Part;
					}
				}
			}
			return null;
		}
	}
}
