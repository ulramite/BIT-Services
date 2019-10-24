using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Commands
{
	public static class Conversions
	{
		/// <summary>
		/// Converts a SecureString into a String
		/// </summary>
		/// <param name="securePassword">A SecureString</param>
		/// <returns>The String contained within the SecureString</returns>
		public static string ConvertToUnsecureString(this SecureString securePassword)
		{
			if (securePassword == null)
				throw new ArgumentNullException("securePassword");

			IntPtr unmanagedString = IntPtr.Zero;
			try
			{
				unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
				return Marshal.PtrToStringUni(unmanagedString);
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
			}
		}
	}
}
