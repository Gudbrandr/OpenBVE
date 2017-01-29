// ╔═════════════════════════════════════════════════════════════╗
// ║ World.cs for Route Checker                                  ║
// ╠═════════════════════════════════════════════════════════════╣
// ║ This file cannot be used in the openBVE main program.       ║
// ║ The file from the openBVE main program cannot be used here. ║
// ╚═════════════════════════════════════════════════════════════╝

using System;

namespace OpenBve {
	public static class World {

		// vectors
		/// <summary>Represents a 2D vector of System.Double coordinates.</summary>
		public struct Vector2D {
			public double X;
			public double Y;
			public Vector2D(double X, double Y) {
				this.X = X;
				this.Y = Y;
			}
		}
		/// <summary>Represents a 2D vector of System.Single coordinates.</summary>
		public struct Vector2Df {
			public float X;
			public float Y;
			public Vector2Df(float X, float Y) {
				this.X = X;
				this.Y = Y;
			}
		}
		/// <summary>Represents a 3D vector of System.Double coordinates.</summary>
		public struct Vector3D {
			public double X;
			public double Y;
			public double Z;
			public Vector3D(double X, double Y, double Z) {
				this.X = X;
				this.Y = Y;
				this.Z = Z;
			}
			/// <summary>Returns a normalized vector based on a 2D vector in the XZ plane and an additional Y-coordinate.</summary>
			/// <param name="Vector">The vector in the XZ-plane. The X and Y components in Vector represent the X- and Z-coordinates, respectively.</param>
			/// <param name="Y">The Y-coordinate.</param>
			public Vector3D(Vector2D Vector, double Y) {
				double t = 1.0 / Math.Sqrt(Vector.X * Vector.X + Vector.Y * Vector.Y + Y * Y);
				this.X = t * Vector.X;
				this.Y = t * Y;
				this.Z = t * Vector.Y;
			}
			/// <summary>Returns the sum of two vectors.</summary>
			public static Vector3D Add(Vector3D A, Vector3D B) {
				return new Vector3D(A.X + B.X, A.Y + B.Y, A.Z + B.Z);
			}
			/// <summary>Returns the difference of two vectors.</summary>
			public static Vector3D Subtract(Vector3D A, Vector3D B) {
				return new Vector3D(A.X - B.X, A.Y - B.Y, A.Z - B.Z);
			}
		}
		/// <summary>Represents a 3D vector of System.Single coordinates.</summary>
		public struct Vector3Df {
			public float X;
			public float Y;
			public float Z;
			public Vector3Df(float X, float Y, float Z) {
				this.X = X;
				this.Y = Y;
				this.Z = Z;
			}
			public bool IsZero() {
				if (this.X != 0.0f) return false;
				if (this.Y != 0.0f) return false;
				if (this.Z != 0.0f) return false;
				return true;
			}
		}

		// colors
		/// <summary>Represents an RGB color with 8-bit precision per channel.</summary>
		internal struct ColorRGB {
			internal byte R;
			internal byte G;
			internal byte B;
			internal ColorRGB(byte R, byte G, byte B) {
				this.R = R;
				this.G = G;
				this.B = B;
			}
		}
		/// <summary>Represents an RGBA color with 8-bit precision per channel.</summary>
		internal struct ColorRGBA {
			internal byte R;
			internal byte G;
			internal byte B;
			internal byte A;
			internal ColorRGBA(byte R, byte G, byte B, byte A) {
				this.R = R;
				this.G = G;
				this.B = B;
				this.A = A;
			}
		}

		// vertices
		/// <summary>Represents a vertex consisting of 3D coordinates and 2D texture coordinates.</summary>
		internal struct Vertex {
			internal Vector3D Coordinates;
			internal Vector2Df TextureCoordinates;
			internal Vertex(double X, double Y, double Z) {
				this.Coordinates = new Vector3D(X, Y, Z);
				this.TextureCoordinates = new Vector2Df(0.0f, 0.0f);
			}
			internal Vertex(Vector3D Coordinates, Vector2Df TextureCoordinates) {
				this.Coordinates = Coordinates;
				this.TextureCoordinates = TextureCoordinates;
			}
			internal static bool Equals(Vertex A, Vertex B) {
				if (A.Coordinates.X != B.Coordinates.X | A.Coordinates.Y != B.Coordinates.Y | A.Coordinates.Z != B.Coordinates.Z) return false;
				if (A.TextureCoordinates.X != B.TextureCoordinates.X | A.TextureCoordinates.Y != B.TextureCoordinates.Y) return false;
				return true;
			}
			// operators
			public static bool operator ==(Vertex A, Vertex B) {
				if (A.Coordinates.X != B.Coordinates.X | A.Coordinates.Y != B.Coordinates.Y | A.Coordinates.Z != B.Coordinates.Z) return false;
				if (A.TextureCoordinates.X != B.TextureCoordinates.X | A.TextureCoordinates.Y != B.TextureCoordinates.Y) return false;
				return true;
			}
			public static bool operator !=(Vertex A, Vertex B) {
				if (A.Coordinates.X != B.Coordinates.X | A.Coordinates.Y != B.Coordinates.Y | A.Coordinates.Z != B.Coordinates.Z) return true;
				if (A.TextureCoordinates.X != B.TextureCoordinates.X | A.TextureCoordinates.Y != B.TextureCoordinates.Y) return true;
				return false;
			}
			public override int GetHashCode() {
				return base.GetHashCode();
			}
			public override bool Equals(object obj) {
				return base.Equals(obj);
			}
		}

		// mesh material
		/// <summary>Represents material properties.</summary>
		internal struct MeshMaterial {
			/// <summary>A bit mask combining constants of the MeshMaterial structure.</summary>
			internal byte Flags;
			internal ColorRGBA Color;
			internal ColorRGB TransparentColor;
			internal ColorRGB EmissiveColor;
			internal int DaytimeTextureIndex;
			internal int NighttimeTextureIndex;
			/// <summary>A value between 0 (daytime) and 255 (nighttime).</summary>
			internal byte DaytimeNighttimeBlend;
			internal MeshMaterialBlendMode BlendMode;
			/// <summary>A bit mask specifying the glow properties. Use GetGlowAttenuationData to create valid data for this field.</summary>
			internal ushort GlowAttenuationData;
			internal const int EmissiveColorMask = 1;
			internal const int TransparentColorMask = 2;
			// operators
			public static bool operator ==(MeshMaterial A, MeshMaterial B) {
				if (A.Flags != B.Flags) return false;
				if (A.Color.R != B.Color.R | A.Color.G != B.Color.G | A.Color.B != B.Color.B | A.Color.A != B.Color.A) return false;
				if (A.TransparentColor.R != B.TransparentColor.R | A.TransparentColor.G != B.TransparentColor.G | A.TransparentColor.B != B.TransparentColor.B) return false;
				if (A.EmissiveColor.R != B.EmissiveColor.R | A.EmissiveColor.G != B.EmissiveColor.G | A.EmissiveColor.B != B.EmissiveColor.B) return false;
				if (A.DaytimeTextureIndex != B.DaytimeTextureIndex) return false;
				if (A.NighttimeTextureIndex != B.NighttimeTextureIndex) return false;
				if (A.BlendMode != B.BlendMode) return false;
				if (A.GlowAttenuationData != B.GlowAttenuationData) return false;
				return true;
			}
			public static bool operator !=(MeshMaterial A, MeshMaterial B) {
				if (A.Flags != B.Flags) return true;
				if (A.Color.R != B.Color.R | A.Color.G != B.Color.G | A.Color.B != B.Color.B | A.Color.A != B.Color.A) return true;
				if (A.TransparentColor.R != B.TransparentColor.R | A.TransparentColor.G != B.TransparentColor.G | A.TransparentColor.B != B.TransparentColor.B) return true;
				if (A.EmissiveColor.R != B.EmissiveColor.R | A.EmissiveColor.G != B.EmissiveColor.G | A.EmissiveColor.B != B.EmissiveColor.B) return true;
				if (A.DaytimeTextureIndex != B.DaytimeTextureIndex) return true;
				if (A.NighttimeTextureIndex != B.NighttimeTextureIndex) return true;
				if (A.BlendMode != B.BlendMode) return true;
				if (A.GlowAttenuationData != B.GlowAttenuationData) return true;
				return false;
			}
			public override int GetHashCode() {
				return base.GetHashCode();
			}
			public override bool Equals(object obj) {
				return base.Equals(obj);
			}
		}
		internal enum MeshMaterialBlendMode : byte {
			Normal = 0,
			Additive = 1
		}
		
		// mesh face vertex
		/// <summary>Represents a reference to a vertex and the normal to be used for that vertex.</summary>
		internal struct MeshFaceVertex {
			/// <summary>A reference to an element in the Vertex array of the contained Mesh structure.</summary>
			internal ushort Index;
			/// <summary>The normal to be used at the vertex.</summary>
			internal Vector3Df Normal;
			internal MeshFaceVertex(int Index) {
				this.Index = (ushort)Index;
				this.Normal = new Vector3Df(0.0f, 0.0f, 0.0f);
			}
			internal MeshFaceVertex(int Index, Vector3Df Normal) {
				this.Index = (ushort)Index;
				this.Normal = Normal;
			}
			// operators
			public static bool operator ==(MeshFaceVertex A, MeshFaceVertex B) {
				if (A.Index != B.Index) return false;
				if (A.Normal.X != B.Normal.X) return false;
				if (A.Normal.Y != B.Normal.Y) return false;
				if (A.Normal.Z != B.Normal.Z) return false;
				return true;
			}
			public static bool operator !=(MeshFaceVertex A, MeshFaceVertex B) {
				if (A.Index != B.Index) return true;
				if (A.Normal.X != B.Normal.X) return true;
				if (A.Normal.Y != B.Normal.Y) return true;
				if (A.Normal.Z != B.Normal.Z) return true;
				return false;
			}
			public override int GetHashCode() {
				return base.GetHashCode();
			}
			public override bool Equals(object obj) {
				return base.Equals(obj);
			}
		}
		
		// mesh face
		/// <summary>Represents a face consisting of vertices and material attributes.</summary>
		internal struct MeshFace {
			internal MeshFaceVertex[] Vertices;
			/// <summary>A reference to an element in the Material array of the containing Mesh structure.</summary>
			internal ushort Material;
			/// <summary>A bit mask combining constants of the MeshFace structure.</summary>
			internal byte Flags;
			internal MeshFace(int[] Vertices) {
				this.Vertices = new MeshFaceVertex[Vertices.Length];
				for (int i = 0; i < Vertices.Length; i++) {
					this.Vertices[i] = new MeshFaceVertex(Vertices[i]);
				}
				this.Material = 0;
				this.Flags = 0;
			}
			internal void Flip() {
				if ((this.Flags & FaceTypeMask) == FaceTypeQuadStrip) {
					for (int i = 0; i < this.Vertices.Length; i += 2) {
						MeshFaceVertex x = this.Vertices[i];
						this.Vertices[i] = this.Vertices[i + 1];
						this.Vertices[i + 1] = x;
					}
				} else {
					int n = this.Vertices.Length;
					for (int i = 0; i < (n >> 1); i++) {
						MeshFaceVertex x = this.Vertices[i];
						this.Vertices[i] = this.Vertices[n - i - 1];
						this.Vertices[n - i - 1] = x;
					}
				}
			}
			internal const int FaceTypeMask = 7;
			internal const int FaceTypePolygon = 0;
			internal const int FaceTypeTriangles = 1;
			internal const int FaceTypeTriangleStrip = 2;
			internal const int FaceTypeQuads = 3;
			internal const int FaceTypeQuadStrip = 4;
			internal const int Face2Mask = 8;
		}
		
		// mesh
		/// <summary>Represents a mesh consisting of a series of vertices, faces and material properties.</summary>
		internal struct Mesh {
			internal Vertex[] Vertices;
			internal MeshMaterial[] Materials;
			internal MeshFace[] Faces;
			/// <summary>Creates a mesh consisting of one face, which is represented by individual vertices, and a color.</summary>
			/// <param name="Vertices">The vertices that make up one face.</param>
			/// <param name="Color">The color to be applied on the face.</param>
			internal Mesh(Vertex[] Vertices, ColorRGBA Color) {
				this.Vertices = Vertices;
				this.Materials = new MeshMaterial[1];
				this.Materials[0].Color = Color;
				this.Materials[0].DaytimeTextureIndex = -1;
				this.Materials[0].NighttimeTextureIndex = -1;
				this.Faces = new MeshFace[1];
				this.Faces[0].Material = 0;
				this.Faces[0].Vertices = new MeshFaceVertex[Vertices.Length];
				for (int i = 0; i < Vertices.Length; i++) {
					this.Faces[0].Vertices[i].Index = (ushort)i;
				}
			}
			/// <summary>Creates a mesh consisting of the specified vertices, faces and color.</summary>
			/// <param name="Vertices">The vertices used.</param>
			/// <param name="FaceVertices">A list of faces represented by a list of references to vertices.</param>
			/// <param name="Color">The color to be applied on all of the faces.</param>
			internal Mesh(Vertex[] Vertices, int[][] FaceVertices, ColorRGBA Color) {
				this.Vertices = Vertices;
				this.Materials = new MeshMaterial[1];
				this.Materials[0].Color = Color;
				this.Materials[0].DaytimeTextureIndex = -1;
				this.Materials[0].NighttimeTextureIndex = -1;
				this.Faces = new MeshFace[FaceVertices.Length];
				for (int i = 0; i < FaceVertices.Length; i++) {
					this.Faces[i] = new MeshFace(FaceVertices[i]);
				}
			}
		}

		// glow
		internal enum GlowAttenuationMode {
			None = 0,
			DivisionExponent2 = 1,
			DivisionExponent4 = 2,
		}
		/// <summary>Creates glow attenuation data from a half distance and a mode. The resulting value can be later passed to SplitGlowAttenuationData in order to reconstruct the parameters.</summary>
		/// <param name="HalfDistance">The distance at which the glow is at 50% of its full intensity. The value is clamped to the integer range from 1 to 4096. Values less than or equal to 0 disable glow attenuation.</param>
		/// <param name="Mode">The glow attenuation mode.</param>
		/// <returns>A System.UInt16 packed with the information about the half distance and glow attenuation mode.</returns>
		internal static ushort GetGlowAttenuationData(double HalfDistance, GlowAttenuationMode Mode) {
			if (HalfDistance <= 0.0 | Mode == GlowAttenuationMode.None) return 0;
			if (HalfDistance < 1.0) {
				HalfDistance = 1.0;
			} else if (HalfDistance > 4095.0) {
				HalfDistance = 4095.0;
			}
			return (ushort)((int)Math.Round(HalfDistance) | ((int)Mode << 12));
		}

		// display
		internal struct Background {
			internal int Texture;
			internal int Repetition;
			internal bool KeepAspectRatio;
			internal Background(int Texture, int Repetition, bool KeepAspectRatio) {
				this.Texture = Texture;
				this.Repetition = Repetition;
				this.KeepAspectRatio = KeepAspectRatio;
			}
		}
		internal enum CameraViewMode { Interior, InteriorLookAhead, Exterior, Track, FlyBy, FlyByZooming }
#pragma warning disable 0649
		internal static CameraViewMode CameraMode;

		// absolute camera
		internal static World.Vector3D AbsoluteCameraPosition;
#pragma warning restore 0649


		// ================================

		// cross
		internal static void Cross(double ax, double ay, double az, double bx, double by, double bz, out double cx, out double cy, out double cz) {
			cx = ay * bz - az * by;
			cy = az * bx - ax * bz;
			cz = ax * by - ay * bx;
		}

		// rotate
		internal static void Rotate(ref double px, ref double py, ref double pz, double dx, double dy, double dz, double cosa, double sina) {
			double t = 1.0 / Math.Sqrt(dx * dx + dy * dy + dz * dz);
			dx *= t; dy *= t; dz *= t;
			double oc = 1.0 - cosa;
			double x = (cosa + oc * dx * dx) * px + (oc * dx * dy - sina * dz) * py + (oc * dx * dz + sina * dy) * pz;
			double y = (cosa + oc * dy * dy) * py + (oc * dx * dy + sina * dz) * px + (oc * dy * dz - sina * dx) * pz;
			double z = (cosa + oc * dz * dz) * pz + (oc * dx * dz - sina * dy) * px + (oc * dy * dz + sina * dx) * py;
			px = x; py = y; pz = z;
		}
		internal static void Rotate(ref float px, ref float py, ref float pz, double dx, double dy, double dz, double cosa, double sina) {
			double t = 1.0 / Math.Sqrt(dx * dx + dy * dy + dz * dz);
			dx *= t; dy *= t; dz *= t;
			double oc = 1.0 - cosa;
			double x = (cosa + oc * dx * dx) * (double)px + (oc * dx * dy - sina * dz) * (double)py + (oc * dx * dz + sina * dy) * (double)pz;
			double y = (cosa + oc * dy * dy) * (double)py + (oc * dx * dy + sina * dz) * (double)px + (oc * dy * dz - sina * dx) * (double)pz;
			double z = (cosa + oc * dz * dz) * (double)pz + (oc * dx * dz - sina * dy) * (double)px + (oc * dy * dz + sina * dx) * (double)py;
			px = (float)x; py = (float)y; pz = (float)z;
		}
		internal static void Normalize(ref double x, ref double y, ref double z) {
			double t = x * x + y * y + z * z;
			if (t != 0.0) {
				t = 1.0 / Math.Sqrt(t);
				x *= t; y *= t; z *= t;
			}
		}

		// create normals
		internal static void CreateNormals(ref Mesh Mesh) {
			for (int i = 0; i < Mesh.Faces.Length; i++) {
				CreateNormals(ref Mesh, i);
			}
		}
		internal static void CreateNormals(ref Mesh Mesh, int FaceIndex) {
			if (Mesh.Faces[FaceIndex].Vertices.Length >= 3) {
				int i0 = (int)Mesh.Faces[FaceIndex].Vertices[0].Index;
				int i1 = (int)Mesh.Faces[FaceIndex].Vertices[1].Index;
				int i2 = (int)Mesh.Faces[FaceIndex].Vertices[2].Index;
				double ax = Mesh.Vertices[i1].Coordinates.X - Mesh.Vertices[i0].Coordinates.X;
				double ay = Mesh.Vertices[i1].Coordinates.Y - Mesh.Vertices[i0].Coordinates.Y;
				double az = Mesh.Vertices[i1].Coordinates.Z - Mesh.Vertices[i0].Coordinates.Z;
				double bx = Mesh.Vertices[i2].Coordinates.X - Mesh.Vertices[i0].Coordinates.X;
				double by = Mesh.Vertices[i2].Coordinates.Y - Mesh.Vertices[i0].Coordinates.Y;
				double bz = Mesh.Vertices[i2].Coordinates.Z - Mesh.Vertices[i0].Coordinates.Z;
				double nx = ay * bz - az * by;
				double ny = az * bx - ax * bz;
				double nz = ax * by - ay * bx;
				double t = nx * nx + ny * ny + nz * nz;
				if (t != 0.0) {
					t = 1.0 / Math.Sqrt(t);
					float mx = (float)(nx * t);
					float my = (float)(ny * t);
					float mz = (float)(nz * t);
					for (int j = 0; j < Mesh.Faces[FaceIndex].Vertices.Length; j++) {
						if (Mesh.Faces[FaceIndex].Vertices[j].Normal.IsZero()) {
							Mesh.Faces[FaceIndex].Vertices[j].Normal = new Vector3Df(mx, my, mz);
						}
					}
				} else {
					for (int j = 0; j < Mesh.Faces[FaceIndex].Vertices.Length; j++) {
						if (Mesh.Faces[FaceIndex].Vertices[j].Normal.IsZero()) {
							Mesh.Faces[FaceIndex].Vertices[j].Normal = new Vector3Df(0.0f, 1.0f, 0.0f);
						}
					}
				}
			}
		}

	}
}