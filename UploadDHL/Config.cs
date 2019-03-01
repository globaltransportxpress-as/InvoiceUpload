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
        public static String FedexRootFileDir
        {
            get
            {

                return ConfigurationManager.AppSettings["FedexRootFileDir"];
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

        public static String OutputDir
        {
            get
            {

                return ConfigurationManager.AppSettings["OutputDir"];
            }
        }
    }
}

