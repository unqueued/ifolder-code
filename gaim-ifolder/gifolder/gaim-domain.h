/***********************************************************************
 *  $RCSfile$
 *
 *  Gaim iFolder Plugin: Allows Gaim users to share iFolders.
 *  Copyright (C) 2005 Novell, Inc.
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
 *  Author: Boyd Timothy <btimothy@novell.com>
 * 
 *  Some code in this file (mostly the saving and reading of the XML files) is
 *  directly based on code found in Gaim's core & plugin files, which is
 *  distributed under the GPL.
 ***********************************************************************/

#ifndef _GAIM_DOMAIN_H
#define _GAIM_DOMAIN_H 1

/* GLib/Gtk Includes */
#include <glib.h>

int gaim_domain_add_gaim_buddy(const char *account_name,
							   const char *account_proto,
							   const char *buddy_name,
							   const char *alias,
							   const char *ip_addr,
							   const char *ip_port);

int gaim_domain_update_gaim_buddy(const char *account_name,
								  const char *account_proto,
								  const char *buddy_name,
								  const char *alias,
								  const char *ip_addr,
								  const char *ip_port);

/**
 * Callback to sync a buddy when using a g_hash_table_for_each.
 */
void sync_buddy_with_simias_roster(gpointer key,
								   gpointer value,
								   gpointer user_data);

char *gaim_domain_get_po_box_id();

#endif
