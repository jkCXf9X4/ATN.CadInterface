using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial;


using Dassault.Catia.R24.INFITF;
using Dassault.Catia.R24.ProductStructureTypeLib;
using Dassault.Catia.R24.MECMOD;
using Dassault.Catia.R24.PARTITF;
using Dassault.Catia.R24.HybridShapeTypeLib;
using Dassault.Catia.R24.KnowledgewareTypeLib;
using Dassault.Catia.R24.SPATypeLib;

using ATN.Utils.NativeExt.ObjectExt;
using ATN.Utils.MathExt.Numerical;
using ATN.Utils.NativeExt.ArrayExt;


namespace ATN.Catia.R24.Ext
{
	/// <summary>
	/// Description of Move.
	/// </summary>
	[Serializable]
	public static class Transform
	{
		
		public static void PositionLeaf(Product leaf, Transformation coordinate)
		{
			leaf.Position.SetComponents(coordinate.ToTranformationArray());
		}
		
		public static void PositionLeaf(Product leaf, int[] matrix)
		{
			leaf.Position.SetComponents(matrix.Select(x => (object)x).ToArray());
		}
		
		// utgår alltid fån partens orginalsystem
		public static void PositionSelection(Selection sel, Transformation coordinate)
		{
			for(int i = 1; i < sel.Count2+1;i++) {
				((Product)sel.Item2(i).LeafProduct).Position.SetComponents(coordinate.ToTranformationArray());
			}
		}
		
		public static void ResetPosition(DocumentNode DocumentNode)
		{
			Transform.PositionLeaf(DocumentNode.product , new Transformation(0, 0, 0, 0, 0, 0));
		}
		
		
		// move utgår från Catias xyz system
		public static void MoveSelection(Selection sel, Transformation coordinate)
		{
			var prods = sel.GetLeafProducts().AsProduct();
			prods.ForEach(x=> x.Move.Apply(coordinate.ToTranformationArray()));
		}
		
		public static object[] GetProductTransformation(this Product leaf)
		{
			var ObjArr = new object[12];
			leaf.Position.GetComponents(ObjArr);
			
			return ObjArr;
		}
		
		public static void MoveLeaf(Product leaf, Transformation coordinate)
		{
			
			leaf.Move.MovableObject.Apply(coordinate.ToTranformationArray());
		}
	}


	// TODO: Move to BasicOperations 
	[Serializable]
	public class Transformation
	{
		public ATN.Utils.MathExt.Numerical.Coordinate position = new ATN.Utils.MathExt.Numerical.Coordinate();
		public ATN.Utils.MathExt.Numerical.Rotation rotation = new ATN.Utils.MathExt.Numerical.Rotation();
		public ATN.Utils.MathExt.Numerical.RotationMatrix rotationMatrix;
		
		public bool hasEuler = false;
		public bool hasMatrix = false;
		
		
		
		public Transformation()
		{
			rotationMatrix =  rotation.ToRotationMatrices();
			
			hasEuler = true;
			hasMatrix = true;
		}
		
		public Transformation(double X, double Y, double Z, double Phi, double Teta, double Psi, string order = "xyz")
		{

			position = new ATN.Utils.MathExt.Numerical.Coordinate(X,Y,Z);
			rotation = new ATN.Utils.MathExt.Numerical.Rotation(Phi,Teta,Psi);
			
			rotationMatrix =  new RotationMatrix(rotation, order);
			
			hasEuler = true;
			hasMatrix = true;
		}
		
		public Transformation(ATN.Utils.MathExt.Numerical.Coordinate pos, RotationMatrix rm)
		{
			position =  pos;

			rotationMatrix =  rm;
			
			hasMatrix = true;
		}
		
		public Transformation(ATN.Utils.MathExt.Numerical.Coordinate pos, Rotation rm, string order = "xyz")
		{
			position =  pos;

			rotationMatrix =  rm.ToRotationMatrices(order);
			
			hasMatrix = true;
		}
		
		public Transformation(object[] transformationArrayObj)
		{
            var transformationArray = transformationArrayObj.Cast<double>().ToArray();
			
			var matrix = Matrix<double>.Build.Dense(3,3);
			
			// { 0, 3, 6 }
			// { 1, 4, 7 }
			// { 2, 5, 8 }

			matrix[0,0] =  transformationArray[0];
			matrix[1,0] =  transformationArray[1];
			matrix[2,0] =  transformationArray[2];
			
			matrix[0,1] =  transformationArray[3];
			matrix[1,1] =  transformationArray[4];
			matrix[2,1] =  transformationArray[5];
			
			matrix[0,2] =  transformationArray[6];
			matrix[1,2] =  transformationArray[7];
			matrix[2,2] =  transformationArray[8];
			
			position.X = transformationArray[9];
			position.Y = transformationArray[10];
			position.Z = transformationArray[11];
			
			rotationMatrix = new RotationMatrix(matrix);
			
			hasMatrix = true;
		}
		
		public void Reset()
		{
			position.Reset();
			rotation.Reset();
			rotationMatrix.Reset();
		}
		
		
		public static Transformation AddEuler(Transformation A, Transformation B, string order = "xyz")
		{
			if(A.hasEuler && B.hasEuler)
			{
				var c = new Transformation(A.position+B.position, A.rotation+B.rotation, order);
				c .rotation = c.rotation.GetSimplifiedRotation();
					
				return c;
			}

			throw new ArgumentException("Both dont have euler");
	
		}
			
		public Transformation NewOrder(string order = "xyz")
		{
			if (!hasEuler) throw new ArgumentException("No Euler availible");
			
			rotationMatrix =  rotation.ToRotationMatrices(order);
			
			return this;
		}
		
		public Transformation ApplyTransformation(Transformation A)
		{
            var b = new Transformation
            {
                position = new Coordinate(A.rotationMatrix.matrix * position.coordinate) + A.position
            };

            try
            {
				b.rotation = rotation+A.rotation;
			}
			catch
			{
				b.hasEuler = false;
			}
			
			b.rotationMatrix = A.rotationMatrix.Multiply(rotationMatrix);

			return b;
		}
		
		
		
		public object[] GetPointArray()
		{
			return position.ToArray().Cast<object>().ToArray();
		}

		public Transformation GetInverse()
		{
            var b = new Transformation
            {
                position = this.position.GetInverse(),
                rotation = this.rotation.GetInverse(),
                rotationMatrix = this.rotationMatrix.GetInverse()
            };
            return b;
		}


		public object[] ToTranformationArray()
		{
			// { 0, 3, 6 }
			// { 1, 4, 7 }
			// { 2, 5, 8 }
			
			var transformationArray = new double[12];

			transformationArray[0] = rotationMatrix.matrix[0,0];
			transformationArray[1] = rotationMatrix.matrix[1,0];
			transformationArray[2] = rotationMatrix.matrix[2,0];
			
			transformationArray[3] = rotationMatrix.matrix[0,1];
			transformationArray[4] = rotationMatrix.matrix[1,1];
			transformationArray[5] = rotationMatrix.matrix[2,1];
			
			transformationArray[6] = rotationMatrix.matrix[0,2];
			transformationArray[7] = rotationMatrix.matrix[1,2];
			transformationArray[8] = rotationMatrix.matrix[2,2];
			
			transformationArray[9] = position.X;
			transformationArray[10] =position.Y;
			transformationArray[11] =position.Z;
			
			return transformationArray.Cast<object>().ToArray();
		}

		public string [] ToStringArray()
		{
			return new string[] {
				position.X.ToString(),
				position.Y.ToString(),
				position.Z.ToString(),
				rotation.Phi.ToString(),
				rotation.Teta.ToString(),
				rotation.Psi.ToString(),
			};
		}
		
		
		
		public override string ToString()
		{
			var dem = " ";
			return position.X.ToString() + dem + position.Y.ToString() + dem + position.Z.ToString() + dem +
			rotation.Phi.ToString() + dem + rotation.Teta.ToString() + dem + rotation.Psi.ToString();
		}
	}
}

	

