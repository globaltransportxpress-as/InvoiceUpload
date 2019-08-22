using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace UploadDHL
{
    class Translation
    {
        public Dictionary<string, TranslationRecord> TranDictionary { get; set; }
        public List<string> AddList = new List<string>();
        private string zDBFile;
        public string Error { get; set; }
        public Translation(string dbFile)
        {
            TranDictionary = new Dictionary<string, TranslationRecord>();
            zDBFile = dbFile; 
            using (StreamReader fileStream = new StreamReader(dbFile))
            {




                string line = fileStream.ReadLine();
                while (line != null)
                {

                    if (false == string.IsNullOrEmpty(line.Replace(";", "")))
                    {
                        var da = line.Split(';');
                        if (da.Length > 2 && da[2] != "")
                        {
                            try
                            {
                                TranDictionary.Add(da[0], new TranslationRecord(da));

                            }
                            catch (Exception)
                            {
                                Error = "Dublicate keys in translatefile";
                            }
                          

                        }
                        else
                        {
                            AddList.Add(da[0] + ";" + da[1]);
                        }

                    }
                    line = fileStream.ReadLine();

                }
            }

        }

        public void SaveAll(List<TranslationRecord> lst )
        {
           
                var f = new System.IO.FileInfo(zDBFile);
                var fs = f.Open(FileMode.Create, FileAccess.Write);
                using (StreamWriter outputFile = new StreamWriter(fs))
                {

                    foreach (var d in lst)
                    {
                    outputFile.WriteLine(d.Key + ";" + d.KeyType + ";" + d.GTXName + ";" +  d.GTXTransp+ ";" + d.GTXProduct);
                    }

              
                    
                }


           



        }
      
        public TranslationRecord DoTranslate(string key, string keyType)
        {

            if(TranDictionary.ContainsKey(key))
            {
                return TranDictionary[key];
            }
            AddMissing(key, keyType);
            return null;


        }

        public void AddMissing(string key, string keytype)
        {
            if (!AddList.Contains(key + ";" + keytype) && key!="")
            {

                var f = new System.IO.FileInfo(zDBFile);
                var fs = f.Open(FileMode.Append, FileAccess.Write);
                using (StreamWriter outputFile = new StreamWriter(fs))
                {
                    outputFile.WriteLine(key + ";" + keytype);
                    AddList.Add(key + ";" + keytype);
                }

            }
           
            // Append text to an existing file named "WriteLines.txt".

        }


    }
}
