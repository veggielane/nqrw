using NQRW.Maths;
using NQRW.Messaging;
using NQRW.Messaging.Messages;
using NQRW.Robotics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using NQRW.Kinematics;

namespace NQRW.Gait
{
    public class GaitEngine : IGaitEngine
    {
        private readonly IMessageBus _bus;
        public double StrideHeight { get; private set; }
        public double StrideLength { get; private set; }
        public int CurrentStep { get; private set; }
        public int StepCount { get; private set; }
        public int MicroSteps { get; private set; }

        public double Lerp { get; private set; }

        public Vector2 Heading { get; set; } = Vector2.Zero;



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

        public bool Moving { get; private set; }

        public IList<Vector3> Positions => new List<Vector3>
        {
            Vector3.Zero,
            new Vector3(-Heading / 2.0, 0),
            new Vector3(-Heading / 2.0, StrideHeight/2.0),
            new Vector3(0,0, StrideHeight),
            new Vector3(Heading / 2.0, 0),
            new Vector3(Heading / 2.0, StrideHeight / 2.0)
        };

        public GaitEngine(IMessageBus bus)
        {
            _bus = bus;
            StrideHeight = 80.0;
            StrideLength = 20.0;

            StepCount = 8;
            MicroSteps = 15;



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

        public void Update()
        {

            if (Moving)
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

        public void Start()
        {
            Moving = true;
        }

        public void Stop()
        {
            Moving = false;
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
