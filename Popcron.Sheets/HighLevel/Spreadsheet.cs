using System;
using System.Collections.Generic;

namespace Popcron.Sheets
{
    [Serializable]
    public class Spreadsheet
    {
        public List<Sheet> Sheets
        {
            get
            {
                return sheets;
            }
        }

        public string ID
        {
            get
            {
                return raw.spreadsheetId;
            }
        }

        public string Title
        {
            get
            {
                return raw.properties.title;
            }
        }

        public string URL
        {
            get
            {
                return raw.spreadsheetUrl;
            }
        }

        private List<Sheet> sheets = new List<Sheet>();
        private SpreadsheetRaw raw;

        public Spreadsheet(SpreadsheetRaw raw)
        {
            this.raw = raw;

            for (int i = 0; i < raw.sheets.Length; i++)
            {
                Sheet sheet = new Sheet(raw.sheets[i]);
                sheets.Add(sheet);
            }
        }
    }
}