using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UploadDHL
{
    class TranslationRecord
    {
        public string Key { get; set; }
        public string KeyType { get; set; }

        public string GTXName { get; set; }

        public int GTXTransp { get; set; }

        public int GTXProduct { get; set; }


        public TranslationRecord(string[] data)
        {
            Key = data[0];
            KeyType = data[1];
            GTXName = SafeRead(data, 2);
            GTXTransp = IntSafeRead(data, 3);
            GTXProduct = IntSafeRead(data, 4);

        }

        private string SafeRead(string[] d, int inx)
        {
            if (d.Length > inx)
            {
                return d[inx];
            }
            return "";
        }
        private int IntSafeRead(string[] d, int inx)
        {
            int r = 0;
            int.TryParse(SafeRead(d, inx), out r);


            return r;
        }

    }
}
