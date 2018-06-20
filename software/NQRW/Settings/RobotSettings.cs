using NQRW.Robotics;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace NQRW.Settings
{
    [DataContract]
    public class RobotSettings
    {
        [DataMember]
        public BodySettings Body { get; set; }

        [JsonConverter(typeof(DictionaryWithSpecialEnumKeyConverter))]
        [DataMember]
        public Dictionary<Leg, LegSettings> Legs { get; set; }
        public static RobotSettings LoadFromFile(string path)
        {
            return JsonConvert.DeserializeObject<RobotSettings>(File.ReadAllText(path));
        }
    }
}
