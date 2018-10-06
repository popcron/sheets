using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Popcron.Sheets
{
    [Serializable]
    public class SheetsClient
    {
        private string spreadsheetId;
        private string apiKey;

        public SheetsClient(string spreadsheetId, string apiKey)
        {
            this.spreadsheetId = spreadsheetId;
            this.apiKey = apiKey;
        }

        protected virtual T DeserializeObject<T>(string data)
        {
            return JsonUtility.FromJson<T>(data);
        }

        protected virtual string SerializeObject(object data)
        {
            return JsonUtility.ToJson(data);
        }

        public async Task<Spreadsheet> Get()
        {
            var raw = await Get(true);
            Spreadsheet spreadsheet = new Spreadsheet(raw);
            return spreadsheet;
        }

        public async Task<SpreadsheetRaw> Get(bool includeGridData = false)
        {
            string address = "https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}?key={key}&includeGridData=" + includeGridData.ToString().ToLower();
            address = address.Replace("{spreadsheetId}", spreadsheetId);
            address = address.Replace("{key}", apiKey);

            WebClient webClient = new WebClient();
            string data = await webClient.DownloadStringTaskAsync(address);

            SpreadsheetRaw spreadsheet = DeserializeObject<SpreadsheetRaw>(data);
            return spreadsheet;
        }
        
        public async Task<SpreadsheetRaw> Create(SpreadsheetRaw spreadsheet)
        {
            string address = "https://sheets.googleapis.com/v4/spreadsheets?key={key}";
            address = address.Replace("{key}", apiKey);

            string data = SerializeObject(spreadsheet);

            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            string response = await webClient.UploadStringTaskAsync(address, data);

            return DeserializeObject<SpreadsheetRaw>(response);
        }

        public async Task<RequestBatchUpdateResponse> BatchUpdate(Request request)
        {
            string address = "https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}:batchUpdate?key={key}";
            address = address.Replace("{spreadsheetId}", spreadsheetId);
            address = address.Replace("{key}", apiKey);

            string data = SerializeObject(request);

            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            string response = await webClient.UploadStringTaskAsync(address, data);

            return DeserializeObject<RequestBatchUpdateResponse>(response);
        }
    }
}