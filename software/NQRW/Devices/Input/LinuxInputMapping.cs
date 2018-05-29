namespace NQRW.Devices.Input
{
    public class LinuxInputMapping : BaseInputMapping
    {

        private readonly PS4Controller _controller;

        public LinuxInputMapping(PS4Controller controller)
        {
            _controller = controller;
        }
    }
}
