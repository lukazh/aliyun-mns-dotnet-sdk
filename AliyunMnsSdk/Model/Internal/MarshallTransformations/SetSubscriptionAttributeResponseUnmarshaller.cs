using System;
using System.Net;
using AliyunMnsSdk.Runtime;
using AliyunMnsSdk.Runtime.Internal;
using AliyunMnsSdk.Runtime.Internal.Transform;

namespace AliyunMnsSdk.Model.Internal.MarshallTransformations
{
    /// <summary>
    /// Response Unmarshaller for SetSubscriptionAttribute operation
    /// </summary>  
    internal class SetSubscriptionAttributeResponseUnmarshaller : XmlResponseUnmarshaller
    {
        public override WebServiceResponse Unmarshall(XmlUnmarshallerContext context)
        {
            SetSubscriptionAttributeResponse response = new SetSubscriptionAttributeResponse();
            // Nothing need to do with this response here
            return response;
        }

        public override AliyunServiceException UnmarshallException(XmlUnmarshallerContext context, Exception innerException, HttpStatusCode statusCode)
        {
            ErrorResponse errorResponse = ErrorResponseUnmarshaller.Instance.Unmarshall(context);
            if (errorResponse.Code != null && errorResponse.Code.Equals(MNSErrorCode.SubscriptionNotExist))
            {
                return new SubscriptionNotExistException(errorResponse.Message, innerException, errorResponse.Code, errorResponse.RequestId, errorResponse.HostId, statusCode);
            }
            return new MNSException(errorResponse.Message, innerException, errorResponse.Code, errorResponse.RequestId, errorResponse.HostId, statusCode);
        }

        private static SetSubscriptionAttributeResponseUnmarshaller _instance = new SetSubscriptionAttributeResponseUnmarshaller();
        public static SetSubscriptionAttributeResponseUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}