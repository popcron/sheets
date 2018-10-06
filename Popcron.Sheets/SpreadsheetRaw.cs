using System;

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
    }
}
