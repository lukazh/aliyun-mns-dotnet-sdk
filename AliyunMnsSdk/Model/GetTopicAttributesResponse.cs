﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 */

using AliyunMnsSdk.Runtime;

namespace AliyunMnsSdk.Model
{
    /// <summary>
    /// Response for MNS GetTopicAttributes service
    /// </summary>
    public partial class GetTopicAttributesResponse : WebServiceResponse
    {
        private TopicAttributes _attributes = new TopicAttributes();

        /// <summary>
        /// Gets and sets the property Attributes. 
        /// </summary>
        public TopicAttributes Attributes
        {
            get { return this._attributes; }
            set { this._attributes = value; }
        }
    }
}