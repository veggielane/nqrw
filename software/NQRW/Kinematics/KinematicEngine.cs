using System.Collections.Generic;

namespace NQRW.Kinematics
{
    public class KinematicEngine : IKinematicEngine
    {
        public IBody Body { get; private set; }
        public IList<ILeg> Legs { get; private set; }

        public KinematicEngine(IBody body)
        {
            Body = body;
            Legs = new List<ILeg>();
        }

        public KinematicEngine(IBody body, params ILeg[] legs)
            : this(body)
        {
            Legs = legs;
        }


        public void Inverse()
        {
            foreach (var leg in Legs)
            {
                leg.Inverse(Body);
            }
        }
    }
}
