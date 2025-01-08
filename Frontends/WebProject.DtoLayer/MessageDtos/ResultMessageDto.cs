﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProject.DtoLayer.MessageDtos
{
    public class ResultMessageDto
    {
        public int UserMessageId { get; set; }
        public string SenderId { get; set; }
        public string? SenderName { get; set; }
        public string ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public bool IsRead { get; set; }
        public DateTime MessageDate { get; set; }
    }
}
