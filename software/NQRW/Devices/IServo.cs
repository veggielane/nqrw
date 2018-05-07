using NQRW.Maths;

namespace NQRW.Devices
{
    public interface IServo
    {
        Angle Min { get; }
        Angle Max { get; }
        Angle Offset { get; }
        Angle Angle { get; set; }
        int Pulse { get; }
    }
}
