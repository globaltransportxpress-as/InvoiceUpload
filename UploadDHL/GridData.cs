﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UploadDHL.DataConnections;

namespace UploadDHL
{
    public class GridData
    {
        public string Status { get; set; }
        public string Filename { get; set; }

        public string Comment { get; set; }

        public int JumpLines { get; set; }
        public List<string> JumpLineData { get; set; }
        public List<InvoiceLine> ErrorLines { get; set; }

    }
    
}
