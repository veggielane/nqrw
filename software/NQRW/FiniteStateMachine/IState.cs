using System.Text;

namespace NQRW.FiniteStateMachine
{
    public interface IState
    {
        string Name { get; }
        void Start();
        void Stop();
    }
}
