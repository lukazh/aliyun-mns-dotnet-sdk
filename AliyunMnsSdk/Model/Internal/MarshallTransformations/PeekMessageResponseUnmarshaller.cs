using System;
using System.Net;
using System.Xml.Serialization;
using AliyunMnsSdk.Runtime;
using AliyunMnsSdk.Runtime.Internal;
using AliyunMnsSdk.Runtime.Internal.Transform;
using AliyunMnsSdk.Util;
using System.Xml;

namespace AliyunMnsSdk.Model.Internal.MarshallTransformations
{
    /// <summary>
    /// Response Unmarshaller for PeekMessage operation
    /// </summary>  
    public class PeekMessageResponseUnmarshaller : XmlResponseUnmarshaller
    {
        public override WebServiceResponse Unmarshall(XmlUnmarshallerContext context)
        {
            XmlTextReader reader = new XmlTextReader(context.ResponseStream);
            Message message = new Message();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.LocalName)
                        {
                            case MNSConstants.XML_ELEMENT_MESSAGE_ID:
                                reader.Read();
                                message.Id = reader.Value;
                                break;
                            case MNSConstants.XML_ELEMENT_MESSAGE_BODY_MD5:
                                reader.Read();
                                message.BodyMD5 = reader.Value;
                                break;
                            case MNSConstants.XML_ELEMENT_MESSAGE_BODY:
                                reader.Read();
                                message.Body = reader.Value;
                                break;
                            case MNSConstants.XML_ELEMENT_ENQUEUE_TIME:
                                reader.Read();
                                message.EnqueueTime = AliyunSDKUtils.ConvertFromUnixEpochSeconds(long.Parse(reader.Value));
                                break;
                            case MNSConstants.XML_ELEMENT_FIRST_DEQUEUE_TIME:
                                reader.Read();
                                message.FirstDequeueTime = AliyunSDKUtils.ConvertFromUnixEpochSeconds(long.Parse(reader.Value));
                                break;
                            case MNSConstants.XML_ELEMENT_DEQUEUE_COUNT:
                                reader.Read();
                                message.DequeueCount = uint.Parse(reader.Value);
                                break;
                            case MNSConstants.XML_ELEMENT_PRIORITY:
                                reader.Read();
                                message.Priority = uint.Parse(reader.Value);
                                break;
                        }
                        break;
                }
            }
            reader.Close();
            return new PeekMessageResponse()
            {
                Message = message
            };
        }
        
        public override AliyunServiceException UnmarshallException(XmlUnmarshallerContext context, Exception innerException, HttpStatusCode statusCode)
        {
            ErrorResponse errorResponse = ErrorResponseUnmarshaller.Instance.Unmarshall(context);
            if (errorResponse.Code != null && errorResponse.Code.Equals(MNSErrorCode.QueueNotExist))
            {
                return new QueueNotExistException(errorResponse.Message, innerException, errorResponse.Code, errorResponse.RequestId, errorResponse.HostId, statusCode);
            }
            if (errorResponse.Code != null && errorResponse.Code.Equals(MNSErrorCode.MessageNotExist))
            {
                return new MessageNotExistException(errorResponse.Message, innerException, errorResponse.Code, errorResponse.RequestId, errorResponse.HostId, statusCode);
            }
            return new MNSException(errorResponse.Message, innerException, errorResponse.Code, errorResponse.RequestId, errorResponse.HostId, statusCode);
        }

        private static PeekMessageResponseUnmarshaller _instance = new PeekMessageResponseUnmarshaller();
        public static PeekMessageResponseUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}