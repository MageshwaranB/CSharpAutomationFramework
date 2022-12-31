using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PracticeAutomationFramework.Utilites
{
    public class JsonReader
    {
        public JsonReader()
        {

        }


        public  String ExtractData(String tokenName)
        {
            String myJsonString= File.ReadAllText("Utilities/TestData.json");
            var jsonTokenObject = JToken.Parse(myJsonString);
            return jsonTokenObject.SelectToken(tokenName).Value<String>();
        }


        public  string[] ExtractDataArray(String tokenName)
        {
            String myJsonString = File.ReadAllText("Utilities/TestData.json");
            var jsonTokenObject = JToken.Parse(myJsonString);
           return jsonTokenObject.SelectTokens(tokenName).Values<string>().ToList().ToArray();
        }
    }
}
