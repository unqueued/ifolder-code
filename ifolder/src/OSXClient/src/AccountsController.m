#import "AccountsController.h"
#import "MainWindowController.h"
#import "iFolderService.h"
#import "iFolderDomain.h"


@implementation AccountsController

- (void)awakeFromNib
{
	NSLog(@"Accounts Controller Awoke from Nib");
	createMode = NO;

	// Initialized the controls
	[name setStringValue:@""];
	[name setEnabled:NO];
	[host setStringValue:@""];
	[host setEnabled:NO];
	[userName setStringValue:@""];
	[userName setEnabled:NO];

	[password setStringValue:@""];
	[password setEnabled:NO];

	[rememberPassword setState:0];
	[rememberPassword setEnabled:NO];
	[enableAccount setState:0];
	[enableAccount setEnabled:NO];
	[defaultAccount setState:0];
	[defaultAccount setEnabled:NO];
	[activate setEnabled:NO];
	
	[removeAccount setEnabled:NO];

	domains = [[NSMutableArray alloc] init];	
	webService = [[iFolderService alloc] init];

	@try
	{
		int x;
		NSArray *newDomains = [webService GetDomains];
		// add all domains that are not workgroup
		for(x=0; x < [newDomains count]; x++)
		{
			iFolderDomain *dom = [newDomains objectAtIndex:x];

			if( [ [dom isSlave] boolValue] )
			{
				NSLog(@"Adding domain %@", [dom name]);
				[domains addObject:dom];
			}
			else
				NSLog(@"Not adding domain %@", [dom name]);
		}
	}
	@catch(NSException ex)
	{
		[[NSApp delegate] addLog:@"Reading domains failed with exception"];
		NSLog(@"Exception in GetDomains: %@ - %@", [ex name], [ex reason]);
	}
}


- (IBAction)activateAccount:(id)sender
{
	NSLog(@"Activate Account clicked");

	if( ([[host stringValue] length] > 0) &&
		([[userName stringValue] length] > 0) &&
		([[password stringValue] length] > 0) )
	{
		@try
		{
			iFolderDomain *newDomain = [webService ConnectToDomain:[userName stringValue] 
				usingPassword:[password stringValue] andHost:[host stringValue]];

			createMode = NO;			
			[domains addObject:newDomain];
			[accounts reloadData];

			NSMutableIndexSet    *childRows = [NSMutableIndexSet indexSet];
			[childRows addIndex:([domains count] - 1)];
			[accounts selectRowIndexes:childRows byExtendingSelection:NO];			
		}
		@catch (NSException *e)
		{
			NSBeginAlertSheet(@"Activation failed", @"OK", nil, nil, 
				parentWindow, nil, nil, nil, nil, 
				[NSString stringWithFormat:@"Activation failed with the error: %@", [e name]]);
		}
	}
}



- (IBAction)addAccount:(id)sender
{
	NSLog(@"Add Account Clicked");
	createMode = YES;
	[accounts deselectAll:self];

	[name setStringValue:@"<new account>"];
	[name setEnabled:YES];
	[host setStringValue:@""];
	[host setEnabled:YES];
	[userName setStringValue:@""];
	[userName setEnabled:YES];
	[password setStringValue:@""];
	[password setEnabled:YES];

	[rememberPassword setState:NO];
	[rememberPassword setEnabled:YES];
	[enableAccount setState:YES];
	[enableAccount setEnabled:NO];
	[defaultAccount setState:YES];
	[defaultAccount setEnabled:NO];

	[removeAccount setEnabled:NO];	

	[activate setEnabled:NO];
	[parentWindow makeFirstResponder:host];
}

- (IBAction)removeAccount:(id)sender
{
	NSLog(@"Remove Account Clicked");
}

- (IBAction)toggleActive:(id)sender
{
	NSLog(@"Toggle Active Clicked");
}

- (IBAction)toggleDefault:(id)sender
{
	NSLog(@"Toggle Default Clicked");
}

- (IBAction)toggleSavePassword:(id)sender
{
	NSLog(@"Toggle Save Password Clicked");
}


-(void)refreshData
{
}


- (void)tableViewSelectionDidChange:(NSNotification *)aNotification
{
	NSLog(@"The selection changed");
	selectedDomain = [domains objectAtIndex:[accounts selectedRow]];

	if(selectedDomain != nil)
	{
		createMode = NO;
		[name setStringValue:[selectedDomain name]];
		[name setEnabled:YES];
		[host setStringValue:[selectedDomain host]];
		[host setEnabled:NO];
		[userName setStringValue:[selectedDomain userName]];
		[userName setEnabled:NO];
		if([selectedDomain password] != nil)
			[password setStringValue:[selectedDomain password]];
		[password setEnabled:YES];

		[rememberPassword setState:0];
		[rememberPassword setEnabled:YES];
		[enableAccount setState:[[selectedDomain isEnabled] boolValue]];
		[enableAccount setEnabled:YES];
		[defaultAccount setState:[[selectedDomain isDefault] boolValue]];
		[defaultAccount setEnabled:![[selectedDomain isDefault] boolValue]];
		
		[activate setEnabled:NO];
		[removeAccount setEnabled:YES];
	}
	else
	{
		[name setStringValue:@""];
		[name setEnabled:NO];
		[host setStringValue:@""];
		[host setEnabled:NO];
		[userName setStringValue:@""];
		[userName setEnabled:NO];

		[password setStringValue:@""];
		[password setEnabled:NO];

		[rememberPassword setState:0];
		[rememberPassword setEnabled:NO];
		[enableAccount setState:0];
		[enableAccount setEnabled:NO];
		[defaultAccount setState:0];
		[defaultAccount setEnabled:NO];	
		
		[activate setEnabled:NO];
		[removeAccount setEnabled:NO];		
	}
}


- (BOOL)selectionShouldChangeInTableView:(NSTableView *)aTableView
{
	NSLog(@"Selection Change queried");

/*
	int selIndex = [domainsController selectionIndex];
	iFolderDomain *dom = [[domainsController arrangedObjects] objectAtIndex:selIndex];
	if([[[dom properties] objectForKey:@"CanActivate"] boolValue] == true )
	{
		NSBeginAlertSheet(@"Save Account", @"Save", @"Don't Save", @"Cancel", 
			[self window], self, @selector(changeAccountResponse:returnCode:contextInfo:), nil, (void *)selIndex, 
			@"The selected account has not been logged in to and saved.  Would you like to login and save it now?");
	}
*/
	return YES;
}



- (void)changeAccountResponse:(NSWindow *)sheet returnCode:(int)returnCode contextInfo:(void *)contextInfo
{
/*
	switch(returnCode)
	{
		case NSAlertDefaultReturn:
			[domainsController setSelectionIndex:(int)contextInfo];
			// login 
			iFolderDomain *dom = [[domainsController arrangedObjects] objectAtIndex:(int)contextInfo];
			break;
		case NSAlertAlternateReturn:
			[domainsController removeObjectAtArrangedObjectIndex:(int)contextInfo];
			break;
		case NSAlertOtherReturn:
		case NSAlertErrorReturn:
			[domainsController setSelectionIndex:(int)contextInfo];
			break;
	}
*/
}


// Delegates for TableView
-(int)numberOfRowsInTableView:(NSTableView *)aTableView
{
	return [domains count];
}

-(id)tableView:(NSTableView *)aTableView objectValueForTableColumn:(NSTableColumn *)aTableColumn row:(int)rowIndex
{
	return [[domains objectAtIndex:rowIndex] name];
}

-(void)tableView:(NSTableView *)aTableView setObjectValue:(id)anObject forTableColumn:(NSTableColumn *)aTableColumn row:(int)rowIndex
{
}

- (void)controlTextDidChange:(NSNotification *)aNotification;
{
	if(	([aNotification object] == userName) ||
		([aNotification object] == host) ||
		([aNotification object] == password) )
	{
		if( ([[host stringValue] length] > 0) &&
			([[userName stringValue] length] > 0) &&
			([[password stringValue] length] > 0) &&
			(createMode) )
			[activate setEnabled:YES];
		else
			[activate setEnabled:NO];
	}
}


@end
