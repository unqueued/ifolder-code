﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.2032
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by wsdl, Version=1.1.4322.2032.
// 
using System.Diagnostics;
using System.Xml.Serialization;
using System;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Services;


/// <remarks/>
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="Simias Web ServiceSoap", Namespace="http://novell.com/simias/web/")]
public class SimiasWebService : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    /// <remarks/>
    public SimiasWebService() {
        this.Url = "http://localhost:4699/simias10/mlasky/Simias.asmx";
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://novell.com/simias/web/GetSimiasInformation", RequestNamespace="http://novell.com/simias/web/", ResponseNamespace="http://novell.com/simias/web/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string GetSimiasInformation() {
        object[] results = this.Invoke("GetSimiasInformation", new object[0]);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginGetSimiasInformation(System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("GetSimiasInformation", new object[0], callback, asyncState);
    }
    
    /// <remarks/>
    public string EndGetSimiasInformation(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://novell.com/simias/web/GetDomainInformation", RequestNamespace="http://novell.com/simias/web/", ResponseNamespace="http://novell.com/simias/web/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public DomainInformation GetDomainInformation(string domainID) {
        object[] results = this.Invoke("GetDomainInformation", new object[] {
                    domainID});
        return ((DomainInformation)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginGetDomainInformation(string domainID, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("GetDomainInformation", new object[] {
                    domainID}, callback, asyncState);
    }
    
    /// <remarks/>
    public DomainInformation EndGetDomainInformation(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((DomainInformation)(results[0]));
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://novell.com/simias/web/SetDomainCredentials", RequestNamespace="http://novell.com/simias/web/", ResponseNamespace="http://novell.com/simias/web/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int SetDomainCredentials(string domainID, string memberID, string password) {
        object[] results = this.Invoke("SetDomainCredentials", new object[] {
                    domainID,
                    memberID,
                    password});
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginSetDomainCredentials(string domainID, string memberID, string password, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("SetDomainCredentials", new object[] {
                    domainID,
                    memberID,
                    password}, callback, asyncState);
    }
    
    /// <remarks/>
    public int EndSetDomainCredentials(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://novell.com/simias/web/ValidCredentials", RequestNamespace="http://novell.com/simias/web/", ResponseNamespace="http://novell.com/simias/web/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public bool ValidCredentials(string domainID, string memberID) {
        object[] results = this.Invoke("ValidCredentials", new object[] {
                    domainID,
                    memberID});
        return ((bool)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginValidCredentials(string domainID, string memberID, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ValidCredentials", new object[] {
                    domainID,
                    memberID}, callback, asyncState);
    }
    
    /// <remarks/>
    public bool EndValidCredentials(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((bool)(results[0]));
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://novell.com/simias/web/SaveDomainCredentials", RequestNamespace="http://novell.com/simias/web/", ResponseNamespace="http://novell.com/simias/web/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void SaveDomainCredentials(string domainID, string credentials, CredentialType type) {
        this.Invoke("SaveDomainCredentials", new object[] {
                    domainID,
                    credentials,
                    type});
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginSaveDomainCredentials(string domainID, string credentials, CredentialType type, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("SaveDomainCredentials", new object[] {
                    domainID,
                    credentials,
                    type}, callback, asyncState);
    }
    
    /// <remarks/>
    public void EndSaveDomainCredentials(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://novell.com/simias/web/GetSavedDomainCredentials", RequestNamespace="http://novell.com/simias/web/", ResponseNamespace="http://novell.com/simias/web/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public CredentialType GetSavedDomainCredentials(string domainID, out string userID, out string credentials) {
        object[] results = this.Invoke("GetSavedDomainCredentials", new object[] {
                    domainID});
        userID = ((string)(results[1]));
        credentials = ((string)(results[2]));
        return ((CredentialType)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginGetSavedDomainCredentials(string domainID, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("GetSavedDomainCredentials", new object[] {
                    domainID}, callback, asyncState);
    }
    
    /// <remarks/>
    public CredentialType EndGetSavedDomainCredentials(System.IAsyncResult asyncResult, out string userID, out string credentials) {
        object[] results = this.EndInvoke(asyncResult);
        userID = ((string)(results[1]));
        credentials = ((string)(results[2]));
        return ((CredentialType)(results[0]));
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://novell.com/simias/web/")]
public class DomainInformation {
    
    /// <remarks/>
    public DomainType Type;
    
    /// <remarks/>
    public string Name;
    
    /// <remarks/>
    public string Description;
    
    /// <remarks/>
    public string ID;
    
    /// <remarks/>
    public string RosterID;
    
    /// <remarks/>
    public string RosterName;
    
    /// <remarks/>
    public string MemberID;
    
    /// <remarks/>
    public string MemberName;
    
    /// <remarks/>
    public string RemoteUrl;
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://novell.com/simias/web/")]
public enum DomainType {
    
    /// <remarks/>
    Workgroup,
    
    /// <remarks/>
    Enterprise,
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://novell.com/simias/web/")]
public enum CredentialType {
    
    /// <remarks/>
    None,
    
    /// <remarks/>
    NotRequired,
    
    /// <remarks/>
    Basic,
    
    /// <remarks/>
    PPK,
}
