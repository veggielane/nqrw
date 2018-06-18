using NQRW.Robotics;
using System.Collections.Generic;
using NQRW.Maths;

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

        public Angle CoxaOffset { get; set; } = Angle.Zero;
        public Angle FemurOffset { get; set; } = Angle.Zero;
        public Angle TibiaOffset { get; set; } = Angle.Zero;
        public Angle TarsusOffset { get; set; } = Angle.Zero;


        public bool CoxaInvert { get; set; } = false;
        public bool FemurInvert { get; set; } = false;
        public bool TibiaInvert { get; set; } = false;
        public bool TarsusInvert { get; set; } = false;


        public static LegSettings Default => new LegSettings(20, 77, 73, 90)
        {
            CoxaOffset = Angle.Zero,
            FemurOffset = Angle.FromDegrees(12.0),
            TibiaOffset = Angle.FromDegrees(70.0),
            TarsusOffset = Angle.FromDegrees(40.0)
        };
        public LegSettings(double coxaLength, double femurLength, double tibiaLength, double tarsusLength)
        {
            CoxaLength = coxaLength;
            FemurLength = femurLength;
            TibiaLength = tibiaLength;
            TarsusLength = tarsusLength;
        }
    }
}
