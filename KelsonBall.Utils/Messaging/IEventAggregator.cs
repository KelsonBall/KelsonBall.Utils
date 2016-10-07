namespace KelsonBall.Utils.Messaging
{
    public interface IEventAggregator
    {
        IEventChannel this[string key] { get; }
    }
}