using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http.Headers;
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

        public static string buildActivationRequest(string orgRa, string activationCode, string jwk)
        {
            // open json object
            return "{" + "\"orgId\": \"" + orgRa + "\", \"otac\": \"" + activationCode + "\",\"key\": " + jwk + "}";
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
    }
}
