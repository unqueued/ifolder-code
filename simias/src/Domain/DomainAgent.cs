/***********************************************************************
 *  $RCSfile$
 *
 *  Copyright (C) 2004 Novell, Inc.
 *
 *  This program is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU General Public
 *  License as published by the Free Software Foundation; either
 *  version 2 of the License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public
 *  License along with this program; if not, write to the Free
 *  Software Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
 *
 *  Author: Rob
 *
 ***********************************************************************/

using System;
using System.Net;
using System.Threading;
using System.Web;
using System.Xml;

using Simias;
using Simias.Authentication;
using Simias.Client;
using Simias.POBox;
using Simias.Storage;
using Simias.Sync;

using Novell.Security.ClientPasswordManager;

using PostOffice = Simias.POBox;

namespace Simias.Domain
{
	/// <summary>
	/// Class used to assist in configuring the domain.
	/// </summary>
	public class DomainConfig
	{
		#region Class Members
		/// <summary>
		/// Configuration file XML tags.
		/// </summary>
		private static string SectionTag = "DomainClient";
		private static string ServersTag = "Servers";
		private static string ServerTag = "Server";
		private static string NameTag = "name";
		private static string IDTag = "id";
		private static string DescriptionTag = "description";
		private static string UriTag = "uri";
		private static string EnabledTag = "enabled";

		/// <summary>
		/// Holds a reference to the simias configuration file.
		/// </summary>
		private Configuration config = Configuration.GetConfiguration();

		/// <summary>
		/// Identifier of the domain for which configuration information is associated with.
		/// </summary>
		private string domainID;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the domain ID.
		/// </summary>
		public string ID
		{
			get { return domainID; }
			set { domainID = value; SetDomainAttribute(IDTag, value); }
		}

		/// <summary>
		/// Gets or sets the domain name.
		/// </summary>
		public string Name
		{
			get { return GetDomainAttribute(NameTag); }
			set { SetDomainAttribute(NameTag, value ); }
		}

		/// <summary>
		/// Gets or sets the domain description.
		/// </summary>
		public string Description
		{
			get { return GetDomainAttribute(DescriptionTag); }
			set { SetDomainAttribute(DescriptionTag, value); }
		}

		/// <summary>
		/// Gets or sets the domain host.
		/// </summary>
		public string Host
		{
			get	
			{ 
				Uri uri = ServiceUrl;
				return (uri != null) ? uri.Host : null; 
			}

			set	
			{ 
				Uri uri = ServiceUrl;
				UriBuilder ub = (uri != null) ? new UriBuilder(uri) : new UriBuilder(Uri.UriSchemeHttp, value);
				ub.Host = value;
				ServiceUrl = ub.Uri;
			}
		}

		/// <summary>
		/// Gets or sets the domain port.
		/// </summary>
		public int Port
		{
			get	
			{ 
				Uri uri = ServiceUrl;
				return (uri != null) ? uri.Port : -1; 
			}

			set 
			{ 
				Uri uri = ServiceUrl;
				UriBuilder ub = (uri != null) ? new UriBuilder(uri) : new UriBuilder(Uri.UriSchemeHttp, IPAddress.Any.ToString());
				ub.Port = value;
				ServiceUrl = ub.Uri;
			}
		}

		/// <summary>
		/// Gets or sets whether the domain is enabled.
		/// </summary>
		public bool Enabled
		{
			get
			{
				string enableString = GetDomainAttribute(EnabledTag);
				return (enableString != null) ? Convert.ToBoolean(enableString) : false;
			}

			set { SetDomainAttribute(EnabledTag, value.ToString()); }
		}

		/// <summary>
		/// Gets or sets the url scheme.
		/// </summary>
		public string Scheme
		{
			get 
			{ 
				Uri uri = ServiceUrl;
				return (uri != null) ? uri.Scheme : Uri.UriSchemeHttp; 
			}

			set 
			{ 
				Uri uri = ServiceUrl;
				UriBuilder ub = (uri != null) ? new UriBuilder(uri) : new UriBuilder(Uri.UriSchemeHttp, IPAddress.Any.ToString());
				ub.Scheme = value;
				ServiceUrl = ub.Uri;
			}
		}

		/// <summary>
		/// Gets the domain url
		/// </summary>
		public Uri ServiceUrl
		{
			get 
			{ 
				string uriString = GetDomainAttribute(UriTag);
				return (uriString != null) ? new Uri(uriString) : null;
			}

			set { SetDomainAttribute(UriTag, value.ToString()); }
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of this object.
		/// </summary>
		/// <param name="domainID">Identifier of the domain.</param>
		public DomainConfig(string domainID)
		{
			this.domainID = domainID;
		}

		/// <summary>
		/// Initializes a new instance of this object.
		/// </summary>
		public DomainConfig()
		{
			Store store = Store.GetStore();
			domainID = store.DefaultDomain;
		}
		#endregion

		#region Private Methods
		private XmlElement GetServerElement(XmlElement rootElement)
		{
			XmlElement serverElement = null;
			foreach(XmlElement element in rootElement)
			{
				if (element.GetAttribute(IDTag) == domainID)
				{
					serverElement = element;
					break;
				}
			}

			return serverElement;
		}

		private string GetDomainAttribute(string tag)
		{
			XmlElement root = config.GetElement(SectionTag, ServersTag);
			XmlElement element = GetServerElement(root);
			return (element != null) ? element.GetAttribute(tag) : null;
		}

		private void SetDomainAttribute(string tag, string tagValue)
		{
			XmlElement root = config.GetElement(SectionTag, ServersTag);
			XmlElement element = GetServerElement(root);
			if (element == null)
			{
				element = root.OwnerDocument.CreateElement(ServerTag);
				element.SetAttribute(IDTag, domainID);
				root.AppendChild(element);
			}

			element.SetAttribute(tag, tagValue);
			config.SetElement(SectionTag, ServersTag, root);
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Sets all the parameters in one shot.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="serviceAddress"></param>
		/// <param name="enabled"></param>
		public void SetAttributes(string name, string description, Uri serviceAddress, bool enabled)
		{
			XmlElement root = config.GetElement(SectionTag, ServersTag);
			XmlElement element = GetServerElement(root);
			if (element == null)
			{
				element = root.OwnerDocument.CreateElement(ServerTag);
				root.AppendChild(element);
			}

			element.SetAttribute(IDTag, domainID);
			element.SetAttribute(NameTag, name);
			element.SetAttribute(DescriptionTag, description);
			element.SetAttribute(UriTag, serviceAddress.ToString());
			element.SetAttribute(EnabledTag, enabled.ToString());
			config.SetElement(SectionTag, ServersTag, root);
		}

		/// <summary>
		/// Removes the domain server entry from the configuration file.
		/// </summary>
		public void Delete()
		{
			XmlElement root = config.GetElement(SectionTag, ServersTag);
			XmlElement element = GetServerElement(root);
			if (element != null)
			{
				root.RemoveChild(element);
				config.SetElement(SectionTag, ServersTag, root);
			}
		}
		#endregion
	}

	/// <summary>
	/// Simias Domain Agent
	/// </summary>
	public class DomainAgent
	{
		#region Class Members
		private static readonly ISimiasLog log = SimiasLogManager.GetLogger(typeof(DomainAgent));
	
		/// <summary>
		/// Service type for this service.
		/// </summary>
		private static string DomainServiceType = "Domain Service";
		private static string DomainServicePath = "/simias10/DomainService.asmx";

		private Store store = Store.GetStore();
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		public DomainAgent()
		{
		}
		#endregion

		#region Private Methods
		private void CreateRosterProxy(Store store, Storage.Domain domain, string userID, DomainInfo info, Uri host)
		{
			// Create a new roster
			Roster roster = new Roster(store, info.RosterID, domain);
			roster.Proxy = true;
			
			// Create roster member.
			Access.Rights rights = ( Access.Rights )Enum.Parse( typeof( Access.Rights ), info.MemberRights );
			Member member = new Member( info.MemberNodeName, info.MemberNodeID, userID, rights, null );
			member.Proxy = true;
			member.IsOwner = true;

			// commit
			roster.Commit( new Node[] { roster, member } );
		}

		private void CreatePOBoxProxy(Store store, string domainID, ProvisionInfo info, Uri host)
		{
			// Create a new POBox
			PostOffice.POBox poBox = new PostOffice.POBox(store, info.POBoxName, info.POBoxID, domainID);
			poBox.Priority = 0;
			poBox.Proxy = true;
			
			// Create member.
			Access.Rights rights = ( Access.Rights )Enum.Parse( typeof( Access.Rights ), info.MemberRights );
			Member member = new Member( info.MemberNodeName, info.MemberNodeID, info.UserID, rights, null );
			member.Proxy = true;
			member.IsOwner = true;
			
			// commit
			poBox.Commit( new Node[] { poBox, member } );
		}
		#endregion

		#region Internal Methods
		/// <summary>
		/// Removes all traces of the domain from this machine.
		/// </summary>
		/// <param name="domainID">The identifier of the domain to remove.</param>
		internal void RemoveDomainInformation( string domainID )
		{
			// Cannot remove the workgroup domain.
			if ( domainID == Simias.Storage.Domain.WorkGroupDomainID )
			{
				throw new SimiasException("The WorkGroup domain cannot be removed.");
			}

			// If the default domain is the one that is being deleted, set a new one.
			if (store.DefaultDomain == domainID)
			{
				// If there are no other domains present, the default goes back to
				// the workgroup domain.
				string defaultDomain = Simias.Storage.Domain.WorkGroupDomainID;

				// Set the new default domain.
				LocalDatabase ldb = store.GetDatabaseObject();
				ICSList dList = ldb.GetNodesByType(NodeTypes.DomainType);
				foreach(ShallowNode sn in dList)
				{
					// Find the first domain that is not the one being deleted or is the
					// WorkGroup domain.
					if ((sn.ID != domainID) && (sn.ID != Simias.Storage.Domain.WorkGroupDomainID))
					{
						defaultDomain = sn.ID;
						break;
					}
				}

				// Set the new default domain.
				ldb.DefaultDomain = defaultDomain;
			}

			// Get a list of all the collections that belong to this domain and delete them.
			ICSList cList = store.GetCollectionsByDomain(domainID);
			foreach(ShallowNode sn in cList)
			{
				Collection c = new Collection(store, sn);
				c.Commit(c.Delete());
			}

			// Remove the local domain information.
			store.DeleteDomainIdentity(domainID);

			// Remove the domain entry from the configuration file.
			DomainConfig domainConfig = new DomainConfig( domainID);
			domainConfig.Delete();
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Attach to an enterprise system.
		/// </summary>
		/// <param name="host">Url of the enterprise server.</param>
		/// <param name="user">User to provision on the server.</param>
		/// <param name="password">Password to validate user.</param>
		/// <returns>
		/// The Domain ID of the newly attached Domain
		/// </returns>
		public string Attach(string host, string user, string password)
		{
			// Get a URL to our service.
			Uri domainServiceUrl = WSInspection.GetServiceUrl( host, DomainServiceType );
			if ( domainServiceUrl == null )
			{
				// There was a failure in obtaining the service url. Try a hard coded one.
				domainServiceUrl = new Uri( Uri.UriSchemeHttp + Uri.SchemeDelimiter + host + DomainServicePath );
			}

			// Get just the path portion of the URL.
			string hostString = domainServiceUrl.ToString();
			int startIndex = hostString.LastIndexOf( "/" );
			Uri hostUri = new Uri( hostString.Remove( startIndex, hostString.Length - startIndex ) );

			// Create the domain service web client object.
			DomainService domainService = new DomainService();
			domainService.Url = domainServiceUrl.ToString();

			// Setup the credentials
			NetworkCredential myCred = new NetworkCredential(user, password); 
			CredentialCache myCache = new CredentialCache();
			myCache.Add(new Uri(domainService.Url), "Basic", myCred);
			domainService.Credentials = myCache;
			domainService.CookieContainer = new CookieContainer();
			domainService.PreAuthenticate = true;
			domainService.Timeout = 30000;
			
			int normalThreads = 9999;
			int	completionPortThreads = 8888;
			ThreadPool.GetAvailableThreads(out normalThreads, out completionPortThreads);
			
			log.Debug("Available threads: " + normalThreads.ToString());
			log.Debug("Calling " + domainService.Url + " to provision the user");

			// provision user
			ProvisionInfo provisionInfo = domainService.ProvisionUser(user, password);
			if (provisionInfo == null)
			{
				throw new ApplicationException("User does not exist on server.");
			}
				
			log.Debug("the user has been provisioned on the remote domain");

			// get domain info
			DomainInfo domainInfo = domainService.GetDomainInfo(provisionInfo.UserID);

			Store store = Store.GetStore();

			// create domain node
			Storage.Domain domain = 
				store.AddDomainIdentity(
				provisionInfo.UserID,
				domainInfo.Name, 
				domainInfo.ID, 
				domainInfo.Description,
				hostUri);

			// set the default domain
			string previousDomain = store.DefaultDomain;
			store.DefaultDomain = domainInfo.ID;

			// authentication was successful - save the credentials
			new NetCredential("iFolder", domainInfo.ID, true, user, password);

			try
			{
				if (store.GetCollectionByID(provisionInfo.POBoxID) == null)
				{
					// create PO Box proxy
					CreatePOBoxProxy(store, domainInfo.ID, provisionInfo, hostUri );
					log.Debug("Creating PO Box Proxy: {0}", provisionInfo.POBoxName);
				}

				// create roster if needed
				if (store.GetCollectionByID(domainInfo.RosterID) == null)
				{
					// create roster proxy
					CreateRosterProxy(store, domain, provisionInfo.UserID, domainInfo, hostUri );
					log.Debug("Creating Roster Proxy: {0}", domainInfo.RosterName);
				}

				// Set the host and port number in the configuration file.
				DomainConfig domainConfig = new DomainConfig(domainInfo.ID);
				domainConfig.SetAttributes(domain.Name, domain.Description, hostUri, true);

				// Commit the changes to the domain object.
				store.LocalDb.Commit( domain );
			}
			catch(Exception e)
			{
				// restore the previous domain
				store.DefaultDomain = previousDomain;
				throw e;
			}

			return domainInfo.ID;
		}

		/// <summary>
		/// Removes this workstation from the domain or removes the workstation from all machines
		/// owned by the user.
		/// </summary>
		/// <param name="domainID">The identifier of the domain to remove.</param>
		/// <param name="localOnly">If true then only this workstation is removed from the domain.
		/// If false, then the domain will be deleted from every workstation that the user owns.</param>
		public void Unattach(string domainID, bool localOnly)
		{
			// Cannot remove the workgroup domain.
			if ( domainID == Simias.Storage.Domain.WorkGroupDomainID )
			{
				throw new SimiasException("The WorkGroup domain cannot be removed.");
			}

			// Get the domain object.
			Simias.Storage.Domain domain = store.GetDomain(domainID);
			if (domain == null)
			{
				throw new SimiasException("The domain does not exist in the store.");
			}

			// Get who the user is in the specified domain.
			string userID = store.GetUserIDFromDomainID(domainID);

			// Construct the web client.
			DomainService domainService = new DomainService();

			// This information needs to be gathered before the local domain collections are deleted.
			if ( !localOnly )
			{
				// Set the address to the server.
				domainService.Url = domain.HostAddress.ToString() + "/DomainService.asmx";

				// Get the credentials for this user.
				Credentials cSimiasCreds = new Credentials(domainID, userID);
				domainService.Credentials = cSimiasCreds.GetCredentials();
				if (domainService.Credentials == null)
				{
					throw new ApplicationException("No credentials available for specified collection.");
				}

				domainService.PreAuthenticate = true;
			}

			// Find the user's POBox for this domain.
			POBox.POBox poBox = POBox.POBox.FindPOBox(store, domainID, userID);
			if (poBox == null)
			{
				throw new SimiasException(String.Format("Cannot find POBox belonging to domain {0}", domainID));
			}

			// Delete the POBox for this domain which will start the domain cleanup process.
			poBox.Commit(poBox.Delete());

			if (!localOnly)
			{
				// Remove the user from the domain server.
				domainService.RemoveServerCollections(domainID, userID);
			}
		}

		/// <summary>
		/// Create the master on the server.
		/// </summary>
		/// <param name="collection">Collection to create on the enterprise server.</param>
		public void CreateMaster(SyncCollection collection)
		{
			// Get the domain object.
			Simias.Storage.Domain domain = store.GetDomain(collection.Domain);
			if (domain == null)
			{
				throw new SimiasException("The domain does not exist in the store.");
			}

			// Construct the web client.
			DomainService domainService = new DomainService();
			domainService.Url = domain.HostAddress.ToString() + "/DomainService.asmx";
			Credentials cSimiasCreds = new Credentials(collection.ID);
			domainService.Credentials = cSimiasCreds.GetCredentials();

			if (domainService.Credentials == null)
			{
				throw new ApplicationException("No credentials available for specified collection.");
			}

			domainService.PreAuthenticate = true;

			string rootID = null;
			string rootName = null;

			DirNode rootNode = collection.GetRootDirectory();
			if (rootNode != null)
			{
				rootID = rootNode.ID;
				rootName = rootNode.Name;
			}

			Member member = collection.Owner;

			domainService.CreateMaster(
				collection.ID, 
				collection.Name, 
				rootID, 
				rootName, 
				member.UserID, 
				member.Name, 
				member.ID, 
				member.Rights.ToString() );

			collection.CreateMaster = false;
			collection.Commit();
		}
		#endregion
	}
}
