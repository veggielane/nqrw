using NQRW.Maths;
using NQRW.Robotics;
using System;
using System.Collections.Generic;

namespace NQRW.Kinematics
{
    public class KinematicEngine : IKinematicEngine
    {
        public Matrix4 BodyPosition => Matrix4.Translate(BodyX, BodyY, BodyZ) * Matrix4.RotateZ(BodyYaw) * Matrix4.RotateY(BodyPitch) * Matrix4.RotateX(BodyRoll);

        public Angle BodyRoll { get; set; } = Angle.FromDegrees(0);

        public Angle BodyPitch { get; set; } = Angle.FromDegrees(0);

        public Angle BodyYaw { get; set; } = Angle.FromDegrees(0);

        public double BodyX { get; set; } = 0;

        public double BodyY { get; set; } = 0;

        public double BodyZ { get; set; } = 0;

        public Dictionary<Leg, ILeg> Legs { get; set; }

        public KinematicEngine()
        {
            Legs = new Dictionary<Leg, ILeg>();
        }

        public KinematicEngine(Dictionary<Leg, ILeg> legs)
        {
            Legs = legs;
        }


        public void Update()
        {
            var body = BodyPosition;
            foreach (var leg in Legs.Values)
            {
                leg.Update(body);
            }
        }

        public void SetOffsets(Dictionary<Leg, Vector3> offsets)
        {
            foreach(var kvp in offsets)
            {
                Legs[kvp.Key].FootOffset = kvp.Value;
            }
        }
    }
}
