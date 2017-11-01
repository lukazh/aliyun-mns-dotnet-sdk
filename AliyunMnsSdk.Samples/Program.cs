using AliyunMnsSdk.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AliyunMnsSdk.Samples
{
    /// <summary>
    /// Samples for all supported async operations of MNS.
    /// </summary>
    internal class Program : Config
    {
        #region Private Properties

        private const string QueueName = "async-queue";
        private const string TopicName = "async-topic";
        private const string QueueNamePrefix = "async";
        private const uint BatchSize = 6;

        private static string _nextMarker = string.Empty;
        private static string _receiptHandle;
        private static BatchReceiveMessageResponse _batchReceiveMessageResponse;

        private static readonly AutoResetEvent AutoSetEvent = new AutoResetEvent(false);
        private static readonly MnsClient Client = new MnsClient(AccessKeyId, SecretAccessKey, Endpoint);
        private static readonly MnsQueue Queue = Client.GetNativeQueue(QueueName);
        private static readonly MnsTopic Topic = Client.GetNativeTopic(TopicName);

        #endregion

        #region Queue async methods

        private static async void CreateQueueAsync(CreateQueueRequest request)
        {
            try
            {
                var queue = await Client.CreateQueueAsync(request);
                Console.WriteLine("## Async create queue successfully, queue name: {0}", queue.QueueName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async create queue failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void ListQueueAsync(ListQueueRequest request)
        {
            try
            {
                var response = await Client.ListQueueAsync(request);
                Console.WriteLine("## Async list queue successfully, status code: {0}", response.HttpStatusCode);
                foreach (var queueUrl in response.QueueUrls)
                {
                    Console.WriteLine(queueUrl);
                }

                if (response.IsSetNextMarker())
                {
                    _nextMarker = response.NextMarker;
                    Console.WriteLine("NextMarker: {0}", response.NextMarker);
                }
                else
                {
                    _nextMarker = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async list queue failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void DeleteQueueAsync(DeleteQueueRequest request)
        {
            try
            {
                var response = await Client.DeleteQueueAsync(request);
                Console.WriteLine("## Async delete queue successfully, status code: {0}", response.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async delete queue failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void GetQueueAttributesAsync(GetQueueAttributesRequest request)
        {
            try
            {
                var response = await Queue.GetAttributesAsync(request);
                Console.WriteLine("## Async get queue attributes successfully, status code: {0}", response.HttpStatusCode);
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("QueueName: {0}", response.Attributes.QueueName);
                Console.WriteLine("CreateTime: {0}", response.Attributes.CreateTime);
                Console.WriteLine("LastModifyTime: {0}", response.Attributes.LastModifyTime);
                Console.WriteLine("VisibilityTimeout: {0}", response.Attributes.VisibilityTimeout);
                Console.WriteLine("MaximumMessageSize: {0}", response.Attributes.MaximumMessageSize);
                Console.WriteLine("MessageRetentionPeriod: {0}", response.Attributes.MessageRetentionPeriod);
                Console.WriteLine("DelaySeconds: {0}", response.Attributes.DelaySeconds);
                Console.WriteLine("PollingWaitSeconds: {0}", response.Attributes.PollingWaitSeconds);
                Console.WriteLine("InactiveMessages: {0}", response.Attributes.InactiveMessages);
                Console.WriteLine("ActiveMessages: {0}", response.Attributes.ActiveMessages);
                Console.WriteLine("DelayMessages: {0}", response.Attributes.DelayMessages);
                Console.WriteLine("----------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async get queue attributes failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void SetQueueAttributesAsync(SetQueueAttributesRequest request)
        {
            try
            {
                var response = await Queue.SetAttributesAsync(request);
                Console.WriteLine("## Async set queue attributes successfully, status code: {0}", response.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async set queue attributes failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void SendMessageAsync(SendMessageRequest request)
        {
            try
            {
                var response = await Queue.SendMessageAsync(request);
                Console.WriteLine("## Async send message successfully, messageId: {0}, MessageBodyMD5: {1}",
                    response.MessageId, response.MessageBodyMD5);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async send message failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void BatchSendMessageAsync(BatchSendMessageRequest request)
        {
            try
            {
                var response = await Queue.BatchSendMessageAsync(request);
                Console.WriteLine("## Async batch send message successfully, messages count {0}", response.Responses.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async batch send message failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void ReceiveMessageAsync(ReceiveMessageRequest request)
        {
            try
            {
                var response = await Queue.ReceiveMessageAsync(request);
                Console.WriteLine("## Async receive message successfully, status code: {0}", response.HttpStatusCode);
                Console.WriteLine("----------------------------------------------------");
                var message = response.Message;
                Console.WriteLine("MessageId: {0}", message.Id);
                Console.WriteLine("ReceiptHandle: {0}", message.ReceiptHandle);
                Console.WriteLine("MessageBody: {0}", message.Body);
                Console.WriteLine("MessageBodyMD5: {0}", message.BodyMD5);
                Console.WriteLine("EnqueueTime: {0}", message.EnqueueTime);
                Console.WriteLine("NextVisibleTime: {0}", message.NextVisibleTime);
                Console.WriteLine("FirstDequeueTime: {0}", message.FirstDequeueTime);
                Console.WriteLine("DequeueCount: {0}", message.DequeueCount);
                Console.WriteLine("Priority: {0}", message.Priority);
                Console.WriteLine("----------------------------------------------------");

                _receiptHandle = message.ReceiptHandle;
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async receive message failed, exception info: " + ex.Message + ex.GetType().Name);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void BatchReceiveMessageAsync(BatchReceiveMessageRequest request)
        {
            try
            {
                _batchReceiveMessageResponse = await Queue.BatchReceiveMessageAsync(request);
                Console.WriteLine("## Async batch receive message successfully, status code: {0}, messages count {1}",
                    _batchReceiveMessageResponse.HttpStatusCode, _batchReceiveMessageResponse.Messages.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async batch receive message failed, exception info: " + ex.Message + ex.GetType().Name);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void ChangeMessageVisibilityAsync(ChangeMessageVisibilityRequest request)
        {
            try
            {
                var response = await Queue.ChangeMessageVisibilityAsync(request);
                Console.WriteLine("## Async change message visibility successfully, ReceiptHandle: {0}, NextVisibleTime: {1}",
                    response.ReceiptHandle, response.NextVisibleTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async change message visibility failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void PeekMessageAsync(PeekMessageRequest request)
        {
            try
            {
                var response = await Queue.PeekMessageAsync(request);
                Console.WriteLine("## Async peek message successfully, status code: {0}", response.HttpStatusCode);
                Console.WriteLine("----------------------------------------------------");
                var message = response.Message;
                Console.WriteLine("MessageId: {0}", message.Id);
                Console.WriteLine("MessageBody: {0}", message.Body);
                Console.WriteLine("MessageBodyMD5: {0}", message.BodyMD5);
                Console.WriteLine("EnqueueTime: {0}", message.EnqueueTime);
                Console.WriteLine("FirstDequeueTime: {0}", message.FirstDequeueTime);
                Console.WriteLine("DequeueCount: {0}", message.DequeueCount);
                Console.WriteLine("Priority: {0}", message.Priority);
                Console.WriteLine("----------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async peek message failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void BatchPeekMessageAsync(BatchPeekMessageRequest request)
        {
            try
            {
                var response = await Queue.BatchPeekMessageAsync(request);
                Console.WriteLine("## Async batch peek message successfully, status code: {0}, messages count {1}",
                    response.HttpStatusCode, response.Messages.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async batch peek message failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void DeleteMessageAsync(DeleteMessageRequest request)
        {
            try
            {
                var response = await Queue.DeleteMessageAsync(request);
                Console.WriteLine("## Async delete message successfully, status code: {0}", response.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async delete message failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void BatchDeleteMessageAsync(BatchDeleteMessageRequest request)
        {
            try
            {
                var response = await Queue.BatchDeleteMessageAsync(request);
                Console.WriteLine("## Async batch delete message successfully, status code: {0}", response.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async batch delete message failed, exception info: " + ex.Message);
                if (ex is BatchDeleteFailException)
                {
                    Console.WriteLine("Error items: ");
                    var errorItems = ((BatchDeleteFailException)ex).ErrorItems;
                    foreach (var errorItem in errorItems)
                    {
                        Console.WriteLine(errorItem.ToString());
                    }
                }
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        #endregion

        #region Topic async methods

        private static async void CreateTopicAsync(CreateTopicRequest request)
        {
            try
            {
                var topic = await Client.CreateTopicAsync(request);
                Console.WriteLine("## Async create topic successfully, topic name: {0}", topic.TopicName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async create topic failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void ListTopicAsync(ListTopicRequest request)
        {
            try
            {
                var response = await Client.ListTopicAsync(request);
                Console.WriteLine("## Async list topic successfully, status code: {0}", response.HttpStatusCode);
                foreach (var topicUrl in response.TopicUrls)
                {
                    Console.WriteLine(topicUrl);
                }

                if (response.IsSetNextMarker())
                {
                    _nextMarker = response.NextMarker;
                    Console.WriteLine("NextMarker: {0}", response.NextMarker);
                }
                else
                {
                    _nextMarker = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async list topic failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void DeleteTopicAsync(DeleteTopicRequest request)
        {
            try
            {
                var response = await Client.DeleteTopicAsync(request);
                Console.WriteLine("## Async delete topic successfully, status code: {0}", response.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async delete topic failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void GetTopicAttributesAsync(GetTopicAttributesRequest request)
        {
            try
            {
                var response = await Topic.GetAttributesAsync(request);
                Console.WriteLine("## Async get topic attributes successfully, status code: {0}", response.HttpStatusCode);
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("TopicName: {0}", response.Attributes.TopicName);
                Console.WriteLine("CreateTime: {0}", response.Attributes.CreateTime);
                Console.WriteLine("LastModifyTime: {0}", response.Attributes.LastModifyTime);
                Console.WriteLine("MaximumMessageSize: {0}", response.Attributes.MaximumMessageSize);
                Console.WriteLine("MessageRetentionPeriod: {0}", response.Attributes.MessageRetentionPeriod);
                Console.WriteLine("----------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async get topic attributes failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void SetTopicAttributesAsync(SetTopicAttributesRequest request)
        {
            try
            {
                var response = await Topic.SetAttributesAsync(request);
                Console.WriteLine("## Async set topic attributes successfully, status code: {0}", response.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async set topic attributes failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void SubscribeAsync(SubscribeRequest request)
        {
            try
            {
                var response = await Topic.SubscribeAsync(request);
                Console.WriteLine("## Async subscribe successfully, status code: {0}", response.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async subscribe failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void UnsubscribeAsync(UnsubscribeRequest request)
        {
            try
            {
                var response = await Topic.UnsubscribeAsync(request);
                Console.WriteLine("## Async unsubscribe successfully, status code: {0}", response.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async unsubscribe failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void GetSubscriptionAttributeAsync(GetSubscriptionAttributeRequest request)
        {
            try
            {
                var response = await Topic.GetSubscriptionAttributeAsync(request);
                Console.WriteLine("## Async get subscription attribute successfully, status code: {0}", response.HttpStatusCode);
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("TopicName: {0}", response.Attributes.TopicName);
                Console.WriteLine("CreateTime: {0}", response.Attributes.CreateTime);
                Console.WriteLine("LastModifyTime: {0}", response.Attributes.LastModifyTime);
                Console.WriteLine("TopicOwner: {0}", response.Attributes.TopicOwner);
                Console.WriteLine("EndPoint: {0}", response.Attributes.EndPoint);
                Console.WriteLine("Strategy: {0}", response.Attributes.Strategy);
                Console.WriteLine("----------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async get subscription attribute failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void SetSubscriptionAttributeAsync(SetSubscriptionAttributeRequest request)
        {
            try
            {
                var response = await Topic.SetSubscriptionAttributeAsync(request);
                Console.WriteLine("## Async set subscription attribute successfully, status code: {0}", response.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async set subscription attribute failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        private static async void PublishMessageAsync(PublishMessageRequest request)
        {
            try
            {
                var response = await Topic.PublishMessageAsync(request);
                Console.WriteLine("## Async publish message successfully, status code: {0}", response.HttpStatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("## Async publish message failed, exception info: " + ex.Message);
            }
            finally
            {
                AutoSetEvent.Set();
            }
        }

        #endregion

        #region Main Routine

        private static void Main(string[] args)
        {
            // do not delete message in order to change visibility
            Queue.DeleteOnReceived = false;

            #region Queue Releated Test Cases

            /* 1.1. Async create queue */
            var createQueueRequest = new CreateQueueRequest
            {
                QueueName = QueueName,
                Attributes =
                {
                    VisibilityTimeout = 30,
                    MaximumMessageSize = 40960,
                    MessageRetentionPeriod = 345600,
                    PollingWaitSeconds = 0
                }
            };
            try
            {
                Console.WriteLine("Async creating queue...");
                CreateQueueAsync(createQueueRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create queue failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 1.2. Async list queue */
            try
            {
                Console.WriteLine("Async listing queue...");
                do
                {
                    var listQueueRequest = new ListQueueRequest
                    {
                        QueueNamePrefix = QueueNamePrefix,
                        Marker = _nextMarker,
                        MaxReturns = 5
                    };
                    ListQueueAsync(listQueueRequest);

                    AutoSetEvent.WaitOne(); Console.WriteLine();

                } while (_nextMarker != string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine("List queue failed, exception info: " + ex.Message);
            }

            /* 1.3. Async delete queue */
            var deleteQueueRequest = new DeleteQueueRequest(QueueName);
            try
            {
                Console.WriteLine("Async deleting queue...");
                DeleteQueueAsync(deleteQueueRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete queue failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 1.4. Async create queue again */
            try
            {
                Console.WriteLine("Async creating queue again for further tests...");
                CreateQueueAsync(createQueueRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create queue failed again, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 1.5. Async get queue attributes */
            try
            {
                var getQueueAttributesRequest = new GetQueueAttributesRequest();
                Console.WriteLine("Async getting queue attributes...");
                GetQueueAttributesAsync(getQueueAttributesRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get queue attributes failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 1.6. Async set queue attributes */
            var setQueueAttributesRequest = new SetQueueAttributesRequest
            {
                Attributes =
                {
                    DelaySeconds = 0,
                    MaximumMessageSize = 10240,
                    MessageRetentionPeriod = 50000
                }
            };
            try
            {
                Console.WriteLine("Async setting queue attributes...");
                SetQueueAttributesAsync(setQueueAttributesRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Set queue attributes failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            #endregion

            #region Queue Messge Releated Test Cases

            /* 2.1. Async send message */
            try
            {
                var sendMessageRequest = new SendMessageRequest("{Id:123,Test:aaa,IsGood:true}");
                Console.WriteLine("Async sending message...");
                SendMessageAsync(sendMessageRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send message failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 2.2. Async receive message */
            try
            {
                var receiveMessageRequest = new ReceiveMessageRequest();
                Console.WriteLine("Async receiving message...");
                ReceiveMessageAsync(receiveMessageRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Receive message failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 2.3. Async change message visibility */
            try
            {
                var changeMessageVisibilityRequest = new ChangeMessageVisibilityRequest
                {
                    ReceiptHandle = _receiptHandle,
                    VisibilityTimeout = 1
                };
                Console.WriteLine("Async changing message visibility...");
                ChangeMessageVisibilityAsync(changeMessageVisibilityRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Change message visibility failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            Console.WriteLine("Wait 2 seconds for message to reactivate...");
            Thread.Sleep(2000);
            Console.WriteLine();

            /* 2.4. Async peek message */
            try
            {
                var peekMessageRequest = new PeekMessageRequest();
                Console.WriteLine("Async peeking message...");
                PeekMessageAsync(peekMessageRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Peek message failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 2.5. Async receive message again for deletion */
            try
            {
                var receiveMessageRequest = new ReceiveMessageRequest();
                Console.WriteLine("Async receiving message again...");
                ReceiveMessageAsync(receiveMessageRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Receive message failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 2.6. Async delete message */
            try
            {
                var deleteMessageRequest = new DeleteMessageRequest(_receiptHandle);
                Console.WriteLine("Async deleting message...");
                DeleteMessageAsync(deleteMessageRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async beginDeleteMessage failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 3.1. Async batch send message */
            try
            {
                var requests = new List<SendMessageRequest>();
                for (uint i = 0; i < BatchSize; i++)
                {
                    requests.Add(new SendMessageRequest("Test message" + i, 0, i + 1));
                }
                BatchSendMessageRequest batchSendRequest = new BatchSendMessageRequest()
                {
                    Requests = requests
                };
                Console.WriteLine("Async batch sending message...");
                BatchSendMessageAsync(batchSendRequest);

                AutoSetEvent.WaitOne(); Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BeginBatchSend message failed, exception info: " + ex.Message);
            }

            Console.WriteLine("Wait 3 seconds to get all messages...");
            Thread.Sleep(3000);
            Console.WriteLine();

            /* 3.2. Async batch peek message */
            try
            {
                var batchPeekMessageRequest = new BatchPeekMessageRequest(BatchSize);
                Console.WriteLine("Async batch peeking message...");
                BatchPeekMessageAsync(batchPeekMessageRequest);

                AutoSetEvent.WaitOne(); Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BeginBatchPeek message failed, exception info: " + ex.Message);
            }

            /* 3.3. Async batch receive message */
            try
            {
                BatchReceiveMessageRequest batchReceiveMessageRequest = new BatchReceiveMessageRequest(BatchSize);
                Console.WriteLine("Async batch receiving message...");
                BatchReceiveMessageAsync(batchReceiveMessageRequest);

                AutoSetEvent.WaitOne(); Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Batch receive message failed, exception info: " + ex.Message);
            }

            /* 3.4. Async batch delete message */
            if (_batchReceiveMessageResponse != null && _batchReceiveMessageResponse.Messages.Count > 0)
            {
                try
                {
                    List<string> receiptHandles = new List<string>();
                    foreach (var message in _batchReceiveMessageResponse.Messages)
                    {
                        receiptHandles.Add(message.ReceiptHandle);
                    }
                    var batchDeleteMessageRequest = new BatchDeleteMessageRequest()
                    {
                        ReceiptHandles = receiptHandles
                    };
                    Console.WriteLine("Async batch deleting message...");
                    BatchDeleteMessageAsync(batchDeleteMessageRequest);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Batch delete message failed, exception info: " + ex.Message);
                }
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            #endregion

            #region Topic Releated Test Cases

            /* ----------!! Creating Topic may charge !!---------- */

            /* 3.1. Async create topic */
            var createTopicRequest = new CreateTopicRequest
            {
                TopicName = TopicName,
                Attributes =
                {
                    MaximumMessageSize = 40960,
                    MessageRetentionPeriod = 345600
                }
            };
            try
            {
                Console.WriteLine("Async creating topic...");
                CreateTopicAsync(createTopicRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create topic failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 3.2. Async list topic */
            try
            {
                Console.WriteLine("Async listing topic...");
                do
                {
                    var listTopicRequest = new ListTopicRequest
                    {
                        Marker = _nextMarker,
                        MaxReturns = 5
                    };
                    ListTopicAsync(listTopicRequest);

                    AutoSetEvent.WaitOne(); Console.WriteLine();

                } while (_nextMarker != string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine("List topic failed, exception info: " + ex.Message);
            }

            /* 3.3. Async delete topic */
            try
            {
                var deleteTopicRequest = new DeleteTopicRequest(TopicName);
                Console.WriteLine("Async deleting topic...");
                DeleteTopicAsync(deleteTopicRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete topic failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 3.4. Async create topic again */
            try
            {
                Console.WriteLine("Async creating topic again for further tests...");
                CreateTopicAsync(createTopicRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create topic failed again, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 3.5. Async get topic attributes */
            try
            {
                var getTopicAttributesRequest = new GetTopicAttributesRequest();
                Console.WriteLine("Async getting topic attributes...");
                GetTopicAttributesAsync(getTopicAttributesRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get topic attributes failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 3.6. Async set topic attributes */
            try
            {
                var setTopicAttributesRequest = new SetTopicAttributesRequest
                {
                    Attributes =
                    {
                        MaximumMessageSize = 10240
                    }
                };
                Console.WriteLine("Async setting topic attributes...");
                SetTopicAttributesAsync(setTopicAttributesRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Set topic attributes failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 3.7. Async subscribe */
            try
            {
                var subscribeRequest = new SubscribeRequest(TopicName, "http://example.com");
                Console.WriteLine("Async subscribing...");
                SubscribeAsync(subscribeRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subscribe failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 3.8.  Async get subscription attribute */
            try
            {
                var getSubscriptionAttribute = new GetSubscriptionAttributeRequest(TopicName);
                Console.WriteLine("Async getting subscription attribute...");
                GetSubscriptionAttributeAsync(getSubscriptionAttribute);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get subscription attribute failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 3.9. Async set subscription attribute */
            try
            {
                var setSubscriptionAttribute = new SetSubscriptionAttributeRequest(TopicName,
                    new SubscriptionAttributes
                    {
                        Strategy = SubscriptionAttributes.NotifyStrategy.EXPONENTIAL_DECAY_RETRY
                    });
                Console.WriteLine("Async setting subscription attribute...");
                SetSubscriptionAttributeAsync(setSubscriptionAttribute);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Set subscription attribute failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 3.10. Async unsubscribe */
            try
            {
                var unsubscribeRequest = new UnsubscribeRequest(TopicName);
                Console.WriteLine("Async unsubscribe...");
                UnsubscribeAsync(unsubscribeRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unsubscribe failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            /* 3.11. Async publish message */
            try
            {
                var publishMessageRequest = new PublishMessageRequest("some message");
                Console.WriteLine("Async publishing message...");
                PublishMessageAsync(publishMessageRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Publish Message failed, exception info: " + ex.Message);
            }

            AutoSetEvent.WaitOne(); Console.WriteLine();

            #endregion

            Console.ReadKey();
        }

        #endregion
    }
}
