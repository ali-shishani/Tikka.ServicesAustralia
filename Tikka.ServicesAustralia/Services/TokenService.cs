using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tikka.ServicesAustralia.Services
{
    public class TokenService : ITokenService
    {
        // Access token store, use a Dictionary with Service Provider name as key, value is tuple of exp time and token
        private Dictionary<string, Tuple<DateTime, string>> accessTokenStore = new Dictionary<string, Tuple<DateTime, string>>();

        public void AddAccessToken(string serviceProviderName, string token, DateTime expiryTime)
        {
            // add token to store
            // check if an existing token for this provider is stored
            if (accessTokenStore.ContainsKey(serviceProviderName))
            {
                // update the value of existing entry
                accessTokenStore[serviceProviderName] = new Tuple<DateTime, string>(expiryTime, token);
            }
            else
            {
                accessTokenStore.Add(serviceProviderName, new Tuple<DateTime, string>(expiryTime, token));
            }
        }

        public Tuple<DateTime, string> retrieveAccessTokenIfValid(string serviceProviderName)
        {
            // retrieve token entry from store
            var tokenTuple = new Tuple<DateTime, string>(new DateTime(0), null);
            if (!accessTokenStore.TryGetValue(serviceProviderName, out tokenTuple))
            {
                return null;
            }

            // If Token expired return null
            if (tokenTuple.Item1.CompareTo(new DateTime()) < 0)
            {
                return null;
            }

            // return saved token
            return tokenTuple;
        }
    }
}
