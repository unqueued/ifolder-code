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
  *                 $Modified by: <Modifier>
  *                 $Mod Date: <Date Modified>
  *                 $Revision: 0.0
  *-----------------------------------------------------------------------------
  * This module is used to:
  *        <Description of the functionality of the file >
  *
  *
  *******************************************************************************/

using System;
using Gtk;

namespace Novell.iFolder
{

/// <summary>
/// class iFolderMsgDialog
/// </summary>
public class iFolderMsgDialog : Dialog
{
//	private Button showDetailsButton;
	private Image			dialogImage;
	private Expander		detailsExpander;
	private ScrolledWindow	showDetailsScrolledWindow;
	private VBox			extraWidgetVBox;
	private Widget			extraWidget;
	
    /// <summary>
    /// Gets the Image
    /// </summary>
	public Image Image
	{
		get
		{
			return dialogImage;
		}
	}

    /// <summary>
    /// Enum Dialog Type
    /// </summary>
	public enum DialogType : int
	{
		Error = 1,
		Info,
		Question,
		Warning
	}

    /// <summary>
    /// Enum Button Set
    /// </summary>
	public enum ButtonSet : int
	{
		Ok = 1,
		OkCancel,
		YesNo,
		AcceptDeny,
		None
	}
	
	/// <summary>
	/// Gets / Sets Extra Widget
	/// </summary>
    public Widget ExtraWidget
	{
		get
		{
			return extraWidget;
		}
		set
		{
			if (extraWidget != null)
			{
				extraWidgetVBox.Remove(extraWidget);
				extraWidget.Destroy();
				extraWidget = null;
			}

			if (value == null)
			{
				extraWidgetVBox.Hide();
			}
			else
			{
				extraWidget = value;
				extraWidgetVBox.PackStart(extraWidget, false, false, 0);
				extraWidgetVBox.Show();
				extraWidget.Show();
			}
		}
	}

	
    /// <summary>
    /// COnstructor
    /// </summary>
    /// <param name="parent">Parent Window</param>
    /// <param name="type">Type</param>
    /// <param name="buttonSet">Button Set</param>
    /// <param name="title">Title</param>
    /// <param name="statement">Message</param>
    /// <param name="secondaryStatement">Secondary MEssage</param>
	public iFolderMsgDialog(	Gtk.Window parent,
								DialogType type,
								ButtonSet buttonSet,
								string title, 
								string statement, 
								string secondaryStatement)
		: base()
	{
		Init(parent, type, buttonSet, title, statement, secondaryStatement, null);
	}

	///
	/// This dialog adds on the "details" parameter.  This will add on
	/// a "Show Details" button that will show the extended details in a text
	/// area.
	///
	public iFolderMsgDialog(	Gtk.Window parent,
								DialogType type,
								ButtonSet buttonSet,
								string title, 
								string statement, 
								string secondaryStatement,
								string details)
		: base()
	{
		Init(parent, type, buttonSet, title, statement, secondaryStatement, details);
	}
	
	internal void Init(Gtk.Window parent,
					   DialogType type,
					   ButtonSet buttonSet,
					   string title,
					   string statement,
					   string secondaryStatement,
					   string details)
	{
		this.Title = title;
		this.HasSeparator = false;
		this.Icon = new Gdk.Pixbuf(Util.ImagesPath("ifolder16.png"));
//		this.BorderWidth = 10;
		this.Resizable = false;
		this.Modal = true;
		if(parent != null)
		{
			this.TransientFor = parent;
			this.WindowPosition = WindowPosition.CenterOnParent;
		}
		else
			this.WindowPosition = WindowPosition.Center;

		HBox h = new HBox();
		h.BorderWidth = 10;
		h.Spacing = 10;

		dialogImage = new Image();
		switch(type)
		{
			case DialogType.Error:
				dialogImage.SetFromStock(Gtk.Stock.DialogError, IconSize.Dialog);
				break;
			case DialogType.Question:
				dialogImage.SetFromStock(Gtk.Stock.DialogQuestion, IconSize.Dialog);
				break;
			case DialogType.Warning:
				dialogImage.SetFromStock(Gtk.Stock.DialogWarning, IconSize.Dialog);
				break;
			default:
			case DialogType.Info:
				dialogImage.SetFromStock(Gtk.Stock.DialogInfo, IconSize.Dialog);
				break;
		}
		dialogImage.SetAlignment(0.5F, 0);
		h.PackStart(dialogImage, false, false, 0);

		VBox v = new VBox();
		v.Spacing = 10;
		Label l = new Label();
		l.LineWrap = true;
		l.UseMarkup = true;
		l.Selectable = false;
		l.CanFocus = false;
		l.Xalign = 0; l.Yalign = 0;
		l.Markup = "<span weight=\"bold\" size=\"larger\">" + GLib.Markup.EscapeText(statement) + "</span>";
		v.PackStart(l);

		l = new Label(secondaryStatement);
		l.LineWrap = true;
		l.Selectable = false;
		l.CanFocus = false;
		l.Xalign = 0; l.Yalign = 0;
		v.PackStart(l, true, true, 8);
		
		if (details != null)
		{
			detailsExpander = new Expander(Util.GS("_Details"));
			v.PackStart(detailsExpander, false, false, 0);
//			showDetailsButton = new Button(Util.GS("Show _Details"));
//			showDetailsButton.Clicked += new EventHandler(ShowDetailsButtonPressed);
			
//			HBox detailsButtonBox = new HBox();
//			detailsButtonBox.PackEnd(showDetailsButton, false, false, 0);

//			v.PackStart(detailsButtonBox, false, false, 4);

			TextView textView = new TextView();
			textView.Editable = false;
			textView.WrapMode = WrapMode.Char;
			TextBuffer textBuffer = textView.Buffer;
			
			textBuffer.Text = details;
			
			showDetailsScrolledWindow = new ScrolledWindow();
			detailsExpander.Add(showDetailsScrolledWindow);
			showDetailsScrolledWindow.AddWithViewport(textView);
			
			showDetailsScrolledWindow.Visible = false;
			
//			v.PackEnd(showDetailsScrolledWindow, false, false, 4);
		}
//		else
//		{
//			showDetailsButton = null;
//			showDetailsScrolledWindow = null;
//		}

		///
		/// Extra Widget
		///
		extraWidgetVBox = new VBox(false, 0);
		v.PackStart(extraWidgetVBox, false, false, 0);
		extraWidgetVBox.NoShowAll = true;
		extraWidget = null;

		h.PackEnd(v);
		h.ShowAll();
		
//		if (details != null)
//			showDetailsScrolledWindow.Visible = false;

		this.VBox.Add(h);
		
		Widget defaultButton;
		switch(buttonSet)
		{
			default:
			case ButtonSet.Ok:
				defaultButton = this.AddButton(Stock.Ok, ResponseType.Ok);
				break;
			case ButtonSet.OkCancel:
				this.AddButton(Stock.Cancel, ResponseType.Cancel);
				defaultButton = this.AddButton(Stock.Ok, ResponseType.Ok);
				break;
			case ButtonSet.YesNo:
		//		this.AddButton(Stock.No, ResponseType.No);
		//		defaultButton = this.AddButton(Stock.Yes, ResponseType.Yes);
				this.AddButton(Util.GS("_No"), ResponseType.No);
				defaultButton = this.AddButton(Util.GS("_Yes"), ResponseType.Yes);
				break;
			case ButtonSet.AcceptDeny:
		//		this.AddButton(Stock.No, ResponseType.No);
		//		defaultButton = this.AddButton(Stock.Yes, ResponseType.Yes);
				this.AddButton(Util.GS("_Deny"), ResponseType.No);
				defaultButton = this.AddButton(Util.GS("_Accept"), ResponseType.Yes);
				break;
		}
		
		defaultButton.CanDefault = true;
		defaultButton.GrabFocus();
		defaultButton.GrabDefault();
	}
	
//	private void ShowDetailsButtonPressed(object o, EventArgs args)
//	{
//		if (showDetailsButton.Label == Util.GS("Show _Details"))
//		{
//			showDetailsButton.Label = Util.GS("Hide _Details");
//			showDetailsScrolledWindow.Visible = true;
//		}
//		else
//		{
//			showDetailsButton.Label = Util.GS("Show _Details");
//			showDetailsScrolledWindow.Visible = false;
//		}
//	}
}

}
