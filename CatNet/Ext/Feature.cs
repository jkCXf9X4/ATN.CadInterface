
using ATN.Utils.NativeExt.ObjectExt;

using Dassault.Catia.R24.INFITF;

namespace ATN.Catia.R24.Ext
{
    public static class Feature
    {

        public enum FeatureType
        {
            Application,
            Part,
            Body,
            Shapes,
            PartDocument,
            Drawing,
            Product,
            Products,
            ProductDocument,
            LeafProduct,
            Documents,
            Document,
            Enovia,
            CATfct,
            String,
            Number,
            Bool,
            Unknown,
            None
        }
        ;


        public static FeatureType GetType2(this AnyObject i)
        {
            i.IfNullThrowException();

            string type = i.TypeName();
            switch (type)
            {
                case "ProductDocument":
                    return FeatureType.ProductDocument;
                case "Product":
                    return FeatureType.Product;
                case "PartDocument":
                    return FeatureType.PartDocument;
                case "Part":
                    return FeatureType.Part;
                case "DrawingDocument":
                    return FeatureType.Drawing;
                case "Products":
                    return FeatureType.Products;
                case "Bodies":
                    return FeatureType.Body;
                case "Shapes":
                    return FeatureType.Shapes;
                case "Application":
                    return FeatureType.Application;
            }
            if (type == "Document")
            {
                // check name  for the Enovia window
                string ext = DocumentExt.GetDocumentExtension((Document)i);
                switch (ext)
                {
                    case "CATfct":
                        return FeatureType.CATfct;
                    default:
                        // not good but no other solution yet
                        return FeatureType.Enovia;
                }
            }
            return FeatureType.Unknown;
        }



    }
}
