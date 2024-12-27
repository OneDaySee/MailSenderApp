using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSenderApp.Models
{
    public class MailSendDto
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ToMailAddress { get; set; }
    }
}