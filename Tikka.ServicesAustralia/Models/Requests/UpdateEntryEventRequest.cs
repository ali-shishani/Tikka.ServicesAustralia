using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Models.Requests
{
    public class UpdateEntryEventRequest
    {
        public string? EntryTypeCode { get; set; }

        public string? ExternalReferenceId { get; set; }

        public string? ExternalVersionNumber { get; set; }

        public string? CareRecipientId { get; set; }

        public string? EntryDate { get; set; }

        public string? PensionerStatusCode { get; set; }

        public string? CentrelinkCrn { get; set; }

        public string? DvaUin { get; set; }

        public string? AwardOrSettlementTypeCode { get; set; }

        public string? AwardOrSettlementDate { get; set; }

        public string? AdjustedSubsidyIndicator { get; set; }

        public string? ReceivingPriorCare { get; set; }

        public string? PreEntryLeaveDate { get; set; }

        public string? AccomPmntTypeCode { get; set; }

        public string? AccomPmntStatusCode { get; set; }

        public string? BondRolloverIndicator { get; set; }

        public int? BondAmount { get; set; }

        public string? UnfundedPriorEntryDate { get; set; }

        public string? HomelessIndigenousCareCode { get; set; }

        public string? MeansTestingOptIn { get; set; }

        public string? NsafBeenSighted { get; set; }

        public string? AgreedAccomodationPrice { get; set; }

        public string? AccommodationAgreementDate { get; set; }

        public string? AccomPmntArrangementCode { get; set; }

        public int? AccomPmntRadAmount { get; set; }

        public double? AccomPmntDapAmount { get; set; }
    }
}
