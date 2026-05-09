using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Models.Responses
{
    public class MessageResponse
    {
        public int Code { get; set; }

        public string CodeType { get; set; }

        public string Message { get; set; }
    }
}
