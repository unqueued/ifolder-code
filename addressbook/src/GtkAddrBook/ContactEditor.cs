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
 *  Library General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 *  Author: Calvin Gaisford <cgaisford@novell.com>
 *
 ***********************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.Text;
using Novell.AddressBook;

using Gtk;
using Gdk;
using Gnome;
using Glade;
using GtkSharp;
using GLib;

namespace Novell.iFolder
{
	/// <summary>
	/// This class is used to edit all data stored on a contact.
	/// </summary>
	public class ContactEditor
	{
		public Contact Contact
		{
			get
			{
				return currentContact;
			}

			set
			{
				currentContact = value;
			}
		}

		public Gtk.Window TransientFor
		{
			set
			{
				contactEditorDialog.TransientFor = value;
			}
		}


		// Glade "autoconnected" members
		// Name Controls
		[Glade.Widget] internal Gtk.Image			UserImage;
		[Glade.Widget] internal Gtk.Entry			FullNameEntry;
		[Glade.Widget] internal Gtk.Entry			NickNameEntry;

		// Email Controls
		[Glade.Widget] internal Gtk.Entry			MOneEntry;
		[Glade.Widget] internal Gtk.Entry			MTwoEntry;
		[Glade.Widget] internal Gtk.Entry			MThreeEntry;
		[Glade.Widget] internal Gtk.Entry			MFourEntry;
		[Glade.Widget] internal Gtk.OptionMenu		MOneOptionMenu;
		[Glade.Widget] internal Gtk.OptionMenu		MTwoOptionMenu;
		[Glade.Widget] internal Gtk.OptionMenu		MThreeOptionMenu;
		[Glade.Widget] internal Gtk.OptionMenu		MFourOptionMenu;

		// Telephone Controls
		[Glade.Widget] internal Gtk.Entry			PhoneOneEntry;
		[Glade.Widget] internal Gtk.Entry			PhoneTwoEntry;
		[Glade.Widget] internal Gtk.Entry			PhoneThreeEntry;
		[Glade.Widget] internal Gtk.Entry			PhoneFourEntry;
		[Glade.Widget] internal Gtk.OptionMenu		POneOptionMenu;
		[Glade.Widget] internal Gtk.OptionMenu		PTwoOptionMenu;
		[Glade.Widget] internal Gtk.OptionMenu		PThreeOptionMenu;
		[Glade.Widget] internal Gtk.OptionMenu		PFourOptionMenu;

		// Instant Message Controls
		[Glade.Widget] internal Gtk.Entry			IMOneEntry;
		[Glade.Widget] internal Gtk.Entry			IMTwoEntry;
		[Glade.Widget] internal Gtk.Entry			IMThreeEntry;

		// Web Addresses Controls
		[Glade.Widget] internal Gtk.Entry			WebHomeEntry;
		[Glade.Widget] internal Gtk.Entry			WebBlogEntry;
		[Glade.Widget] internal Gtk.Entry			WebCalEntry;
		[Glade.Widget] internal Gtk.Entry			WebCamEntry;

		// Job Controls
		[Glade.Widget] internal Gtk.Entry			RoleEntry;
		[Glade.Widget] internal Gtk.Entry			OrgEntry;
		[Glade.Widget] internal Gtk.Entry			ManagerEntry;
		[Glade.Widget] internal Gtk.Entry			TitleEntry;
		[Glade.Widget] internal Gtk.Entry			WorkForceEntry;

		// Misc Controls
		[Glade.Widget] internal Gtk.Entry			BirthdayEntry;
		[Glade.Widget] internal Gtk.TextView		NoteTextView;

		// Work Address Controls
		[Glade.Widget] internal Gtk.Entry			WorkAddrEntry;
		[Glade.Widget] internal Gtk.Entry			WorkAddr2Entry;
		[Glade.Widget] internal Gtk.Entry			WorkCityEntry;
		[Glade.Widget] internal Gtk.Entry			WorkZipEntry;
		[Glade.Widget] internal Gtk.Entry			WorkStateEntry;
		[Glade.Widget] internal Gtk.Entry			WorkCountryEntry;

		// Home Address Controls
		[Glade.Widget] internal Gtk.Entry			HomeAddrEntry;
		[Glade.Widget] internal Gtk.Entry			HomeAddr2Entry;
		[Glade.Widget] internal Gtk.Entry			HomeCityEntry;
		[Glade.Widget] internal Gtk.Entry			HomeZipEntry;
		[Glade.Widget] internal Gtk.Entry			HomeStateEntry;
		[Glade.Widget] internal Gtk.Entry			HomeCountryEntry;

		// Other Address Controls
		[Glade.Widget] internal Gtk.Entry			OtherAddrEntry;
		[Glade.Widget] internal Gtk.Entry			OtherAddr2Entry;
		[Glade.Widget] internal Gtk.Entry			OtherCityEntry;
		[Glade.Widget] internal Gtk.Entry			OtherZipEntry;
		[Glade.Widget] internal Gtk.Entry			OtherStateEntry;
		[Glade.Widget] internal Gtk.Entry			OtherCountryEntry;


		internal Gtk.Dialog		contactEditorDialog;
		internal Contact		currentContact;

		internal Name			preferredName;
		internal Email			emailOne = null;
		internal Email			emailTwo = null;
		internal Email			emailThree = null;
		internal Email			emailFour = null;
		internal Telephone		phoneOne = null;
		internal Telephone		phoneTwo = null;
		internal Telephone		phoneThree = null;
		internal Telephone		phoneFour = null;
		internal Address		preferredAddress;


		/// <summary>
		/// Constructs a new ContactEditor
		/// </summary>
		public ContactEditor()
		{
			InitGlade();
		}


		/// <summary>
		/// Method used to load the glade resources and setup default
		/// behaviors in for the ContactEditor dialog
		/// </summary>
		private void InitGlade()
		{
			Glade.XML gxml = new Glade.XML ("contact-editor.glade", 
					"ContactEditor", null);

			gxml.Autoconnect (this);

			contactEditorDialog = (Gtk.Dialog) gxml.GetWidget("ContactEditor");

			//------------------------------
			// This will setup the tab stops
			//------------------------------
/*			Widget[] widArray = new Widget[13];

			widArray[0] = FullNameEntry;
			widArray[1] = NickNameEntry;

			widArray[2] = MOneEntry;
			widArray[3] = MTwoEntry;
			widArray[4] = MThreeEntry;
			widArray[5] = MFourEntry;

			widArray[6] = PhoneOneEntry;
			widArray[7] = PhoneTwoEntry;
			widArray[8] = PhoneThreeEntry;
			widArray[9] = PhoneFourEntry;

			widArray[10] = IMOneEntry;
			widArray[11] = IMTwoEntry;
			widArray[12] = IMThreeEntry;

			generalTabTable.FocusChain = widArray;
*/
			FullNameEntry.HasFocus = true;
		}
	


		/// <summary>
		/// Method to run the dialog
		/// </summary>
		public int Run()
		{
			int rc = 0;

			if(contactEditorDialog != null)
			{
				if(currentContact == null)
					currentContact = new Contact();

				PopulateWidgets();
				rc = contactEditorDialog.Run();
				if(rc == -5)
					SaveContact();
				contactEditorDialog.Hide();
				contactEditorDialog.Destroy();
				contactEditorDialog = null;
			}
			return rc;
		}



		/// <summary>
		/// Method used to populate all of the data from the current
		/// contact into the UI controls
		/// </summary>
		private void PopulateWidgets()
		{
			Pixbuf pb = GetScaledPhoto(currentContact, 64);
			if(pb != null)
				UserImage.FromPixbuf = pb;


			try
			{
				preferredName = currentContact.GetPreferredName();
			}
			catch(Exception e)
			{
				Console.WriteLine("We blew because there was not a preferred name, adding one...");
				preferredName = new Name("", "");
				preferredName.Preferred = true;
				currentContact.AddName(preferredName);
			}

			FullNameEntry.Text = currentContact.FN;

			NickNameEntry.Text = currentContact.Nickname;


			PopulateEmails();
			PopulatePhoneNumbers();


			WebHomeEntry.Text = currentContact.Url;
			WebBlogEntry.Text = currentContact.Blog;
			//WebCalEntry.Text = ;
			//WebCamEntry.Text = ;

			RoleEntry.Text = currentContact.Role;
			OrgEntry.Text = currentContact.Organization;
			ManagerEntry.Text = currentContact.ManagerID;
			TitleEntry.Text = currentContact.Title;
			WorkForceEntry.Text = currentContact.WorkForceID;

			BirthdayEntry.Text = currentContact.Birthday;
			NoteTextView.Buffer.Text = currentContact.Note;
/*
			try
			{
				preferredAddress = currentContact.GetPreferredAddress();
			}
			catch(Exception e)
			{
				preferredAddress = null;
			}
*/

/*			if(preferredAddress != null)
			{
				if(preferredAddress.Street.Length > 0)
					streetEntry.Text = preferredAddress.Street;
				if(preferredAddress.Locality.Length > 0)
					cityEntry.Text = preferredAddress.Locality;
				if(preferredAddress.Region.Length > 0)
					stateEntry.Text = preferredAddress.Region;
				if(preferredAddress.PostalCode.Length > 0)
					zipEntry.Text = preferredAddress.PostalCode;
				if(preferredAddress.Country.Length > 0)
					countryEntry.Text = preferredAddress.Country;
			}
*/
		}




		/// <summary>
		/// Method used to retrieve a specific type of phone number
		/// from the current contact
		/// </summary>
		internal Telephone GetPhoneType(PhoneTypes type)
		{
			foreach(Telephone tel in currentContact.GetTelephoneNumbers())
			{
				if( (tel.Types & type) == type)
					return tel;
			}
			return null;
		}




		/// <summary>
		/// Method used to populate the phone controls in the dialog
		/// with the data from the currentContact
		/// </summary>
		internal void PopulatePhoneNumbers()
		{
			PopulatePhoneType(PhoneTypes.preferred);
			PopulatePhoneType(PhoneTypes.work);
			PopulatePhoneType(PhoneTypes.home);
			PopulatePhoneType(PhoneTypes.other);
		}




		/// <summary>
		/// Method used to populate the phone controls in the dialog
		/// with the data from the currentContact
		/// </summary>
		internal void PopulatePhoneType(PhoneTypes types)
		{
			foreach(Telephone phone in currentContact.GetTelephoneNumbers())
			{
				// if the type being asked for is not
				// preferred, filter out all preferred
				if( types != PhoneTypes.preferred )
				{
					if( (phone.Types & PhoneTypes.preferred) ==
							PhoneTypes.preferred)
					{
						continue;
					}
				}

				if( (phone.Types & types) != types)
				{
					continue;
				}

				if(phoneOne == null)
				{
					phoneOne = phone;
					PhoneOneEntry.Text = phone.Number;
					SetPhoneOptionMenu(POneOptionMenu, phone.Types);
				}
				else if(phoneTwo == null)
				{
					phoneTwo = phone;
					PhoneTwoEntry.Text = phone.Number;
					SetPhoneOptionMenu(PTwoOptionMenu, phone.Types);
				}
				else if(phoneThree == null)
				{
					phoneThree = phone;
					PhoneThreeEntry.Text = phone.Number;
					SetPhoneOptionMenu(PThreeOptionMenu, phone.Types);
				}
				else if(phoneFour == null)
				{
					phoneFour = phone;
					PhoneFourEntry.Text = phone.Number;
					SetPhoneOptionMenu(PFourOptionMenu, phone.Types);
				}
			}
		}




		/// <summary>
		/// Method used to retrieve a specific type of email from
		/// the current contact
		/// </summary>
		internal void SetPhoneOptionMenu(Gtk.OptionMenu optMenu, 
				PhoneTypes type)
		{
			if(	(type & PhoneTypes.work) == PhoneTypes.work)
			{
				optMenu.SetHistory(0);
				return;
			}

			if(	(type & PhoneTypes.home) == PhoneTypes.home)
			{
				optMenu.SetHistory(1);
				return;
			}

			optMenu.SetHistory(2);
		}




		/// <summary>
		/// Method used to save phone numbers to the current contact
		/// the current contact still needs to be persisted
		/// </summary>
		internal void SavePhoneNumbers()
		{
			// Phone One
			if(PhoneOneEntry.Text.Length > 0)
			{
				PhoneTypes selType = GetPhoneType(POneOptionMenu);
				if(phoneOne == null)
				{
					phoneOne = new Telephone( 
						PhoneOneEntry.Text,
						(selType | PhoneTypes.preferred)); 

					currentContact.AddTelephoneNumber(phoneOne);
				}
				else
				{
					phoneOne.Types = (selType | PhoneTypes.preferred);
					phoneOne.Number = PhoneOneEntry.Text;
				}
			}
			else
			{
				if(phoneOne != null)
				{
					phoneOne.Delete();
				}
			}

			// Phone Two
			if(PhoneTwoEntry.Text.Length > 0)
			{
				PhoneTypes selType = GetPhoneType(PTwoOptionMenu);
				if(phoneTwo == null)
				{
					phoneTwo = new Telephone(PhoneTwoEntry.Text, selType);
					currentContact.AddTelephoneNumber(phoneTwo);
				}
				else
				{
					phoneTwo.Types = selType;
					phoneTwo.Number = PhoneTwoEntry.Text;
				}
			}
			else
			{
				if(phoneTwo != null)
				{
					phoneTwo.Delete();
				}
			}

			// Phone Three 
			if(PhoneThreeEntry.Text.Length > 0)
			{
				PhoneTypes selType = GetPhoneType(PThreeOptionMenu);
				if(phoneThree == null)
				{
					phoneThree = new Telephone(PhoneThreeEntry.Text, selType); 
					currentContact.AddTelephoneNumber(phoneThree);
				}
				else
				{
					phoneThree.Types = selType;
					phoneThree.Number = PhoneThreeEntry.Text;
				}
			}
			else
			{
				if(phoneThree != null)
				{
					phoneThree.Delete();
				}
			}

			// Phone Four 
			if(PhoneFourEntry.Text.Length > 0)
			{
				PhoneTypes selType = GetPhoneType(PFourOptionMenu);
				if(phoneFour == null)
				{
					phoneFour = new Telephone(PhoneFourEntry.Text, selType); 
					currentContact.AddTelephoneNumber(phoneFour);
				}
				else
				{
					phoneFour.Types = selType;
					phoneFour.Number = PhoneFourEntry.Text;
				}
			}
			else
			{
				if(phoneFour != null)
				{
					phoneFour.Delete();
				}
			}
		}




		/// <summary>
		/// Method used to retrieve a specific type of email from
		/// the current contact
		/// </summary>
		internal PhoneTypes GetPhoneType(Gtk.OptionMenu optMenu)
		{
			switch(optMenu.History)
			{
				case 0:
					return PhoneTypes.work;
				case 1:
					return PhoneTypes.home;
				case 2:
					return PhoneTypes.other;
			}

			return PhoneTypes.other;
		}




		/// <summary>
		/// Method used to populate the email controls in the dialog
		/// with the data from the currentContact
		/// </summary>
		internal void PopulateEmails()
		{
			// Because there is no order preserved with emails
			// we have to populate them by picking out the
			// specific types we want
			PopulateEmailType(EmailTypes.preferred);
			PopulateEmailType(EmailTypes.work);
			PopulateEmailType(EmailTypes.personal);
			PopulateEmailType(EmailTypes.other);
		}




		/// <summary>
		/// Method used to populate the email controls in the dialog
		/// with the data from the currentContact
		/// </summary>
		internal void PopulateEmailType(EmailTypes types)
		{
			foreach(Email e in currentContact.GetEmailAddresses())
			{
				// if the type being asked for is not
				// preferred, filter out all preferred
				if( types != EmailTypes.preferred )
				{
					if( (e.Types & EmailTypes.preferred)==EmailTypes.preferred)
					{
						continue;
					}
				}

				if( (e.Types & types) != types)
				{
					continue;
				}

				if(emailOne == null)
				{
					emailOne = e;
					MOneEntry.Text = emailOne.Address;
					SetMailOptionMenu(MOneOptionMenu, emailOne.Types);
				}
				else if(emailTwo == null)
				{
					emailTwo = e;
					MTwoEntry.Text = e.Address;
					SetMailOptionMenu(MTwoOptionMenu, emailTwo.Types);
				}
				else if(emailThree == null)
				{
					emailThree = e;
					MThreeEntry.Text = e.Address;
					SetMailOptionMenu(MThreeOptionMenu, emailThree.Types);
				}
				else if(emailFour == null)
				{
					emailFour = e;
					MFourEntry.Text = e.Address;
					SetMailOptionMenu(MFourOptionMenu, emailFour.Types);
				}
			}
		}





		/// <summary>
		/// Method used to retrieve a specific type of email from
		/// the current contact
		/// </summary>
		internal void SetMailOptionMenu(Gtk.OptionMenu optMenu, EmailTypes type)
		{
			if(	(type & EmailTypes.work) == EmailTypes.work)
			{
				optMenu.SetHistory(0);
				return;
			}

			if(	(type & EmailTypes.personal) == EmailTypes.personal)
			{
				optMenu.SetHistory(1);
				return;
			}

			optMenu.SetHistory(2);
		}




		/// <summary>
		/// Method used to retrieve a specific type of email from
		/// the current contact
		/// </summary>
		internal EmailTypes GetMailType(Gtk.OptionMenu optMenu)
		{
			switch(optMenu.History)
			{
				case 0:
					return EmailTypes.work;
				case 1:
					return EmailTypes.personal;
				case 2:
					return EmailTypes.other;
			}

			return EmailTypes.other;
		}




		/// <summary>
		/// Method used to store the edited data to the currentContact
		/// the contact must still be committed to persist the data
		/// in the store
		/// </summary>
		internal void SaveCurrentEmails()
		{
			// Email One
			if(MOneEntry.Text.Length > 0)
			{
				EmailTypes selType = GetMailType(MOneOptionMenu);
				if(emailOne == null)
				{
					emailOne = new Email( 
						(selType | EmailTypes.preferred), 
						MOneEntry.Text);
					currentContact.AddEmailAddress(emailOne);
				}
				else
				{
					emailOne.Types = (selType | EmailTypes.preferred);
					emailOne.Address = MOneEntry.Text;
				}
			}
			else
			{
				if(emailOne != null)
				{
					emailOne.Delete();
				}
			}

			// These are being saved in this order to preserve the
			// the order of the widgets

			// Email Two
			if(MTwoEntry.Text.Length > 0)
			{
				EmailTypes selType = GetMailType(MTwoOptionMenu);
				if(emailTwo == null)
				{
					emailTwo = new Email( 
						selType, 
						MTwoEntry.Text);
					currentContact.AddEmailAddress(emailTwo);
				}
				else
				{
					emailTwo.Types = selType;
					emailTwo.Address = MTwoEntry.Text;
				}
			}
			else
			{
				if(emailTwo != null)
				{
					emailTwo.Delete();
				}
			}

			// Email Three 
			if(MThreeEntry.Text.Length > 0)
			{
				EmailTypes selType = GetMailType(MThreeOptionMenu);
				if(emailThree == null)
				{
					emailThree = new Email( 
						selType, 
						MThreeEntry.Text);
					currentContact.AddEmailAddress(emailThree);
				}
				else
				{
					emailThree.Types = selType;
					emailThree.Address = MThreeEntry.Text;
				}
			}
			else
			{
				if(emailThree != null)
				{
					emailThree.Delete();
				}
			}

			// Email Four 
			if(MFourEntry.Text.Length > 0)
			{
				EmailTypes selType = GetMailType(MFourOptionMenu);
				if(emailFour == null)
				{
					emailFour = new Email( 
						selType, 
						MFourEntry.Text);
					currentContact.AddEmailAddress(emailFour);
				}
				else
				{
					emailFour.Types = selType;
					emailFour.Address = MFourEntry.Text;
				}
			}
			else
			{
				if(emailFour != null)
				{
					emailFour.Delete();
				}
			}
		}




		/// <summary>
		/// Method used to gather data from entry fields that are not tied
		/// in any way to the actual Contact object.
		/// </summary>
		private void SaveContact()
		{
			if(NickNameEntry.Text.Length > 0)
				currentContact.Nickname = NickNameEntry.Text;
			else
				currentContact.Nickname = null;

/*
			// First Name 
			if(firstNameEntry.Text.Length > 0)
				preferredName.Given = firstNameEntry.Text;
			else
				preferredName.Given = null;
			// Last Name 
			if(lastNameEntry.Text.Length > 0)
				preferredName.Family = lastNameEntry.Text;
			else
				preferredName.Family = null;
*/

			SaveCurrentEmails();
			SavePhoneNumbers();


			if(WebHomeEntry.Text.Length > 0)
				currentContact.Url = WebHomeEntry.Text;
			else
				currentContact.Url = null;

			if(WebBlogEntry.Text.Length > 0)
				currentContact.Blog = WebBlogEntry.Text;
			else
				currentContact.Blog = null;

			if(RoleEntry.Text.Length > 0)
				currentContact.Role = RoleEntry.Text;
			else
				currentContact.Role = null;

			if(OrgEntry.Text.Length > 0)
				currentContact.Organization = OrgEntry.Text;
			else
				currentContact.Organization = null;

			if(ManagerEntry.Text.Length > 0)
				currentContact.ManagerID = ManagerEntry.Text;
			else
				currentContact.ManagerID = null;

			if(TitleEntry.Text.Length > 0)
				currentContact.Title = TitleEntry.Text;
			else
				currentContact.Title = null;

			if(WorkForceEntry.Text.Length > 0)
				currentContact.WorkForceID = WorkForceEntry.Text;
			else
				currentContact.WorkForceID = null;

			if(BirthdayEntry.Text.Length > 0)
				currentContact.Birthday = BirthdayEntry.Text;
			else
				currentContact.Birthday = null;

			if(NoteTextView.Buffer.Text.Length > 0)
				currentContact.Note = NoteTextView.Buffer.Text;
			else
				currentContact.Note = null;
/*
			if( (streetEntry.Text.Length == 0) &&
					(cityEntry.Text.Length == 0) &&
					(stateEntry.Text.Length == 0) &&
					(zipEntry.Text.Length == 0) &&
					(countryEntry.Text.Length == 0) )
			{
				if(preferredAddress != null)
					preferredAddress.Delete();
			}
			else
			{
				if(preferredAddress == null)
				{
					preferredAddress = new Address();
					preferredAddress.Preferred = true;
					currentContact.AddAddress(preferredAddress);
				}

				preferredAddress.Street = streetEntry.Text;
				preferredAddress.Locality = cityEntry.Text;
				preferredAddress.Region = stateEntry.Text;
				preferredAddress.PostalCode = zipEntry.Text;
				preferredAddress.Country = countryEntry.Text;

			}
*/
		}




		/// <summary>
		/// Glade autoconnected method that is called when the email
		/// button on the dialog is pressed.
		/// </summary>
		private void on_emailButton_clicked(object o, EventArgs args) 
		{
			Console.WriteLine("Edit Email here");
		}

		/// <summary>
		/// Glade autoconnected method that is called when the phone
		/// button on the dialog is pressed.
		/// </summary>
		private void on_phoneButton_clicked(object o, EventArgs args) 
		{
			Console.WriteLine("Edit Phone here");
		}

		/// <summary>
		/// Glade autoconnected method that is called when the address
		/// button on the dialog is pressed.
		/// </summary>
		private void on_addrChangeButton_clicked(object o, EventArgs args) 
		{
			Console.WriteLine("Edit Address here");
		}

		/// <summary>
		/// Glade autoconnected method that is called when the name 
		/// button on the dialog is pressed.
		/// </summary>
		private void on_nameButton_clicked(object o, EventArgs args) 
		{
/*
			NameEditor ne = new NameEditor(contactEditorDialog, preferredName);
			if(ne.Run() == -5)
			{
				firstNameEntry.Text = preferredName.Given;
				lastNameEntry.Text = preferredName.Family;
			}
*/
		}

		/// <summary>
		/// Glade autoconnected method that is called when the photo
		/// button on the dialog is pressed.
		/// </summary>
		private void on_UserImageButton_clicked(object o, EventArgs args) 
		{
			FileSelection fs = new FileSelection("Choose a new Image");

			// setup file selector to be modal to the ContactEditor
			fs.Modal = true;
			fs.TransientFor = contactEditorDialog;


			int retVal = fs.Run();

			fs.Hide();

			// if they selected a file, try to import it
			if(retVal == -5)
			{
				if(currentContact.ImportPhoto(fs.Filename))
				{
					Pixbuf pb = GetScaledPhoto(currentContact, 64);
					if(pb != null)
						UserImage.FromPixbuf = pb;
				}
			}
		}


		/// <summary>
		/// This is a method used to scale a photo to the specified height.
		/// The method should be moved to a common place for all of the
		/// addressbook to use.
		/// </summary>
		/// <param name="contact">The contact from which to extrace the 
		/// photo</param>
		/// <param name="height">The height to scale the photo in 
		/// pixels</param>
		private Pixbuf GetScaledPhoto(Contact contact, int height)
		{
			Pixbuf pb = null;

			try
			{
				int newWidth, newHeight;

				pb = new Pixbuf(contact.ExportPhoto());

				newHeight = height;
				newWidth = height;

				if(pb.Height != pb.Width)
				{
					int perc = (height * 1000) / pb.Height;
					newWidth = pb.Width * perc / 1000;
				}

				pb = pb.ScaleSimple(newWidth, newHeight, 
						InterpType.Bilinear);
			}
			catch(Exception e)
			{
				pb = null;
			}
			return pb;
		}
	}
}
