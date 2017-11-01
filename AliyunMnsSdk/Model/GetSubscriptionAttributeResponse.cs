﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 */

using AliyunMnsSdk.Runtime;

namespace AliyunMnsSdk.Model
{
    /// <summary>
    /// Response for MNS GetSubscriptionAttribute service
    /// </summary>
    public partial class GetSubscriptionAttributeResponse : WebServiceResponse
    {
        private SubscriptionAttributes _attributes = new SubscriptionAttributes();

        /// <summary>
        /// Gets and sets the property Attributes. 
        /// </summary>
        public SubscriptionAttributes Attributes
        {
            get { return this._attributes; }
            set { this._attributes = value; }
        }
    }
}