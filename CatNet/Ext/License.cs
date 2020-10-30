
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
	/// Description of CatLicense.
	/// 
	/// </summary>
	/// 
	//https://v5vb.wordpress.com/2010/03/23/checking-licenses/ 
	
	

	
	public static class License
	{
		public enum Licenses{
		SheetMetal,
		Step
		};

		static readonly Dictionary< Licenses, string> dict = new Dictionary< Licenses, string>
         {
			{ Licenses.Step , "SXT.prd" },
			{ Licenses.SheetMetal , "SMD.prd" }
         };

		
		public static bool CheckIfOpen(Licenses license)
		{
			var Catia = CatiaApp.Instance;
			Debug.WriteLine("Entering checkIfOpen:CatLicence: ");
			
			SettingControllers SettingsContrs = Catia.SettingControllers; // error
			
			SettingController SettingsContr = SettingsContrs.Item("CATSysDynLicenseSettingCtrl");
			var objDynSettingsContr = (DynLicenseSettingAtt)SettingsContr;

			if (objDynSettingsContr.GetLicense(dict[license]) == "Requested") {
				return true;
			}
			return false;
		}
		
		public static bool IsStepLicenceAvailible()
		{
			return License.CheckIfOpen(License.Licenses.Step);
		}
		
		public static void OpenOptions()
		{
			//CatNet.startCommand("%TO");
		}
	}
}
