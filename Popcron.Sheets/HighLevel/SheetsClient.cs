using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Popcron.Sheets
{
    [Serializable]
    public class SheetsClient
    {
        private readonly string spreadsheetId;
        private readonly string apiKey;
        private SheetsSerializer serializer;

        public SheetsClient(string spreadsheetId, string apiKey, SheetsSerializer serializer)
        {
            this.serializer = serializer ?? throw new Exception("No serializer was given.");
            this.spreadsheetId = spreadsheetId;
            this.apiKey = apiKey;
        }

        protected virtual T DeserializeObject<T>(string data)
        {
            return serializer.DeserializeObject<T>(data);
        }

        protected virtual string SerializeObject(object data)
        {
            return serializer.SerializeObject(data);
        }

        /// <summary>  
        /// Returns a high level representation of a spreadsheet.  
        /// </summary>
        public async Task<Spreadsheet> Get()
        {
            var raw = await GetRaw(true);
            Spreadsheet spreadsheet = new Spreadsheet(raw);
            return spreadsheet;
        }

        /// <summary>  
        /// Returns the raw data that is contingent to the Google Sheets API.  
        /// </summary>
        public async Task<SpreadsheetRaw> GetRaw(bool includeGridData = false)
        {
            string address = "https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}?key={key}&includeGridData=" + includeGridData.ToString().ToLower();
            address = address.Replace("{spreadsheetId}", spreadsheetId);
            address = address.Replace("{key}", apiKey);

            using (WebClient webClient = new WebClient())
            {
                string data = await webClient.DownloadStringTaskAsync(address);
                SpreadsheetRaw spreadsheet = DeserializeObject<SpreadsheetRaw>(data);
                return spreadsheet;
            }
        }

        /// <summary>  
        /// Creates a spreadsheet, returning the newly created spreadsheet.  
        /// </summary>
        public async Task<SpreadsheetRaw> Create(SpreadsheetRaw spreadsheet)
        {
            string address = "https://sheets.googleapis.com/v4/spreadsheets?key={key}";
            address = address.Replace("{key}", apiKey);

            string data = SerializeObject(spreadsheet);

            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string response = await webClient.UploadStringTaskAsync(address, data);

                return DeserializeObject<SpreadsheetRaw>(response);
            }
        }

        /// <summary>  
        /// Applies one or more updates to the spreadsheet.  
        /// </summary>
        public async Task<RequestBatchUpdateResponse> BatchUpdate(Request request)
        {
            string address = "https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}:batchUpdate?key={key}";
            address = address.Replace("{spreadsheetId}", spreadsheetId);
            address = address.Replace("{key}", apiKey);

            string data = SerializeObject(request);

            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string response = await webClient.UploadStringTaskAsync(address, data);

                return DeserializeObject<RequestBatchUpdateResponse>(response);
            }
        }
    }
}