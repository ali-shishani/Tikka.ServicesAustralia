using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Models.Responses
{
    public class CareRecipientSearchResponse
    {
        public string CareRecipientId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string BirthDate { get; set; }

        public string Gender { get; set; }

        public string TempAccessKey { get; set; }

        public string TempAccessExpiry { get; set; }

    }
}
