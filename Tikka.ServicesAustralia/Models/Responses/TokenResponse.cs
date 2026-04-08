using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Models.Responses
{
    public class TokenResponse
    {
        public string device_expiry { get; set; }
        public string key_expiry { get; set; }
        public int expires_in { get; set; }
        public string access_token { get; set; }
    }
}
