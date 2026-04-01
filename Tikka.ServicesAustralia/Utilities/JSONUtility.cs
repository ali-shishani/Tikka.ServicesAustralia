using System;
using System.Collections.Generic;
using System.Text;

namespace Tikka.ServicesAustralia.Utilities
{
    public class JSONUtility
    {

        public string convertDictionaryToJson(Dictionary<string, object> dict)
        {
            // open json object
            string result = "{";

            // iterate through dictionary and populate json object
            foreach (var kvp in dict)
            {
                result += "\"" + kvp.Key + "\":\"" + kvp.Value + "\"";

                // add comma after each key-value pair except the last one
                if (!kvp.Equals(dict.Last()))
                {
                    result += ",";
                }
            }

            // close json object
            result += "}";
            return result;
        }
    }
}
