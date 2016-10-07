using System;

namespace KelsonBall.Utils.Messaging
{
    public interface IEventToken
    {
        Action Unsubscribe { get; }
    }
}
