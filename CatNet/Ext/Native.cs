using System;
using System.Runtime.InteropServices;

using ATN.Utils.NativeExt.ObjectExt;

using Dassault.Catia.R24.INFITF;
using Dassault.Catia.R24.ProductStructureTypeLib;

namespace ATN.Catia.R24.Ext
{
    public static class VBExtension
	{
		public static string TypeName(this AnyObject i)
		{
			return Microsoft.VisualBasic.Information.TypeName(i);
		}
		public static string TypeName(this Collection i)
		{
			return Microsoft.VisualBasic.Information.TypeName(i);
		}
	}
	

	public static class NativeExtension
	{
		
		public static Document GetParent(this Collection obj)
		{
			return (Document)obj.SimpleCheck(x => ((Collection)x).Parent);
		}
		
		public static Document GetParent(this AnyObject obj) 
		{
			return (Document)obj.SimpleCheck(x => ((AnyObject)x).Parent);
		}

		public static string GetName(this AnyObject obj) 
		{
			return (string)obj.SimpleCheck(x => ((AnyObject)x).get_Name());
		}
		
		public static string GetName(this Collection obj) 
		{
			return (string)obj.SimpleCheck(x => ((Collection)x).Name);
		}
		
		public static string GetDefinition(this Product obj)
		{
			return (string)obj.SimpleCheck(x => ((Product)x).get_Definition());
		}
		
		public static string GetPartNumber(this Product obj)
		{
			return (string)obj.SimpleCheck(x => ((Product)x).get_PartNumber());
		}
		
		private static object SimpleCheck(this object obj, Func<object, object> action)
		{
			
			
			try {
				obj.IfNullThrowException();
				
				return action(obj);
				
			} catch (NullReferenceException) {
				return "";
			} catch (COMException) {
				return "";
			}
			
		}
	}
	
}