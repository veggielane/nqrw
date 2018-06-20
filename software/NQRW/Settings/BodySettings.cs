using System.Runtime.Serialization;

namespace NQRW.Settings
{
    [DataContract]
    public class BodySettings
    {
        [DataMember]
        public double A { get; set; }
        [DataMember]
        public double B { get; set; }
        [DataMember]
        public double C { get; set; }
        [DataMember]
        public double D { get; set; }
        [DataMember]
        public double E { get; set; }
        [DataMember]
        public double F { get; set; }
        [DataMember]
        public double StartHeight { get; set; }
        [DataMember]
        public double FootOffset { get; set; }
    }
}