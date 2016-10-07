using System;

namespace KelsonBall.Utils.Messaging
{
    public interface IResourceToken
    {        
        Action Unsubscribe { get; }
    }
}