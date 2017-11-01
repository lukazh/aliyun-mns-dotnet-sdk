using AliyunMnsSdk.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliyunMnsSdk.Model
{
    public partial class BatchPeekMessageResponse : WebServiceResponse
    {
        private List<Message> _messages = new List<Message>();

        public List<Message> Messages
        {
            get { return this._messages; }
            set { this._messages = value; }
        }
    }
}
