using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace UploadDHL
{
    class Config
    {
        

        public static String InputDir
        {
            get
            {

                return ConfigurationManager.AppSettings["InputDir"];
            }
        }

        public static String TranslationFilePDK
        {
            get
            {

                return ConfigurationManager.AppSettings["TranslationFilePDK"];
            }
        }
        public static String TranslationFileDHL
        {
            get
            {

                return ConfigurationManager.AppSettings["TranslationFileDHL"];
            }
        }
        public static String TranslationFilePickupHS
        {
            get
            {

                return ConfigurationManager.AppSettings["TranslationFileHS"];
            }
        }
        public static String TranslationFileGtx
        {
            get
            {

                return ConfigurationManager.AppSettings["TranslationFileGtx"];
            }
        }
        public static String TranslationFilePickupGLS
        {
            get
            {

                return ConfigurationManager.AppSettings["TranslationFilePickupGLS"];
            }
        }
        public static String ShipXRootFileDir
        {
            get
            {

                return ConfigurationManager.AppSettings["ShipXRootFileDir"];
            }
        }
        public static String GLSRootFileDir
        {
            get
            {

                return ConfigurationManager.AppSettings["GLSRootFileDir"];
            }
        }
        public static String DHLRootFileDir
        {
            get
            {

                return ConfigurationManager.AppSettings["DHLRootFileDir"];
            }
        }
        public static String GTXRootFileDir
        {
            get
            {

                return ConfigurationManager.AppSettings["GTXRootFileDir"];
            }
        }
        
       
        public static String EndDir(string carrier)
        {
           
                return ConfigurationManager.AppSettings[carrier+"EndDir"];
            
        }
        public static String AccountsGTX
        {
            get
            {

                return ConfigurationManager.AppSettings["TranslationAccountGTX"];
            }
        }
        public static String AccountsShipX
        {
            get
            {

                return ConfigurationManager.AppSettings["TranslationAccountShipx"];
            }
        }

        public static String System
        {
            get
            {

                return ConfigurationManager.AppSettings["System"];
            }
        }
    }
}

