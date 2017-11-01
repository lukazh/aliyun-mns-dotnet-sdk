using System.Threading.Tasks;
using AliyunMnsSdk.Service;
using AliyunMnsSdk.Model;

namespace AliyunMnsSdk
{
    public class MnsTopic
    {
        #region Properties

        private readonly string _topicName;
        private readonly Topic _topic;

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiates Topic with the parameterized properties
        /// </summary>
        public MnsTopic(string topicName, MNSClient mnslient)
        {
            _topicName = topicName;
            _topic = new Topic(topicName, mnslient);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets and sets the property TopicName
        /// </summary>
        public string TopicName
        {
            get { return this._topicName; }
        }

        /// <summary>
        /// Check to see if TopicName property is set
        /// </summary>
        public bool IsSetTopicName
        {
            get { return this._topicName != null; }
        }

        #endregion

        #region  GetAttributes

        public Task<GetTopicAttributesResponse> GetAttributesAsync(GetTopicAttributesRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _topic.BeginGetAttributes(request, callback, stateObject),
                _topic.EndGetAttributes, this);
        }

        #endregion

        #region  SetAttributes

        public Task<SetTopicAttributesResponse> SetAttributesAsync(SetTopicAttributesRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _topic.BeginSetAttributes(request, callback, stateObject),
                _topic.EndSetAttributes, this);
        }

        #endregion

        #region  Subscribe

        public Task<SubscribeResponse> SubscribeAsync(SubscribeRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _topic.BeginSubscribe(request, callback, stateObject),
                _topic.EndSubscribe, this);
        }

        #endregion

        #region Unsubscribe
        
        public Task<UnsubscribeResponse> UnsubscribeAsync(UnsubscribeRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _topic.BeginUnsubscribe(request, callback, stateObject),
                _topic.EndUnsubscribe, this);
        }

        #endregion

        #region  GetSubscriptionAttribute

        public Task<GetSubscriptionAttributeResponse> GetSubscriptionAttributeAsync(GetSubscriptionAttributeRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _topic.BeginGetSubscriptionAttribute(request, callback, stateObject),
                _topic.EndGetSubscriptionAttribute, this);
        }

        #endregion

        #region  SetSubscriptionAttribute

        public Task<SetSubscriptionAttributeResponse> SetSubscriptionAttributeAsync(SetSubscriptionAttributeRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _topic.BeginSetSubscriptionAttribute(request, callback, stateObject),
                _topic.EndSetSubscriptionAttribute, this);
        }

        #endregion

        #region  ListSubscription

        public Task<ListSubscriptionResponse> ListSubscriptionAsync(ListSubscriptionRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _topic.BeginListSubscription(request, callback, stateObject),
                _topic.EndListSubscription, this);
        }

        #endregion

        #region  PublishMessage

        public Task<PublishMessageResponse> PublishMessageAsync(PublishMessageRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _topic.BeginPublishMessage(request, callback, stateObject),
                _topic.EndPublishMessage, this);
        }
        
        #endregion
    }
}
