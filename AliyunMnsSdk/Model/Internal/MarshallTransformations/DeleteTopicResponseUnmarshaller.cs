using System;
using System.Net;
using AliyunMnsSdk.Runtime;
using AliyunMnsSdk.Runtime.Internal;
using AliyunMnsSdk.Runtime.Internal.Transform;

namespace AliyunMnsSdk.Model.Internal.MarshallTransformations
{
    /// <summary>
    /// Response Unmarshaller for DeleteQueue operation
    /// </summary>  
    internal class DeleteTopicResponseUnmarshaller : XmlResponseUnmarshaller
    {
        public override WebServiceResponse Unmarshall(XmlUnmarshallerContext context)
        {
            DeleteTopicResponse response = new DeleteTopicResponse();
            // Nothing need to do with this response here
            return response;
        }

        public override AliyunServiceException UnmarshallException(XmlUnmarshallerContext context, Exception innerException, HttpStatusCode statusCode)
        {
            ErrorResponse errorResponse = ErrorResponseUnmarshaller.Instance.Unmarshall(context);
            return new MNSException(errorResponse.Message, innerException, errorResponse.Code, errorResponse.RequestId, errorResponse.HostId, statusCode);
        }

        private static DeleteTopicResponseUnmarshaller _instance = new DeleteTopicResponseUnmarshaller();
        public static DeleteTopicResponseUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}