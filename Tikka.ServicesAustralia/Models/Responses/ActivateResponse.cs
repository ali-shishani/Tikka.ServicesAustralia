using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Models.Responses
{
    public class ActivateResponse
    {
        public string orgId { get; set; }
        public string deviceName { get; set; }
        public string deviceStatus { get; set; }
        public string deviceExpiry { get; set; }
        public string keyStatus { get; set; }
        public string keyExpiry { get; set; }

    }
}
