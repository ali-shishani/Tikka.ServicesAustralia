using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Models.Responses
{
    public class EntryEventHistoryResponse
    {
        public string? EntryTypeCode { get; set; }

        public string? externalReferenceId { get; set; }

        public string? externalVersionNumber { get; set; }

        public string? serviceNapsId { get; set; }

        public string? careRecipientId { get; set; }

        public string? entryDate { get; set; }

        public string? pensionerStatusCode { get; set; }

        public string? centrelinkCrn { get; set; }

        public string? dvaUin { get; set; }

        public string? awardOrSettlementTypeCode { get; set; }

        public string? awardOrSettlementDate { get; set; }

        public string? adjustedSubsidyIndicator { get; set; }

        public string? receivingPriorCare { get; set; }

        public string? preEntryLeaveDate { get; set; }

        public string? accomPmntTypeCode { get; set; }

        public string? accomPmntStatusCode { get; set; }

        public string? bondRolloverIndicator { get; set; }

        public int bondAmount { get; set; }

        public string? unfundedPriorEntryDate { get; set; }

        public string? homelessIndigenousCareCode { get; set; }

        public string? meansTestingOptIn { get; set; }

        public string? nsafBeenSighted { get; set; }

        public string? agreedAccomodationPrice { get; set; }

        public string? accommodationAgreementDate { get; set; }

        public string? accomPmntArrangementCode { get; set; }

        public string? accomPmntRadAmount { get; set; }
        public string? accomPmntDapAmount { get; set; }

        public string? palliativeOnEntry { get; set; }

        public string? departureDate { get; set; }

        public string? departureReasonCode { get; set; }

        public string? wardTypeCode { get; set; }
        public string? eventId { get; set; }

        public string? versionNumber { get; set; }

        public string? status { get; set; }

        public string? channel { get; set; }

        public string? careRecipientName { get; set; }

        public string? serviceId { get; set; }

        public string? accomPmntTypeText { get; set; }

        public string? pensionerStatusText { get; set; }
        public string? awardOrSettlementTypeText { get; set; }

        public string? accomPmntStatusText { get; set; }

        public string? homelessIndigenousCareText { get; set; }

        public string? accomPmntArrangementText { get; set; }
        public string? departureReasonText { get; set; }

        public string? createdAtDateTime { get; set; }

        public string? updatedAtDateTime { get; set; }
    }
}
