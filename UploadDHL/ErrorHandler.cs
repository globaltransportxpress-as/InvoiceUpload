using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UploadDHL
{
    class ErrorHandler
    {
        public string Location { get; set; }
        public string File { get; set; }
        private List<string[]> zErrorList = new List<string[]>();
        public void Add(string er, string message)
        {
            
            zErrorList.Add(new string[] {er, message, Location});
        }
        public void Add(string er, string message, string location)
        {

            zErrorList.Add(new string[] { er, message, location });
        }



    }
}
