/***********************************************************************
 *  $RCSfile$
 *
 *  Copyright � Unpublished Work of Novell, Inc. All Rights Reserved.
 *
 *  THIS WORK IS AN UNPUBLISHED WORK AND CONTAINS CONFIDENTIAL,
 *  PROPRIETARY AND TRADE SECRET INFORMATION OF NOVELL, INC. ACCESS TO 
 *  THIS WORK IS RESTRICTED TO (I) NOVELL, INC. EMPLOYEES WHO HAVE A 
 *  NEED TO KNOW HOW TO PERFORM TASKS WITHIN THE SCOPE OF THEIR 
 *  ASSIGNMENTS AND (II) ENTITIES OTHER THAN NOVELL, INC. WHO HAVE 
 *  ENTERED INTO APPROPRIATE LICENSE AGREEMENTS. NO PART OF THIS WORK 
 *  MAY BE USED, PRACTICED, PERFORMED, COPIED, DISTRIBUTED, REVISED, 
 *  MODIFIED, TRANSLATED, ABRIDGED, CONDENSED, EXPANDED, COLLECTED, 
 *  COMPILED, LINKED, RECAST, TRANSFORMED OR ADAPTED WITHOUT THE PRIOR 
 *  WRITTEN CONSENT OF NOVELL, INC. ANY USE OR EXPLOITATION OF THIS 
 *  WORK WITHOUT AUTHORIZATION COULD SUBJECT THE PERPETRATOR TO 
 *  CRIMINAL AND CIVIL LIABILITY.  
 *
 *  Author: Brady Anderson <banderso@novell.com>
 *
 ***********************************************************************/

using System;
using System.Collections;
using System.ComponentModel;
//using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using Simias;
using Simias.Storage;
using Simias.Sync;
using Simias.POBox;
using Simias.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Simias.POBoxService.Web
{
	/// <summary>
	/// Summary description for Service1.
	/// </summary>
	/// 
	
	[WebService(Namespace="http://novell.com/simias/pobox/")]
	public class POBoxService : System.Web.Services.WebService
	{
		public POBoxService()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		/// <summary>
		/// Ping
		/// Method for clients to determine if POBoxService is
		/// up and running.
		/// </summary>
		/// <param name="sleepFor"></param>
		/// <returns>0</returns>
		[WebMethod]
		public int Ping(int sleepFor)
		{
			Thread.Sleep(sleepFor * 1000);
			return 0;
		}

		/// <summary>
		/// Accept subscription
		/// </summary>
		/// <param name="domainID"></param>
		/// <param name="identityID"></param>
		/// <param name="subscriptionID"></param>
		[WebMethod]
		[SoapDocumentMethod]
		public
		void
		AcceptedSubscription(
			string				domainID, 
			string				fromIdentity, 
			string				toIdentity, 
			string				subscriptionID)
		{
			Simias.POBox.POBox	poBox;
			Store				store = Store.GetStore();
			
			// open the post office box
			poBox = (domainID == Simias.Storage.Domain.WorkGroupDomainID) 
				? Simias.POBox.POBox.GetPOBox(store, domainID)
				: Simias.POBox.POBox.GetPOBox(store, domainID, fromIdentity);

			// check the post office box
			if (poBox == null)
			{
				throw new ApplicationException("PO Box not found.");
			}

			// check that the message has already not been posted
			IEnumerator e = 
				poBox.Search(
					Message.MessageIDProperty, 
					subscriptionID, 
					SearchOp.Equal).GetEnumerator();
			ShallowNode sn = null;
			if (e.MoveNext())
			{
				sn = (ShallowNode) e.Current;
			}

			if (sn == null)
			{
				throw new ApplicationException("Subscription does not exist.");
			}

			// get the subscription object
			Subscription cSub = new Subscription(poBox, sn);

			// Identities need to match up
			if (fromIdentity != cSub.FromIdentity)
			{
				throw new ApplicationException("Identity does not match.");
			}

			if (toIdentity != cSub.ToIdentity)
			{
				throw new ApplicationException("Identity does not match.");
			}

			// FIXME: need to match the caller's ID against the toIdentity

			cSub.Accept(store, cSub.SubscriptionRights);
			poBox.Commit(cSub);
		}

		/// <summary>
		/// Decline subscription
		/// </summary>
		/// <param name="domainID"></param>
		/// <param name="identityID"></param>
		/// <param name="subscriptionID"></param>
		[WebMethod]
		[SoapDocumentMethod]
		public
		void
		DeclinedSubscription(
			string			domainID, 
			string			fromIdentity, 
			string			toIdentity, 
			string			subscriptionID)
		{
			Simias.POBox.POBox	poBox;
			Store				store = Store.GetStore();
			
			// FIXME:  Temp remove
			Console.WriteLine("POBoxService::DeclinedSubscription - called");
			Console.WriteLine("  Subscription ID: " + subscriptionID);

			// open the post office box
			poBox = (domainID == Simias.Storage.Domain.WorkGroupDomainID) 
				? Simias.POBox.POBox.GetPOBox(store, domainID)
				: Simias.POBox.POBox.GetPOBox(store, domainID, fromIdentity);

			// check the post office box
			if (poBox == null)
			{
				throw new ApplicationException("PO Box not found.");
			}

			// check that the message has already not been posted
			IEnumerator e = 
				poBox.Search(
				Message.MessageIDProperty, 
				subscriptionID, 
				SearchOp.Equal).GetEnumerator();
			ShallowNode sn = null;
			if (e.MoveNext())
			{
				sn = (ShallowNode) e.Current;
			}

			if (sn == null)
			{
				throw new ApplicationException("Subscription does not exist.");
			}

			// get the subscription object
			Subscription cSub = new Subscription(poBox, sn);

			// Identities need to match up
			if (fromIdentity != cSub.FromIdentity)
			{
				throw new ApplicationException("Identity does not match.");
			}

			if (toIdentity != cSub.ToIdentity)
			{
				throw new ApplicationException("Identity does not match.");
			}

			cSub.Decline();
			poBox.Commit(cSub);
		}

		/// <summary>
		/// Acknowledge the subscription.
		/// </summary>
		/// <param name="domainID"></param>
		/// <param name="identityID"></param>
		/// <param name="messageID"></param>
		[WebMethod]
		[SoapDocumentMethod]
		public
		void
		AckSubscription(
			string			domainID, 
			string			fromIdentity, 
			string			toIdentity, 
			string			messageID)
		{
			Simias.POBox.POBox	poBox;
			Store				store = Store.GetStore();
			
			// FIXME:  Temp remove
			Console.WriteLine("POBoxService::AckSubscription - called");

			// open the post office box
			poBox = (domainID == Simias.Storage.Domain.WorkGroupDomainID) 
				? Simias.POBox.POBox.GetPOBox(store, domainID)
				: Simias.POBox.POBox.GetPOBox(store, domainID, fromIdentity);

			// check the post office box
			if (poBox == null)
			{
				throw new ApplicationException("PO Box not found.");
			}

			// check that the message has already not been posted
			IEnumerator e = 
				poBox.Search(
					Message.MessageIDProperty, 
					messageID, 
					SearchOp.Equal).GetEnumerator();
			ShallowNode sn = null;
			if (e.MoveNext())
			{
				sn = (ShallowNode) e.Current;
			}

			if (sn == null)
			{
				throw new ApplicationException("Subscription does not exist.");
			}

			// get the subscription object
			Subscription cSub = new Subscription(poBox, sn);

			// Identities need to match up
			if (fromIdentity != cSub.FromIdentity)
			{
				throw new ApplicationException("Identity does not match.");
			}

			if (toIdentity != cSub.ToIdentity)
			{
				throw new ApplicationException("Identity does not match.");
			}

			// FIXME: need to match the caller's ID against the toIdentity

			cSub.SubscriptionState = Simias.POBox.SubscriptionStates.Acknowledged;
			poBox.Commit(cSub);
			poBox.Commit(poBox.Delete(cSub));
		}

		/// <summary>
		/// Get the subscription information
		/// </summary>
		/// <param name="domainID"></param>
		/// <param name="identityID"></param>
		/// <param name="messageID"></param>
		/// <returns></returns>
		[WebMethod]
		[SoapDocumentMethod]
		public
		SubscriptionInformation 
		GetSubscriptionInfo(string domainID, string identityID, string messageID)
		{
			Simias.POBox.POBox	poBox;
			Store store = Store.GetStore();

			// open the post office box
			poBox =
				(domainID == Simias.Storage.Domain.WorkGroupDomainID)
				? Simias.POBox.POBox.GetPOBox(store, domainID)
				: Simias.POBox.POBox.GetPOBox(store, domainID, identityID);
			
			// check the post office box
			if (poBox == null)
			{
				throw new ApplicationException("PO Box not found.");
			}

			// check that the message has already not been posted
			IEnumerator e = 
				poBox.Search(Message.MessageIDProperty, messageID, SearchOp.Equal).GetEnumerator();
			
			ShallowNode sn = null;

			if (e.MoveNext())
			{
				sn = (ShallowNode) e.Current;
			}

			if (sn == null)
			{
				throw new ApplicationException("Subscription does not exists.");
			}

			// generate the subscription info object and return it
			Subscription cSub = new Subscription(poBox, sn);

			// Validate the shared collection
			Collection cSharedCollection = store.GetCollectionByID(cSub.SubscriptionCollectionID);
			if (cSharedCollection == null)
			{
				throw new ApplicationException("Collection not found.");
			}

			if (cSub.SubscriptionCollectionURL == null)
			{
				SyncCollection sc = new SyncCollection(cSharedCollection);
				cSub.SubscriptionCollectionURL = sc.MasterUrl.ToString();
				poBox.Commit(cSub);
			}

			SubscriptionInformation subInfo = new SubscriptionInformation();
			subInfo.GenerateFromSubscription(cSub);

			return subInfo;
		}

		/// <summary>
		/// Invite a user to a shared collection
		/// </summary>
		/// <param name="domainID"></param>
		/// <param name="fromUserID"></param>
		/// <param name="toUserID"></param>
		/// <param name="sharedCollectionID"></param>
		/// <param name="sharedCollectionType"></param>
		/// <returns>success subscription ID - failure empty string</returns>
		[WebMethod]
		[SoapDocumentMethod]
		public string Invite(
			string			domainID, 
			string			fromUserID,
			string			toUserID,
			string			sharedCollectionID,
			string			sharedCollectionType)
		{
			Collection			sharedCollection;
			Simias.POBox.POBox	poBox = null;
			Store				store = Store.GetStore();
			Subscription		cSub = null;

			if (domainID == null || domainID == "")
			{
				domainID = store.DefaultDomain;
			}
			
			// Verify domain
			Simias.Storage.Domain cDomain = store.GetDomain(domainID);
			if (cDomain == null)
			{
				throw new ApplicationException("Invalid Domain ID");
			}

			// Verify and get additional information about the "To" user
			Simias.Storage.Roster currentRoster = cDomain.GetRoster(store);
			if (currentRoster == null)
			{
				throw new ApplicationException("No member Roster exists for the specified Domain");
			}

			Member toMember = currentRoster.GetMemberByID(toUserID);
			if (toMember == null)
			{
				throw new ApplicationException("Specified \"toUserID\" does not exist in the Domain Roster");
			}

			Member fromMember = currentRoster.GetMemberByID(fromUserID);
			if (fromMember == null)
			{
				throw new ApplicationException("Specified \"fromUserID\" does not exist in the Domain Roster");
			}

			// FIXME:  Verify the fromMember is the caller

			sharedCollection = store.GetCollectionByID(sharedCollectionID); 
			if (sharedCollection == null)
			{
				throw new ApplicationException("Invalid shared collection ID");
			}

			try
			{
				poBox = 
					(domainID == Simias.Storage.Domain.WorkGroupDomainID)
						? POBox.POBox.GetPOBox(store, domainID)
						: POBox.POBox.GetPOBox(store, domainID, toUserID);

				cSub = new Subscription(sharedCollection.Name + " subscription", "Subscription", fromUserID);
				cSub.SubscriptionState = Simias.POBox.SubscriptionStates.Received;
				cSub.ToName = toMember.Name;
				cSub.ToIdentity = toUserID;
				cSub.FromName = fromMember.Name;
				cSub.FromIdentity = fromUserID;

				// FIXME:
				string serviceUrl = 
						"http://" + 
						this.Context.Request.Url.Host +
						":" +
						this.Context.Request.Url.Port.ToString() +
						"/POBoxService.asmx";

				cSub.POServiceURL = new Uri(serviceUrl);
				cSub.SubscriptionCollectionID = sharedCollection.ID;
				cSub.SubscriptionCollectionType = sharedCollectionType;
				cSub.SubscriptionCollectionName = sharedCollection.Name;
				//cSub.SubscriptionCollectionURL = "http://" + hostAndPort[0] + ":6436/SyncService.rem";
				cSub.DomainID = domainID;
				cSub.DomainName = cDomain.Name;
				cSub.SubscriptionKey = Guid.NewGuid().ToString();
				cSub.MessageType = "Outbound";  // ????

				SyncCollection sc = new SyncCollection(sharedCollection);
				cSub.SubscriptionCollectionURL = sc.MasterUrl.ToString();

				DirNode dirNode = sharedCollection.GetRootDirectory();
				if(dirNode != null)
				{
					cSub.DirNodeID = dirNode.ID;
					cSub.DirNodeName = dirNode.Name;
				}

				poBox.Commit(cSub);
				return(cSub.MessageID);
			}
			catch{}
			return("");
		}

		/// <summary>
		/// Return the Default Domain
		/// </summary>
		/// <param name="dummy">Dummy parameter so stub generators won't produce empty structures</param>
		/// <returns>default domain</returns>
		[WebMethod]
		public string GetDefaultDomain(int dummy)
		{
			return(Store.GetStore().DefaultDomain);
		}
	}

	[Serializable]
	public class SubscriptionInformation
	{
		public string	MsgID;
		public string	FromID;
		public string	FromName;
		public string	ToID;
		public string	ToName;

		public string	CollectionID;
		public string	CollectionName;
		public string	CollectionType;
		public string	CollectionUrl;

		public string	DirNodeID;
		public string	DirNodeName;

		public string	DomainID;
		public string	DomainName;

		public int		State;
		public int		Disposition;

		public SubscriptionInformation()
		{

		}

		internal void GenerateFromSubscription(Subscription cSub)
		{
			this.MsgID = cSub.MessageID;
			this.FromID = cSub.FromIdentity;
			this.FromName = cSub.FromName;
			this.ToID = cSub.ToIdentity;
			this.ToName = cSub.ToName;

			this.CollectionID = cSub.SubscriptionCollectionID;
			this.CollectionName = cSub.SubscriptionCollectionName;
			this.CollectionType = cSub.SubscriptionCollectionType;
			this.CollectionUrl = cSub.SubscriptionCollectionURL;

			this.DirNodeID = cSub.DirNodeID;
			this.DirNodeName = cSub.DirNodeName;

			this.DomainID = cSub.DomainID;
			this.DomainName = cSub.DomainName;

			this.State = (int) cSub.SubscriptionState;
			this.Disposition = (int) cSub.SubscriptionDisposition;
		}
	}
}
