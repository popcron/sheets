using System;
using System.Threading.Tasks;

namespace Popcron.Sheets
{
    [Serializable]
    public class SpreadsheetRaw
    {
        public string spreadsheetId;
        public SpreadsheetProperties properties;
        public SheetRaw[] sheets;
        public NamedRange[] namedRanges;
        public string spreadsheetUrl;
        public DeveloperMetadata[] developerMetadata;

        /// <summary>  
        /// Returns the raw data that is contingent to the Google Sheets API.  
        /// </summary>
        public static async Task<SpreadsheetRaw> Get(string spreadsheetId, string token, SheetsSerializer serializer, bool includeGridData)
        {
            SheetsClient client = new SheetsClient(spreadsheetId, token, serializer);
            SpreadsheetRaw raw = await client.GetRaw(includeGridData);

            return raw;
        }
    }
}