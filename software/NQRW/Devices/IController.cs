using System;
using System.Collections.Generic;
using System.Text;

namespace NQRW.Devices
{
    public interface IController:IDisposable
    {

    }

    public class KeyboardController : IController
    {
        public void Dispose()
        {
            
        }
    }
}
