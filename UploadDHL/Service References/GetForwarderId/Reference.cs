﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UploadDHL.GetForwarderId {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GetForwarderId.GetForwarderIdSoap")]
    public interface GetForwarderIdSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetId", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int GetId(string awb, string name, string carrier, System.DateTime date);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetForwarderList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        UploadDHL.GetForwarderId.ForwObj[] GetForwarderList(string carrier, System.DateTime sdate, System.DateTime edate);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class ForwObj : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int idField;
        
        private string companyNameField;
        
        private string streetField;
        
        private string zipField;
        
        private string cityField;
        
        private string alertReasonField;
        
        private string noteInternalField;
        
        private string operatorFeedbackField;
        
        private int parcelCountField;
        
        private System.DateTime pickupDateField;
        
        private string pickupOperatorField;
        
        private string pickupReferenceField;
        
        private string pickupTypeField;
        
        private decimal pricePurchaseField;
        
        private decimal priceSellingField;
        
        private string specialTreatmentField;
        
        private decimal totalWeightField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
                this.RaisePropertyChanged("Id");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string CompanyName {
            get {
                return this.companyNameField;
            }
            set {
                this.companyNameField = value;
                this.RaisePropertyChanged("CompanyName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string Street {
            get {
                return this.streetField;
            }
            set {
                this.streetField = value;
                this.RaisePropertyChanged("Street");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string Zip {
            get {
                return this.zipField;
            }
            set {
                this.zipField = value;
                this.RaisePropertyChanged("Zip");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string City {
            get {
                return this.cityField;
            }
            set {
                this.cityField = value;
                this.RaisePropertyChanged("City");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string AlertReason {
            get {
                return this.alertReasonField;
            }
            set {
                this.alertReasonField = value;
                this.RaisePropertyChanged("AlertReason");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string NoteInternal {
            get {
                return this.noteInternalField;
            }
            set {
                this.noteInternalField = value;
                this.RaisePropertyChanged("NoteInternal");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string OperatorFeedback {
            get {
                return this.operatorFeedbackField;
            }
            set {
                this.operatorFeedbackField = value;
                this.RaisePropertyChanged("OperatorFeedback");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public int ParcelCount {
            get {
                return this.parcelCountField;
            }
            set {
                this.parcelCountField = value;
                this.RaisePropertyChanged("ParcelCount");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public System.DateTime PickupDate {
            get {
                return this.pickupDateField;
            }
            set {
                this.pickupDateField = value;
                this.RaisePropertyChanged("PickupDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string PickupOperator {
            get {
                return this.pickupOperatorField;
            }
            set {
                this.pickupOperatorField = value;
                this.RaisePropertyChanged("PickupOperator");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public string PickupReference {
            get {
                return this.pickupReferenceField;
            }
            set {
                this.pickupReferenceField = value;
                this.RaisePropertyChanged("PickupReference");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public string PickupType {
            get {
                return this.pickupTypeField;
            }
            set {
                this.pickupTypeField = value;
                this.RaisePropertyChanged("PickupType");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public decimal PricePurchase {
            get {
                return this.pricePurchaseField;
            }
            set {
                this.pricePurchaseField = value;
                this.RaisePropertyChanged("PricePurchase");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=14)]
        public decimal PriceSelling {
            get {
                return this.priceSellingField;
            }
            set {
                this.priceSellingField = value;
                this.RaisePropertyChanged("PriceSelling");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=15)]
        public string SpecialTreatment {
            get {
                return this.specialTreatmentField;
            }
            set {
                this.specialTreatmentField = value;
                this.RaisePropertyChanged("SpecialTreatment");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=16)]
        public decimal TotalWeight {
            get {
                return this.totalWeightField;
            }
            set {
                this.totalWeightField = value;
                this.RaisePropertyChanged("TotalWeight");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface GetForwarderIdSoapChannel : UploadDHL.GetForwarderId.GetForwarderIdSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetForwarderIdSoapClient : System.ServiceModel.ClientBase<UploadDHL.GetForwarderId.GetForwarderIdSoap>, UploadDHL.GetForwarderId.GetForwarderIdSoap {
        
        public GetForwarderIdSoapClient() {
        }
        
        public GetForwarderIdSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GetForwarderIdSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GetForwarderIdSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GetForwarderIdSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int GetId(string awb, string name, string carrier, System.DateTime date) {
            return base.Channel.GetId(awb, name, carrier, date);
        }
        
        public UploadDHL.GetForwarderId.ForwObj[] GetForwarderList(string carrier, System.DateTime sdate, System.DateTime edate) {
            return base.Channel.GetForwarderList(carrier, sdate, edate);
        }
    }
}