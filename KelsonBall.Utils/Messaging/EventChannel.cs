using System;
using System.Collections.Generic;
using System.Linq;

namespace KelsonBall.Utils.Messaging
{
    public class EventChannel : IEventChannel
    {        
        readonly object eventTokenMutex = new object();
        private readonly List<EventToken> eventTokens = new List<EventToken>();

        private void unsubcribeEvent(EventToken token)
        {
            lock (eventTokenMutex)
                eventTokens.Remove(token);
        }

        public IEventToken Subscribe(Action action)
        {
            EventToken token = new EventToken(this.unsubcribeEvent, message => action());
            lock (eventTokenMutex)
                eventTokens.Add(token);
            return token;
        }

        public IEventToken Subscribe(Action<object> action)
        {
            EventToken token = new EventToken(this.unsubcribeEvent, action);
            lock (eventTokenMutex)
                eventTokens.Add(token);
            return token;
        }

        public IEventChannel Publish()
        {
            EventToken[] localTokens = null;
            lock (eventTokenMutex)
                localTokens = eventTokens.ToArray();
            foreach (var token in localTokens)
                token.Invoke(null);
            return this;
        }

        public IEventChannel Publish(object message)
        {
            EventToken[] localTokens = null;
            lock (eventTokenMutex)
                localTokens = eventTokens.ToArray();
            foreach (var token in localTokens)
                token.Invoke(message);
            return this;
        }       

        private readonly object resourceTokenMutex = new object();
        private readonly IDictionary<string, List<ResourceToken>> resourceTokens = new Dictionary<string, List<ResourceToken>>();

        private void unsubscribeResource(List<ResourceToken> list, ResourceToken token)
        {
            lock (resourceTokenMutex)
                list.Remove(token);
        }

        public IResourceToken Provide(string key, Func<object> factory)
        {
            lock (resourceTokenMutex)
            {
                if (!resourceTokens.ContainsKey(key))
                    resourceTokens[key] = new List<ResourceToken>();
                List<ResourceToken> tokens = resourceTokens[key];
                ResourceToken resourceToken = new ResourceToken(token => unsubscribeResource(tokens, token), factory);
                tokens.Add(resourceToken);
                return resourceToken;
            }
        }

        public IResourceToken Provide(string key, Func<object, object> factory)
        {
            lock (resourceTokenMutex)
            {
                if (!resourceTokens.ContainsKey(key))
                    resourceTokens[key] = new List<ResourceToken>();
                List<ResourceToken> tokens = resourceTokens[key];
                ResourceToken resourceToken = new ResourceToken(token => unsubscribeResource(tokens, token), factory);
                tokens.Add(resourceToken);
                return resourceToken;
            }
        }

        public IEnumerable<object> Request(string key, object data = null)
        {
            List<ResourceToken> tokens = null;
            lock (resourceTokenMutex)
                if (resourceTokens.ContainsKey(key))
                    tokens = resourceTokens[key].ToList();
            return tokens?
                        .Select(t => t.Invoke(data))
                        .Where(t => t != null)
                        .Distinct()
                        ?? Enumerable.Empty<object>();
        }
    }
}
