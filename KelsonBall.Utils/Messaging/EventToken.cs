using System;

namespace KelsonBall.Utils.Messaging
{
    public class EventToken : IEventToken
    {
        public readonly Action<object> Invoke;

        public Action Unsubscribe { get; }

        internal EventToken(Action<EventToken> unsubcriber, Action<object> action)
        {
            Unsubscribe = () => unsubcriber(this);
            Invoke = data => action?.Invoke(data);
        }
    }
}
