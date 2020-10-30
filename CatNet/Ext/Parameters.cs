using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;

//using ATN.WinApi;

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
	/// Description of Parameters.
	/// </summary>
	public static class Parameters
	{
		
		public static bool HasParameter(this Document doc, string parameterName)
		{
			string s = "";	
			try {
				GetParameterValue(doc, parameterName);
				
			} catch (Exception) {
				return false;
			}
			if (s != "") {
				return true;
			}
			return false;
		}
		
		public static string GetParameterValue(this Document doc, string parameterName)
		{
			
			if (doc.GetType2() ==  Feature.FeatureType.PartDocument){
				var part = doc.AsPartDocument().GetPartBase();
				return GetParameterValue( part, parameterName);
			}
			
			if (doc.GetType2() ==  Feature.FeatureType.Part){
				return GetParameterValue( doc.AsPart(), parameterName);
			}
			
			if (doc.GetType2() ==  Feature.FeatureType.ProductDocument){
				 var prod = doc.GetProduct();
				 return GetParameterValue(prod, parameterName);
			}
				
			if (doc.GetType2() ==  Feature.FeatureType.Product){
				var prod = (doc.AsProduct().ReferenceProduct);
				return GetParameterValue(prod , parameterName);
			}
			return null;
		}
		
		public static string GetParameterValue(this Part part, string parameterName)
		{
			return part.GetParameter(parameterName).ValueAsString();
		}
		
		public static string GetParameterValue(this Product product, string parameterName)
		{
			return product.GetParameter(parameterName).ValueAsString();
		}
		
		public static Parameter GetParameter(this Part part, string parameterName)
		{
			var parameters = part.Parameters;
			return (Parameter)parameters.Item(parameterName);
		}
		
		public static Parameter GetParameter(this Product product, string parameterName)
		{
			var parameters = product.UserRefProperties;
			return (Parameter) parameters.GetItem(parameterName).AsParameter();

		}
		
		public static void SetParameterValue(this Part part, string parameterName, string value)
		{
			part.GetParameter(parameterName).ValuateFromString(value);
		}
	}
	
}
