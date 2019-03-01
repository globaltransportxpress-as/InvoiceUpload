using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace UploadDHL
{
    class DHLXML
    {
        public string FillFacturaXml(string facturaNummer, DateTime facturaDate, DateTime payDate,string kundeNummer,decimal sumFragt, decimal sumOil, decimal sumTax, string loopXML2)
        {
            return string.Format(CultureInfo.InvariantCulture,XML1, facturaNummer, facturaDate, payDate, kundeNummer, sumFragt, sumOil, sumTax,sumFragt+ sumOil+ sumTax,
                loopXML2);


        }
        public string FillShipmentXml(string AWB, string shipmentRef, DateTime shipdate,string productName, int noOfColli, decimal billWeight,  decimal price, string fromZip, string fromCountry, string toZip, string toCountry, string loopServices)
        {
            return string.Format(CultureInfo.InvariantCulture,XML2, AWB, SXml(shipmentRef), shipdate, productName, noOfColli, billWeight, price, fromZip, fromCountry, toZip, toCountry, loopServices);


        }

        public string FillServicesXml(string serviceName, decimal servicePrice )
        {
            return string.Format(CultureInfo.InvariantCulture, xmlServiceItems, serviceName, servicePrice);
        }

        private string SXml(string data)
        {
            return data.Replace("&", "_");
        }
        //0:facturaNummer,1:Facturadato,2:facturaDate+30,3:kundenumber;4: Factura sum grundpris;5: Sum olie; 6: Sum Tax;7: Sum All;8 LoopXML2
        public string XML1 = "<?xml version=\"1.0\"?><dow:Download xmlns:dow=\"download.gfbo.fedex.com\" ><dow:Settlement><dow:cntry_cd>DK</dow:cntry_cd><dow:setlm_type_cd>Freight</dow:setlm_type_cd>" +
                             "<dow:settlement_type_ind_cd>Standard</dow:settlement_type_ind_cd>" +
                             "<dow:chronos_setlm_nbr>{0}</dow:chronos_setlm_nbr>" +
                             "<dow:local_seq_setlm_nbr>{0}</dow:local_seq_setlm_nbr>" +
                             "<dow:setlm_dt>{1:dd-MMM-yyyy}</dow:setlm_dt>" +
                             "<dow:setlm_due_dt>{2:dd-MMM-yyyy}</dow:setlm_due_dt>" +
                             "<dow:bill_to_curr_cd>DKK</dow:bill_to_curr_cd>" +
                             "<dow:cust_acct_nbr>{3}</dow:cust_acct_nbr>" +
                             "<dow:total_freight_charge_amt>{4}</dow:total_freight_charge_amt>" +
                             "<dow:total_discount_amt></dow:total_discount_amt>" +
                             "<dow:total_addl_charge_amt>{5}</dow:total_addl_charge_amt>" +
                             "<dow:total_tax_amt>{6}</dow:total_tax_amt>" +
                             "<dow:total_setlm_due_amt>{7}</dow:total_setlm_due_amt>" +
                             "<dow:alt_curr_cd/>" +
                             "<dow:alt_curr_exch_rate_amt/>{8}</dow:Settlement></dow:Download>" ;
                             


        // 0:AWB;1:Shipment_Ref; 2 Shipdate;3:ProductName;4:NoColli;5:BillWeight;6:PriceTotal;7:From-zip;8:from Country;9:To-zip;10:To Country;11:LoopChargeItems :  

        public string XML2 = "<dow:Shipment>" +
        "<dow:tendr_dt/><dow:package_tracking_nbr>{0}</dow:package_tracking_nbr>" +
        "<dow:payer_type_cd>Consignee</dow:payer_type_cd>"+
        "<dow:shipper_ref_field_1_desc>{1}</dow:shipper_ref_field_1_desc>"+
        "<dow:shipper_ref_field_2_desc/>"+
        "<dow:shipper_ref_field_3_desc/>"+
        "<dow:pod_dt></dow:pod_dt>"+
        "<dow:pod_tm></dow:pod_tm>"+
        "<dow:pod_signature_nm></dow:pod_signature_nm>"+
        "<dow:char_format_ship_dt>{2:dd-MMM-yyyy}</dow:char_format_ship_dt>" +
        "<dow:ship_dt>{2:dd-MMM-yyyy}</dow:ship_dt>" +
        "<dow:svc_base_cd>{3}</dow:svc_base_cd>"+
        "<dow:svc_base_pack_cd>0</dow:svc_base_pack_cd>"+
        "<dow:svc_base_pack_label_desc>{3}</dow:svc_base_pack_label_desc>"+
        "<dow:orig_loc_id></dow:orig_loc_id>"+
        "<dow:dest_loc_id></dow:dest_loc_id>"+
        "<dow:total_piece_qty>{4}</dow:total_piece_qty>"+
        "<dow:actual_wgt>{5}</dow:actual_wgt>"+
        "<dow:actual_wgt_uom_cd>kg</dow:actual_wgt_uom_cd>"+
        "<dow:meter_nbr/>"+
        "<dow:child_account_nbr></dow:child_account_nbr>"+
        "<dow:master_tracking_nbr/>"+
        "<dow:total_shpmt_amt>{6}</dow:total_shpmt_amt>"+
        "<dow:Shipment_Customer>"+
        "<dow:customer_role_type_cd>S</dow:customer_role_type_cd>"+
        "<dow:company_nm>NA</dow:company_nm>"+
        "<dow:contact_nm>NA</dow:contact_nm>"+
        "<dow:address_line_1_desc>NA</dow:address_line_1_desc>"+
        "<dow:address_line_2_desc>NA</dow:address_line_2_desc>"+
        "<dow:address_line_3_desc/>"+
        "<dow:city_desc>NA</dow:city_desc>"+
        "<dow:state_cd/>"+
        "<dow:postal_cd>{7}</dow:postal_cd>"+
        "<dow:cntry_cd>{8}</dow:cntry_cd>"+
        "</dow:Shipment_Customer>"+
        "<dow:Shipment_Customer>"+
        "<dow:customer_role_type_cd>R</dow:customer_role_type_cd>"+
        "<dow:company_nm>NA</dow:company_nm>"+
        "<dow:contact_nm>NA</dow:contact_nm>"+
        "<dow:address_line_1_desc>NA</dow:address_line_1_desc>"+
        "<dow:address_line_2_desc>NA</dow:address_line_2_desc>"+
        "<dow:address_line_3_desc/>"+
        "<dow:city_desc></dow:city_desc>"+
        "<dow:state_cd/>"+
        "<dow:postal_cd>{9}</dow:postal_cd>"+
        "<dow:cntry_cd>{10}</dow:cntry_cd>"+
        "</dow:Shipment_Customer>{11}</dow:Shipment>";



        //0:Servicename;//1:Serviceprice 
        public string xmlServiceItems = "<dow:Shipment_Charge_Item> " +
                                        "<dow:charge_lable_desc>{0}</dow:charge_lable_desc> " +
                                        "<dow:charge_amt>{1}</dow:charge_amt> " +
                                        "</dow:Shipment_Charge_Item>";












        public string zStart = "<?xml version = \"1.0\" encoding=\"UTF-8\"?>"+
        "<Invoice xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\" xmlns:ccts=\"urn:oasis:names:specification:ubl:schema:xsd:CoreComponentParameters-2\" xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\" xmlns:sdt=\"urn:oasis:names:specification:ubl:schema:xsd:SpecializedDatatypes-2\" xmlns:udt=\"urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2\" xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\" xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2\"  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\" urn:oasis:names:specification:ubl:schema:xsd:Invoice-2 UBL-Invoice-2.0.xsd\">"+
        "<cbc:UBLVersionID>2.0</cbc:UBLVersionID>"
            + "<cbc:UBLVersionID>2.0</cbc:UBLVersionID><cbc:CustomizationID>OIOUBL-2.01</cbc:CustomizationID><cbc:ProfileID schemeAgencyID = \"320\" schemeID=\"urn:oioubl:id:profileid-1.2\">urn:www.nesubl.eu:profiles:profile5:ver2.0</cbc:ProfileID><cbc:ID>{1}</cbc:ID>"
            + "<cbc:CopyIndicator>false</cbc:CopyIndicator>"
            + "<cbc:IssueDate>{0}</cbc:IssueDate>"
            + "<cbc:InvoiceTypeCode listID = \"urn:oioubl:codelist:invoicetypecode-1.1\" listAgencyID=\"320\">380</cbc:InvoiceTypeCode>"
            + "<cbc:DocumentCurrencyCode>DKK</cbc:DocumentCurrencyCode>"
            + "<cac:OrderReference>"
            + "<cbc:ID>{1}</cbc:ID>"
            + "</cac:OrderReference>";

        public string Start(DHLRecord dhlRecord)
        {
           return string.Format(CultureInfo.InvariantCulture,zStart, dhlRecord.Invoice_Date,dhlRecord.Invoice_Number);

        }
        public string Start(PDKrecord pdkRecord)
        {
            return string.Format(CultureInfo.InvariantCulture, zStart, pdkRecord.FacturaDate, pdkRecord.Factura);

        }





        public string Accounting = "<cac:AccountingSupplierParty>" +
    "<cac:Party>" +
      "<cbc:EndpointID schemeDataURI=\"urn:oioubl:scheme:endpointid-1.0\" schemeID=\"GLN\">5790000110209</cbc:EndpointID>" +
      "<cac:PartyName>" +
        "<cbc:Name>DHL DK</cbc:Name>" +
      "</cac:PartyName>" +
      "<cac:PostalAddress>" +
        "<cbc:AddressFormatCode listAgencyID=\"320\" listID=\"urn:oioubl:codelist:addressformatcode-1.1\">StructuredLax</cbc:AddressFormatCode>" +
        "<cbc:StreetName>N/A</cbc:StreetName>" +
        "<cbc:CityName>N/A</cbc:CityName>" +
        "<cbc:PostalZone>DK-2625</cbc:PostalZone>" +
      "</cac:PostalAddress>" +
      "<cac:PartyLegalEntity>" +
        "<cbc:RegistrationName>DHL DK</cbc:RegistrationName>" +
        "<cbc:CompanyID >DK10154596</cbc:CompanyID>" +
      "</cac:PartyLegalEntity>" +
    "</cac:Party>" +
  "</cac:AccountingSupplierParty>";

        public string Accounting2 = "<cac:AccountingCustomerParty>" +
                                      "<cac:Party>" +
                                      "<cbc:EndpointID schemeDataURI=\"urn:oioubl:scheme:endpointid-1.0\" schemeID=\"ISO 6523\">0</cbc:EndpointID>" +
                                      "<cac:PartyName>" +
                                      "<cbc:Name>GLOBAL TRANSPORT XPRESS A/S</cbc:Name>" +
                                      "</cac:PartyName>" +
                                      "<cac:PostalAddress>" +
                                      "<cbc:AddressFormatCode listAgencyID=\"320\" listID=\"urn:oioubl:codelist:addressformatcode-1.1\">StructuredLax</cbc:AddressFormatCode>" +
                                      "<cbc:StreetName>Spotorno Alle 10</cbc:StreetName>" +
                                      "<cbc:PostalZone>2630</cbc:PostalZone>" +
                                      "</cac:PostalAddress>" +
                                      "<cac:Contact>" +
                                      "<cbc:ID>302611823</cbc:ID>" +
                                      "</cac:Contact>" +
                                      "</cac:Party>" +
                                      "</cac:AccountingCustomerParty>";

        public string Payment3(DHLRecord dhlRecord)
        {
            return string.Format(CultureInfo.InvariantCulture,zPayment3, dhlRecord.Due_Date, dhlRecord.Billing_Account);
        }

        public string Payment3(PDKrecord pdkRecord)
        {
            return string.Format(CultureInfo.InvariantCulture, zPayment3, pdkRecord.FacturaDate.AddDays(30), pdkRecord.CustomerNumber);
        }
        private string zPayment3 = "<cac:PaymentMeans>" +
        "<cbc:ID>1</cbc:ID>" +
        "<cbc:PaymentMeansCode>93</cbc:PaymentMeansCode>" +
        "<cbc:PaymentDueDate>{0}</cbc:PaymentDueDate>" +
        "<cbc:InstructionID>000000016649519</cbc:InstructionID>" +
        "<cbc:PaymentID >71</cbc:PaymentID>" +
        "<cac:CreditAccount>" +
        "<cbc:AccountID>{1}</cbc:AccountID>" +
        "</cac:CreditAccount>" +
        "</cac:PaymentMeans>";

        public string Payment4(DHLRecord dhlRecord)
        {
            return string.Format(zPAmount4, dhlRecord.Total_amount_incl_VAT);
        }
        public string Payment4( decimal sumPriceVat)
        {
            return string.Format(zPAmount4, sumPriceVat);
        }




        private string zPAmount4 = "<cac:PaymentTerms>" +
                                   "<cbc:ID>1</cbc:ID>" +
                                   "<cbc:Amount currencyID=\"DKK\">{0}</cbc:Amount>" +
                                   "</cac:PaymentTerms>";


        public string TaxTotal(DHLRecord dhlRecord)
        {
            return string.Format(CultureInfo.InvariantCulture , zTaxTotal5, dhlRecord.Total_Tax + dhlRecord.Tax_Adjustment, dhlRecord.Tax_Adjustment, dhlRecord.Total_Tax);
        }
        public string TaxTotal(PDKrecord pdkRecord)
        {
            return string.Format(CultureInfo.InvariantCulture, zTaxTotal5, 0, 0, 0);
        }

        private string zTaxTotal5 = "<cac:TaxTotal>" +
                                   "<cbc:TaxAmount currencyID=\"DKK\">{0}</cbc:TaxAmount>" + //Total Tax+Tax Adjustment
                                   "<cbc:RoundingAmount currencyID=\"DKK\">{1}</cbc:RoundingAmount>" + //Tax Adjustment

                                   "<cac:TaxSubtotal>" +
                                   "<cbc:TaxableAmount currencyID=\"DKK\">*ASumTax*</cbc:TaxableAmount>" + //Calc on sum( Total amount (excl. VAT)) when TaxCode="A"


                                   "<cbc:TaxAmount currencyID=\"DKK\">{2}</cbc:TaxAmount>" + //Total Tax

                                   "<cac:TaxCategory>" +
                                   "<cbc:ID schemeID=\"urn:oioubl:id:taxcategoryid-1.1\" schemeAgencyID=\"320\">StandardRated</cbc:ID>" +
                                   "<cac:TaxScheme>" +
                                   "<cbc:ID schemeAgencyID=\"320\" schemeID=\"urn:oioubl:id:taxschemeid-1.1\">63</cbc:ID>" +
                                   "<cbc:Name>Moms</cbc:Name>" +
                                   "</cac:TaxScheme>" +
                                   "</cac:TaxCategory>" +
                                   "</cac:TaxSubtotal>" +
                                   "<cac:TaxSubtotal>" +
                                   "<cbc:TaxableAmount currencyID=\"DKK\">*BSumTax*</cbc:TaxableAmount>" + //Calc on sum( Total amount (excl. VAT)) when TaxCode="B"

                                   "<cbc:TaxAmount currencyID=\"DKK\">0.00</cbc:TaxAmount>" +
                                   "<cac:TaxCategory>" +
                                   "<cbc:ID schemeID=\"urn:oioubl:id:taxcategoryid-1.1\" schemeAgencyID=\"320\">ZeroRated</cbc:ID>" +
                                   "<cac:TaxScheme>" +
                                   "<cbc:ID schemeAgencyID=\"320\" schemeID=\"urn:oioubl:id:taxschemeid-1.1\">63</cbc:ID>" +
                                   "<cbc:Name>Moms</cbc:Name>" +
                                   "</cac:TaxScheme>" +
                                   "</cac:TaxCategory>" +
                                   "</cac:TaxSubtotal>" +
                                   "</cac:TaxTotal>";
        public string LegalMon(DHLRecord dhlRecord)
        {
            return string.Format(CultureInfo.InvariantCulture, zLegalMon6, dhlRecord.Total_amount_excl_VAT+dhlRecord.Tax_Adjustment,dhlRecord.Total_Tax , dhlRecord.Total_amount_incl_VAT,dhlRecord.Tax_Adjustment);
        }
        public string LegalMon(PDKrecord pdkRecord)
        {
            return string.Format(CultureInfo.InvariantCulture, zLegalMon6, pdkRecord.Price, pdkRecord.PriceVat- pdkRecord.Price, pdkRecord.PriceVat, 0);
        }

        private string zLegalMon6 = "<cac:LegalMonetaryTotal>" +
                                    "<cbc:LineExtensionAmount currencyID=\"DKK\">{0}</cbc:LineExtensionAmount>" + //Total amount (excl. VAT)

                                    "<cbc:TaxExclusiveAmount currencyID=\"DKK\">{1}</cbc:TaxExclusiveAmount>" + //Total Tax
                                    "<cbc:TaxInclusiveAmount currencyID=\"DKK\">{2}</cbc:TaxInclusiveAmount>" + //Total amount (incl. VAT)

                                    "<cbc:PayableRoundingAmount currencyID=\"DKK\">{3}</cbc:PayableRoundingAmount>" + //Tax Adjustment
                                    "<cbc:PayableAmount currencyID=\"DKK\">{2}</cbc:PayableAmount>" + //Total amount (incl. VAT)
                                    "</cac:LegalMonetaryTotal>";

        public string MakeLine(int count, DHLRecord dhlRec , string code, string name, decimal charge, string taxcode, decimal tax, decimal discount, decimal total)
        {
            decimal taxPrc = taxcode == "A" ? 25.000M : 0M;
            return string.Format(CultureInfo.InvariantCulture,zInvoiceLine,
                count,
                dhlRec.Shipment_Number,
                dhlRec.Weight_kg,
                dhlRec.Weight_Charge,
                dhlRec.Shipment_Date,
                dhlRec.Receivers_Address_1,
                dhlRec.Receivers_Name,
                dhlRec.Receivers_City,
                dhlRec.Receivers_Postcode,
                tax,
                charge,
                taxPrc,
                dhlRec.Shipment_Reference_1,
                name

            );


        }
        public string MakeLine( int count ,PDKrecord pdkRecord, string name, decimal charge, decimal tax)
        {
           
            return string.Format(CultureInfo.InvariantCulture, zInvoiceLine,
               count,
                pdkRecord.AWB,
                pdkRecord.Weight,
                pdkRecord.BillWeight,
                pdkRecord.Date,
                pdkRecord.Address,
                pdkRecord.Name,
                "ZIPCODE",
                pdkRecord.ToZip,
                tax,
                charge,
                0,
                "",
                name

            );


        }

        private string zInvoiceLine = "<cac:InvoiceLine>" +
                                      "<cbc:ID>{0}</cbc:ID>" + //count
                                      "<cbc:UUID>{1}</cbc:UUID>" +
                                      "<cbc:Note>{9}</cbc:Note>" +
                                      "<cbc:InvoicedQuantity unitCode=\"58\">{2}</cbc:InvoicedQuantity>" + //Weight (kg)

                                      "<cbc:LineExtensionAmount currencyID=\"DKK\">{3}</cbc:LineExtensionAmount>" + //Total amount (excl. VAT)

                                      "<cac:Delivery>" +
                                      "<cbc:ActualDeliveryDate>{4}</cbc:ActualDeliveryDate>" + //Shipment Date

                                      "<cac:DeliveryLocation>" +
                                      "<cac:Address>" +
                                      "<cbc:AddressFormatCode listAgencyID=\"320\" listID=\"urn:oioubl:codelist:addressformatcode-1.1\">StructuredLax</cbc:AddressFormatCode>" +
                                      "<cbc:StreetName>{5}</cbc:StreetName>" + //Receivers Address 1


                                      "<cbc:MarkAttention>{6}</cbc:MarkAttention>" + //Receivers Name


                                      "<cbc:CityName>{7}</cbc:CityName>" + //Receivers City


                                      "<cbc:PostalZone>{8}</cbc:PostalZone>" + //Receivers Postcode


                                      "</cac:Address>" +
                                      "</cac:DeliveryLocation>" +
                                      "<cac:Despatch>" +
                                      //"<cac:DespatchAddress>" +
                                      //"<cac:AddressLine>" +
                                      //"<cbc:Line>DK</cbc:Line>" +//Orig Country Code


                                      //"</cac:AddressLine>" +
                                      //"<cac:AddressLine>" +
                                      //"<cbc:Line>AARHUS</cbc:Line>" + //Orig Name

                                      //"</cac:AddressLine>" +
                                      //"<cac:AddressLine>" +
                                      //"<cbc:Line>AAR</cbc:Line>" + //Origin

                                      //"</cac:AddressLine>" +
                                      //"</cac:DespatchAddress>" +
                                      "</cac:Despatch>" +
                                      "</cac:Delivery>" +
                                      "<cac:TaxTotal>" +
                                      "<cbc:TaxAmount currencyID=\"DKK\">{9}</cbc:TaxAmount>" +//Weight Tax (VAT) || XC1 Tax
                                                  "<cac:TaxSubtotal>" +
                                      "<cbc:TaxableAmount currencyID=\"DKK\">{10}</cbc:TaxableAmount>" +//||XC1 Charge

                                      "<cbc:TaxAmount currencyID=\"DKK\">{9}</cbc:TaxAmount>" + //Weight Tax (VAT) || XC1 Tax
                                      "<cac:TaxCategory>" +
                                      "<cbc:ID schemeID=\"urn:oioubl:id:taxcategoryid-1.1\" schemeAgencyID=\"320\">StandardRated</cbc:ID>" +
                                      "<cbc:Percent>{11}</cbc:Percent>" +
                                      "<cac:TaxScheme>" +
                                      "<cbc:ID schemeAgencyID=\"320\" schemeID=\"urn:oioubl:id:taxschemeid-1.1\">63</cbc:ID>" +
                                      "<cbc:Name>Moms</cbc:Name>" +
                                      "</cac:TaxScheme>" +
                                      "</cac:TaxCategory>" +
                                      "</cac:TaxSubtotal>" +
                                      "</cac:TaxTotal>" +
                                      "<cac:Item>" +
                                      "<cbc:Name>{12}</cbc:Name>" +//Product Name||XC1 Name


                                      "<cbc:AdditionalInformation>1</cbc:AdditionalInformation>" +
                                      "</cac:Item>" +
                                      "<cac:Price>" +
                                      "<cbc:PriceAmount currencyID=\"DKK\">{2}</cbc:PriceAmount>" +
                                      "<cbc:BaseQuantity unitCode=\"58\">500</cbc:BaseQuantity>" +
                                      "</cac:Price>" +
                                      "</cac:InvoiceLine>";
    }






}
