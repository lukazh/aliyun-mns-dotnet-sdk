using System;
using System.Net;
using AliyunMnsSdk.Runtime;
using AliyunMnsSdk.Runtime.Internal;
using AliyunMnsSdk.Runtime.Internal.Transform;
using AliyunMnsSdk.Util;

namespace AliyunMnsSdk.Model.Internal.MarshallTransformations
{
    /// <summary>
    /// Response Unmarshaller for CreateQueue operation
    /// </summary>  
    internal class CreateQueueResponseUnmarshaller : XmlResponseUnmarshaller
    {
        public override WebServiceResponse Unmarshall(XmlUnmarshallerContext context)
        {
            var response = new CreateQueueResponse();
            if (context.ResponseData.IsHeaderPresent(HttpHeader.LocationHeader))
                response.QueueUrl = context.ResponseData.GetHeaderValue(HttpHeader.LocationHeader);
            return response;
        }

        public override AliyunServiceException UnmarshallException(XmlUnmarshallerContext context, Exception innerException, HttpStatusCode statusCode)
        {
            ErrorResponse errorResponse = ErrorResponseUnmarshaller.Instance.Unmarshall(context);
            if (errorResponse.Code != null && errorResponse.Code.Equals(MNSErrorCode.QueueAlreadyExist))
            {
                return new QueueAlreadyExistException(errorResponse.Message, innerException, errorResponse.Code, errorResponse.RequestId, errorResponse.HostId, statusCode);
            }
            if (errorResponse.Code != null && errorResponse.Code.Equals(MNSErrorCode.InvalidArgument))
            {
                return new InvalidArgumentException(errorResponse.Message, innerException, errorResponse.Code, errorResponse.RequestId, errorResponse.HostId, statusCode);
            }
            return new MNSException(errorResponse.Message, innerException, errorResponse.Code, errorResponse.RequestId, errorResponse.HostId, statusCode);
        }

        private static CreateQueueResponseUnmarshaller _instance = new CreateQueueResponseUnmarshaller();        
        public static CreateQueueResponseUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}