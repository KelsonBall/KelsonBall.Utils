using System;
using System.Collections.Generic;

namespace KelsonBall.Utils.Messaging
{
    public interface IEventChannel
    {
        IEventToken Subscribe(Action action);
        IEventToken Subscribe(Action<object> action);
        IEventChannel Publish();
        IEventChannel Publish(object message);

        IResourceToken Provide(string key, Func<object> factory);
        IResourceToken Provide(string key, Func<object, object> factory);

        IEnumerable<object> Request(string key, object data = null);
    }
}
