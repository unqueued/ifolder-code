#import "MainWindowController.h"

@implementation MainWindowController


-(id)init
{
    [super init];
    return self;
}




- (void)dealloc
{
    [super dealloc];
}




-(void)awakeFromNib
{
	webService = [[iFolderService alloc] init];

	@try
	{
		NSArray *newDomains = [webService GetDomains];

		[domainsController addObjects:newDomains];
		

		NSArray *newiFolders = [webService GetiFolders];
		if(newiFolders != nil)
		{
			[ifoldersController addObjects:newiFolders];
		}

		// if we have less than two domains, we don't have enterprise
		// so we better ask the user to login
//		if([newDomains count] < 2)
//			[self showLoginWindow:self];
//		else
		[self showWindow:self];
	}
	@catch (NSException *e)
	{
		[self showWindow:self];
	}
}




- (IBAction)showLoginWindow:(id)sender
{
	if(loginController == nil)
	{
		loginController = [[LoginWindowController alloc] initWithWindowNibName:@"LoginWindow"];
	}
	
	[[loginController window] center];
	[loginController showWindow:self];
}




-(void)login:(NSString *)username withPassword:(NSString *)password toServer:(NSString *)server
{
	@try
	{
		iFolderDomain *domain = [webService ConnectToDomain:username usingPassword:password andHost:server];
		[domainsController addObject:domain];
	}
	@catch (NSException *e)
	{
		NSString *error = [e name];
		NSRunAlertPanel(@"Error connecting to Server", [e name], @"OK",nil, nil);
	}

	[self showWindow:self];
}




- (void)createiFolder:(NSString *)path inDomain:(NSString *)domainID
{
	@try
	{
		iFolder *newiFolder = [webService CreateiFolder:path InDomain:domainID];
		[ifoldersController addObject:newiFolder];
	}
	@catch (NSException *e)
	{
		NSString *error = [e name];
		NSRunAlertPanel(@"Error connecting to Server", [e name], @"OK",nil, nil);
	}
}




- (void)addDomain:(iFolderDomain *)newDomain
{
	[domainsController addObject:newDomain];
}




- (void)addiFolder:(iFolder *)newiFolder
{
	[ifoldersController addObject:newiFolder];
}



@end
