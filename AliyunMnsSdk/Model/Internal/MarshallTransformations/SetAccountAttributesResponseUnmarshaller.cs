using System;
using System.Net;
using AliyunMnsSdk.Runtime;
using AliyunMnsSdk.Runtime.Internal;
using AliyunMnsSdk.Runtime.Internal.Transform;

namespace AliyunMnsSdk.Model.Internal.MarshallTransformations
{
    /// <summary>
    /// Response Unmarshaller for SetQueueAttributes operation
    /// </summary>  
    internal class SetAccountAttributesResponseUnmarshaller : XmlResponseUnmarshaller
    {
        public override WebServiceResponse Unmarshall(XmlUnmarshallerContext context)
        {
            SetAccountAttributesResponse response = new SetAccountAttributesResponse();
            // Nothing need to do with this response here
            return response;
        }

        public override AliyunServiceException UnmarshallException(XmlUnmarshallerContext context, Exception innerException, HttpStatusCode statusCode)
        {
            ErrorResponse errorResponse = ErrorResponseUnmarshaller.Instance.Unmarshall(context);
            return new MNSException(errorResponse.Message, innerException, errorResponse.Code, errorResponse.RequestId, errorResponse.HostId, statusCode);
        }

        private static SetAccountAttributesResponseUnmarshaller _instance = new SetAccountAttributesResponseUnmarshaller();
        public static SetAccountAttributesResponseUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}