/*****************************************************************************
*
* Copyright (c) [2009] Novell, Inc.
* All Rights Reserved.
*
* This program is free software; you can redistribute it and/or
* modify it under the terms of version 2 of the GNU General Public License as
* published by the Free Software Foundation.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.   See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, contact Novell, Inc.
*
* To contact Novell about this file by physical or electronic mail,
* you may find current contact information at www.novell.com
*
*-----------------------------------------------------------------------------
*
*                 $Author: Calvin Gaisford <cgaisford@novell.com>
*                 $Modified by: Satyam <ssutapalli@novell.com>  22/05/2008  Commented log statements
*-----------------------------------------------------------------------------
* This module is used to:
*        <Description of the functionality of the file >
*
*
*******************************************************************************/

#include "SimiasEventHandlers.h"
#include "iFolderApplication.h"
#include "SMEvents.h"
#include "SimiasEventData.h"
#include "applog.h"

SimiasEventClient simiasEventClient;
BOOL connectClients=FALSE;

//===================================================================
// SimiasEventInitialize
// Initializes simias event
//===================================================================
void SimiasEventInitialize(void)
{
	if(sec_init (&simiasEventClient, SimiasEventStateCallBack, &simiasEventClient) != 0)
	{
		[[NSApp delegate] addLogTS:NSLocalizedString(@"Error initializing the events", nil)];
		return;
	}

	if(sec_register(simiasEventClient) != 0)
	{
		[[NSApp delegate] addLogTS:NSLocalizedString(@"Error registering the events", nil)];
		return;
	}
	
	connectClients = TRUE;

	[[NSApp delegate] addLogTS:NSLocalizedString(@"Events initialized and registered", nil)];
}

//===================================================================
// SimiasEventDisconnect
// Disconnect the simias event
//===================================================================
void SimiasEventDisconnect(void)
{
	connectClients = FALSE;
	sec_set_event(simiasEventClient, ACTION_NODE_CREATED, false, nil, nil);
	sec_set_event(simiasEventClient, ACTION_NODE_DELETED, false, nil, nil);
	sec_set_event(simiasEventClient, ACTION_NODE_CHANGED, false, nil, nil);

	sec_set_event(simiasEventClient, ACTION_COLLECTION_SYNC, false, nil, nil);
	sec_set_event(simiasEventClient, ACTION_FILE_SYNC, false, nil, nil);
	sec_set_event(simiasEventClient, ACTION_NOTIFY_MESSAGE, false, nil, nil);

	if(sec_deregister(simiasEventClient) != 0)
	{
		[[NSApp delegate] addLogTS:NSLocalizedString(@"Error deregistering the events", nil)];
		return;
	}

	[[NSApp delegate] addLogTS:NSLocalizedString(@"events de-registered", nil)];
}

//===================================================================
// SimiasEventStateCallBack
// Callback to simias event state
//===================================================================
int SimiasEventStateCallBack(SEC_STATE_EVENT state_event, const char *message, void *data)
{
    NSAutoreleasePool *pool=[[NSAutoreleasePool alloc] init];
	
	SimiasEventClient *sec = (SimiasEventClient *)data;

	if(sec != NULL)
	{
		switch(state_event)
		{
			case SEC_STATE_EVENT_CONNECTED:
				if(	connectClients )
				{
					ifconlog1(@"Event client connected");
					//[[NSApp delegate] simiasHasStarted];
					sec_set_event(*sec, ACTION_NODE_CREATED, true, (SimiasEventFunc)SimiasEventNode, nil);
					sec_set_event(*sec, ACTION_NODE_DELETED, true, (SimiasEventFunc)SimiasEventNode, nil);
					sec_set_event(*sec, ACTION_NODE_CHANGED, true, (SimiasEventFunc)SimiasEventNode, nil);

					sec_set_event(*sec, ACTION_COLLECTION_SYNC, true, (SimiasEventFunc)SimiasEventSyncCollection, nil);
					sec_set_event(*sec, ACTION_FILE_SYNC, true, (SimiasEventFunc)SimiasEventSyncFile, nil);
					sec_set_event(*sec, ACTION_NOTIFY_MESSAGE, true, (SimiasEventFunc)SimiasEventNotifyMessage, nil);
				}
				break;
			case SEC_STATE_EVENT_DISCONNECTED:
				ifconlog1(@"Event client disconnected!");
				break;
			case SEC_STATE_EVENT_ERROR:
				ifconlog1(@"ERROR with Simias Event Client");
				break;
		}
	}
    [pool release];	
	return 0;
}

//===================================================================
// SimiasEventNode
// Get the simias event node fro simias node event queue
//===================================================================
int SimiasEventNode(SimiasNodeEvent *nodeEvent, void *data)
{
    NSAutoreleasePool *pool=[[NSAutoreleasePool alloc] init];
	//ifconlog2(@"SimiasNodeEvent fired: %s", nodeEvent->node);

	NSDictionary *neProps = [getNodeEventProperties(nodeEvent) retain];
	SMNodeEvent *ne = [[SMNodeEvent alloc] init];
	[ne setProperties:neProps];
	[[SimiasEventData sharedInstance] pushEvent:ne];

	[neProps release];
	[ne release];
    [pool release];	
    return 0;
}

//===================================================================
// getNodeEventProperties
// Get the properties of node event
//===================================================================
NSDictionary *getNodeEventProperties(SimiasNodeEvent *nodeEvent)
{
	NSMutableDictionary *newProperties = [[NSMutableDictionary alloc] init];

	if(nodeEvent->event_type != nil)
		[newProperties setObject:[NSString stringWithUTF8String:nodeEvent->event_type] forKey:@"eventType"];
	if(nodeEvent->action != nil)
		[newProperties setObject:[NSString stringWithUTF8String:nodeEvent->action] forKey:@"action"];
	if(nodeEvent->time != nil)
		[newProperties setObject:[NSString stringWithUTF8String:nodeEvent->time] forKey:@"time"];
	if(nodeEvent->source != nil)
		[newProperties setObject:[NSString stringWithUTF8String:nodeEvent->source] forKey:@"source"];
	if(nodeEvent->collection != nil)
		[newProperties setObject:[NSString stringWithUTF8String:nodeEvent->collection] forKey:@"collectionID"];
	if(nodeEvent->type != nil)
		[newProperties setObject:[NSString stringWithUTF8String:nodeEvent->type] forKey:@"type"];
	if(nodeEvent->event_id != nil)
		[newProperties setObject:[NSString stringWithUTF8String:nodeEvent->event_id] forKey:@"event_id"];
	if(nodeEvent->node != nil)
		[newProperties setObject:[NSString stringWithUTF8String:nodeEvent->node] forKey:@"nodeID"];
	if(nodeEvent->flags != nil)
		[newProperties setObject:[NSString stringWithUTF8String:nodeEvent->flags] forKey:@"flags"];
	if(nodeEvent->master_rev != nil)
		[newProperties setObject:[NSString stringWithUTF8String:nodeEvent->master_rev] forKey:@"master_rev"];
	if(nodeEvent->slave_rev != nil)
		[newProperties setObject:[NSString stringWithUTF8String:nodeEvent->slave_rev] forKey:@"slave_rev"];
	if(nodeEvent->file_size != nil)
		[newProperties setObject:[NSString stringWithUTF8String:nodeEvent->file_size] forKey:@"file_size"];

	return [newProperties autorelease];
}


//===================================================================
// CollectionSyncEvents Handlers
//===================================================================

//===================================================================
// SimiasEventSyncCollection
// Get the Collection sync intem from simias event queue
//===================================================================
int SimiasEventSyncCollection(SimiasCollectionSyncEvent *collectionEvent, void *data)
{
    NSAutoreleasePool *pool=[[NSAutoreleasePool alloc] init];
	//ifconlog2(@"SimiasCollectionSyncEvent fired: %s", collectionEvent->name);

	NSDictionary *cseProps = [getCollectionSyncEventProperties(collectionEvent) retain];
	SMCollectionSyncEvent *cse = [[SMCollectionSyncEvent alloc] init];
	[cse setProperties:cseProps];
	[[SimiasEventData sharedInstance] pushEvent:cse];


	
	[cseProps release];
	[cse release];
    [pool release];	
    return 0;
}

//===================================================================
// getCollectionSyncEventProperties
// Get the properties of collection sync event
//===================================================================
NSDictionary *getCollectionSyncEventProperties(SimiasCollectionSyncEvent *collectionEvent)
{
	NSMutableDictionary *newProperties = [[NSMutableDictionary alloc] init];

	if(collectionEvent->event_type != nil)
		[newProperties setObject:[NSString stringWithUTF8String:collectionEvent->event_type] forKey:@"eventType"];
	if(collectionEvent->name != nil)
		[newProperties setObject:[NSString stringWithUTF8String:collectionEvent->name] forKey:@"name"];
	if(collectionEvent->id != nil)
		[newProperties setObject:[NSString stringWithUTF8String:collectionEvent->id] forKey:@"ID"];
	if(collectionEvent->action != nil)
		[newProperties setObject:[NSString stringWithUTF8String:collectionEvent->action] forKey:@"action"];
	if(collectionEvent->connected != nil)
		[newProperties setObject:[NSString stringWithUTF8String:collectionEvent->connected] forKey:@"connected"];

	return [newProperties autorelease];
}


//===================================================================
// FileSyncEvents Handlers
//===================================================================

//===================================================================
// SimiasEventSyncFile
// Get the Sync File event from sync node event queue
//===================================================================
int SimiasEventSyncFile(SimiasFileSyncEvent *fileEvent, void *data)
{
    NSAutoreleasePool *pool=[[NSAutoreleasePool alloc] init];
	//ifconlog2(@"SimiasFileSyncEvent fired: %s", fileEvent->name);

	NSDictionary *fseProps = [getFileSyncEventProperties(fileEvent) retain];
	SMFileSyncEvent *fse = [[SMFileSyncEvent alloc] init];
	[fse setProperties:fseProps];
	[[SimiasEventData sharedInstance] pushEvent:fse];

	[fseProps release];
	[fse release];
    [pool release];	
    return 0;
}

//===================================================================
// getFileSyncEventProperties
// Get the properties of file sync event
//===================================================================
NSDictionary *getFileSyncEventProperties(SimiasFileSyncEvent *fileEvent)
{
	NSMutableDictionary *newProperties = [[NSMutableDictionary alloc] init];

	if(fileEvent->event_type != nil)
		[newProperties setObject:[NSString stringWithUTF8String:fileEvent->event_type] forKey:@"eventType"];
	if(fileEvent->collection_id != nil)
		[newProperties setObject:[NSString stringWithUTF8String:fileEvent->collection_id] forKey:@"collectionID"];
	if(fileEvent->name != nil)
		[newProperties setObject:[NSString stringWithUTF8String:fileEvent->name] forKey:@"name"];
	if(fileEvent->object_type != nil)
		[newProperties setObject:[NSString stringWithUTF8String:fileEvent->object_type] forKey:@"objectType"];
	if(fileEvent->delete_str != nil)
		[newProperties setObject:[NSString stringWithUTF8String:fileEvent->delete_str] forKey:@"delete_str"];
	if(fileEvent->size != nil)
		[newProperties 
			setObject:[NSNumber numberWithDouble:[[NSString stringWithUTF8String:fileEvent->size] doubleValue]]
			forKey:@"size"];
	if(fileEvent->size_to_sync != nil)
		[newProperties 
			setObject:[NSNumber numberWithDouble:[[NSString stringWithUTF8String:fileEvent->size_to_sync] doubleValue]]
			forKey:@"sizeToSync"];
	if(fileEvent->size_remaining != nil)
		[newProperties 
			setObject:[NSNumber numberWithDouble:[[NSString stringWithUTF8String:fileEvent->size_remaining] doubleValue]]
			forKey:@"sizeRemaining"];
	if(fileEvent->direction != nil)
		[newProperties setObject:[NSString stringWithUTF8String:fileEvent->direction] forKey:@"direction"];
	if(fileEvent->status != nil)
		[newProperties setObject:[NSString stringWithUTF8String:fileEvent->status] forKey:@"status"];

	return [newProperties autorelease];
}




//===================================================================
// NotifyEvent Handlers
//===================================================================

//===================================================================
// SimiasEventNotifyMessage
// Get the notify message event from simias node event queue
//===================================================================
int SimiasEventNotifyMessage(SimiasNotifyEvent *notifyEvent, void *data)
{
    NSAutoreleasePool *pool=[[NSAutoreleasePool alloc] init];
	ifconlog3(@"SimiasNotifyEvent fired: %s - %s", notifyEvent->message, notifyEvent->type);

	NSDictionary *neProps = [getNotifyEventProperties(notifyEvent) retain];
	SMNotifyEvent *ne = [[SMNotifyEvent alloc] init];
	[ne setProperties:neProps];
	[[SimiasEventData sharedInstance] pushEvent:ne];

	[neProps release];
	[ne release];
    [pool release];
    return 0;
}

//===================================================================
// getNotifyEventProperties
// Get the properties of notify event
//===================================================================
NSDictionary *getNotifyEventProperties(SimiasNotifyEvent *notifyEvent)
{
	NSMutableDictionary *newProperties = [[NSMutableDictionary alloc] init];

	if(notifyEvent->event_type != nil)
		[newProperties setObject:[NSString stringWithUTF8String:notifyEvent->event_type] forKey:@"eventType"];
	if(notifyEvent->message != nil)
		[newProperties setObject:[NSString stringWithUTF8String:notifyEvent->message] forKey:@"message"];
	if(notifyEvent->time != nil)
		[newProperties setObject:[NSString stringWithUTF8String:notifyEvent->time] forKey:@"time"];
	if(notifyEvent->type != nil)
		[newProperties setObject:[NSString stringWithUTF8String:notifyEvent->type] forKey:@"type"];

	return [newProperties autorelease];
}
