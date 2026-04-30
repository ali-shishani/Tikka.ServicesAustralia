using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Models.Responses
{
    public class MessageResponse
    {
        public string? Type { get; set; }

        public string? ShortText { get; set; }

        public string? LongText { get; set; }
    }
}
