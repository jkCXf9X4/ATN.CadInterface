using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;

using Dassault.Catia.R24.INFITF;


namespace ATN.Catia.R24.Ext
{
    public static class TypeCheck
    {
        public static bool IsType2(this AnyObject i, Feature.FeatureType type)
        {
            if (i.GetType2() == type)
            {
                return true;
            }
            return false;
        }

        public static bool IsProductOrProductDocument(this Document document)
        {
            if (document.IsProduct() || document.IsProductDocument())
            {
                return true;
            }
            return false;
        }

        public static bool IsProduct(this Document doc)
        {
            if (doc.IsType2(Feature.FeatureType.Product))
            {
                return true;
            }
            return false;
        }

        public static bool IsProductDocument(this Document doc)
        {
            if (doc.IsType2(Feature.FeatureType.ProductDocument))
            {
                return true;
            }
            return false;
        }

        public static bool IsPartOrPartDocument(this Document document)
        {
            if (document.IsPart() || document.IsPartDocument())
            {
                return true;
            }
            return false;
        }

        public static bool IsPart(this Document doc)
        {
            if (doc.IsType2(Feature.FeatureType.Part))
            {
                return true;
            }
            return false;
        }

        public static bool IsPartDocument(this Document doc)
        {
            if (doc.IsType2(Feature.FeatureType.PartDocument))
            {
                return true;
            }
            return false;
        }

        public static bool IsDrawing(this Document doc)
        {
            if (doc.IsType2(Feature.FeatureType.Drawing))
            {
                return true;
            }
            return false;
        }
    }
}
