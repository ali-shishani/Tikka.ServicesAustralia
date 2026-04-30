using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Models.Requests
{
    public class CreateEntryEventRequestInput: CreateEntryEventRequest
    {
        public string? ServiceNapsId { get; set; }
    }
}
