/***********************************************************************
 *  $RCSfile$
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
 *  Author: Rob
 * 
 ***********************************************************************/

using System;

using Simias.Agent;

namespace Simias.Agent
{
	/// <summary>
	/// Agent Console
	/// </summary>
	class MainClass
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length == 1)
			{
				(new AgentFactory()).GetInviteAgent().Accept(new Invitation(args[0]));

				Console.WriteLine("Invitation Successfully Accepted!");
			}
			else
			{
				Console.WriteLine("USAGE: InviteAgentConsole.exe invitation.ifi");
			}

			Console.WriteLine();
			Console.WriteLine("Press [Enter] to continue...");

			Console.ReadLine();
		}
	}
}
