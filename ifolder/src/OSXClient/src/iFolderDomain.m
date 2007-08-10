/***********************************************************************
 |  $RCSfile$
 |
 | Copyright (c) 2007 Novell, Inc.
 | All Rights Reserved.
 |
 | This program is free software; you can redistribute it and/or
 | modify it under the terms of version 2 of the GNU General Public License as
 | published by the Free Software Foundation.
 |
 | This program is distributed in the hope that it will be useful,
 | but WITHOUT ANY WARRANTY; without even the implied warranty of
 | MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 | GNU General Public License for more details.
 |
 | You should have received a copy of the GNU General Public License
 | along with this program; if not, contact Novell, Inc.
 |
 | To contact Novell about this file by physical or electronic mail,
 | you may find current contact information at www.novell.com 
 |
 |  Author: Calvin Gaisford <cgaisford@novell.com>
 | 
 ***********************************************************************/

#import "iFolderDomain.h"
#import "iFolder.h"

@implementation iFolderDomain

-(id) init
{
	if(self = [super init])
	{
		NSArray *keys	= [NSArray arrayWithObjects:	@"name", 
														@"password",
														nil];

		NSArray *values = [NSArray arrayWithObjects:	@"New Domain",
														@"",
														nil];

		properties = [[NSMutableDictionary alloc]
			initWithObjects:values forKeys:keys];
	}
	
	return self;
}



-(void) dealloc
{
	[properties release];
	
	[super dealloc];
}



- (NSMutableDictionary *) properties
{
	return properties;
}




-(void) setProperties: (NSDictionary *)newProperties
{
	if(properties != newProperties)
	{
		[properties autorelease];
		properties = [[NSMutableDictionary alloc] initWithDictionary:newProperties];
	}
}




-(NSString *)ID
{
	return [properties objectForKey:@"ID"];
}


-(NSString *)name
{
	return [self valueForKeyPath:@"properties.name"]; 
}


-(NSString *)userName
{
	return [self valueForKeyPath:@"properties.userName"]; 
}

-(NSString *)userID
{
	return [self valueForKeyPath:@"properties.userID"]; 
}

-(NSString *)host
{
	return [self valueForKeyPath:@"properties.host"]; 
}


-(NSString *)hostURL
{
	return [self valueForKeyPath:@"properties.hostURL"]; 
}


-(NSString *)poBoxID
{
	return [self valueForKeyPath:@"properties.poboxID"]; 
}


-(NSString *)password
{
	return [self valueForKeyPath:@"properties.password"]; 
}

-(NSString *)description
{
	return [self valueForKeyPath:@"properties.description"]; 
}


-(NSNumber *)isDefault
{
	return [self valueForKeyPath:@"properties.isDefault"]; 
}


-(NSNumber *)isSlave
{
	return [self valueForKeyPath:@"properties.isSlave"]; 
}


-(NSNumber *)isEnabled
{
	return [self valueForKeyPath:@"properties.isEnabled"]; 
}

-(BOOL)authenticated
{
	NSNumber *num = [self valueForKeyPath:@"properties.authenticated"];
	if(num != nil)
		return [num boolValue];
	else
		return NO;
}


-(NSNumber *)statusCode
{
	return [self valueForKeyPath:@"properties.statusCode"]; 
}

-(NSNumber *)remainingGraceLogins
{
	return [self valueForKeyPath:@"properties.remainingGraceLogins"]; 
}


@end
