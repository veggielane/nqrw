using NQRW.Maths;

namespace NQRW.Devices
{
    public class Servo : IServo
    {
        private Angle _angle = Angle.Zero;
        public Angle Min { get; }
        public Angle Max { get; }
        public Angle Offset { get; }
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
        public int Pulse => (int)(Angle + Offset).Radians.Map(-Trig.PiOverTwo, Trig.PiOverTwo, 600.0, 2400.0);
        public Servo(Angle angle, Angle offset):this(angle,offset,-Angle.PI, Angle.PI)
        {
            Angle = angle;
            Offset = offset;
        }
        public Servo(Angle angle, Angle offset, Angle min, Angle max)
        {
            Min = min;
            Max = max;
            Angle = angle;
            Offset = offset;

        }
    }
}
