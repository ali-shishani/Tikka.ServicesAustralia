using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Jose;
using static System.Net.WebRequestMethods;

namespace Tikka.ServicesAustralia.Utilities
{
    public class JWTUtility
    {

        public string createJWTAssertion(string deviceName, string orgRA, string tokenAudience, double expiresInSeconds, RSACryptoServiceProvider rsaCsp)
        {
            // first set up the payload of the JWT
            var payload = new Dictionary<string, object>()
            {
                { "sub", deviceName }, // this is the name of the device
                { "iss", orgRA }, // this is the PRODA Organisation RA number
                { "aud", "http://proda.humanservices.gov.au" }, // this represents who will receive the JWT, PRODA.
                { "token.aud", tokenAudience }, // this will be the audience of the generated access token, in this case medicare online
                { "exp", getCurrentUnixTimestamp(expiresInSeconds) }, // this is current timestamp + 600 seconds (10 minutes), the expiry time of this JWT
                { "iat", getCurrentUnixTimestamp() },
            };

            // Set up extra headers
            var extraHeaders = new Dictionary<string, object>()
            {
                { "kid", deviceName }, // this is the name of the device, the key id (kid) of the key used to sign this JWT
            };

            // create the signed JWT assertion
            var token = JWT.Encode(payload, rsaCsp, JwsAlgorithm.RS256, extraHeaders);
            return token;
        }

        private long getCurrentUnixTimestamp(double secondsToAdd = 0)
        {
            var tmpDT = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)); //creates a UTC datetime that "starts" at 01-01-1970
            return (long)Math.Truncate(tmpDT.TotalSeconds + secondsToAdd);
        }
    }
}
