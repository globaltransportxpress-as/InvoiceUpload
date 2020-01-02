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
        public static String TranslationFileFedex
        {
            get
            {

                return ConfigurationManager.AppSettings["TranslationFileFedex"];
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
        public static String PDKRootFileDir
        {
            get
            {

                return ConfigurationManager.AppSettings["PDKRootFileDir"];
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
        public static String FedexRootFileDir
        {
            get
            {

                return ConfigurationManager.AppSettings["FedexRootFileDir"];
            }
        }
        public static String ConvertFolder
        {
            get
            {

                return ConfigurationManager.AppSettings["ConvertFolder"];
            }
        }
        public static String LogFile
        {
            get
            {

                return ConfigurationManager.AppSettings["LogFile"];
            }
        }
        public static String ErrorDir
        {
            get
            {

                return ConfigurationManager.AppSettings["ErrorDir"];
            }
        }
        public static String DoneDir
        {
            get
            {

                return ConfigurationManager.AppSettings["DoneDir"];
            }
        }

        public static String EndDir(string carrier)
        {
           
                return ConfigurationManager.AppSettings[carrier+"EndDir"];
            
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

