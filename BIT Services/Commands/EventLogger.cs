using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Commands
{
	class EventLogger
	{
		protected readonly object lockObj = new object();
		public void Log(string message)
		{
			lock (lockObj)
			{
				/*
				EventLog eventLog = new EventLog("");
				eventLog.Source = "BITServicesApplication";
				eventLog.WriteEntry(message);
				*/

/*
 * This does not work because of incorrect permissions, but would work if permissions are given and above code is uncommented/
 * : 'The source was not found, but some or all event logs could not be searched.  To create the source, you need permission to read all event logs to make sure that the new source name is unique.  Inaccessible logs: Security.'
 */
			}
		}
	}
}
