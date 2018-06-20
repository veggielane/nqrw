using JetBrains.Annotations;

namespace NQRW.Devices.Input
{
    [UsedImplicitly]
    public class LinuxInputMapping : IPlatformInput
    {
        [UsedImplicitly]
        public readonly PS4Controller Controller;
        public LinuxInputMapping(PS4Controller controller)
        {
            Controller = controller;
        }
    }
}
