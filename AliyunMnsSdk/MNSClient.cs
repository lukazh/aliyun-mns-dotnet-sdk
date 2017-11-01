using System;
using System.Threading.Tasks;
using AliyunMnsSdk.Model;
using AliyunMnsSdk.Runtime;
using AliyunMnsSdk.Service;

namespace AliyunMnsSdk
{
    public class MnsClient
    {
        private readonly MNSClient _client;

        #region Constructors

        /// <summary>
        /// Constructs MNSClient with Aliyun Access Key ID and Aliyun Secret Key
        /// </summary>
        /// <param name="accessKeyId">Aliyun Access Key ID</param>
        /// <param name="secretAccessKey">Aliyun Secret Access Key</param>
        /// <param name="regionEndpoint">
        /// The region endpoint to connect. http://$AccountID.mns.$region.aliyuncs.com
        /// </param>
        public MnsClient(string accessKeyId, string secretAccessKey, string regionEndpoint) 
            :this(accessKeyId, secretAccessKey, regionEndpoint, null)
        {
        }

        /// <summary>
        /// Constructs MNSClient with Aliyun Access Key ID and AliyunMnsSdk Secret Key
        /// </summary>
        /// <param name="accessKeyId">Aliyun Access Key ID</param>
        /// <param name="secretAccessKey">Aliyun Secret Access Key</param>
        /// <param name="regionEndpoint">
        /// The region endpoint to connect. http://$AccountID.mns.$region.aliyuncs.com
        /// </param>
        /// <param name="stsToken">STS token</param>
        public MnsClient(string accessKeyId, string secretAccessKey, string regionEndpoint, string stsToken)
        {
            _client = new MNSClient(accessKeyId, secretAccessKey, regionEndpoint, stsToken);
        }

        #endregion

        #region GetNativeQueue

        /// <summary>
        /// Get a native queue object with the specified queueName.
        /// </summary>
        /// <param name="queueName">The name of native queue object to be created</param>
        /// <returns>A native queue object</returns>
        public MnsQueue GetNativeQueue(string queueName)
        {
            return new MnsQueue(queueName, _client);
        }

        #endregion
        
        #region  CreateQueue

        /// <summary>
        /// Ends an asynchronous request for MNS CreateQueue service.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginCreateQueue.</param>
        /// <returns>Returns a native queue object.</returns>
        private MnsQueue EndCreateQueue(IAsyncResult asyncResult)
        {
            var response = AliyunServiceClient.EndInvoke<CreateQueueResponse>(asyncResult);
            return new MnsQueue(response.QueueUrl.Substring(response.QueueUrl.LastIndexOf("/") + 1), _client);
        }

        public Task<MnsQueue> CreateQueueAsync(CreateQueueRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _client.BeginCreateQueue(request, callback, stateObject),
                EndCreateQueue, this);
        }

        #endregion

        #region  DeleteQueue

        public Task<DeleteQueueResponse> DeleteQueueAsync(DeleteQueueRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _client.BeginDeleteQueue(request, callback, stateObject),
                _client.EndDeleteQueue, this);
        }

        #endregion

        #region  ListQueue

        public Task<ListQueueResponse> ListQueueAsync(ListQueueRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _client.BeginListQueue(request, callback, stateObject),
                _client.EndListQueue, this);
        }

        #endregion

        #region GetNativeTopic

        /// <summary>
        /// Get a native topic object with the specified topicName.
        /// </summary>
        /// <param name="topicName">The name of native topic object to be created</param>
        /// <returns>A native topic object</returns>
        public MnsTopic GetNativeTopic(string topicName)
        {
            return new MnsTopic(topicName, _client);
        }

        #endregion

        #region  CreateTopic

        public Task<MnsTopic> CreateTopicAsync(CreateTopicRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _client.BeginCreateTopic(request, callback, stateObject),
                EndCreateTopic, this);
        }

        /// <summary>
        /// Ends an asynchronous request for MNS CreateTopic service.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginCreateTopic.</param>
        /// <returns>Returns a native queue object.</returns>
        private MnsTopic EndCreateTopic(IAsyncResult asyncResult)
        {
            var response = AliyunServiceClient.EndInvoke<CreateTopicResponse>(asyncResult);
            return new MnsTopic(response.TopicUrl.Substring(response.TopicUrl.LastIndexOf("/") + 1), _client);
        }

        #endregion

        #region  DeleteTopic

        public Task<DeleteTopicResponse> DeleteTopicAsync(DeleteTopicRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _client.BeginDeleteTopic(request, callback, stateObject),
                _client.EndDeleteTopic, this);
        }

        #endregion

        #region  ListTopic

        public Task<ListTopicResponse> ListTopicAsync(ListTopicRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _client.BeginListTopic(request, callback, stateObject),
                _client.EndListTopic, this);
        }

        #endregion

        #region SetAccountAttributes

        public Task<SetAccountAttributesResponse> SetAccountAttributesAsync(SetAccountAttributesRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _client.BeginSetAccountAttributes(request, callback, stateObject),
                _client.EndSetAccountAttributes, this);
        }

        #endregion

        #region GetAccountAttributes

        public Task<GetAccountAttributesResponse> GetAccountAttributesAsync(GetAccountAttributesRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _client.BeginGetAccountAttributes(request, callback, stateObject),
                _client.EndGetAccountAttributes, this);
        }

        #endregion
    }
}
