using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Models.Responses
{
    public class GetEntryEventDetailsResponse
    {
        public string? EventId { get; set; }

        public string? VersionNumber { get; set; }

        public string? Status { get; set; }

        public string? Channel { get; set; }

        public string? CareRecipientName { get; set; }

        public string? AccomPmntTypeText { get; set; }

        public string? PensionerStatusText { get; set; }

        public string? AwardOrSettlementTypeText { get; set; }

        public string? AccomPmntStatusText { get; set; }

        public string? HomelessIndigenousCareText { get; set; }

        public string? AccomPmntArrangementText { get; set; }

        public string? DepartureReasonText { get; set; }

        public string? CreatedAtDateTime { get; set; }

        public string? UpdatedAtDateTime { get; set; }
    }
}
