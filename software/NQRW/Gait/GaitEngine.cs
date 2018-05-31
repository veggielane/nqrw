using NQRW.Maths;
using NQRW.Messaging;
using NQRW.Messaging.Messages;
using NQRW.Robotics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace NQRW.Gait
{
    public class GaitEngine : IGaitEngine
    {
        public double StrideHeight { get; private set; }
        public double StrideLength { get; private set; }
        public int CurrentStep { get; private set; }
        public int StepCount { get; private set; }
        public int MicroSteps { get; private set; }

        public double Lerp { get; private set; }

        public Vector2 Heading { get; set; } = Vector2.Zero;


        public IList<Vector3> Positions => new List<Vector3>
        {
            Vector3.Zero,
            new Vector3(-Heading / 2.0, 0),
            new Vector3(-Heading / 2.0, StrideHeight/2.0),
            new Vector3(0,0, StrideHeight),
            new Vector3(Heading / 2.0, 0),
            new Vector3(Heading / 2.0, StrideHeight / 2.0)
        };

        public Dictionary<Leg, int[]> Steps { get; private set; }

        public bool Moving { get; set; } = false;

        public GaitEngine(IMessageBus bus)
        {
            StrideHeight = 20.0;
            StrideLength = 20.0;

            StepCount = 8;
            MicroSteps = 20;
            CurrentStep = 0;
            Lerp = 0;

            Steps = new Dictionary<Leg, int[]>()
            {
                {Leg.LeftFront, new[]{0, 1, 1, 2, 3, 4, 5, 5 }},
                {Leg.LeftMiddle, new[]{ 3, 4, 5, 5, 0, 1, 1, 2 }},
                {Leg.LeftRear, new[]{0, 1, 1, 2, 3, 4, 5, 5 }},
                {Leg.RightFront, new[]{3, 4, 5, 5, 0, 1, 1, 2 }},
                {Leg.RightMiddle, new[]{0, 1, 1, 2, 3, 4, 5, 5 }},
                {Leg.RightRear, new[]{3, 4, 5, 5, 0, 1, 1, 2 }},
            };
  
            bus.Messages.OfType<HeadingEvent>().Subscribe(e=> Heading = e.Heading);
        }


        private void IncrementStep()
        {
            CurrentStep++;
            if (CurrentStep >= StepCount) CurrentStep = 0;
        }

        public Dictionary<Leg, Vector3> Update()
        {
            var nextStep = CurrentStep + 1;
            if (nextStep >= StepCount) nextStep = 0;
            var positions = Steps.ToDictionary(kvp => kvp.Key, kvp => {

                return Positions[kvp.Value[CurrentStep]].Lerp(Positions[kvp.Value[nextStep]], Lerp);
            });
            if (Moving)
            {
                Lerp = Lerp + 1.0 / MicroSteps;
                if (Lerp > 1.0)
                {
                    Lerp = 1.0 / MicroSteps;
                    IncrementStep();
                }
            }
            return positions;
        }
    }
}
