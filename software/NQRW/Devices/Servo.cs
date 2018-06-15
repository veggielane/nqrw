using NQRW.Maths;

namespace NQRW.Devices
{
    public class Servo : IServo
    {
        private Angle _angle = Angle.Zero;

        public Angle Min { get; private set; } = Angle.FromDegrees(-180);

        public Angle Max { get; private set; } = Angle.FromDegrees(180);

        public Angle Offset { get; set; } = Angle.Zero;

        public Angle Angle
        {
            get => _angle;
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

        public int Pulse => (int)(Angle+Offset).Radians.Map(-Trig.PiOverTwo, Trig.PiOverTwo, 600.0, 2400.0);
    }
}
