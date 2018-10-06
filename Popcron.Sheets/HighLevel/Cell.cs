using System;

namespace Popcron.Sheets
{
    [Serializable]
    public class Cell
    {
        public static Cell Empty
        {
            get
            {
                return new Cell();
            }
        }

        public bool IsEmpty
        {
            get
            {
                return raw == null;
            }
        }

        public string Value
        {
            get
            {
                if (raw != null)
                {
                    return raw.formattedValue;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public ExtendedValue ExtendedValue
        {
            get
            {
                if (raw != null)
                {
                    return raw.effectiveValue;
                }
                else
                {
                    return null;
                }
            }
        }

        private CellData raw;

        internal Cell()
        {

        }

        internal Cell(CellData raw)
        {
            this.raw = raw;
        }
    }
}