using System;

namespace KelsonBall.Utils.Messaging
{
    public class ResourceToken : IResourceToken
    {
        public Func<object, object> Invoke { get; }

        public Action Unsubscribe { get; }

        internal ResourceToken(Action<ResourceToken> unsubscriber, Func<object> factory)
        {
            Invoke = data => factory?.Invoke();
            Unsubscribe = () => unsubscriber(this);
        }

        internal ResourceToken(Action<ResourceToken> unsubscriber, Func<object, object> factory)
        {
            Invoke = data => factory?.Invoke(data);
            Unsubscribe = () => unsubscriber(this);
        }
    }
}
