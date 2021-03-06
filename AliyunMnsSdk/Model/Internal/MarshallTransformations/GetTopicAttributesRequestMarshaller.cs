﻿using AliyunMnsSdk.Runtime;
using AliyunMnsSdk.Runtime.Internal;
using AliyunMnsSdk.Runtime.Internal.Transform;
using AliyunMnsSdk.Util;

namespace AliyunMnsSdk.Model.Internal.MarshallTransformations
{
    /// <summary>
    /// GetTopicAttributes Request Marshaller
    /// </summary>       
    internal class GetTopicAttributesRequestMarshaller : IMarshaller<IRequest, GetTopicAttributesRequest>, IMarshaller<IRequest, WebServiceRequest>
    {
        public IRequest Marshall(WebServiceRequest input)
        {
            return this.Marshall((GetTopicAttributesRequest)input);
        }

        public IRequest Marshall(GetTopicAttributesRequest publicRequest)
        {
            IRequest request = new DefaultRequest(publicRequest, MNSConstants.MNS_SERVICE_NAME);
            request.HttpMethod = HttpMethod.GET.ToString();
            request.ResourcePath = MNSConstants.MNS_TOPIC_PRE_RESOURCE + publicRequest.TopicName;
            return request;
        }
    }
}