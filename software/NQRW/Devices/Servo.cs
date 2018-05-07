using NQRW.Maths;

namespace NQRW.Devices
{
    public class Servo : IServo
    {
        private Angle _angle = Angle.FromDegrees(0);
        public Angle Min { get; private set; }

        public Angle Max { get; private set; }

        public Angle Offset { get; private set; }

        public Angle Angle
        {
            get { return _angle; }
            set
            {
                if (value > Max)
                {
                    _angle = Max;
                }
                else if (value < Min)
                {
                    _angle = Min;
                }
                else
                {
                    _angle = value;
                }
            }
        }

        public int Pulse => (int)Angle.Radians.Map(-Trig.PiOverTwo, Trig.PiOverTwo, 600.0, 2400.0);
    }
}
