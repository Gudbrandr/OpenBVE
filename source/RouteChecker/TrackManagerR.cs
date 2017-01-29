// ╔═════════════════════════════════════════════════════════════╗
// ║ TrackManager.cs for the Route Checker                       ║
// ╠═════════════════════════════════════════════════════════════╣
// ║ This file cannot be used in the openBVE main program.       ║
// ║ The file from the openBVE main program cannot be used here. ║
// ╚═════════════════════════════════════════════════════════════╝

using System;

namespace OpenBve {
    internal static class TrackManager {

        // events
        internal enum EventTriggerType {
            None = 0,
            Camera = 1,
            FrontCarFrontAxle = 2,
            RearCarRearAxle = 3,
            OtherCarFrontAxle = 4,
            OtherCarRearAxle = 5
        }
        internal abstract class GeneralEvent {
            internal abstract void Trigger(int Direction, EventTriggerType TriggerType, TrainManager.Train Train, int CarIndex);
        }
        // transponder
		internal enum TransponderType {
			None = -1,
			SLong = 0,
			SN = 1,
			AccidentalDeparture = 2,
			AtsPPatternOrigin = 3,
			AtsPImmediateStop = 4,
			AtsPTemporarySpeedRestriction = -2,
			AtsPPermanentSpeedRestriction = -3
		}
        internal enum TransponderSpecialSection : int {
            NextRedSection = -2,
        }

        // ================================

        // track element
		internal struct TrackElement {
			internal double StartingTrackPosition;
			internal double CurveRadius;
			internal double CurveCant;
			internal double CurveCantTangent;
			internal double AdhesionMultiplier;
			internal double CsvRwAccuracyLevel;
			internal World.Vector3D WorldPosition;
			internal World.Vector3D WorldDirection;
			internal World.Vector3D WorldUp;
			internal World.Vector3D WorldSide;
			internal GeneralEvent[] Events;
			internal TrackElement(double StartingTrackPosition) {
				this.StartingTrackPosition = StartingTrackPosition;
				this.CurveRadius = 0.0;
				this.CurveCant = 0.0;
				this.CurveCantTangent = 0.0;
				this.AdhesionMultiplier = 1.0;
				this.CsvRwAccuracyLevel = 2.0;
				this.WorldPosition = new World.Vector3D(0.0, 0.0, 0.0);
				this.WorldDirection = new World.Vector3D(0.0, 0.0, 1.0);
				this.WorldUp = new World.Vector3D(0.0, 1.0, 0.0);
				this.WorldSide = new World.Vector3D(1.0, 0.0, 0.0);
				this.Events = new GeneralEvent[] { };
			}
		}

		// track follower
		internal struct TrackFollower {
#pragma warning disable 0649
			internal double TrackPosition;
			internal World.Vector3D WorldPosition;
			internal double CurveRadius;
			internal double CurveCant;
#pragma warning restore 0649
		}
		
    }
}