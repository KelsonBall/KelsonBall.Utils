using KelsonBall.TestRunner;
using KelsonBall.Utils.Messaging;

namespace KelsonBall.Utils.Tests.Messaging
{
    [TestClass]
    public class EventPubSubTests
    {
        private const string TestChannelName = "TestChannel";

        [TestMethod]
        public void PublishEventTest()
        {
            // Setup
            EventAggregator events = new EventAggregator();

            // Subscribe listener to confirm event published
            bool published = false;
            events[TestChannelName].Subscribe(() => published = true);
            Assert.True(!published);

            // Act
            events[TestChannelName].Publish();
            Assert.True(published);
        }

        [TestMethod]
        public void PublishEventWithMessageTest()
        {
            // Setup
            EventAggregator events = new EventAggregator();

            // Subscribe listener to confirm event published
            bool recievedFirst = false;
            int recievedSecond = 0;
            events[TestChannelName].Subscribe(() => recievedFirst= true);
            events[TestChannelName].Subscribe(message => recievedSecond = (int)message);
            Assert.True(!recievedFirst);
            Assert.True(recievedSecond == 0);

            // Act
            int payload = 3;
            events[TestChannelName].Publish(payload);
            Assert.True(recievedFirst);
            Assert.True(recievedSecond == payload);
        }

        [TestMethod]
        public void UnsubscribeEventTest()
        {
            // Setup
            EventAggregator events = new EventAggregator();

            // Subscribe to events
            int fireFirstCount = 0;
            int fireSecondCount = 0;
            var token = events[TestChannelName].Subscribe(() => fireFirstCount++);
            events[TestChannelName].Subscribe(() => fireSecondCount++);
            Assert.True(fireFirstCount == 0);
            Assert.True(fireSecondCount == 0);

            // Act
            // Fire event for both subscribers
            events[TestChannelName].Publish();
            Assert.True(fireFirstCount == 1);
            Assert.True(fireSecondCount == 1);

            // Unsubscribe first subscriber
            token.Unsubscribe();

            // Fire event for only second subscriber
            events[TestChannelName].Publish();
            Assert.True(fireFirstCount == 1);
            Assert.True(fireSecondCount == 2);
        }
    }
}
