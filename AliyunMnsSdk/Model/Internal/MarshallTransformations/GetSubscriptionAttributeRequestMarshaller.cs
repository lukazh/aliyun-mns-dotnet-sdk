using AliyunMnsSdk.Runtime;
using AliyunMnsSdk.Runtime.Internal;
using AliyunMnsSdk.Runtime.Internal.Transform;
using AliyunMnsSdk.Util;

namespace AliyunMnsSdk.Model.Internal.MarshallTransformations
{
    /// <summary>
    /// GetSubscriptionAttribute Request Marshaller
    /// </summary>       
    internal class GetSubscriptionAttributeRequestMarshaller : IMarshaller<IRequest, GetSubscriptionAttributeRequest>, IMarshaller<IRequest, WebServiceRequest>
    {
        public IRequest Marshall(WebServiceRequest input)
        {
            return this.Marshall((GetSubscriptionAttributeRequest)input);
        }

        public IRequest Marshall(GetSubscriptionAttributeRequest publicRequest)
        {
            IRequest request = new DefaultRequest(publicRequest, MNSConstants.MNS_SERVICE_NAME);
            request.HttpMethod = HttpMethod.GET.ToString();
            request.ResourcePath = MNSConstants.MNS_TOPIC_PRE_RESOURCE + publicRequest.TopicName
                + MNSConstants.MNS_SUBSCRIBE_PRE_RESOURCE + publicRequest.SubscriptionName;
            return request;
        }
    }
}