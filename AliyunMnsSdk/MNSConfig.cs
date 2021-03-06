/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 */

using AliyunMnsSdk.Runtime;

namespace AliyunMnsSdk
{
    /// <summary>
    /// Configuration for accessing Aliyun MNS Service
    /// </summary>
    public partial class MNSConfig : ClientConfig
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MNSConfig()
        {
        }

        /// <summary>
        /// Gets the ServiceVersion property.
        /// </summary>
        public override string ServiceVersion
        {
            get
            {
                return "2015-06-06";
            }
        }

        /// <summary>
        /// Gets the ServiceName property.
        /// </summary>
        public override string ServiceName
        {
            get
            {
                return "Aliyun.MNS";
            }
        }
    }
}
