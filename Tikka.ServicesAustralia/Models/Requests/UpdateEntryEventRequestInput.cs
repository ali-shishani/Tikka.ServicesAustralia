using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Models.Requests
{
    public class UpdateEntryEventRequestInput : UpdateEntryEventRequest
    {
        public string? ServiceNapsId { get; set; }
    }
}
