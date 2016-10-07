using System.Collections.Generic;

namespace KelsonBall.Utils.Messaging
{
    public class EventAggregator : IEventAggregator
    {
        private readonly IDictionary<string, IEventChannel> _channels = new Dictionary<string, IEventChannel>();

        public IEventChannel this[string key]
        {
            get
            {
                if (!_channels.ContainsKey(key))
                    _channels[key] = (IEventChannel)new EventChannel();
                return _channels[key];                
            }
        }
    }
}
