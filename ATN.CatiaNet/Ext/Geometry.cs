using System;
using System.Diagnostics;
using System.Linq;

using ATN.Utils.MathExt.Numerical;

using Dassault.Catia.R24.INFITF;
using Dassault.Catia.R24.MECMOD;

using Dassault.Catia.R24.HybridShapeTypeLib;
using Dassault.Catia.R24.KnowledgewareTypeLib;
using Dassault.Catia.R24.SPATypeLib;

namespace ATN.Catia.R24.Ext
{
    /// <summary>
    /// Description of Geometry.
    /// </summary>
    public static class GeometryExt
    {


        public static HybridShape GetGeometry(this Part part, string geo)
        {
            return SearchDeepBodies(part.HybridBodies, "", geo);
        }

        public static HybridShape SearchDeepBodies(HybridBodies body, string soFar, string quary)
        {
            foreach (HybridBody i in body)
            {

                foreach (HybridShape s in i.HybridShapes)
                {
                    var newName = soFar + i.get_Name() + @"/" + s.get_Name();
                    //Debug.WriteLine(newName);

                    if (newName == quary)
                        return s;
                }

                var match = SearchDeepBodies(i.HybridBodies, soFar + i.get_Name() + @"/", quary);
                if (match != null)
                    return match;
            }
            return null;
        }

        public static Reference GetReference(this Part part, string geo)
        {
            var obj = part.FindObjectByName(geo);
            return part.CreateReferenceFromObject(obj);
        }

    }

    public static class PointExtension
    {
        public static Transformation GetCoordinates(this Document doc, Point point)
        {
            if (doc.IsPart())
            {

                var TheSPAWorkbench = (SPAWorkbench)doc.GetWorkbench("SPAWorkbench");

                Measurable mesPoint = TheSPAWorkbench.GetMeasurable((Reference)point);

                var arrPoint = new object[3];
                mesPoint.GetPoint(arrPoint);

                return new Transformation(new ATN.Utils.MathExt.Numerical.Coordinate(arrPoint.Cast<double>().ToArray()), new RotationMatrix());
            }
            return null;
        }



        public static bool SetPoint(this Part part, string pointName, object[] coords)
        {
            var point = (Point)part.FindObjectByName(pointName); //.GetGeometry(pointName);

            if (point == null)
                throw new NotFoundException("Point not found");

            point.SetCoordinates(coords);

            return true;
        }

    }

    public static class SplineExtension
    {
        public static bool Tension(this HybridShapeSpline spline, double tension)
        {
            throw new NotImplementedException();

            //var nrPoints = spline.GetNbControlPoint();


            //HybridShapeTypeLib.HybridShapeDirection hsdTangency;
            //double tangencyNom;
            //int invTangency;
            //HybridShapeTypeLib.HybridShapeDirection hsdCurvature;
            //double CurvatureRadius;
            //spline.GetPointConstraintExplicit(1, out hsdTangency, out tangencyNom, out invTangency, out hsdCurvature, out CurvatureRadius);

            //spline.SetPointConstraintExplicit(1, hsdTangency, tangencyNom+0.2, invTangency, hsdCurvature, CurvatureRadius);

        }
    }

    public static class PolylineExtension
    {
        public static double GetRadius(this HybridShapePolyline polyline, int position)
        {
            polyline.GetElement(position, out _, out Length radius);

            return radius.Value;
        }
    }
}
