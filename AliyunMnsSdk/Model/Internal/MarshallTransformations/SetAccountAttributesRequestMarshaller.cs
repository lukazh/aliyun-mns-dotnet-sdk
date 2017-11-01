using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using AliyunMnsSdk.Runtime;
using AliyunMnsSdk.Runtime.Internal;
using AliyunMnsSdk.Runtime.Internal.Transform;
using AliyunMnsSdk.Util;
using System.Xml;
using System.Text;

namespace AliyunMnsSdk.Model.Internal.MarshallTransformations
{
    /// <summary>
    /// SetQueueAttributes Request Marshaller
    /// </summary>       
    internal class SetAccountAttributesRequestMarshaller : IMarshaller<IRequest, SetAccountAttributesRequest>, IMarshaller<IRequest, WebServiceRequest>
    {
        public IRequest Marshall(WebServiceRequest input)
        {
            return this.Marshall((SetAccountAttributesRequest)input);
        }
    
        public IRequest Marshall(SetAccountAttributesRequest publicRequest)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
            writer.WriteStartDocument();
            writer.WriteStartElement(MNSConstants.XML_ROOT_ACCOUNT, MNSConstants.MNS_XML_NAMESPACE);
            var attrs = publicRequest.Attributes;
            if (attrs.IsSetLoggingBucket())
                writer.WriteElementString(MNSConstants.XML_ELEMENT_LOGGING_BUCKET, attrs.LoggingBucket);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            IRequest request = new DefaultRequest(publicRequest, MNSConstants.MNS_SERVICE_NAME);
            request.HttpMethod = HttpMethod.PUT.ToString();
            request.ContentStream = stream;
            request.ResourcePath = "";
            PopulateSpecialParameters(request.Parameters);
            return request;
        }

        private static void PopulateSpecialParameters(IDictionary<string, string> paramters)
        {
            paramters.Add(MNSConstants.MNS_PARAMETER_ACCOUNT_META, "true");
        }
    }
}