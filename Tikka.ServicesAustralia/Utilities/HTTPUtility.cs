using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http.Headers;
using Tikka.ServicesAustralia.Models.Requests;
using static System.Net.WebRequestMethods;

namespace Tikka.ServicesAustralia.Utilities
{
    public class HTTPUtility
    {
        // proxy for http requests
        private WebProxy proxy = new();

        public HTTPUtility(string proxyUrl, string proxyUser, string proxyPass)
        {
            if (proxyUrl?.Length > 0)
            {
                proxy.Address = new Uri(proxyUrl);
                proxy.Credentials = new NetworkCredential(proxyUser, proxyPass);
            }
        }

        public string buildActivationRequest(string orgRa, string activationCode, string jwk)
        {
            // open json object
            return "{" + "\"orgId\": \"" + orgRa + "\", \"otac\": \"" + activationCode + "\",\"key\": " + jwk + "}";
        }

        public string buildAccessTokenRequest(string clientId, string assertion)
        {
            // Builds an access token request payload

            return "client_id=" + clientId + "&grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer&assertion=" + assertion;
        }

        private byte[] prepareRequestBody(string reqBody)
        {
            var encoder = new ASCIIEncoding();
            return encoder.GetBytes(reqBody);
        }

        private void addStandardHeaders(ref HttpWebRequest request, string orgRA, string deviceName, string productId)
        {
            // standard headers used in activate device and
            request.Headers.Add("dhs-auditid", orgRA);
            request.Headers.Add("dhs-auditidtype", "http://ns.humanservices.gov.au/audit/type/proda/organisation");
            request.Headers.Add("dhs-subjectid", deviceName);
            request.Headers.Add("dhs-subjectidtype", "http://ns.humanservices.gov.au/audit/type/proda/device");
            request.Headers.Add("dhs-productid", productId);
            request.Headers.Add("dhs-messageid", Guid.NewGuid().ToString());
            request.Headers.Add("dhs-correlationid", Guid.NewGuid().ToString());
        }

        public string executeActivateDeviceRequest(string orgId, string deviceName, string productId, string reqBody)
        {
            string responseData;
            try
            {
                // create the http request object & apply proxy
                var url = "https://test.5.rsp.humanservices.gov.au/piaweb/api/b2b/v1/devices/" + deviceName + "/jwk";
                var request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Proxy = proxy;

                // convert request body to byte array
                var reqBodyBytes = prepareRequestBody(reqBody);

                // build request headers
                addStandardHeaders(ref request, orgId, deviceName, productId);
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.ContentLength = reqBodyBytes.Length;

                //send the request
                var requestStream = request.GetRequestStream() as Stream;
                requestStream.Write(reqBodyBytes, 0, reqBodyBytes.Length);
                requestStream.Close();

                // get response
                var responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                // handle errors
                // get the error message and display in log window
                var responseReader = new StreamReader(ex.Response.GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (Exception)
            {
                throw;
            }

            return responseData;
        }

        public string executeGetAccessTokenRequest(string reqBody)
        {
            string responseData;
            try
            {
                // create the http request object & apply proxy
                var url = "https://vnd.proda.humanservices.gov.au/mga/sps/oauth/oauth20/token";
                var request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Proxy = proxy;

                // convert request body to byte array
                var reqBodyBytes = prepareRequestBody(reqBody);

                // build request headers
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                //request.Accept = "application/json";
                request.ContentLength = reqBodyBytes.Length;

                //send the request
                var requestStream = request.GetRequestStream() as Stream;
                requestStream.Write(reqBodyBytes, 0, reqBodyBytes.Length);
                requestStream.Close();

                // get response
                var responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                // handle errors
                // get the error message and display in log window
                var responseReader = new StreamReader(ex.Response.GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (Exception)
            {
                throw;
            }

            return responseData;
        }

        public string executeRefreshKeyRequest(string orgId, string deviceName, string productId, string reqBody, string accessToken)
        {
            string responseData;
            try
            {
                // create the http request object & apply proxy
                var url = "https://test.5.rsp.humanservices.gov.au/piaweb/api/b2b/v1/orgs/" + orgId + "/devices/" + deviceName + "/jwk";
                var request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Proxy = proxy;

                // convert request body to byte array
                var reqBodyBytes = prepareRequestBody(reqBody);

                // build request headers
                addStandardHeaders(ref request, orgId, deviceName, productId);
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.ContentLength = reqBodyBytes.Length;

                // The following Authorization header demonstrates how to use
                // a bearer token for authorisation.
                request.Headers.Add("Authorization", "Bearer " + accessToken);

                //send the request
                var requestStream = request.GetRequestStream() as Stream;
                requestStream.Write(reqBodyBytes, 0, reqBodyBytes.Length);
                requestStream.Close();

                // get response
                var responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                // handle errors
                var responseReader = new StreamReader(ex.Response.GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (Exception)
            {
                throw;
            }

            return responseData;
        }

        public async Task<string> executeCareRecipientSearch(string orgId, string deviceName, string productId, string accessToken,
            string? careRecipientId,
            string? firstName,
            string? middleName,
            string? lastName,
            string? gender,
            string? birthDate,
            string? postCode,
            string? State)
        {
            var responseBody = string.Empty;
            var paramDictionary = new Dictionary<string, string?>();

            paramDictionary.Add("careRecipientId", careRecipientId);
            paramDictionary.Add("firstName", firstName);
            paramDictionary.Add("middleName", middleName);
            paramDictionary.Add("lastName", lastName);
            paramDictionary.Add("gender", gender);
            paramDictionary.Add("birthDate", birthDate);
            paramDictionary.Add("postCode", postCode);
            paramDictionary.Add("State", State);

            var url = "https://test.healthclaiming.api.humanservices.gov.au/claiming/ext-vnd/acws/care-recipients/search/v2?";

            var first = true;
            paramDictionary.Where(kv => !string.IsNullOrWhiteSpace(kv.Value)).ToList().ForEach((kv) => {
                Debug.WriteLine(kv.Key + ":" + kv.Value);

                url += first ? "" : "&";
                url += kv.Key + "=" + kv.Value;

                first = false;
            });

            try
            {
                using var client = new HttpClient();
                client.addStandardHeaders(orgId, deviceName, productId);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", "3eb5ebd382f844fe50568e042787fcc3");
                
                var stream = await client.GetStreamAsync(url);
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    responseBody = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return await Task.FromResult(responseBody);
        }

        public async Task<string> executeQueryEntryEvents(string orgId, string deviceName, string productId, string accessToken,
            string? serviceNapsId,
            string? serviceId,
            string? careRecipientId,
            string? externalReferenceId,
            string? entryDateFrom,
            string? entryDateTo,
            int limit,
            string? page,
            string? sort)
        {
            var responseBody = string.Empty;
            var paramDictionary = new Dictionary<string, string?>();

            paramDictionary.Add("serviceNapsId", serviceNapsId);
            paramDictionary.Add("serviceId", serviceId);
            paramDictionary.Add("careRecipientId", careRecipientId);
            paramDictionary.Add("externalReferenceId", externalReferenceId);
            paramDictionary.Add("entryDateFrom", entryDateFrom);
            paramDictionary.Add("entryDateTo", entryDateTo);
            paramDictionary.Add("limit", limit.ToString());
            paramDictionary.Add("page", page);
            paramDictionary.Add("sort", sort);

            var url = "https://test.healthclaiming.api.humanservices.gov.au/claiming/ext-vnd/acws/residential-care/events/entry/v2?";

            var first = true;
            paramDictionary.Where(kv => !string.IsNullOrWhiteSpace(kv.Value)).ToList().ForEach((kv) => {
                Debug.WriteLine(kv.Key + ":" + kv.Value);

                url += first ? "" : "&";
                url += kv.Key + "=" + kv.Value;

                first = false;
            });

            try
            {
                using var client = new HttpClient();
                client.addStandardHeaders(orgId, deviceName, productId);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", "3eb5ebd382f844fe50568e042787fcc3");

                var stream = await client.GetStreamAsync(url);
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    responseBody = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return await Task.FromResult(responseBody);
        }

        public async Task<string> executeGetEntryEventDetails(string orgId, string deviceName, string productId, string accessToken,
            string? serviceNapsId,
            string? serviceId,
            string? eventId)
        {
            var responseBody = string.Empty;

            var url = $"https://test.healthclaiming.api.humanservices.gov.au/claiming/ext-vnd/acws/residential-care/events/entry/v2/{eventId}";

            try
            {
                using var client = new HttpClient();
                client.addStandardHeaders(orgId, deviceName, productId);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", "3eb5ebd382f844fe50568e042787fcc3");

                var stream = await client.GetStreamAsync(url);
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    responseBody = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return await Task.FromResult(responseBody);
        }

        public async Task<string> executeCreateEntryEvent(string orgId, string deviceName, string productId, string accessToken,
            CreateEntryEventRequest request,
            string tempAccessKey,
            string? serviceNapsId,
            string? serviceId)
        {
            var responseBody = string.Empty;
            var url = "https://test.healthclaiming.api.humanservices.gov.au/claiming/ext-vnd/acws/residential-care/events/entry/v2";

            try
            {
                using var client = new HttpClient();
                client.addStandardHeaders(orgId, deviceName, productId);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", "3eb5ebd382f844fe50568e042787fcc3");
                client.DefaultRequestHeaders.Add("temp-access-key", tempAccessKey);
                client.DefaultRequestHeaders.Add("x-requested-with", "X");

                client.DefaultRequestHeaders.Add("accept", "application/json");

                var requestInput = request as CreateEntryEventRequestInput;
                requestInput?.ServiceNapsId = serviceNapsId;

                var response = await client.PostAsJsonAsync(url, requestInput);

                //var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                //var response = await client.PostAsync(url, content);

                //var requestInput = new
                //{
                //    externalReferenceId = "PACE",
                //    externalVersionNumber= "PACE",
                //    serviceNapsId= "5678",
                //    serviceId = "4040089223",
                //    careRecipientId = "415115258",
                //    entryTypeCode = "PERM_ENTRY",
                //    receivingPriorCare = "N",
                //    bondRolloverIndicator = "",
                //    nsafBeenSighted = "Y",
                //    agreedAccomodationPrice = 0,
                //    accomPmntArrangementCode = "BTH",
                //    accomPmntRadAmount = 0,
                //    accomPmntDapAmount = 0,
                //    palliativeOnEntry = "N",
                //    entryDate= "2025-08-02",
                //    pensionerStatusCode= "PART",
                //    adjustedSubsidyIndicator= "N"
                //};

                //var response = await client.PostAsJsonAsync(url, requestInput);

                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return await Task.FromResult(responseBody);
        }

        public async Task<string> executeUpdateEntryEvent(string orgId, string deviceName, string productId, string accessToken,
            string? eventId,
            UpdateEntryEventRequest request,
            string? serviceNapsId,
            string? serviceId)
        {
            var responseBody = string.Empty;
            var url = $"https://test.healthclaiming.api.humanservices.gov.au/claiming/ext-vnd/acws/residential-care/events/entry/v2/{eventId}";

            try
            {
                using var client = new HttpClient();
                client.addStandardHeaders(orgId, deviceName, productId);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", "3eb5ebd382f844fe50568e042787fcc3");
                client.DefaultRequestHeaders.Add("x-requested-with", "X");

                var requestInput = request as UpdateEntryEventRequestInput;
                requestInput?.ServiceNapsId = serviceNapsId;

                var response = await client.PutAsJsonAsync(url, request);
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return await Task.FromResult(responseBody);
        }

        public async Task<string> executeDeleteEntryEvent(string orgId, string deviceName, string productId, string accessToken,
            string? serviceNapsId,
            string? serviceId,
            string? eventId)
        {
            var responseBody = string.Empty;

            var url = $"https://test.healthclaiming.api.humanservices.gov.au/claiming/ext-vnd/acws/residential-care/events/entry/v2/{eventId}";

            try
            {
                using var client = new HttpClient();
                client.addStandardHeaders(orgId, deviceName, productId);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", "3eb5ebd382f844fe50568e042787fcc3");
                client.DefaultRequestHeaders.Add("x-requested-with", "X");

                //client.DefaultRequestHeaders.Add("etag", "000010");
                //client.DefaultRequestHeaders.IfMatch.Add(new EntityTagHeaderValue("\"000010\""));
                //client.DefaultRequestHeaders.IfMatch.Add(EntityTagHeaderValue.Any);
                client.DefaultRequestHeaders.Add("if-match", "*");


                var response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return await Task.FromResult(responseBody);
        }
    }
}
