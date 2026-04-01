using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Jose;
using static System.Net.WebRequestMethods;

namespace Tikka.ServicesAustralia.Utilities
{
    public class RSAKeyUtility
    {
        private const string keyContainerBaseName = "PRODADeviceExampleKeyContainer-";

        private JSONUtility jsonUtility { get; set; } = new JSONUtility();

        public RSACryptoServiceProvider createKeys(string deviceName, bool useKeystore = true)
        {
            /* create an RSA key.  This key MUST be stored securely on the application computer.  See the following web page for more information:
             * https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-store-asymmetric-keys-in-a-key-container
             * we are using the constructor that uses a key container, automatically puts in secure keystore
             * as such, we need to delete any existing key and create a new one 
             */

            RSACryptoServiceProvider rsaCsp;
            var cspParms = new CspParameters();
            var keyContainerName = keyContainerBaseName + deviceName;

            if (useKeystore)
            {
                // load existing key (if it doesnt exist it will create it, but then we delete it)
                rsaCsp = GetKeyIfExists(deviceName);

                // if key exists delete it
                if (rsaCsp == null)
                {
                    // Delete the key entry in the container.
                    rsaCsp.PersistKeyInCsp = false;
                    rsaCsp.Clear();
                }

                // set up new key container information if we are using keystore
                cspParms = new CspParameters(24, "Microsoft Enhanced RSA and AES Cryptographic Provider", keyContainerName);
                cspParms.Flags = CspProviderFlags.UseMachineKeyStore;
            }

            // now make a new key
            rsaCsp = new RSACryptoServiceProvider(2048, cspParms);
            return rsaCsp;
        }

        public RSACryptoServiceProvider getCurrentKey(string deviceName)
        {
            // build key container name
            var keyContainerName = keyContainerBaseName + deviceName;

            // load existing key
            var cspParms = new CspParameters(24, "Microsoft Enhanced RSA and AES Cryptographic Provider", keyContainerName);
            cspParms.Flags = CspProviderFlags.UseMachineKeyStore;
            var rsaCsp = new RSACryptoServiceProvider(2048, cspParms);

            return rsaCsp;
        }

        public string generatePublicJwk(string keyId, RSACryptoServiceProvider theKey = null)
        {
            var jwkDict = new Dictionary<string, object>();
            RSACryptoServiceProvider rsaKey;

            // was a key supplied? If yes, use it
            if (theKey != null)
            {
                // use the supplied key
                rsaKey = theKey;
            }
            else
            {
                rsaKey = getCurrentKey(keyId);
            }

            // get modulus and exponent from key and convert to base64url encoding
            jwkDict.Add("n", Base64Url.Encode(rsaKey.ExportParameters(false).Modulus)); // public key modulus
            jwkDict.Add("e", Base64Url.Encode(rsaKey.ExportParameters(false).Exponent)); // public key exponent

            // Add extra fields required by PRODA JWK definitions (note: these are standard fields in most JWKs)
            jwkDict.Add("kid", keyId); // the key id, this is the PRODA B2B device name
            jwkDict.Add("kty", "RSA"); // key type, PRODA only accepts RSA signing keys
            jwkDict.Add("use", "sig"); // the use of the key, will always be signing for PRODA
            jwkDict.Add("alg", "RS256"); // the signing algorithm, PRODA accepts RS256, RS512 and RS384. This example uses RS256.

            // convert the dictionary to a json string
            var jwk = jsonUtility.convertDictionaryToJson(jwkDict);
            return jwk;
        }

        public RSACryptoServiceProvider GetKeyIfExists(string deviceName)
        {
            // build key container name
            var keyContainerName = keyContainerBaseName + deviceName;

            // set up parms that will prevent key being created if it doesn't exist
            var cspParms = new CspParameters(24, "Microsoft Enhanced RSA and AES Cryptographic Provider", keyContainerName);
            cspParms.Flags = CspProviderFlags.UseExistingKey | CspProviderFlags.UseMachineKeyStore;
            RSACryptoServiceProvider csp;

            try
            {
                csp = new RSACryptoServiceProvider(cspParms);
            }
            catch (Exception)
            {
                // if key does not exist an error is caught, so return nothing
                return null;
            }

            return csp;
        }
    }
}
