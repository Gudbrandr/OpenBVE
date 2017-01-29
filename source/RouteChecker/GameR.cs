// ╔═════════════════════════════════════════════════════════════╗
// ║ Game.cs for the Route Checker                               ║
// ╠═════════════════════════════════════════════════════════════╣
// ║ This file cannot be used in the openBVE main program.       ║
// ║ The file from the openBVE main program cannot be used here. ║
// ╚═════════════════════════════════════════════════════════════╝

using System;

namespace OpenBve {
	internal static class Game {

		// random numbers
		internal static Random Generator = new Random();

		// date and time
		internal static double SecondsSinceMidnight = 0.0;
		internal static double[] RouteUnitOfLength = new double[] { 1.0 };

		// fog
		internal struct Fog {
			internal float Start;
			internal float End;
			internal World.ColorRGB Color;
			internal double TrackPosition;
			internal Fog(float Start, float End, World.ColorRGB Color, double TrackPosition) {
				this.Start = Start;
				this.End = End;
				this.Color = Color;
				this.TrackPosition = TrackPosition;
			}
		}
		internal static Fog PreviousFog = new Fog(0.0f, 0.0f, new World.ColorRGB(128, 128, 128), 0.0);
		internal static Fog CurrentFog = new Fog(0.0f, 0.0f, new World.ColorRGB(128, 128, 128), 0.5);
		internal static Fog NextFog = new Fog(0.0f, 0.0f, new World.ColorRGB(128, 128, 128), 1.0);
		internal static float NoFogStart = 800.0f;
		internal static float NoFogEnd = 1600.0f;

		// route constants
		internal static string RouteComment = "";
		internal static string RouteImage = "";
		internal static double RouteAccelerationDueToGravity = 9.80665;
		internal static double RouteRailGauge = 1.435;
		internal static double RouteInitialAirPressure = 101325.0;
		internal static double RouteInitialAirTemperature = 293.15;
		internal static double RouteInitialElevation = 0.0;
		internal static double RouteSeaLevelAirPressure = 101325.0;
		internal static double RouteSeaLevelAirTemperature = 293.15;

		// game constants
		internal static double[] PrecedingTrainTimeDeltas;
		internal static double PrecedingTrainSpeedLimit;

		// startup
		internal enum TrainStartMode {
			ServiceBrakesAts = -1,
			EmergencyBrakesAts = 0,
			EmergencyBrakesNoAts = 1
		}
		internal static TrainStartMode TrainStart = TrainStartMode.EmergencyBrakesNoAts;
		internal static string TrainName = "";

		// ================================

		internal static void Reset() {
			// game
			Interface.ClearMessages();
			RouteComment = "";
			BogusPretrainInstructions = new BogusPretrainInstruction[] { };
			// renderer / sound
//			Renderer.Reset();
//			SoundManager.StopAllSounds(true);
			GC.Collect();
		}

		// ================================

		// stations
		internal struct StationStop {
		}
		internal enum SafetySystem {
			Any = -1,
			Ats = 0,
			Atc = 1
		}
		internal enum StationStopMode {
			AllStop = 0,
			AllPass = 1,
			PlayerStop = 2,
			PlayerPass = 3
		}
		internal enum StationType {
			Normal = 0,
			ChangeEnds = 1,
			Terminal = 2
		}
		internal struct Station {
			internal string Name;
			internal double ArrivalTime;
			internal int ArrivalSoundIndex;
			internal double DepartureTime;
			internal int DepartureSoundIndex;
			internal double StopTime;
			internal StationStopMode StopMode;
			internal StationType StationType;
			internal bool ForceStopSignal;
			internal bool OpenLeftDoors;
			internal bool OpenRightDoors;
			internal SafetySystem SafetySystem;
			internal StationStop[] Stops;
			internal double PassengerRatio;
			internal int TimetableDaytimeTexture;
			internal int TimetableNighttimeTexture;
			internal double DefaultTrackPosition;
		}
		internal static Station[] Stations = new Station[] { };

		// ================================

		// sections
		internal enum SectionType { ValueBased, IndexBased }
		internal struct SectionAspect {
			internal int Number;
			internal double Speed;
			internal SectionAspect(int Number, double Speed) {
				this.Number = Number;
				this.Speed = Speed;
			}
		}
		internal struct Section {
			internal TrainManager.Train[] Trains;
#pragma warning disable 0649
			internal SectionAspect[] Aspects;
			internal int CurrentAspect;
#pragma warning restore 0649
			internal void Enter(TrainManager.Train Train) {
				int n = this.Trains.Length;
				for (int i = 0; i < n; i++) {
					if (this.Trains[i] == Train) return;
				}
				Array.Resize<TrainManager.Train>(ref this.Trains, n + 1);
				this.Trains[n] = Train;
			}
			internal void Leave(TrainManager.Train Train) {
				int n = this.Trains.Length;
				for (int i = 0; i < n; i++) {
					if (this.Trains[i] == Train) {
						for (int j = i; j < n - 1; j++) {
							this.Trains[j] = this.Trains[j + 1];
						}
						Array.Resize<TrainManager.Train>(ref this.Trains, n - 1);
						return;
					}
				}
			}
			internal bool Exists(TrainManager.Train Train) {
				for (int i = 0; i < this.Trains.Length; i++) {
					if (this.Trains[i] == Train) return true;
				} return false;
			}
		}
		internal static Section[] Sections = new Section[] { };

		// buffers
		internal static double[] BufferTrackPositions = new double[] { };

		// ================================

		// marker
		internal static int[] MarkerTextures = new int[] { };
		internal static void AddMarker(int TextureIndex) {
			int n = MarkerTextures.Length;
			Array.Resize<int>(ref MarkerTextures, n + 1);
			MarkerTextures[n] = TextureIndex;
		}
		internal static void RemoveMarker(int TextureIndex) {
			int n = MarkerTextures.Length;
			for (int i = 0; i < n; i++) {
				if (MarkerTextures[i] == TextureIndex) {
					for (int j = i; j < n - 1; j++) {
						MarkerTextures[j] = MarkerTextures[j + 1];
					}
					Array.Resize<int>(ref MarkerTextures, n - 1);
					break;
				}
			}
		}

		// ================================

		// bogus pretrain
		internal struct BogusPretrainInstruction {
			internal double TrackPosition;
			internal double Time;
		}
		internal static BogusPretrainInstruction[] BogusPretrainInstructions = new BogusPretrainInstruction[] { };

		// ================================

	}
}