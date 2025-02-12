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
*                 $Author: Timothy Hatcher <timothy@colloquy.info> Karl?Adam?<karl@colloquy.info>
*                 $Modified by: <Modifier>
*                 $Mod Date: <Date Modified>
*                 $Revision: 0.0
*-----------------------------------------------------------------------------
* This module is used to:
*        <Description of the functionality of the file >
*
*
*******************************************************************************/

#import "KABubbleWindowController.h"
#import "KABubbleWindowView.h"



@implementation KABubbleWindowController

#define TIMER_INTERVAL ( 1. / 30. )
#define FADE_INCREMENT 0.05
#define DISPLAY_TIME 10.
#define KABubblePadding 10.

#pragma mark -

+ (KABubbleWindowController *) bubble
{
	return [[[self alloc] init] autorelease];
}

+ (KABubbleWindowController *) bubbleWithTitle:(NSString *) title text:(id) text icon:(NSImage *) icon
 {	
	float titleHeight;
	float titleWidth;
	float textWidth;
	float textHeight;
	
	NSAttributedString *attStrTitle = [[NSAttributedString alloc] initWithString:(NSString*)title attributes:[NSDictionary dictionaryWithObjectsAndKeys:[NSFont boldSystemFontOfSize:13.],NSFontAttributeName,[NSColor blackColor],NSForegroundColorAttributeName,nil]];
	
	NSAttributedString *attStrText = [[NSAttributedString alloc] initWithString:(NSString*)text attributes:[NSDictionary dictionaryWithObjectsAndKeys:[NSFont messageFontOfSize:11.],NSFontAttributeName,[NSColor blackColor],NSForegroundColorAttributeName,nil]];

	NSSize sizTitle = [attStrTitle size];
	titleWidth = sizTitle.width;
	titleHeight = sizTitle.height;
	
	NSSize sizText = [attStrText size];
	textWidth = sizText.width;
	textHeight = sizText.height;

	//[ret setFrame:newRect];
	
	id ret = [[[self alloc] init] autorelease];
	
	NSRect windowRect = NSMakeRect(0,0,titleWidth + 75.0,(textWidth/titleWidth)*textHeight + titleHeight + 30.0);
	[[ret window] setFrame:windowRect display:NO];
	NSRect screen = [[NSScreen mainScreen] visibleFrame];
	
	[[ret window] setFrameTopLeftPoint:NSMakePoint( NSWidth( screen ) - NSWidth( [[ret window] frame] ) - KABubblePadding, NSMaxY( screen ) - KABubblePadding )];
	
	[ret setTitle:title];
	if( [text isKindOfClass:[NSString class]] ) [ret setText:text];
	else if( [text isKindOfClass:[NSAttributedString class]] ) [ret setAttributedText:text];
	[ret setIcon:icon];
	return ret;
}

- (id) init
{	
	NSPanel *panel = [[[NSPanel alloc] initWithContentRect:NSMakeRect( 0., 0., 75.0,30.0) styleMask:NSBorderlessWindowMask backing:NSBackingStoreBuffered defer:NO] autorelease];
		
	[panel setBecomesKeyOnlyIfNeeded:YES];
	[panel setHidesOnDeactivate:NO];
	[panel setBackgroundColor:[NSColor clearColor]];
	[panel setLevel:NSStatusWindowLevel];
	[panel setAlphaValue:0.];
	[panel setOpaque:NO];
	[panel setHasShadow:YES];
	[panel setCanHide:NO];
	[panel setReleasedWhenClosed:YES];
	[panel setDelegate:self];

	KABubbleWindowView *view = [[[KABubbleWindowView alloc] initWithFrame:[panel frame]] autorelease];
	[view setTarget:self];
	[view setAction:@selector( _bubbleClicked: )];
	[panel setContentView:view];

	//NSRect screen = [[NSScreen mainScreen] visibleFrame];
		
	//[panel setFrameTopLeftPoint:NSMakePoint( NSWidth( screen ) - NSWidth( [panel frame] ) - KABubblePadding, NSMaxY( screen ) - KABubblePadding )];
	
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector( _applicationDidSwitch: ) name:NSApplicationDidBecomeActiveNotification object:[NSApplication sharedApplication]];
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector( _applicationDidSwitch: ) name:NSApplicationDidHideNotification object:[NSApplication sharedApplication]];

	_autoFadeOut = YES;
	_delegate = nil;
	_target = nil;
	_representedObject = nil;
	_action = NULL;
	_animationTimer = nil;
	
	return ( self = [super initWithWindow:panel] );
}

- (void) dealloc
{
	[[NSNotificationCenter defaultCenter] removeObserver:self];
	
	[_target release];
	[_representedObject release];
	[_animationTimer invalidate];
	[_animationTimer release];

	_target = nil;
	_representedObject = nil;
	_delegate = nil;
	_animationTimer = nil;

	[super dealloc];
}

#pragma mark -

- (void) _stopTimer
{
	[_animationTimer invalidate];
	[_animationTimer release];
	_animationTimer = nil;
}

- (void) _waitBeforeFadeOut
{
	[self _stopTimer];
	_animationTimer = [[NSTimer scheduledTimerWithTimeInterval:DISPLAY_TIME target:self selector:@selector( startFadeOut ) userInfo:nil repeats:NO] retain];
}

- (void) _fadeIn:(NSTimer *) inTimer
{
	if( [[self window] alphaValue] < 1. )
	{
		[[self window] setAlphaValue:[[self window] alphaValue] + FADE_INCREMENT];
	}
	else if( _autoFadeOut )
	{
		if( [_delegate respondsToSelector:@selector( bubbleDidFadeIn: )] )
			[_delegate bubbleDidFadeIn:self];
		[self _waitBeforeFadeOut];
	}
}

- (void) _fadeOut:(NSTimer *) inTimer
{
	if( [[self window] alphaValue] > 0. )
	{
		[[self window] setAlphaValue:[[self window] alphaValue] - FADE_INCREMENT];
	}
	else
	{
		[self _stopTimer];
		if( [_delegate respondsToSelector:@selector( bubbleDidFadeOut: )] )
			[_delegate bubbleDidFadeOut:self];
		[self close];
		[self autorelease]; // Relase, we retained when we faded in.
	}
}

- (void) _applicationDidSwitch:(NSNotification *) notification {
	[self startFadeOut];
}

- (void) _bubbleClicked:(id) sender
{
	if( _target && _action && [_target respondsToSelector:_action] )
		[_target performSelector:_action withObject:self];
	[self startFadeOut];
}

#pragma mark -

- (void) startFadeIn
{
	if( [_delegate respondsToSelector:@selector( bubbleWillFadeIn: )] )
		[_delegate bubbleWillFadeIn:self];
	[self retain]; // Retain, after fade out we release.
	[self showWindow:nil];
	[self _stopTimer];
	_animationTimer = [[NSTimer scheduledTimerWithTimeInterval:TIMER_INTERVAL target:self selector:@selector( _fadeIn: ) userInfo:nil repeats:YES] retain];
}

- (void) startFadeOut
{
	if( [_delegate respondsToSelector:@selector( bubbleWillFadeOut: )] )
		[_delegate bubbleWillFadeOut:self];
	[self _stopTimer];
	_animationTimer = [[NSTimer scheduledTimerWithTimeInterval:TIMER_INTERVAL target:self selector:@selector( _fadeOut: ) userInfo:nil repeats:YES] retain];
}

#pragma mark -

- (BOOL) automaticallyFadesOut
{
	return _autoFadeOut;
}

- (void) setAutomaticallyFadesOut:(BOOL) autoFade
{
	_autoFadeOut = autoFade;
}

#pragma mark -

- (id) target
{
	return _target;
}

- (void) setTarget:(id) object
{
	[_target autorelease];
	_target = [object retain];
}

#pragma mark -

- (SEL) action
{
	return _action;
}

- (void) setAction:(SEL) selector
{
	_action = selector;
}

#pragma mark -

- (id) representedObject
{
	return _representedObject;
}

- (void) setRepresentedObject:(id) object
{
	[_representedObject autorelease];
	_representedObject = [object retain];
}

#pragma mark -

- (id) delegate
{
	return _delegate;
}

- (void) setDelegate:(id) delegate
{
	_delegate = delegate;
}

#pragma mark -

- (BOOL) respondsToSelector:(SEL) selector
{
	if( [[[self window] contentView] respondsToSelector:selector] )
		return [[[self window] contentView] respondsToSelector:selector];
	else return [super respondsToSelector:selector];
}

- (void) forwardInvocation:(NSInvocation *) invocation
{
	if( [[[self window] contentView] respondsToSelector:[invocation selector]] )
		[invocation invokeWithTarget:[[self window] contentView]];
	else [super forwardInvocation:invocation];
}

- (NSMethodSignature *) methodSignatureForSelector:(SEL) selector
{
	if( [[[self window] contentView] respondsToSelector:selector] )
		return [(NSObject *)[[self window] contentView] methodSignatureForSelector:selector];
	else return [super methodSignatureForSelector:selector];
}
@end
