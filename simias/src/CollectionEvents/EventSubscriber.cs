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
 *  Author: Russ Young
 *
 ***********************************************************************/

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Security.Permissions;
using System.Threading;


//[assembly:PermissionSetAttribute(SecurityAction.RequestMinimum, Name = "FullTrust")]
namespace Simias.Event
{
	#region Delegate Definitions.

	/// <summary>
	/// Delegate definition for handling collection events.
	/// </summary>
	public delegate void NodeEventHandler(NodeEventArgs args);

	/// <summary>
	/// Delegate definition for handling Collection root change events.
	/// </summary>
	public delegate void CollectionRootChangedHandler(CollectionRootChangedEventArgs args);
	
	/// <summary>
	/// Delegate definition for file events.
	/// </summary>
	public delegate void FileEventHandler(FileEventArgs args);

	/// <summary>
	/// Delegate definition for rename events.
	/// </summary>
	public delegate void FileRenameEventHandler(FileRenameEventArgs args);

	/// <summary>
	/// Used to get around a marshalling problem seen with explorer.
	/// </summary>
	internal delegate void TemporayEventHandler(EventType type, string args);

	
	#endregion
	
	/// <summary>
	/// Class to Subscibe to collection events.
	/// </summary>
	public class EventSubscriber : MarshalByRefObject, IDisposable
	{
		#region Events

		/// <summary>
		/// Delegate to handle Collection Creations.
		/// A Node or Collection has been created.
		/// </summary>
		public event NodeEventHandler NodeCreated;
		/// <summary>
		/// Delegate to handle Collection Deletions.
		/// A Node or Collection has been deleted.
		/// </summary>
		public event NodeEventHandler NodeDeleted;
		/// <summary>
		/// Delegate to handle Collection Changes.
		/// A Node or Collection modification.
		/// </summary>
		public event NodeEventHandler NodeChanged;
		/// <summary>
		/// Delegate to handle Collection Root Path changes.
		/// </summary>
		public event CollectionRootChangedHandler CollectionRootChanged;
		/// <summary>
		/// Delegate to handle File Creations.
		/// </summary>
		public event FileEventHandler FileCreated;
		/// <summary>
		/// Delegate to handle File Deletions.
		/// </summary>
		public event FileEventHandler FileDeleted;
		/// <summary>
		/// Delegate to handle Files Changes.
		/// </summary>
		public event FileEventHandler FileChanged;
		/// <summary>
		/// Delegate to handle File Renames.
		/// </summary>
		public event FileRenameEventHandler FileRenamed;
		
		#endregion

		#region Private Fields

		InProcessEventBroker	broker = null;
		bool		enabled;
		Regex		fileNameFilter;
		Regex		fileTypeFilter;
		string		nodeIdFilter;
		string		nodeTypeFilter;
		Regex		nodeTypeRegex;
		string		collectionId;
		bool		alreadyDisposed;
		
		#endregion

		#region Constructor/Finalizer

		/// <summary>
		/// Creates a Subscriber to watch the specified Collection.
		/// </summary>
		/// <param name="conf">Configuration Object.</param>
		/// <param name="collectionId">The collection to watch for events.</param>
		public EventSubscriber(Configuration conf, string collectionId)
		{
			enabled = true;
			fileNameFilter = null;
			fileTypeFilter = null;
			nodeIdFilter = null;
			nodeTypeFilter = null;
			nodeTypeRegex = null;
			this.collectionId = collectionId;
			alreadyDisposed = false;
			
			broker = InProcessEventBroker.GetSubscriberBroker(conf);
			broker.NodeChanged += new NodeEventHandler(OnNodeChanged);
			broker.NodeCreated += new NodeEventHandler(OnNodeCreated);
			broker.NodeDeleted += new NodeEventHandler(OnNodeDeleted);
			broker.CollectionRootChanged += new CollectionRootChangedHandler(OnCollectionRootChanged);
			broker.FileChanged += new FileEventHandler(OnFileChanged);
			broker.FileCreated += new FileEventHandler(OnFileCreated);
			broker.FileDeleted += new FileEventHandler(OnFileDeleted);
			broker.FileRenamed += new FileRenameEventHandler(OnFileRenamed);
		}

		/// <summary>
		/// Create a Subscriber to monitor changes in the complete Collection Store.
		/// </summary>
		/// <param name="conf">Configuration object.</param>
		public EventSubscriber(Configuration conf) :
			this(conf, (string)null)
		{
		}

		/// <summary>
		/// Finalizer.
		/// </summary>
		~EventSubscriber()
		{
			Dispose(true);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets and set the enabled state.
		/// </summary>
		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}

		/// <summary>
		/// Gets and Sets the NameFilter.
		/// </summary>
		public string FileNameFilter
		{
			get
			{
				return fileNameFilter.ToString();
			}
			set
			{
				if (value != null)
					fileNameFilter = new Regex(value);
				else
					fileNameFilter = null;
			}
		}

		/// <summary>
		/// Gets and Sets the Type Filter.
		/// </summary>
		public string FileTypeFilter
		{
			get
			{
				return fileTypeFilter.ToString();
			}
			set
			{
				if (value != null)
                    fileTypeFilter = new Regex(value);
				else
					fileTypeFilter = null;
			}
		}

		/// <summary>
		/// Gets and Sets the Node ID filter for collection events.
		/// </summary>
		public string NodeIDFilter
		{
			get {return nodeIdFilter;}
			set {nodeIdFilter = value;}
		}

		/// <summary>
		/// Gets and sets the Node type filter for collection events.
		/// </summary>
		public string NodeTypeFilter
		{
			get {return nodeTypeFilter;}
			set 
			{
				if (value != null)
				{
					nodeTypeFilter = value;
					nodeTypeRegex = new Regex(@"\.*" + nodeTypeFilter);
				}
				else
				{
					nodeTypeFilter = null;
					nodeTypeRegex = null;
				}
			}
		}

		/// <summary>
		/// Gets and Sets the collection to filter on.
		/// </summary>
		public string CollectionId
		{
			get
			{
				return collectionId;
			}
			set
			{
				if (value != null)
					collectionId = value;
				else
					collectionId = null;
			}
		}

		#endregion

		#region Callbacks

		/// <summary>
		/// Callback used by the EventBroker for Change events.
		/// </summary>
		/// <param name="args">Arguments for the event.</param>
		//[OneWay]
		private void OnNodeChanged(NodeEventArgs args)
		{
			if (applyNodeFilter(args))
				NodeChanged(args);
		}

		/// <summary>
		/// Callback used by the EventBroker for Create events.
		/// </summary>
		/// <param name="args">Arguments for the event.</param>
		//[OneWay]
		private void OnNodeCreated(NodeEventArgs args)
		{
			if (applyNodeFilter(args))
				NodeCreated(args);
		}

		/// <summary>
		/// Callback used by the EventBroker for Delete events.
		/// </summary>
		/// <param name="args">Arguments for the event.</param>
		//[OneWay]
		private void OnNodeDeleted(NodeEventArgs args)
		{
			if (applyNodeFilter(args))
				NodeDeleted(args);
		}

		/// <summary>
		/// Callback for Collection Root Change events.
		/// </summary>
		/// <param name="args"></param>
		//[OneWay]
		private void OnCollectionRootChanged(CollectionRootChangedEventArgs args)
		{
			if (applyNodeFilter(args))
				CollectionRootChanged(args);
		}

		/// <summary>
		/// Callback used by the EventBroker for Change events.
		/// </summary>
		/// <param name="args">Arguments for the event.</param>
		//[OneWay]
		private void OnFileChanged(FileEventArgs args)
		{
			if (applyFileFilter(args))
				FileChanged(args);
		}

		/// <summary>
		/// Callback used by the EventBroker for Create events.
		/// </summary>
		/// <param name="args">Arguments for the event.</param>
		//[OneWay]
		private void OnFileCreated(FileEventArgs args)
		{
			if (applyFileFilter(args))
				FileCreated(args);
		}

		/// <summary>
		/// Callback used by the EventBroker for Delete events.
		/// </summary>
		/// <param name="args">Arguments for the event.</param>
		//[OneWay]
		private void OnFileDeleted(FileEventArgs args)
		{
			if (applyFileFilter(args))
				FileDeleted(args);
		}

		/// <summary>
		/// Callback used by the EventBroker for Rename events.
		/// </summary>
		/// <param name="args">Arguments for the event.</param>
		//[OneWay]
		private void OnFileRenamed(FileRenameEventArgs args)
		{
			if (applyFileFilter(args))
				FileRenamed(args);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Called to apply the subscribers filter.
		/// </summary>
		/// <param name="args">The arguments supplied with the event.</param>
		/// <returns>True If matches the filter. False no match.</returns>
		private bool applyNodeFilter(NodeEventArgs args)
		{
			if (enabled)
			{
				if (collectionId == null || args.Collection == collectionId)
				{
					if (this.nodeIdFilter == null || nodeIdFilter == args.Node)
					{
						if (nodeTypeFilter == null || nodeTypeRegex.IsMatch(args.Type))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Called to apply the subscribers filter.
		/// </summary>
		/// <param name="args">The arguments supplied with the event.</param>
		/// <returns>True If matches the filter. False no match.</returns>
		private bool applyFileFilter(FileEventArgs args)
		{
			if (enabled)
			{
				if (collectionId == null || args.Collection == collectionId)
				{
					if (fileNameFilter == null || fileNameFilter.IsMatch(args.Name))
					{
						if (fileTypeFilter == null || fileTypeFilter.IsMatch(args.Type))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private void Dispose(bool inFinalize)
		{
			try 
			{
				if (!alreadyDisposed)
				{
					alreadyDisposed = true;
					
					// Deregister delegates.
					broker.NodeChanged -= new NodeEventHandler(OnNodeChanged);
					broker.NodeCreated -= new NodeEventHandler(OnNodeCreated);
					broker.NodeDeleted -= new NodeEventHandler(OnNodeDeleted);
					broker.CollectionRootChanged -= new CollectionRootChangedHandler(OnCollectionRootChanged);
					broker.FileChanged -= new FileEventHandler(OnFileChanged);
					broker.FileCreated -= new FileEventHandler(OnFileCreated);
					broker.FileDeleted -= new FileEventHandler(OnFileDeleted);
					broker.FileRenamed -= new FileRenameEventHandler(OnFileRenamed);
					broker.Dispose();
					if (!inFinalize)
					{
						GC.SuppressFinalize(this);
					}
				}
			}
			catch {};
		}

		#endregion

		#region MarshalByRefObject overrides

		/// <summary>
		/// This object should not time out.
		/// </summary>
		/// <returns></returns>
		public override Object InitializeLifetimeService()
		{
			return null;
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Called to cleanup any resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(false);
		}

		#endregion

		internal void broker_InternalEvent(EventType type, string args)
		{
			Console.WriteLine(args);
		}
	}
}
