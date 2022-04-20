using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalChat.Models
{
    public class Message
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public long DatePosted { get; set; }
    }
}
