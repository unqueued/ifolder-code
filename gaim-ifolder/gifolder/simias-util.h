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

#ifndef _SIMIAS_UTIL_H
#define _SIMIAS_UTIL_H 1

#include <time.h>

/* Gaim iFolder Includes */
#include "simias-invitation-store.h"

#define TIMESTAMP_FORMAT "%I/%d/%Y %I:%M %p"

char * simias_fill_time_str(char *time_str, int buf_len, time_t time);
char * simias_fill_state_str(char *state_str, INVITATION_STATE state);

/* These next two are likely not needed anymore */
gboolean simias_is_valid_ip_part(const char *ip_part);
int simias_length_of_ip_address(const char *buffer);

#endif
