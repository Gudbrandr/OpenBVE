// ╔═════════════════════════════════════════════════════════════╗
// ║ ObjectManager.cs for the Route Checker                      ║
// ╠═════════════════════════════════════════════════════════════╣
// ║ This file cannot be used in the openBVE main program.       ║
// ║ The file from the openBVE main program cannot be used here. ║
// ╚═════════════════════════════════════════════════════════════╝

using System;

namespace OpenBve {
	internal static class ObjectManager {

		// unified objects
		internal abstract class UnifiedObject { }

		// static objects
		internal class StaticObject : UnifiedObject {
#pragma warning disable 0649
			internal World.Mesh Mesh;
			/// <summary>The index to the Renderer.Object array, plus 1. The value of zero represents that the object is not currently shown by the renderer.</summary>
			internal int RendererIndex;
			/// <summary>The starting track position, for static objects only.</summary>
			internal float StartingDistance;
			/// <summary>The ending track position, for static objects only.</summary>
			internal float EndingDistance;
			/// <summary>The block mod group, for static objects only.</summary>
			internal short GroupIndex;
			/// <summary>Whether the object is dynamic, i.e. not static.</summary>
			internal bool Dynamic;
#pragma warning restore 0649
		}
		internal static StaticObject[] Objects = new StaticObject[16];
		internal static int ObjectsUsed;

		// animated objects
		internal class Damping {
			internal double NaturalFrequency;
			internal double NaturalTime;
			internal double DampingRatio;
			internal double NaturalDampingFrequency;
			internal double OriginalAngle;
			internal double OriginalDerivative;
			internal double TargetAngle;
			internal double CurrentAngle;
			internal double CurrentValue;
			internal double CurrentTimeDelta;
			internal Damping(double NaturalFrequency, double DampingRatio) {
				if (NaturalFrequency < 0.0) {
					throw new ArgumentException("NaturalFrequency must be non-negative in the constructor of the Damping class.");
				} else if (DampingRatio < 0.0) {
					throw new ArgumentException("DampingRatio must be non-negative in the constructor of the Damping class.");
				} else {
					this.NaturalFrequency = NaturalFrequency;
					this.NaturalTime = NaturalFrequency != 0.0 ? 1.0 / NaturalFrequency : 0.0;
					this.DampingRatio = DampingRatio;
					if (DampingRatio < 1.0) {
						this.NaturalDampingFrequency = NaturalFrequency * Math.Sqrt(1.0 - DampingRatio * DampingRatio);
					} else if (DampingRatio == 1.0) {
						this.NaturalDampingFrequency = NaturalFrequency;
					} else {
						this.NaturalDampingFrequency = NaturalFrequency * Math.Sqrt(DampingRatio * DampingRatio - 1.0);
					}
					this.OriginalAngle = 0.0;
					this.OriginalDerivative = 0.0;
					this.TargetAngle = 0.0;
					this.CurrentAngle = 0.0;
					this.CurrentValue = 1.0;
					this.CurrentTimeDelta = 0.0;
				}
			}
			internal Damping Clone() {
				return (Damping)this.MemberwiseClone();
			}
		}
		internal struct AnimatedObjectState {
			internal World.Vector3D Position;
			internal ObjectManager.StaticObject Object;
		}
		internal class AnimatedObject {
			// states
			internal AnimatedObjectState[] States;
			internal FunctionScripts.FunctionScript StateFunction;
			internal int CurrentState;
			internal World.Vector3D TranslateXDirection;
			internal World.Vector3D TranslateYDirection;
			internal World.Vector3D TranslateZDirection;
			internal FunctionScripts.FunctionScript TranslateXFunction;
			internal FunctionScripts.FunctionScript TranslateYFunction;
			internal FunctionScripts.FunctionScript TranslateZFunction;
			internal World.Vector3D RotateXDirection;
			internal World.Vector3D RotateYDirection;
			internal World.Vector3D RotateZDirection;
			internal FunctionScripts.FunctionScript RotateXFunction;
			internal FunctionScripts.FunctionScript RotateYFunction;
			internal FunctionScripts.FunctionScript RotateZFunction;
			internal Damping RotateXDamping;
			internal Damping RotateYDamping;
			internal Damping RotateZDamping;
			internal World.Vector2D TextureShiftXDirection;
			internal World.Vector2D TextureShiftYDirection;
			internal FunctionScripts.FunctionScript TextureShiftXFunction;
			internal FunctionScripts.FunctionScript TextureShiftYFunction;
			internal bool LEDClockwiseWinding;
			internal double LEDInitialAngle;
			internal double LEDLastAngle;
			/// <summary>If LEDFunction is used, an array of five vectors representing the bottom-left, up-left, up-right, bottom-right and center coordinates of the LED square, or a null reference otherwise.</summary>
			internal World.Vector3D[] LEDVectors;
			internal FunctionScripts.FunctionScript LEDFunction;
			internal double RefreshRate;
			internal double CurrentTrackZOffset;
			internal double SecondsSinceLastUpdate;
			internal int ObjectIndex;
			// methods
			internal bool IsFreeOfFunctions() {
				if (this.StateFunction != null) return false;
				if (this.TranslateXFunction != null | this.TranslateYFunction != null | this.TranslateZFunction != null) return false;
				if (this.RotateXFunction != null | this.RotateYFunction != null | this.RotateZFunction != null) return false;
				if (this.TextureShiftXFunction != null | this.TextureShiftYFunction != null) return false;
				if (this.LEDFunction != null) return false;
				return true;
			}
			internal AnimatedObject Clone() {
				AnimatedObject Result = new AnimatedObject();
				Result.States = new AnimatedObjectState[this.States.Length];
				for (int i = 0; i < this.States.Length; i++) {
					Result.States[i].Position = this.States[i].Position;
					Result.States[i].Object = CloneObject(this.States[i].Object);
				}
				Result.StateFunction = this.StateFunction == null ? null : this.StateFunction.Clone();
				Result.CurrentState = this.CurrentState;
				Result.TranslateZDirection = this.TranslateZDirection;
				Result.TranslateYDirection = this.TranslateYDirection;
				Result.TranslateXDirection = this.TranslateXDirection;
				Result.TranslateXFunction = this.TranslateXFunction == null ? null : this.TranslateXFunction.Clone();
				Result.TranslateYFunction = this.TranslateYFunction == null ? null : this.TranslateYFunction.Clone();
				Result.TranslateZFunction = this.TranslateZFunction == null ? null : this.TranslateZFunction.Clone();
				Result.RotateXDirection = this.RotateXDirection;
				Result.RotateYDirection = this.RotateYDirection;
				Result.RotateZDirection = this.RotateZDirection;
				Result.RotateXFunction = this.RotateXFunction == null ? null : this.RotateXFunction.Clone();
				Result.RotateXDamping = this.RotateXDamping == null ? null : this.RotateXDamping.Clone();
				Result.RotateYFunction = this.RotateYFunction == null ? null : this.RotateYFunction.Clone();
				Result.RotateYDamping = this.RotateYDamping == null ? null : this.RotateYDamping.Clone();
				Result.RotateZFunction = this.RotateZFunction == null ? null : this.RotateZFunction.Clone();
				Result.RotateZDamping = this.RotateZDamping == null ? null : this.RotateZDamping.Clone();
				Result.TextureShiftXDirection = this.TextureShiftXDirection;
				Result.TextureShiftYDirection = this.TextureShiftYDirection;
				Result.TextureShiftXFunction = this.TextureShiftXFunction == null ? null : this.TextureShiftXFunction.Clone();
				Result.TextureShiftYFunction = this.TextureShiftYFunction == null ? null : this.TextureShiftYFunction.Clone();
				Result.LEDClockwiseWinding = this.LEDClockwiseWinding;
				Result.LEDInitialAngle = this.LEDInitialAngle;
				Result.LEDLastAngle = this.LEDLastAngle;
				if (this.LEDVectors != null) {
					Result.LEDVectors = new World.Vector3D[this.LEDVectors.Length];
					for (int i = 0; i < this.LEDVectors.Length; i++) {
						Result.LEDVectors[i] = this.LEDVectors[i];
					}
				} else {
					Result.LEDVectors = null;
				}
				Result.LEDFunction = this.LEDFunction == null ? null : this.LEDFunction.Clone();
				Result.RefreshRate = this.RefreshRate;
				Result.CurrentTrackZOffset = 0.0;
				Result.SecondsSinceLastUpdate = 0.0;
				Result.ObjectIndex = -1;
				return Result;
			}
		}
		internal class AnimatedObjectCollection : UnifiedObject {
			internal AnimatedObject[] Objects;
		}

		// load object
		internal enum ObjectLoadMode { Normal, DontAllowUnloadOfTextures }
		internal static UnifiedObject LoadObject(string FileName, System.Text.Encoding Encoding, ObjectLoadMode LoadMode, bool PreserveVertices, bool ForceTextureRepeatX, bool ForceTextureRepeatY) {
			#if !DEBUG
			try {
				#endif
				if (!System.IO.Path.HasExtension(FileName)) {
					while (true) {
						string f;
						f = OpenBveApi.Path.CombineFile(System.IO.Path.GetDirectoryName(FileName), System.IO.Path.GetFileName(FileName) + ".x");
						if (System.IO.File.Exists(f)) {
							FileName = f;
							break;
						}
						f = OpenBveApi.Path.CombineFile(System.IO.Path.GetDirectoryName(FileName), System.IO.Path.GetFileName(FileName) + ".csv");
						if (System.IO.File.Exists(f)) {
							FileName = f;
							break;
						}
						f = OpenBveApi.Path.CombineFile(System.IO.Path.GetDirectoryName(FileName), System.IO.Path.GetFileName(FileName) + ".b3d");
						if (System.IO.File.Exists(f)) {
							FileName = f;
							break;
						}
						break;
					}
				}
				UnifiedObject Result;
				switch (System.IO.Path.GetExtension(FileName).ToLowerInvariant()) {
					case ".csv":
					case ".b3d":
						Result = CsvB3dObjectParser.ReadObject(FileName, Encoding, LoadMode, ForceTextureRepeatX, ForceTextureRepeatY);
						break;
					case ".x":
						Result = XObjectParser.ReadObject(FileName, Encoding, LoadMode, ForceTextureRepeatX, ForceTextureRepeatY);
						break;
					case ".animated":
						Result = AnimatedObjectParser.ReadObject(FileName, Encoding, LoadMode);
						break;
					default:
						Interface.AddMessage(Interface.MessageType.Error, false, "The file extension is not supported: " + FileName);
						return null;
				}
				OptimizeObject(Result, PreserveVertices);
				return Result;
				#if !DEBUG
			} catch (Exception ex) {
				Interface.AddMessage(Interface.MessageType.Error, true, "An unexpected error occured (" + ex.Message + ") while attempting to load the file " + FileName);
				return null;
			}
			#endif
		}
		internal static StaticObject LoadStaticObject(string FileName, System.Text.Encoding Encoding, ObjectLoadMode LoadMode, bool PreserveVertices, bool ForceTextureRepeatX, bool ForceTextureRepeatY) {
			#if !DEBUG
			try {
				#endif
				if (!System.IO.Path.HasExtension(FileName)) {
					while (true) {
						string f;
						f = OpenBveApi.Path.CombineFile(System.IO.Path.GetDirectoryName(FileName), System.IO.Path.GetFileName(FileName) + ".x");
						if (System.IO.File.Exists(f)) {
							FileName = f;
							break;
						}
						f = OpenBveApi.Path.CombineFile(System.IO.Path.GetDirectoryName(FileName), System.IO.Path.GetFileName(FileName) + ".csv");
						if (System.IO.File.Exists(f)) {
							FileName = f;
							break;
						}
						f = OpenBveApi.Path.CombineFile(System.IO.Path.GetDirectoryName(FileName), System.IO.Path.GetFileName(FileName) + ".b3d");
						if (System.IO.File.Exists(f)) {
							FileName = f;
							break;
						}
						break;
					}
				}
				StaticObject Result;
				switch (System.IO.Path.GetExtension(FileName).ToLowerInvariant()) {
					case ".csv":
					case ".b3d":
						Result = CsvB3dObjectParser.ReadObject(FileName, Encoding, LoadMode, ForceTextureRepeatX, ForceTextureRepeatY);
						break;
					case ".x":
						Result = XObjectParser.ReadObject(FileName, Encoding, LoadMode, ForceTextureRepeatX, ForceTextureRepeatY);
						break;
					case ".animated":
						Interface.AddMessage(Interface.MessageType.Error, false, "Tried to load an animated object even though only static objects are allowed: " + FileName);
						return null;
					default:
						Interface.AddMessage(Interface.MessageType.Error, false, "The file extension is not supported: " + FileName);
						return null;
				}
				OptimizeObject(Result, PreserveVertices);
				return Result;
				#if !DEBUG
			} catch (Exception ex) {
				Interface.AddMessage(Interface.MessageType.Error, true, "An unexpected error occured (" + ex.Message + ") while attempting to load the file " + FileName);
				return null;
			}
			#endif
		}

		// optimize object
		internal static void OptimizeObject(UnifiedObject Prototype, bool PreserveVertices) {
			if (Prototype is StaticObject) {
				StaticObject s = (StaticObject)Prototype;
				OptimizeObject(s, PreserveVertices);
			} else if (Prototype is AnimatedObjectCollection) {
				AnimatedObjectCollection a = (AnimatedObjectCollection)Prototype;
				for (int i = 0; i < a.Objects.Length; i++) {
					for (int j = 0; j < a.Objects[i].States.Length; j++) {
						OptimizeObject(a.Objects[i].States[j].Object, PreserveVertices);
					}
				}
			}
		}
		internal static void OptimizeObject(StaticObject Prototype, bool PreserveVertices) {
			if (Prototype == null) return;
			int v = Prototype.Mesh.Vertices.Length;
			int m = Prototype.Mesh.Materials.Length;
			int f = Prototype.Mesh.Faces.Length;
			if (f >= Interface.CurrentOptions.ObjectOptimizationBasicThreshold) return;
			// eliminate invalid faces and reduce incomplete faces
			for (int i = 0; i < f; i++) {
				int type = Prototype.Mesh.Faces[i].Flags & World.MeshFace.FaceTypeMask;
				bool keep;
				if (type == World.MeshFace.FaceTypeTriangles) {
					keep = Prototype.Mesh.Faces[i].Vertices.Length >= 3;
					if (keep) {
						int n = (Prototype.Mesh.Faces[i].Vertices.Length / 3) * 3;
						if (Prototype.Mesh.Faces[i].Vertices.Length != n) {
							Array.Resize<World.MeshFaceVertex>(ref Prototype.Mesh.Faces[i].Vertices, n);
						}
					}
				} else if (type == World.MeshFace.FaceTypeQuads) {
					keep = Prototype.Mesh.Faces[i].Vertices.Length >= 4;
					if (keep) {
						int n = Prototype.Mesh.Faces[i].Vertices.Length & ~3;
						if (Prototype.Mesh.Faces[i].Vertices.Length != n) {
							Array.Resize<World.MeshFaceVertex>(ref Prototype.Mesh.Faces[i].Vertices, n);
						}
					}
				} else if (type == World.MeshFace.FaceTypeQuadStrip) {
					keep = Prototype.Mesh.Faces[i].Vertices.Length >= 4;
					if (keep) {
						int n = Prototype.Mesh.Faces[i].Vertices.Length & ~1;
						if (Prototype.Mesh.Faces[i].Vertices.Length != n) {
							Array.Resize<World.MeshFaceVertex>(ref Prototype.Mesh.Faces[i].Vertices, n);
						}
					}
				} else {
					keep = Prototype.Mesh.Faces[i].Vertices.Length >= 3;
				}
				if (!keep) {
					for (int j = i; j < f - 1; j++) {
						Prototype.Mesh.Faces[j] = Prototype.Mesh.Faces[j + 1];
					}
					f--;
					i--;
				}
			}
			// eliminate unused vertices
			if (!PreserveVertices) {
				for (int i = 0; i < v; i++) {
					bool keep = false;
					for (int j = 0; j < f; j++) {
						for (int k = 0; k < Prototype.Mesh.Faces[j].Vertices.Length; k++) {
							if (Prototype.Mesh.Faces[j].Vertices[k].Index == i) {
								keep = true;
								break;
							}
						}
						if (keep) {
							break;
						}
					}
					if (!keep) {
						for (int j = 0; j < f; j++) {
							for (int k = 0; k < Prototype.Mesh.Faces[j].Vertices.Length; k++) {
								if (Prototype.Mesh.Faces[j].Vertices[k].Index > i) {
									Prototype.Mesh.Faces[j].Vertices[k].Index--;
								}
							}
						}
						for (int j = i; j < v - 1; j++) {
							Prototype.Mesh.Vertices[j] = Prototype.Mesh.Vertices[j + 1];
						}
						v--;
						i--;
					}
				}
			}
			// eliminate duplicate vertices
			if (!PreserveVertices) {
				for (int i = 0; i < v - 1; i++) {
					for (int j = i + 1; j < v; j++) {
						if (Prototype.Mesh.Vertices[i] == Prototype.Mesh.Vertices[j]) {
							for (int k = 0; k < f; k++) {
								for (int h = 0; h < Prototype.Mesh.Faces[k].Vertices.Length; h++) {
									if (Prototype.Mesh.Faces[k].Vertices[h].Index == j) {
										Prototype.Mesh.Faces[k].Vertices[h].Index = (ushort)i;
									} else if (Prototype.Mesh.Faces[k].Vertices[h].Index > j) {
										Prototype.Mesh.Faces[k].Vertices[h].Index--;
									}
								}
							}
							for (int k = j; k < v - 1; k++) {
								Prototype.Mesh.Vertices[k] = Prototype.Mesh.Vertices[k + 1];
							}
							v--;
							j--;
						}
					}
				}
			}
			// eliminate unused materials
			bool[] materialUsed = new bool[m];
			for (int i = 0; i < f; i++) {
				materialUsed[Prototype.Mesh.Faces[i].Material] = true;
			}
			for (int i = 0; i < m; i++) {
				if (!materialUsed[i]) {
					for (int j = 0; j < f; j++) {
						if (Prototype.Mesh.Faces[j].Material > i) {
							Prototype.Mesh.Faces[j].Material--;
						}
					}
					for (int j = i; j < m - 1; j++) {
						Prototype.Mesh.Materials[j] = Prototype.Mesh.Materials[j + 1];
						materialUsed[j] = materialUsed[j + 1];
					}
					m--;
					i--;
				}
			}
			// eliminate duplicate materials
			for (int i = 0; i < m - 1; i++) {
				for (int j = i + 1; j < m; j++) {
					if (Prototype.Mesh.Materials[i] == Prototype.Mesh.Materials[j]) {
						for (int k = 0; k < f; k++) {
							if (Prototype.Mesh.Faces[k].Material == j) {
								Prototype.Mesh.Faces[k].Material = (ushort)i;
							} else if (Prototype.Mesh.Faces[k].Material > j) {
								Prototype.Mesh.Faces[k].Material--;
							}
						}
						for (int k = j; k < m - 1; k++) {
							Prototype.Mesh.Materials[k] = Prototype.Mesh.Materials[k + 1];
						}
						m--;
						j--;
					}
				}
			}
			// structure optimization
			if (!PreserveVertices & f < Interface.CurrentOptions.ObjectOptimizationFullThreshold) {
				// create TRIANGLES and QUADS from POLYGON
				for (int i = 0; i < f; i++) {
					int type = Prototype.Mesh.Faces[i].Flags & World.MeshFace.FaceTypeMask;
					if (type == World.MeshFace.FaceTypePolygon) {
						if (Prototype.Mesh.Faces[i].Vertices.Length == 3) {
							unchecked {
								Prototype.Mesh.Faces[i].Flags &= (byte)~World.MeshFace.FaceTypeMask;
								Prototype.Mesh.Faces[i].Flags |= World.MeshFace.FaceTypeTriangles;
							}
						} else if (Prototype.Mesh.Faces[i].Vertices.Length == 4) {
							unchecked {
								Prototype.Mesh.Faces[i].Flags &= (byte)~World.MeshFace.FaceTypeMask;
								Prototype.Mesh.Faces[i].Flags |= World.MeshFace.FaceTypeQuads;
							}
						}
					}
				}
				// decomposit TRIANGLES and QUADS
				for (int i = 0; i < f; i++) {
					int type = Prototype.Mesh.Faces[i].Flags & World.MeshFace.FaceTypeMask;
					if (type == World.MeshFace.FaceTypeTriangles) {
						if (Prototype.Mesh.Faces[i].Vertices.Length > 3) {
							int n = (Prototype.Mesh.Faces[i].Vertices.Length - 3) / 3;
							while (f + n > Prototype.Mesh.Faces.Length) {
								Array.Resize<World.MeshFace>(ref Prototype.Mesh.Faces, Prototype.Mesh.Faces.Length << 1);
							}
							for (int j = 0; j < n; j++) {
								Prototype.Mesh.Faces[f + j].Vertices = new World.MeshFaceVertex[3];
								for (int k = 0; k < 3; k++) {
									Prototype.Mesh.Faces[f + j].Vertices[k] = Prototype.Mesh.Faces[i].Vertices[3 + 3 * j + k];
								}
								Prototype.Mesh.Faces[f + j].Material = Prototype.Mesh.Faces[i].Material;
								Prototype.Mesh.Faces[f + j].Flags = Prototype.Mesh.Faces[i].Flags;
								unchecked {
									Prototype.Mesh.Faces[i].Flags &= (byte)~World.MeshFace.FaceTypeMask;
									Prototype.Mesh.Faces[i].Flags |= World.MeshFace.FaceTypeTriangles;
								}
							}
							Array.Resize<World.MeshFaceVertex>(ref Prototype.Mesh.Faces[i].Vertices, 3);
							f += n;
						}
					} else if (type == World.MeshFace.FaceTypeQuads) {
						if (Prototype.Mesh.Faces[i].Vertices.Length > 4) {
							int n = (Prototype.Mesh.Faces[i].Vertices.Length - 4) >> 2;
							while (f + n > Prototype.Mesh.Faces.Length) {
								Array.Resize<World.MeshFace>(ref Prototype.Mesh.Faces, Prototype.Mesh.Faces.Length << 1);
							}
							for (int j = 0; j < n; j++) {
								Prototype.Mesh.Faces[f + j].Vertices = new World.MeshFaceVertex[4];
								for (int k = 0; k < 4; k++) {
									Prototype.Mesh.Faces[f + j].Vertices[k] = Prototype.Mesh.Faces[i].Vertices[4 + 4 * j + k];
								}
								Prototype.Mesh.Faces[f + j].Material = Prototype.Mesh.Faces[i].Material;
								Prototype.Mesh.Faces[f + j].Flags = Prototype.Mesh.Faces[i].Flags;
								unchecked {
									Prototype.Mesh.Faces[i].Flags &= (byte)~World.MeshFace.FaceTypeMask;
									Prototype.Mesh.Faces[i].Flags |= World.MeshFace.FaceTypeQuads;
								}
							}
							Array.Resize<World.MeshFaceVertex>(ref Prototype.Mesh.Faces[i].Vertices, 4);
							f += n;
						}
					}
				}
				// optimize for TRIANGLE_STRIP
				int index = -1;
				while (true) {
					// add TRIANGLES to TRIANGLE_STRIP
					for (int i = 0; i < f; i++) {
						if (index == i | index == -1) {
							int type = Prototype.Mesh.Faces[i].Flags & World.MeshFace.FaceTypeMask;
							if (type == World.MeshFace.FaceTypeTriangleStrip) {
								int face = Prototype.Mesh.Faces[i].Flags & World.MeshFace.Face2Mask;
								for (int j = 0; j < f; j++) {
									int type2 = Prototype.Mesh.Faces[j].Flags & World.MeshFace.FaceTypeMask;
									int face2 = Prototype.Mesh.Faces[j].Flags & World.MeshFace.Face2Mask;
									if (type2 == World.MeshFace.FaceTypeTriangles & face == face2) {
										if (Prototype.Mesh.Faces[i].Material == Prototype.Mesh.Faces[j].Material) {
											bool keep = true;
											for (int k = 0; k < 3; k++) {
												int l = (k + 1) % 3;
												int n = Prototype.Mesh.Faces[i].Vertices.Length;
												if (Prototype.Mesh.Faces[i].Vertices[0] == Prototype.Mesh.Faces[j].Vertices[k] & Prototype.Mesh.Faces[i].Vertices[1] == Prototype.Mesh.Faces[j].Vertices[l]) {
													Array.Resize<World.MeshFaceVertex>(ref Prototype.Mesh.Faces[i].Vertices, n + 1);
													for (int h = n; h >= 1; h--) {
														Prototype.Mesh.Faces[i].Vertices[h] = Prototype.Mesh.Faces[i].Vertices[h - 1];
													}
													Prototype.Mesh.Faces[i].Vertices[0] = Prototype.Mesh.Faces[j].Vertices[(k + 2) % 3];
													keep = false;
												} else if (Prototype.Mesh.Faces[i].Vertices[n - 1] == Prototype.Mesh.Faces[j].Vertices[l] & Prototype.Mesh.Faces[i].Vertices[n - 2] == Prototype.Mesh.Faces[j].Vertices[k]) {
													Array.Resize<World.MeshFaceVertex>(ref Prototype.Mesh.Faces[i].Vertices, n + 1);
													Prototype.Mesh.Faces[i].Vertices[n] = Prototype.Mesh.Faces[j].Vertices[(k + 2) % 3];
													keep = false;
												}
												if (!keep) {
													break;
												}
											}
											if (!keep) {
												for (int k = j; k < f - 1; k++) {
													Prototype.Mesh.Faces[k] = Prototype.Mesh.Faces[k + 1];
												}
												if (j < i) {
													i--;
												}
												f--;
												j--;
											}
										}
									}
								}
							}
						}
					}
					// join TRIANGLES into new TRIANGLE_STRIP
					index = -1;
					for (int i = 0; i < f - 1; i++) {
						int type = Prototype.Mesh.Faces[i].Flags & World.MeshFace.FaceTypeMask;
						if (type == World.MeshFace.FaceTypeTriangles) {
							int face = Prototype.Mesh.Faces[i].Flags & World.MeshFace.Face2Mask;
							for (int j = i + 1; j < f; j++) {
								int type2 = Prototype.Mesh.Faces[j].Flags & World.MeshFace.FaceTypeMask;
								int face2 = Prototype.Mesh.Faces[j].Flags & World.MeshFace.Face2Mask;
								if (type2 == World.MeshFace.FaceTypeTriangles & face == face2) {
									if (Prototype.Mesh.Faces[i].Material == Prototype.Mesh.Faces[j].Material) {
										for (int ik = 0; ik < 3; ik++) {
											int il = (ik + 1) % 3;
											for (int jk = 0; jk < 3; jk++) {
												int jl = (jk + 1) % 3;
												if (Prototype.Mesh.Faces[i].Vertices[ik] == Prototype.Mesh.Faces[j].Vertices[jl] & Prototype.Mesh.Faces[i].Vertices[il] == Prototype.Mesh.Faces[j].Vertices[jk]) {
													unchecked {
														Prototype.Mesh.Faces[i].Flags &= (byte)~World.MeshFace.FaceTypeMask;
														Prototype.Mesh.Faces[i].Flags |= World.MeshFace.FaceTypeTriangleStrip;
													}
													Prototype.Mesh.Faces[i].Vertices = new World.MeshFaceVertex[] {
														Prototype.Mesh.Faces[i].Vertices[(ik + 2) % 3],
														Prototype.Mesh.Faces[i].Vertices[ik],
														Prototype.Mesh.Faces[i].Vertices[il],
														Prototype.Mesh.Faces[j].Vertices[(jk + 2) % 3]
													};
													for (int k = j; k < f - 1; k++) {
														Prototype.Mesh.Faces[k] = Prototype.Mesh.Faces[k + 1];
													}
													f--;
													index = i;
													break;
												}
											}
											if (index >= 0) break;
										}
									}
								}
								if (index >= 0) break;
							}
						}
					}
					if (index == -1) break;
				}
				// optimize for QUAD_STRIP
				index = -1;
				while (true) {
					// add QUADS to QUAD_STRIP
					for (int i = 0; i < f; i++) {
						if (index == i | index == -1) {
							int type = Prototype.Mesh.Faces[i].Flags & World.MeshFace.FaceTypeMask;
							if (type == World.MeshFace.FaceTypeQuadStrip) {
								int face = Prototype.Mesh.Faces[i].Flags & World.MeshFace.Face2Mask;
								for (int j = 0; j < f; j++) {
									int type2 = Prototype.Mesh.Faces[j].Flags & World.MeshFace.FaceTypeMask;
									int face2 = Prototype.Mesh.Faces[j].Flags & World.MeshFace.Face2Mask;
									if (type2 == World.MeshFace.FaceTypeQuads & face == face2) {
										if (Prototype.Mesh.Faces[i].Material == Prototype.Mesh.Faces[j].Material) {
											bool keep = true;
											for (int k = 0; k < 4; k++) {
												int l = (k + 1) & 3;
												int n = Prototype.Mesh.Faces[i].Vertices.Length;
												if (Prototype.Mesh.Faces[i].Vertices[0] == Prototype.Mesh.Faces[j].Vertices[l] & Prototype.Mesh.Faces[i].Vertices[1] == Prototype.Mesh.Faces[j].Vertices[k]) {
													Array.Resize<World.MeshFaceVertex>(ref Prototype.Mesh.Faces[i].Vertices, n + 2);
													for (int h = n + 1; h >= 2; h--) {
														Prototype.Mesh.Faces[i].Vertices[h] = Prototype.Mesh.Faces[i].Vertices[h - 2];
													}
													Prototype.Mesh.Faces[i].Vertices[0] = Prototype.Mesh.Faces[j].Vertices[(k + 2) & 3];
													Prototype.Mesh.Faces[i].Vertices[1] = Prototype.Mesh.Faces[j].Vertices[(k + 3) & 3];
													keep = false;
												} else if (Prototype.Mesh.Faces[i].Vertices[n - 1] == Prototype.Mesh.Faces[j].Vertices[l] & Prototype.Mesh.Faces[i].Vertices[n - 2] == Prototype.Mesh.Faces[j].Vertices[k]) {
													Array.Resize<World.MeshFaceVertex>(ref Prototype.Mesh.Faces[i].Vertices, n + 2);
													Prototype.Mesh.Faces[i].Vertices[n] = Prototype.Mesh.Faces[j].Vertices[(k + 3) & 3];
													Prototype.Mesh.Faces[i].Vertices[n + 1] = Prototype.Mesh.Faces[j].Vertices[(k + 2) & 3];
													keep = false;
												}
												if (!keep) {
													break;
												}
											}
											if (!keep) {
												for (int k = j; k < f - 1; k++) {
													Prototype.Mesh.Faces[k] = Prototype.Mesh.Faces[k + 1];
												}
												if (j < i) {
													i--;
												}
												f--;
												j--;
											}
										}
									}
								}
							}
						}
					}
					// join QUADS into new QUAD_STRIP
					index = -1;
					for (int i = 0; i < f - 1; i++) {
						int type = Prototype.Mesh.Faces[i].Flags & World.MeshFace.FaceTypeMask;
						if (type == World.MeshFace.FaceTypeQuads) {
							int face = Prototype.Mesh.Faces[i].Flags & World.MeshFace.Face2Mask;
							for (int j = i + 1; j < f; j++) {
								int type2 = Prototype.Mesh.Faces[j].Flags & World.MeshFace.FaceTypeMask;
								int face2 = Prototype.Mesh.Faces[j].Flags & World.MeshFace.Face2Mask;
								if (type2 == World.MeshFace.FaceTypeQuads & face == face2) {
									if (Prototype.Mesh.Faces[i].Material == Prototype.Mesh.Faces[j].Material) {
										for (int ik = 0; ik < 4; ik++) {
											int il = (ik + 1) & 3;
											for (int jk = 0; jk < 4; jk++) {
												int jl = (jk + 1) & 3;
												if (Prototype.Mesh.Faces[i].Vertices[ik] == Prototype.Mesh.Faces[j].Vertices[jl] & Prototype.Mesh.Faces[i].Vertices[il] == Prototype.Mesh.Faces[j].Vertices[jk]) {
													unchecked {
														Prototype.Mesh.Faces[i].Flags &= (byte)~World.MeshFace.FaceTypeMask;
														Prototype.Mesh.Faces[i].Flags |= World.MeshFace.FaceTypeQuadStrip;
													}
													Prototype.Mesh.Faces[i].Vertices = new World.MeshFaceVertex[] {
														Prototype.Mesh.Faces[i].Vertices[(ik + 2) & 3],
														Prototype.Mesh.Faces[i].Vertices[(ik + 3) & 3],
														Prototype.Mesh.Faces[i].Vertices[il],
														Prototype.Mesh.Faces[i].Vertices[ik],
														Prototype.Mesh.Faces[j].Vertices[(jk + 3) & 3],
														Prototype.Mesh.Faces[j].Vertices[(jk + 2) & 3]
													};
													for (int k = j; k < f - 1; k++) {
														Prototype.Mesh.Faces[k] = Prototype.Mesh.Faces[k + 1];
													}
													f--;
													index = i;
													break;
												}
											}
											if (index >= 0) break;
										}
									}
								}
								if (index >= 0) break;
							}
						}
					}
					if (index == -1) break;
				}
				// join TRIANGLES, join QUADS
				for (int i = 0; i < f - 1; i++) {
					int type = Prototype.Mesh.Faces[i].Flags & World.MeshFace.FaceTypeMask;
					if (type == World.MeshFace.FaceTypeTriangles | type == World.MeshFace.FaceTypeQuads) {
						int face = Prototype.Mesh.Faces[i].Flags & World.MeshFace.Face2Mask;
						for (int j = i + 1; j < f; j++) {
							int type2 = Prototype.Mesh.Faces[j].Flags & World.MeshFace.FaceTypeMask;
							int face2 = Prototype.Mesh.Faces[j].Flags & World.MeshFace.Face2Mask;
							if (type == type2 & face == face2) {
								if (Prototype.Mesh.Faces[i].Material == Prototype.Mesh.Faces[j].Material) {
									int n = Prototype.Mesh.Faces[i].Vertices.Length;
									Array.Resize<World.MeshFaceVertex>(ref Prototype.Mesh.Faces[i].Vertices, n + Prototype.Mesh.Faces[j].Vertices.Length);
									for (int k = 0; k < Prototype.Mesh.Faces[j].Vertices.Length; k++) {
										Prototype.Mesh.Faces[i].Vertices[n + k] = Prototype.Mesh.Faces[j].Vertices[k];
									}
									for (int k = j; k < f - 1; k++) {
										Prototype.Mesh.Faces[k] = Prototype.Mesh.Faces[k + 1];
									}
									f--;
									j--;
								}
							}
						}
					}
				}
			}
			// finalize arrays
			if (v != Prototype.Mesh.Vertices.Length) {
				Array.Resize<World.Vertex>(ref Prototype.Mesh.Vertices, v);
			}
			if (m != Prototype.Mesh.Materials.Length) {
				Array.Resize<World.MeshMaterial>(ref Prototype.Mesh.Materials, m);
			}
			if (f != Prototype.Mesh.Faces.Length) {
				Array.Resize<World.MeshFace>(ref Prototype.Mesh.Faces, f);
			}
		}

		private static double Mod(double a, double b) {
			return a - b * Math.Floor(a / b);
		}

		// create dynamic object
		internal static int CreateDynamicObject() {
			int a = ObjectsUsed;
			if (a >= Objects.Length) {
				Array.Resize<StaticObject>(ref Objects, Objects.Length << 1);
			}
			Objects[a] = new StaticObject();
			Objects[a].Mesh.Faces = new World.MeshFace[] { };
			Objects[a].Mesh.Materials = new World.MeshMaterial[] { };
			Objects[a].Mesh.Vertices = new World.Vertex[] { };
			Objects[a].Dynamic = true;
			ObjectsUsed++;
			return a;
		}

		// clone object
		internal static StaticObject CloneObject(StaticObject Prototype) {
			if (Prototype == null) return null;
			return CloneObject(Prototype, -1, -1);
		}
		internal static StaticObject CloneObject(StaticObject Prototype, int DaytimeTextureIndex, int NighttimeTextureIndex) {
			if (Prototype == null) return null;
			StaticObject Result = new StaticObject();
			Result.StartingDistance = Prototype.StartingDistance;
			Result.EndingDistance = Prototype.EndingDistance;
			Result.Dynamic = Prototype.Dynamic;
			// vertices
			Result.Mesh.Vertices = new World.Vertex[Prototype.Mesh.Vertices.Length];
			for (int j = 0; j < Prototype.Mesh.Vertices.Length; j++) {
				Result.Mesh.Vertices[j] = Prototype.Mesh.Vertices[j];
			}
			// faces
			Result.Mesh.Faces = new World.MeshFace[Prototype.Mesh.Faces.Length];
			for (int j = 0; j < Prototype.Mesh.Faces.Length; j++) {
				Result.Mesh.Faces[j].Flags = Prototype.Mesh.Faces[j].Flags;
				Result.Mesh.Faces[j].Material = Prototype.Mesh.Faces[j].Material;
				Result.Mesh.Faces[j].Vertices = new World.MeshFaceVertex[Prototype.Mesh.Faces[j].Vertices.Length];
				for (int k = 0; k < Prototype.Mesh.Faces[j].Vertices.Length; k++) {
					Result.Mesh.Faces[j].Vertices[k] = Prototype.Mesh.Faces[j].Vertices[k];
				}
			}
			// materials
			Result.Mesh.Materials = new World.MeshMaterial[Prototype.Mesh.Materials.Length];
			for (int j = 0; j < Prototype.Mesh.Materials.Length; j++) {
				Result.Mesh.Materials[j] = Prototype.Mesh.Materials[j];
				if (DaytimeTextureIndex >= 0) {
					Result.Mesh.Materials[j].DaytimeTextureIndex = DaytimeTextureIndex;
				}
				if (NighttimeTextureIndex >= 0) {
					Result.Mesh.Materials[j].NighttimeTextureIndex = NighttimeTextureIndex;
				}
			}
			return Result;
		}

	}
}