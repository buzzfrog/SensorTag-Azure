using Newtonsoft.Json;
using SensorTagReader.Messages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace SensorTagReader
{
    public class EventHubService
    {

        private string serviceBusNamespace;
        private string eventHubName;
        private string sharedAccessPolicyName;
        private string sharedAccessPolicyKey;

        private EventHubService() { }

        public EventHubService(string serviceBusNamespace, string eventHubName, string sharedAccessPolicyName,
            string sharedAccessPolicyKey)
        {
            this.serviceBusNamespace = serviceBusNamespace;
            this.eventHubName = eventHubName;
            this.sharedAccessPolicyName = sharedAccessPolicyName;
            this.sharedAccessPolicyKey = sharedAccessPolicyKey;
        }

        public async Task<bool> SendMessage(EventHubSensorMessage message)
        {
            string json = JsonConvert.SerializeObject(message, new JsonSerializerSettings() { Culture = new CultureInfo("en-US") });

            var resourceUri = String.Format("https://{0}.servicebus.windows.net/{1}/publishers/{2}",
                serviceBusNamespace, eventHubName, message.SensorName);
            var sas = ServiceBusSASAuthentication(sharedAccessPolicyName, sharedAccessPolicyKey, resourceUri);

            var uri = new Uri(String.Format("https://{0}.servicebus.windows.net/{1}/publishers/{2}/messages?api-version=2014-05",
                serviceBusNamespace, eventHubName, message.SensorName));

            var webClient = new Windows.Web.Http.HttpClient();
            webClient.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("SharedAccessSignature", sas);
            webClient.DefaultRequestHeaders.TryAppendWithoutValidation("Content-Type", "application/atom+xml;type=entry;charset=utf-8");
            var request = new Windows.Web.Http.HttpRequestMessage(Windows.Web.Http.HttpMethod.Post, uri)
            {
                Content = new HttpStringContent(json)
            };
            var nowait = await webClient.SendRequestAsync(request);

            return nowait.IsSuccessStatusCode;
        }

        private string ServiceBusSASAuthentication(string sharedAccessPolicyName, string sharedAccessPolicyKey, string uri)
        {
            var expiry = (int)DateTime.UtcNow.AddMinutes(20)
                .Subtract(new DateTime(1970, 1, 1))
                .TotalSeconds;
            var stringToSign = WebUtility.UrlEncode(uri) + "\n" + expiry.ToString();
            var signature = HmacSha256(sharedAccessPolicyKey, stringToSign);
            var token = String.Format("sr={0}&sig={1}&se={2}&skn={3}",
                WebUtility.UrlEncode(uri), WebUtility.UrlEncode(signature),
                expiry, sharedAccessPolicyName);

            return token;
        }

        public string HmacSha256(string key, string value)
        {
            var keyStrm = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            var valueStrm = CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf8);

            var objMacProv = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha256);
            var hash = objMacProv.CreateHash(keyStrm);
            hash.Append(valueStrm);

            return CryptographicBuffer.EncodeToBase64String(hash.GetValueAndReset());
        }
    }
}
