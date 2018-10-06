using System;

namespace Popcron.Sheets
{
    [Serializable]
    public class Sheet
    {
        public int ID
        {
            get
            {
                return raw.properties.sheetId;
            }
        }

        public int Rows
        {
            get
            {
                return raw.data[0].rowData.Length;
            }
        }

        public int Columns
        {
            get
            {
                int maxColumn = 0;
                for (int i = 0; i < raw.data[0].rowData.Length; i++)
                {
                    int values = 0;
                    for (int v = 0; v < raw.data[0].rowData[i].values.Length; v++)
                    {
                        if (!string.IsNullOrEmpty(raw.data[0].rowData[i].values[v].formattedValue)) values++;
                    }
                    if (values > maxColumn)
                    {
                        maxColumn = values;
                    }
                }

                return maxColumn;
            }
        }

        public Cell[,] Data
        {
            get
            {
                Cell[,] data = new Cell[Columns, Rows];

                for (int y = 0; y < Rows; y++)
                {
                    for (int x = 0; x < Columns; x++)
                    {
                        if (raw.data[0].rowData.Length > y && raw.data[0].rowData[y].values.Length > x)
                        {
                            data[x, y] = new Cell(raw.data[0].rowData[y].values[x]);
                        }
                        else
                        {
                            data[x, y] = Cell.Empty;
                        }
                    }
                }

                return data;
            }
        }

        public string Title
        {
            get
            {
                return raw.properties.title;
            }
        }
        
        public bool Hidden
        {
            get
            {
                return raw.properties.hidden;
            }
        }

        public bool RightToLeft
        {
            get
            {
                return raw.properties.rightToLeft;
            }
        }
        
        private SheetRaw raw;

        public Sheet(SheetRaw raw)
        {
            this.raw = raw;
        }
    }
}