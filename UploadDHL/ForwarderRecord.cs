using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UploadDHL.GetForwarderId;

namespace UploadDHL
{
    public class ForwarderRecord:ForwObj
    {
       

       

        public string Error { get; set; }

       
        public ForwarderRecord(ForwObj forwobj, Translation translation)
        {


            Id = forwobj.Id;
            AlertReason = forwobj.AlertReason;
            City = forwobj.City;
            CompanyName = forwobj.CompanyName;
            Link = forwobj.Link;
            NoteInternal = forwobj.NoteInternal;
            OperatorFeedback = forwobj.OperatorFeedback;
            ParcelCount = forwobj.ParcelCount;
            PickupDate = forwobj.PickupDate;
            PriceList = forwobj.PriceList;
            PickupOperator = forwobj.PickupOperator;
            PickupReference = forwobj.PickupReference;
            PickupType = forwobj.PickupType;
            PricePurchase = forwobj.PricePurchase;
            PriceSelling = forwobj.PriceSelling;
            Street = forwobj.Street;
            Zip = forwobj.Zip;
            SpecialTreatment = forwobj.SpecialTreatment;
            TotalWeight = forwobj.TotalWeight;

           
            foreach (var line in PriceList)
            {
                var transn = translation.DoTranslate(line.LineName, VendorHandler.FRAGT);
                if (transn.KeyType.StartsWith("E_"))
                {
                    Error = "Translation";
                }
                else
                {
                    line.LineName = transn.GTXName;
                }



            }
          
           
           

        }
        public ForwarderRecord(ForwObj forwobj)
        {


            Id = forwobj.Id;
            AlertReason = forwobj.AlertReason;
            City = forwobj.City;
            CompanyName = forwobj.CompanyName;
            Link = forwobj.Link;
            NoteInternal = forwobj.NoteInternal;
            OperatorFeedback = forwobj.OperatorFeedback;
            ParcelCount = forwobj.ParcelCount;
            PickupDate = forwobj.PickupDate;
            PriceList = forwobj.PriceList;
            PickupOperator = forwobj.PickupOperator;
            PickupReference = forwobj.PickupReference;
            PickupType = forwobj.PickupType;
            PricePurchase = forwobj.PricePurchase;
            PriceSelling = forwobj.PriceSelling;
            Street = forwobj.Street;
            Zip = forwobj.Zip;
            SpecialTreatment = forwobj.SpecialTreatment;
            TotalWeight = forwobj.TotalWeight;

            Error = "";



        }
    }



}
