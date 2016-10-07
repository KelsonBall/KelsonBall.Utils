using System;
using System.Collections.Concurrent;
using System.Threading;

namespace KelsonBall.Utils.ActorModel
{
    public abstract class AbstractActor : IActor
    {
        private bool _running = true;
        private readonly Thread _actorThread;
        private readonly ConcurrentQueue<Action> _messageQueue = new ConcurrentQueue<Action>();        

        // Create actor, subscribe to stop event, start thread loop
        protected AbstractActor(Action<Action> subscribeToStopEvent)
        {
            subscribeToStopEvent(Stop);
            _actorThread = new Thread(_actorLoop);
            _actorThread.Start();
        }
        
        // Keep looping, executing any actions pushed onto the queue
        private void _actorLoop()
        {
            Action message;
            while (_running)
                if (_messageQueue.TryDequeue(out message))
                    message?.Invoke();
                else
                    Idle();                            
        }


        #region Protected API
        // Durration to wait when default Idle method invoked.
        protected int IdleWaitDurration = 30;

        // Do this if there is no action on the queue
        protected virtual void Idle()
        {
            Thread.Sleep(IdleWaitDurration);
        }
        #endregion

        #region Public API        
        // Queue an action to be invoked on this thread        
        public void Invoke(Action action)
        {
            _messageQueue.Enqueue(action);
        }

        // Shutdown this thread after completing the current action
        public void Stop()
        {
            _running = false;
        }

        // Hard stop this thread
        public void Abort()
        {
            _actorThread.Abort();
        }
        #endregion
    }
}