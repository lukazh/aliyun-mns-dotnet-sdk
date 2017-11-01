using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AliyunMnsSdk.Model;
using AliyunMnsSdk.Model.Internal.MarshallTransformations;
using AliyunMnsSdk.Runtime;
using AliyunMnsSdk.Service;

namespace AliyunMnsSdk
{
    public sealed class MnsQueue
    {
        #region Properties

        private bool _queueExists;
        private bool _checkQueueExistance = true;
        private bool _deleteOnReceived = true;
        
        private readonly string _queueName;
        private readonly MNSClient _mnsClient;
        private readonly Queue _queue;

        private Action<Exception, List<string>> _deleteMessageFailureCallback;

        private delegate IAsyncResult AsyncOperationCallback();

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiates Queue with the parameterized properties
        /// </summary>
        public MnsQueue(string queueName, MNSClient mnsClient)
        {
            _queueName = queueName;
            _mnsClient = mnsClient;
            _queue = new Queue(queueName, mnsClient);
        }
        #endregion

        #region Public properties

        /// <summary>
        /// Gets and sets the property QueueName
        /// </summary>
        public string QueueName
        {
            get { return _queueName; }
        }

        /// <summary>
        /// Check to see if QueueName property is set
        /// </summary>
        public bool IsSetQueueName
        {
            get { return _queueName != null; }
        }

        /// <summary>
        /// Whether to check if the queue exists in server
        /// </summary>
        public bool CheckQueueExistance
        {
            get { return _checkQueueExistance; }
            set { _checkQueueExistance = value; }
        }

        /// <summary>
        /// Whether to delete message(s) on received
        /// </summary>
        public bool DeleteOnReceived
        {
            get { return _deleteOnReceived; }
            set { _deleteOnReceived = value; }
        }

        /// <summary>
        /// Action to take when it fails to asynchronously delete message(s) after reception.
        /// The parameters are the exception and list of messageIds.
        /// </summary>
        public Action<Exception, List<string>> DeleteMessageFailureCallback
        {
            get { return _deleteMessageFailureCallback; }
            set { _deleteMessageFailureCallback = value; }
        }

        #endregion

        #region  GetAttributes

        /// <summary>
        /// Asynchronously get queue attributes in the Asynchronous Programming Model pattern.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS GetAttributes service.</param>
        /// <returns>The response returned by the MNS GetAttributes service.</returns>
        public Task<GetQueueAttributesResponse> GetAttributesAsync(GetQueueAttributesRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _queue.BeginGetAttributes(request, callback, stateObject),
                _queue.EndGetAttributes, this);
        }

        #endregion

        #region  SetAttributes

        /// <summary>
        /// Asynchronously set queue attributes in the Asynchronous Programming Model pattern.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS SetAttributes service.</param>
        /// <returns>The response returned by the MNS SetAttributes service.</returns>
        public Task<SetQueueAttributesResponse> SetAttributesAsync(SetQueueAttributesRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _queue.BeginSetAttributes(request, callback, stateObject),
                _queue.EndSetAttributes, this);
        }

        #endregion

        #region  ChangeMessageVisibility

        /// <summary>
        /// Asynchronously change the visibility timeout of a specified message of the queue to a new value in the Asynchronous Programming Model pattern.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS ChangeMessageVisibility service.</param>
        /// <returns>The response returned by the MNS ChangeMessageVisibility service.</returns>
        public Task<ChangeMessageVisibilityResponse> ChangeMessageVisibilityAsync(ChangeMessageVisibilityRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _queue.BeginChangeMessageVisibility(request, callback, stateObject),
                _queue.EndChangeMessageVisibility, this);
        }

        #endregion

        #region DeleteMessage

        /// <summary>
        /// Asynchronously delete the specified message from the specified queue in the Asynchronous Programming Model pattern.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS DeleteMessage service.</param>
        /// <returns>The response returned by the MNS DeleteMessage service.</returns>
        public Task<DeleteMessageResponse> DeleteMessageAsync(DeleteMessageRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _queue.BeginDeleteMessage(request, callback, stateObject),
                _queue.EndDeleteMessage, this);
        }

        /// <summary>
        /// Asynchronously batch delete the specified message from the specified queue in the Asynchronous Programming Model pattern.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS BatchDeleteMessage service.</param>
        /// <returns>The response returned by the MNS BatchDeleteMessage service.</returns>
        public Task<BatchDeleteMessageResponse> BatchDeleteMessageAsync(BatchDeleteMessageRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _queue.BeginBatchDeleteMessage(request, callback, stateObject),
                _queue.EndBatchDeleteMessage, this);
        }

        #endregion

        #region ReceiveMessage

        /// <summary>
        /// Begins an asynchronous request to MNS ReceiveMessage service.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS ReceiveMessage service.</param>
        /// <param name="callback">The AsyncCallback delegate.</param>
        /// <param name="state">The state object for this request.</param>
        /// <returns>An IAsyncResult that references the asynchronous request for a response.</returns>
        private IAsyncResult BeginReceiveMessage(ReceiveMessageRequest request, AsyncCallback callback, object state)
        {
            // Process before invoke callback
            void PreCallback(IAsyncResult ar)
            {
                try
                {
                    var response = _queue.EndReceiveMessage(ar);
                    DeleteReceivedMessage(response.Message);
                }
                catch { /* ignored */ }

                callback.Invoke(ar);
            }

            request.QueueName = _queueName;

            var marshaller = new ReceiveMessageRequestMarshaller();
            var unmarshaller = ReceiveMessageResponseUnmarshaller.Instance;

            var cb = _deleteOnReceived ? PreCallback : callback;

            return _mnsClient.BeginInvoke<ReceiveMessageRequest>(request, marshaller, unmarshaller,
                cb, state);
        }

        /// <summary>
        /// Ends an asynchronous request for MNS ReceiveMessage service and convert message body from base64 string to normal string.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginReceiveMessage.</param>
        /// <returns>The response returned by the MNS ReceiveMessage service.</returns>
        private ReceiveMessageResponse EndReceiveMessage(IAsyncResult asyncResult)
        {
            var response = AliyunServiceClient.EndInvoke<ReceiveMessageResponse>(asyncResult);
            if (response.Message != null && response.Message.IsSetBody())
            {
                byte[] bytes = Convert.FromBase64String(response.Message.Body);
                response.Message.Body = Encoding.UTF8.GetString(bytes);
            }
            return response;
        }

        /// <summary>
        /// Asynchronously receive message in the Asynchronous Programming Model pattern.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS ReceiveMessage service.</param>
        /// <returns>The response returned by the MNS ReceiveMessage service.</returns>
        public Task<ReceiveMessageResponse> ReceiveMessageAsync(ReceiveMessageRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => BeginReceiveMessage(request, callback, stateObject),
                EndReceiveMessage, this);
        }

        /// <summary>
        /// Begins an asynchronous request to MNS BatchReceiveMessage service.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS BatchReceiveMessage service.</param>
        /// <param name="callback">The AsyncCallback delegate.</param>
        /// <param name="state">The state object for this request.</param>
        /// <returns>An IAsyncResult that references the asynchronous request for a response.</returns>
        private IAsyncResult BeginBatchReceiveMessage(BatchReceiveMessageRequest request, AsyncCallback callback, object state)
        {
            // Process before invoke callback
            void PreCallback(IAsyncResult ar)
            {
                try
                {
                    var response = _queue.EndBatchReceiveMessage(ar);
                    DeleteReceivedMessages(response.Messages);
                }
                catch { /* ignored */ }

                callback.Invoke(ar);
            }

            request.QueueName = _queueName;

            var marshaller = new BatchReceiveMessageRequestMarshaller();
            var unmarshaller = BatchReceiveMessageResponseUnmarshaller.Instance;

            var cb = _deleteOnReceived ? PreCallback : callback;

            return _mnsClient.BeginInvoke<BatchReceiveMessageRequest>(request, marshaller, unmarshaller,
                cb, state);
        }

        /// <summary>
        /// Ends an asynchronous request for MNS BatchReceiveMessage service and convert messages' body from base64 string to normal string.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginBatchReceiveMessage.</param>
        /// <returns>The response returned by the MNS BatchReceiveMessage service.</returns>
        private BatchReceiveMessageResponse EndBatchReceiveMessage(IAsyncResult asyncResult)
        {
            var response = AliyunServiceClient.EndInvoke<BatchReceiveMessageResponse>(asyncResult);
            if (response.Messages != null && response.Messages.Count > 0)
            {
                foreach (var message in response.Messages)
                {
                    if (message != null && message.IsSetBody())
                    {
                        byte[] bytes = Convert.FromBase64String(message.Body);
                        message.Body = Encoding.UTF8.GetString(bytes);
                    }
                }
            }
            return response;
        }

        /// <summary>
        /// Asynchronously batch receive message in the Asynchronous Programming Model pattern.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS BatchReceiveMessage service.</param>
        /// <returns>The response returned by the MNS BatchReceiveMessage service.</returns>
        public Task<BatchReceiveMessageResponse> BatchReceiveMessageAsync(BatchReceiveMessageRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => BeginBatchReceiveMessage(request, callback, stateObject),
                EndBatchReceiveMessage, this);
        }

        #endregion

        #region  PeekMessage

        /// <summary>
        /// Ends an asynchronous request for MNS PeekMessage service and convert message body from base64 string to normal string.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginPeekMessage.</param>
        /// <returns>The response returned by the MNS PeekMessage service.</returns>
        public PeekMessageResponse EndPeekMessage(IAsyncResult asyncResult)
        {
            var response = AliyunServiceClient.EndInvoke<PeekMessageResponse>(asyncResult);
            if (response.Message != null && response.Message.IsSetBody())
            {
                byte[] bytes = Convert.FromBase64String(response.Message.Body);
                response.Message.Body = Encoding.UTF8.GetString(bytes);
            }
            return response;
        }

        /// <summary>
        /// Asynchronously peek message in the Asynchronous Programming Model pattern.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS PeekMessage service.</param>
        /// <returns>The response returned by the MNS PeekMessage service.</returns>
        public Task<PeekMessageResponse> PeekMessageAsync(PeekMessageRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _queue.BeginPeekMessage(request, callback, stateObject),
                EndPeekMessage, this);
        }

        /// <summary>
        /// Ends an asynchronous request for MNS BatchPeekMessage service and convert messages' body from base64 string to normal string.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginBatchPeekMessage.</param>
        /// <returns>The response returned by the MNS BatchPeekMessage service.</returns>
        public BatchPeekMessageResponse EndBatchPeekMessage(IAsyncResult asyncResult)
        {
            var response = AliyunServiceClient.EndInvoke<BatchPeekMessageResponse>(asyncResult);
            if (response.Messages != null && response.Messages.Count > 0)
            {
                foreach (var message in response.Messages)
                {
                    if (message != null && message.IsSetBody())
                    {
                        byte[] bytes = Convert.FromBase64String(message.Body);
                        message.Body = Encoding.UTF8.GetString(bytes);
                    }
                }
            }
            return response;
        }

        /// <summary>
        /// Asynchronously batch peek message in the Asynchronous Programming Model pattern.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS BatchPeekMessage service.</param>
        /// <returns>The response returned by the MNS BatchPeekMessage service.</returns>
        public Task<BatchPeekMessageResponse> BatchPeekMessageAsync(BatchPeekMessageRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => _queue.BeginBatchPeekMessage(request, callback, stateObject),
                EndBatchPeekMessage, this);
        }

        #endregion

        #region SendMessage

        /// <summary>
        /// Begins an asynchronous request to MNS SendMessage service.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS SendMessage service.</param>
        /// <param name="callback">The AsyncCallback delegate.</param>
        /// <param name="state">The state object for this request.</param>
        /// <returns>An IAsyncResult that references the asynchronous request for a response.</returns>
        private IAsyncResult BeginSendMessage(SendMessageRequest request, AsyncCallback callback, object state)
        {
            request.QueueName = _queueName;
            if (request.IsSetMessageBody())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(request.MessageBody);
                request.MessageBody = Convert.ToBase64String(bytes);
            }

            var marshaller = new SendMessageRequestMarshaller();
            var unmarshaller = SendMessageResponseUnmarshaller.Instance;

            IAsyncResult SendMessage()
            {
                return _mnsClient.BeginInvoke<SendMessageRequest>(request, marshaller, unmarshaller,
                    callback, state);
            }

            return _checkQueueExistance ? EnsureQueueExists(SendMessage) : SendMessage();
        }

        /// <summary>
        /// Asynchronously send message in the Asynchronous Programming Model pattern.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS SendMessage service.</param>
        /// <returns>The response returned by the MNS SendMessage service.</returns>
        public Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => BeginSendMessage(request, callback, stateObject),
                _queue.EndSendMessage, this);
        }

        /// <summary>
        /// Begins an asynchronous request to MNS BatchSendMessage service.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS BatchSendMessage service.</param>
        /// <param name="callback">The AsyncCallback delegate.</param>
        /// <param name="state">The state object for this request.</param>
        /// <returns>An IAsyncResult that references the asynchronous request for a response.</returns>
        private IAsyncResult BeginBatchSendMessage(BatchSendMessageRequest request, AsyncCallback callback, object state)
        {
            request.QueueName = _queueName;
            if (request.Requests != null && request.Requests.Count > 0)
            {
                foreach (var r in request.Requests)
                {
                    if (r != null && r.IsSetMessageBody())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(r.MessageBody);
                        r.MessageBody = Convert.ToBase64String(bytes);
                    }
                }
            }

            var marshaller = new BatchSendMessageRequestMarshaller();
            var unmarshaller = BatchSendMessageResponseUnmarshaller.Instance;

            IAsyncResult SendMessages()
            {
                return _mnsClient.BeginInvoke<BatchSendMessageRequest>(request, marshaller, unmarshaller,
                    callback, state);
            }

            return _checkQueueExistance ? EnsureQueueExists(SendMessages) : SendMessages();
        }

        /// <summary>
        /// Asynchronously batch send message in the Asynchronous Programming Model pattern.
        /// </summary>
        /// <param name="request">The request object to be sent to MNS BatchSendMessage service.</param>
        /// <returns>The response returned by the MNS BatchSendMessage service.</returns>
        public Task<BatchSendMessageResponse> BatchSendMessageAsync(BatchSendMessageRequest request)
        {
            return Task.Factory.FromAsync(
                (callback, stateObject) => BeginBatchSendMessage(request, callback, stateObject),
                _queue.EndBatchSendMessage, this);
        }

        #endregion

        #region Ensure queue exists

        /// <summary>
        /// Create queue if it does not exist and then call back
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IAsyncResult EnsureQueueExists(AsyncOperationCallback callback)
        {
            if (_queueExists)
            {
                return callback();
            }

            void CreateQueueCallback(IAsyncResult ar)
            {
                var client = (MNSClient)ar.AsyncState;
                try
                {
                    client.EndCreateQueue(ar);
                    _queueExists = true;
                    callback();
                }
                catch (QueueAlreadyExistException)
                {
                    _queueExists = true;
                    callback();
                }
            }

            var createQueueRequest = new CreateQueueRequest
            {
                QueueName = _queueName
            };
            return _mnsClient.BeginCreateQueue(createQueueRequest, CreateQueueCallback, _mnsClient);
        }

        #endregion

        #region Delete received message(s)

        /// <summary>
        /// Delete message on received
        /// </summary>
        /// <param name="message">Received message</param>
        private void DeleteReceivedMessage(Message message)
        {
            // handle exception in order that main task will not be interrupted
            void ExceptionHandler(Exception exception)
            {
                if (_deleteMessageFailureCallback != null)
                    _deleteMessageFailureCallback(exception, new List<string> { message.Id });
            }

            // async call back
            void Callback(IAsyncResult ar)
            {
                try
                {
                    var queue = (Queue)ar.AsyncState;
                    queue.EndDeleteMessage(ar);
                }
                catch (Exception e)
                {
                    ExceptionHandler(e);
                }
            }

            try
            {
                var deleteMessageRequest = new DeleteMessageRequest(message.ReceiptHandle);
                _queue.BeginDeleteMessage(deleteMessageRequest, Callback, this);
            }
            catch (Exception e)
            {
                ExceptionHandler(e);
            }
        }

        /// <summary>
        /// Delete messages on received
        /// </summary>
        /// <param name="messages">List of received messages</param>
        private void DeleteReceivedMessages(List<Message> messages)
        {
            // handle exception in order that main task will not be interrupted
            void ExceptionHandler(Exception exception)
            {
                if (_deleteMessageFailureCallback != null)
                {
                    var list = new List<string>();
                    foreach (var message in messages)
                    {
                        list.Add(message.Id);
                    }
                    _deleteMessageFailureCallback(exception, list);
                }
            }

            // async call back
            void Callback(IAsyncResult ar)
            {
                try
                {
                    var queue = (Queue)ar.AsyncState;
                    queue.EndDeleteMessage(ar);
                }
                catch (Exception e)
                {
                    ExceptionHandler(e);
                }
            }

            try
            {
                var receiptHandles = new List<string>();
                foreach (var message in messages)
                {
                    receiptHandles.Add(message.ReceiptHandle);
                }
                var batchDeleteMessageRequest = new BatchDeleteMessageRequest()
                {
                    ReceiptHandles = receiptHandles
                };
                _queue.BeginBatchDeleteMessage(batchDeleteMessageRequest, Callback, this);
            }
            catch (Exception e)
            {
                ExceptionHandler(e);
            }
        }

        #endregion

    }
}
