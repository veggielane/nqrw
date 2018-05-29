﻿using NQRW.Maths;
using NQRW.Robotics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public GaitEngine()
        {
            StrideHeight = 20.0;
            StrideLength = 20.0;

            StepCount = 8;
            MicroSteps = 20;
            CurrentStep = 0;
            Lerp = 0;

            Steps = new Dictionary<Leg, int[]>()
            {
                {Leg.LeftFront, new int[]{0, 1, 1, 2, 3, 4, 5, 5 }},
                {Leg.LeftMiddle, new int[]{ 3, 4, 5, 5, 0, 1, 1, 2 }},
                {Leg.LeftRear, new int[]{0, 1, 1, 2, 3, 4, 5, 5 }},
                {Leg.RightFront, new int[]{3, 4, 5, 5, 0, 1, 1, 2 }},
                {Leg.RightMiddle, new int[]{0, 1, 1, 2, 3, 4, 5, 5 }},
                {Leg.RightRear, new int[]{3, 4, 5, 5, 0, 1, 1, 2 }},
            };

        }

        private Vector3 PositionA => Vector3.Zero;
        private Vector3 PositionB => new Vector3(-Heading / 2.0, 0);
        private Vector3 PositionC => new Vector3(-Heading / 2.0, StrideHeight / 2.0);
        private Vector3 PositionD => new Vector3(0, 0, StrideHeight);
        private Vector3 PositionE => new Vector3(Heading / 2.0, 0);
        private Vector3 PositionF => new Vector3(Heading / 2.0, StrideHeight / 2.0);

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
