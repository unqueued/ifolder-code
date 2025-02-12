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
*                 $Author: Bruce Getter <bgetter@novell.com>
*                 $Modified by: <Modifier>
*                 $Mod Date: <Date Modified>
*                 $Revision: 0.0
*-----------------------------------------------------------------------------
* This module is used to:
*        <Description of the functionality of the file >
*
*
*******************************************************************************/
 
#ifndef _NIFVER_
#define _NIFVER_

//===[ Header files specific to NT          ]==============================

#include <windef.h>
#include <winver.h>

//===[ Header files specific to this module ]==============================

//===[ External data                        ]==============================

//===[ External prototypes                  ]==============================

//===[ Manifest constants                   ]==============================

//
// VER_ID, VER_PROD_VER, VER_PROD_STR, VER_STR, and VER_DATE are normally defined on the command line.
// These are the default values.
//

#ifndef VER_ID
#define VER_ID   3,7,1,3
#endif

#ifndef VER_STR
#define VER_STR  "3.7.1.3"
#endif

#ifndef VER_PROD_ID
#define VER_PROD_ID   3,7,1,3
#endif

#ifndef VER_PROD_STR
#define VER_PROD_STR  "3.7.1.3"
#endif

#ifndef VER_DATE
#define VER_DATE IF_VERSION
#endif

#define VER_VERSION_TEXT(module)   static char versionText[] = \
	"VeRsIoN=" VER_STR " Novell iFolder " module " module " VER_DATE

#ifndef VER_COPYRIGHT_YEARS
#define VER_COPYRIGHT_YEARS "2008"
#endif
#define VER_COPYRIGHT_TEXT static char copyrightText[] = \
   "CoPyRiGhT=Copyright " VER_COPYRIGHT_YEARS ", by Novell, Inc. All rights reserved."

// Definitions for resource files
#define VER_PRODUCTVERSION      	VER_PROD_ID
#define VER_FILEVERSION      		VER_ID
#define VER_COMPANYNAME_STR		TEXT("Novell, Inc.")
#define VER_PRODUCTNAME_STR		TEXT("Novell iFolder\0")
#define VER_PRODUCTVERSION_STR	TEXT(VER_PROD_STR)
#define VER_FILEVERSION_STR		TEXT(VER_STR)
#define VER_LEGALCOPYRIGHT_STR	TEXT("Copyright \251 2008 ") VER_COMPANYNAME_STR, TEXT("\0")

//===[ Type definitions                     ]==============================

//===[ Function Prototypes                  ]==============================

//===[ Global Variables                     ]==============================

#endif

//=========================================================================
//=========================================================================

