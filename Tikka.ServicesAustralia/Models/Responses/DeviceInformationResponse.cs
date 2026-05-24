using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Models.Responses
{
    public class DeviceInformationResponse
    {
        public string DeviceName { get; set; }

        public string ClientId { get; set; }

        public string ProductId { get; set; }

        public string OrganisationRA { get; set; }

        public string ServiceNapsId { get; set; }

        public string AgedCareResidentialServiceId { get; set; }

        public string AgedCareHomeServiceId { get; set; }

        public string TokenAud { get; set; }

        public string DeviceExpiry { get; set; }

        public string KeyExpiry { get; set; }
    }
}
