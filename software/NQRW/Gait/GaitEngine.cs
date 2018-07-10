using System;
using NQRW.Maths;
using NQRW.Robotics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NQRW.Kinematics;

namespace NQRW.Gait
{
    public enum WalkMode { Stop, Moving, Rotating }
    public class GaitEngine : IGaitEngine
    {
        public double StrideLength { get; }
        public Angle StrideAngle { get; }
        public double StrideHeight { get; }
        public int CurrentStep { get; private set; }
        public int StepCount { get; private set; }
        public int MicroSteps { get; private set; }

        public double Lerp { get; private set; }


        public Vector2 Heading { get; set; } = Vector2.Zero;
        public Angle Rotation { get; set; } = 0.0;


        public IDictionary<Leg, Vector3> Offsets { get; private set; } = new Dictionary<Leg, Vector3>
        {
            {Leg.LeftFront, Vector3.Zero},
            {Leg.LeftMiddle, Vector3.Zero},
            {Leg.LeftRear, Vector3.Zero},
            {Leg.RightFront, Vector3.Zero},
            {Leg.RightMiddle, Vector3.Zero},
            {Leg.RightRear, Vector3.Zero}
        };

        
        public Dictionary<Leg, int[]> Steps { get; }

        

        public WalkMode Mode { get; set; }

        public IList<Vector3> Positions => new List<Vector3>
        {
            Vector3.Zero,
            new Vector3(-Heading * StrideLength, 0),
            new Vector3(-Heading * StrideLength, StrideHeight / 2.0),
            new Vector3(0,0, StrideHeight),
            new Vector3(Heading * StrideLength, 0),
            new Vector3(Heading * StrideLength, StrideHeight / 2.0)
        };

        private Vector3 Rotations(ILeg leg, int step)
        {
            switch (step)
            {
                case 0:
                    return Vector3.Zero;
                case 1:
                    return CalculateRotationOffset(leg, -Angle.FromDegrees(Rotation.Degrees * StrideAngle.Degrees)).ToVector3(0);
                case 2:
                    return CalculateRotationOffset(leg, -Angle.FromDegrees(Rotation.Degrees * StrideAngle.Degrees)).ToVector3(StrideHeight / 2.0);
                case 3:
                    return new Vector3(0, 0, StrideHeight);
                case 4:
                    return CalculateRotationOffset(leg, Angle.FromDegrees(Rotation.Degrees * StrideAngle.Degrees)).ToVector3(0);
                case 5:
                    return CalculateRotationOffset(leg, Angle.FromDegrees(Rotation.Degrees * StrideAngle.Degrees)).ToVector3(StrideHeight / 2.0);
                default:
                    return Vector3.Zero;
            }
        }

        public IList<Vector3> Rotatdions() => new List<Vector3>
        {
            Vector3.Zero,
            new Vector3(-Heading / 2.0, 0),
            new Vector3(-Heading / 2.0, StrideHeight/2.0),
            new Vector3(0,0, StrideHeight),
            new Vector3(Heading / 2.0, 0),
            new Vector3(Heading / 2.0, StrideHeight / 2.0)
        };

        public GaitEngine()
        {
            StrideHeight = 70.0;
            StrideLength = 35.0;
            StrideAngle = Angle.FromDegrees(10.0);

            StepCount = 8;
            MicroSteps = 10;


            var groupA = new[] {3, 5, 4, 4, 0, 1, 1, 2};
            var groupB = new[] {0, 1, 1, 2, 3, 5, 4, 4};


            Steps = new Dictionary<Leg, int[]>
            {
                {Leg.LeftFront, groupA},
                {Leg.LeftMiddle, groupB},
                {Leg.LeftRear, groupA},
                {Leg.RightFront, groupB},
                {Leg.RightMiddle, groupA},
                {Leg.RightRear, groupB},
            };

            //Steps = new Dictionary<Leg, int[]>
            //{
            //    {Leg.LeftFront, new[]{0, 1, 1, 2, 3, 4, 5, 5 }},
            //    {Leg.LeftMiddle, new[]{ 3, 4, 5, 5, 0, 1, 1, 2 }},
            //    {Leg.LeftRear, new[]{0, 1, 1, 2, 3, 4, 5, 5 }},
            //    {Leg.RightFront, new[]{3, 4, 5, 5, 0, 1, 1, 2 }},
            //    {Leg.RightMiddle, new[]{0, 1, 1, 2, 3, 4, 5, 5 }},
            //    {Leg.RightRear, new[]{3, 4, 5, 5, 0, 1, 1, 2 }},
            //};
            Stop();
        }


        private void IncrementStep()
        {
            CurrentStep++;
            if (CurrentStep >= StepCount) CurrentStep = 0;
        }

        private Vector2 CalculateRotationOffset(ILeg leg , Angle a)
        {
            var end = leg.FootPosition.ToVector2();
            return end.Rotate(a) - end;
        }


        public void Update(Dictionary<Leg, ILeg> legs)
        {

            if (Mode == WalkMode.Moving)
            {
                var nextStep = CurrentStep + 1;
                if (nextStep >= StepCount) nextStep = 0;
                var positions = Steps.ToDictionary(kvp => kvp.Key, kvp => Positions[kvp.Value[CurrentStep]].Lerp(Positions[kvp.Value[nextStep]], Lerp));
                Lerp = Lerp + 1.0 / MicroSteps;
                if (Lerp > 1.0)
                {
                    Lerp = 1.0 / MicroSteps;
                    IncrementStep();
                }
                Offsets = positions;
            }
            else if (Mode == WalkMode.Rotating)
            {
                var nextStep = CurrentStep + 1;
                if (nextStep >= StepCount) nextStep = 0;

                var positions = Steps.ToDictionary(kvp => kvp.Key, kvp => Rotations(legs[kvp.Key], kvp.Value[CurrentStep]).Lerp(Rotations(legs[kvp.Key], kvp.Value[nextStep]), Lerp));
                Lerp = Lerp + 1.0 / MicroSteps;
                if (Lerp > 1.0)
                {
                    Lerp = 1.0 / MicroSteps;
                    IncrementStep();
                }
                Offsets = positions;
            }
            else
            {
                var numLegsOnFloor = Offsets.Count(p => p.Value.Z.NearlyEquals(0));
                if (numLegsOnFloor == 6)
                {
                    if(Offsets.Count(p => p.Value.Equals(Vector3.Zero)) == 6) return;

                    Offsets = Offsets.Select(pair =>
                    {
                        if (pair.Value.Equals(Vector3.Zero)) return pair;
                        return new KeyValuePair<Leg, Vector3>(pair.Key, new Vector3(0, 0, StrideHeight));
                    }).ToDictionary(p => p.Key,p => p.Value);
                }
                else if(numLegsOnFloor == 3)
                {
                    Offsets = Offsets.Select(pair => new KeyValuePair<Leg, Vector3>(pair.Key, Vector3.Zero)).ToDictionary(p => p.Key, p => p.Value);
                }
            }
        }


        public void Stop()
        {
            Mode = WalkMode.Stop;
            CurrentStep = 0;
            Lerp = 0;



            Offsets = new Dictionary<Leg, Vector3>
            {
                {Leg.LeftFront, Vector3.Zero},
                {Leg.LeftMiddle, Vector3.Zero},
                {Leg.LeftRear, Vector3.Zero},
                {Leg.RightFront, Vector3.Zero},
                {Leg.RightMiddle, Vector3.Zero},
                {Leg.RightRear, Vector3.Zero}
            };
        }
    }
}
