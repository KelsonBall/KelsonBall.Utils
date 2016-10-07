using KelsonBall.TestRunner;
using KelsonBall.Utils.Messaging;
using System.Linq;

namespace KelsonBall.Utils.Tests.Messaging
{
    [TestClass]
    public class EventProReqTests
    {
        private const string TestChannelName = "TestChannel";

        [TestMethod]
        public void SingleProviderRequestTest()
        {
            // Setup
            var events = new EventAggregator();

            events[TestChannelName].Provide("item1", () => 3);

            // Act
            int result = (int)events[TestChannelName].Request("item1").First();

            Assert.True(result == 3);
        }

        [TestMethod]
        public void MultipleProviderRequestTest()
        {
            // Setup
            var events = new EventAggregator();

            events[TestChannelName].Provide("item1", () => 3);
            events[TestChannelName].Provide("item1", () => "hello");

            // Act
            var result = events[TestChannelName].Request("item1");

            Assert.True(result.SequenceEqual(new object[] { 3, "hello" }));
        }

        [TestMethod]
        public void UnsubscribeProviderTest()
        {
            // Setup
            var events = new EventAggregator();

            var token = events[TestChannelName].Provide("item1", () => 3);
            events[TestChannelName].Provide("item1", () => "hello");
            // Act
            var result = events[TestChannelName].Request("item1");
            Assert.True(result.SequenceEqual(new object[] { 3, "hello" }));

            // Unsubscribe first provider
            token.Unsubscribe();
            result = events[TestChannelName].Request("item1");
            Assert.True(result.SequenceEqual(new object[] { "hello" }));
        }

        [TestMethod]
        public void ProvideWithPayloadTest()
        {
            // Setup
            var events = new EventAggregator();

            events[TestChannelName].Provide("item1", payload => payload is int);
            events[TestChannelName].Provide("item1", payload => payload.GetType());

            // Act
            var result = events[TestChannelName].Request("item1", 3);
            Assert.True(result.SequenceEqual(new object[] { true, typeof(int) }));

            result = events[TestChannelName].Request("item1", "hello");
            Assert.True(result.SequenceEqual(new object[] { false, typeof(string) }));
        }
    }
}
