using System.Collections.Generic;
using AliyunMnsSdk.Runtime;
using AliyunMnsSdk.Runtime.Internal;
using AliyunMnsSdk.Runtime.Internal.Transform;
using AliyunMnsSdk.Util;

namespace AliyunMnsSdk.Model.Internal.MarshallTransformations
{
    /// <summary>
    /// ReceiveMessage Request Marshaller
    /// </summary>       
    public class ReceiveMessageRequestMarshaller : IMarshaller<IRequest, ReceiveMessageRequest> , IMarshaller<IRequest,WebServiceRequest>
    {
        public IRequest Marshall(WebServiceRequest input)
        {
            return this.Marshall((ReceiveMessageRequest)input);
        }
    
        public IRequest Marshall(ReceiveMessageRequest publicRequest)
        {
            IRequest request = new DefaultRequest(publicRequest, MNSConstants.MNS_SERVICE_NAME);
            request.HttpMethod = HttpMethod.GET.ToString();
            request.ResourcePath = MNSConstants.MNS_MESSAGE_PRE_RESOURCE + publicRequest.QueueName 
                + MNSConstants.MNS_MESSAGE_SUB_RESOURCE;
            PopulateSpecialParameters(publicRequest, request.Parameters);
            return request;
        }

        private static void PopulateSpecialParameters(ReceiveMessageRequest request, IDictionary<string, string> paramters)
        {
            if (request.IsSetWaitSeconds())
            {
                paramters.Add(MNSConstants.MNS_PARAMETER_WAIT_SECONDS, request.WaitSeconds.ToString());
            }
        }
    }
}