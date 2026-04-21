using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Utilities
{
    public static class Extenssions
    {
        public static void addStandardHeaders(this HttpClient client, string orgRA, string deviceName, string productId)
        {
            // standard headers used in activate device and
            client.DefaultRequestHeaders.Add("dhs-auditid", orgRA);
            client.DefaultRequestHeaders.Add("dhs-auditidtype", "http://ns.humanservices.gov.au/audit/type/proda/organisation");
            client.DefaultRequestHeaders.Add("dhs-subjectid", deviceName);
            client.DefaultRequestHeaders.Add("dhs-subjectidtype", "http://ns.humanservices.gov.au/audit/type/proda/device");
            client.DefaultRequestHeaders.Add("dhs-productid", productId);
            client.DefaultRequestHeaders.Add("dhs-messageid", Guid.NewGuid().ToString());
            client.DefaultRequestHeaders.Add("dhs-correlationid", Guid.NewGuid().ToString());
        }
    }
}
