using AliyunMnsSdk.Runtime;
using AliyunMnsSdk.Runtime.Internal;
using AliyunMnsSdk.Runtime.Internal.Transform;
using AliyunMnsSdk.Util;

namespace AliyunMnsSdk.Model.Internal.MarshallTransformations
{
    /// <summary>
    /// DeleteTopic Request Marshaller
    /// </summary>       
    internal class DeleteTopicRequestMarshaller : IMarshaller<IRequest, DeleteTopicRequest>, IMarshaller<IRequest, WebServiceRequest>
    {
        public IRequest Marshall(WebServiceRequest input)
        {
            return this.Marshall((DeleteTopicRequest)input);
        }

        public IRequest Marshall(DeleteTopicRequest publicRequest)
        {
            IRequest request = new DefaultRequest(publicRequest, MNSConstants.MNS_SERVICE_NAME);
            request.HttpMethod = HttpMethod.DELETE.ToString();
            request.ResourcePath = MNSConstants.MNS_TOPIC_PRE_RESOURCE + publicRequest.TopicName;
            return request;
        }
    }
}