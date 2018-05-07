using System.Collections.Generic;

namespace NQRW.Kinematics
{
    public interface IKinematicEngine
    {
        IBody Body { get; }
        IList<ILeg> Legs { get; }
        void Inverse();
    }
}
