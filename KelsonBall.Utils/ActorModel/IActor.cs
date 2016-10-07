using System;

namespace KelsonBall.Utils.ActorModel
{
    public interface IActor
    {
        void Invoke(Action action);
        void Stop();
        void Abort();
    }
}
