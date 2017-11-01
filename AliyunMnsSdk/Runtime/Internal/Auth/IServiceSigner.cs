using AliyunMnsSdk.Runtime.Internal.Util;

namespace AliyunMnsSdk.Runtime.Internal.Auth
{
    /// <summary>
    /// Interface for signing MNS request.
    /// </summary>
    public partial interface IServiceSigner
    {
         void Sign(IRequest request, RequestMetrics metrics, string accessKeyId, string secretAccessKey, string stsToken);
    }
}
