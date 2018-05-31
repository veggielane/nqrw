using NQRW.Robotics;
using System.Collections.Generic;

namespace NQRW.Settings
{
    public class RobotSettings
    {
        public Dictionary<Leg, LegSettings> Legs { get; } = new Dictionary<Leg, LegSettings>();

        public RobotSettings()
        {
            Legs.Add(Leg.LeftFront, LegSettings.Default);
            Legs.Add(Leg.LeftMiddle, LegSettings.Default);
            Legs.Add(Leg.LeftRear, LegSettings.Default);
            Legs.Add(Leg.RightFront, LegSettings.Default);
            Legs.Add(Leg.RightMiddle, LegSettings.Default);
            Legs.Add(Leg.RightRear, LegSettings.Default);
        }
    }

    public class LegSettings
    {
        public double CoxaLength { get; set; }
        public double FemurLength { get; set; }
        public double TibiaLength { get; set; }
        public double TarsusLength { get; set; }


        public static LegSettings Default => new LegSettings(20,76,76,96);
        public LegSettings(double coxaLength, double femurLength, double tibiaLength, double tarsusLength)
        {
            CoxaLength = coxaLength;
            FemurLength = femurLength;
            TibiaLength = tibiaLength;
            TarsusLength = tarsusLength;
        }
    }
}
