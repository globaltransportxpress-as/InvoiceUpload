using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UploadDHL.DataConnections;

namespace UploadDHL
{
    class ShipXHandler : VendorHandler

    {



        private Translation zTranslation = new Translation(Config.TranslationFilePickupGLS, Config.AccountsShipX);
        public ShipXHandler()
        {
            Error = "";
            RootDir = Config.ShipXRootFileDir;
            CarrierName = "ShipX";

        }


       





    }

}

