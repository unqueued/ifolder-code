/***********************************************************************
 *  IResultSet.cs - The query resultset interface.
 * 
 *  Copyright (C) 2004 Novell, Inc.
 *
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU General Public
 *  License as published by the Free Software Foundation; either
 *  version 2 of the License, or (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Library General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public
 *  License along with this library; if not, write to the Free
 *  Software Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
 *
 *  Author: Russ Young <ryoung@novell.com>
 * 
 ***********************************************************************/
using System;

namespace Simias.Storage.Provider
{
	/// <summary>
	/// Result set interface.
	/// </summary>
	public interface IResultSet : IDisposable
	{
		/// <summary>
		/// Method to return the next set of objects.
		/// All the objects that can fit in the buffer will be returned.
		/// returns false when no more objects exist.
		/// </summary>
		/// <param name="buffer">Buffer used to return the objects.</param>
		/// <returns>true - objects returned. false - no more objects</returns>
		int GetNext(ref char[] buffer);
	}
}
