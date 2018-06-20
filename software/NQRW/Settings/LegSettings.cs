using System.Runtime.Serialization;
using Newtonsoft.Json;
using NQRW.Maths;

namespace NQRW.Settings
{
    [DataContract]
    public class LegSettings
    {
        [DataMember]
        public double CoxaLength { get; set; }
        [DataMember]
        public double FemurLength { get; set; }
        [DataMember]
        public double TibiaLength { get; set; }
        [DataMember]
        public double TarsusLength { get; set; }

        [JsonConverter(typeof(DegreeConverter))]
        [DataMember]
        public Angle CoxaOffset { get; set; } = Angle.Zero;
        [JsonConverter(typeof(DegreeConverter))]
        [DataMember]
        public Angle FemurOffset { get; set; } = Angle.Zero;
        [JsonConverter(typeof(DegreeConverter))]
        [DataMember]
        public Angle TibiaOffset { get; set; } = Angle.Zero;
        [JsonConverter(typeof(DegreeConverter))]
        [DataMember]
        public Angle TarsusOffset { get; set; } = Angle.Zero;
        [DataMember]
        public bool CoxaInvert { get; set; }
        [DataMember]
        public bool FemurInvert { get; set; }
        [DataMember]
        public bool TibiaInvert { get; set; }
        [DataMember]
        public bool TarsusInvert { get; set; }


        public static LegSettings Default => new LegSettings(20, 77, 73, 90)
        {
            CoxaOffset = Angle.Zero,
            FemurOffset = Angle.FromDegrees(12.0),
            TibiaOffset = Angle.FromDegrees(70.0),
            TarsusOffset = Angle.FromDegrees(40.0)
        };

        public LegSettings()
        {

        }
        public LegSettings(double coxaLength, double femurLength, double tibiaLength, double tarsusLength)
        {
            CoxaLength = coxaLength;
            FemurLength = femurLength;
            TibiaLength = tibiaLength;
            TarsusLength = tarsusLength;
        }
    }
}