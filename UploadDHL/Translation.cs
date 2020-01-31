using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace UploadDHL
{
    public class Translation
    {
        public Dictionary<string, TranslationRecord> TranDictionary { get; set; }
        public Dictionary<string, string> AccountDictionary { get; set; }
        public List<string> AddList = new List<string>();
        private string zDBFile;

        private string zDBAccount;
        public string Error { get; set; }
        public Translation(string dbFile, string accountFile)
        {
            Error = "";

            try
            {
                TranDictionary = new Dictionary<string, TranslationRecord>();
                zDBFile = dbFile;
                zDBAccount = accountFile;
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
                AccountDictionary = new Dictionary<string, string>();
                using (StreamReader fileStream = new StreamReader(zDBAccount))
                {




                    string line = fileStream.ReadLine();
                    while (line != null)
                    {

                        if (false == string.IsNullOrEmpty(line.Replace(";", "")))
                        {
                            var da = line.Split(';');
                            if (da.Length == 2 )
                            {
                                try
                                {
                                    AccountDictionary.Add(da[0],da[1]);

                                }
                                catch (Exception)
                                {
                                    Error = "Dublicate keys in Account";
                                }


                            }
                            
                        }
                        line = fileStream.ReadLine();

                    }
                }



            }
            catch (Exception ex)
            {

                Error = "Cannot open Account File";
            }

        }
        public void AddAccounts(List<string> accountcompany)
        {
            
            foreach (var c in accountcompany)
            {
                var str = c.Split('|');
                if (str.Length == 2)
                {
                    AddAccount(str[0], str[1]);
                }
               
            }

            SaveAllAccounts();

        }
        public void AddAccount(string account, string company)
        {
            if (!AccountDictionary.ContainsKey(account))
            {
                AccountDictionary.Add(account,company);
            }



        }

        public void SaveAllAccounts()
        {

            var f = new System.IO.FileInfo(zDBAccount);
            var fs = f.Open(FileMode.Create, FileAccess.Write);
            using (StreamWriter outputFile = new StreamWriter(fs))
            {

                foreach (var d in AccountDictionary.Keys)
                {
                    outputFile.WriteLine(d + ";" + AccountDictionary[d]);
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
            return TranslationError();


        }

        public TranslationRecord TranslationError()
        {
            return new TranslationRecord
            {
                KeyType = VendorHandler.E_TRANS,
                GTXName = "ERROR"



            };




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
