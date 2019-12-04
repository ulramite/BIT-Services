using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Model
{
	public class User
	{

		private int userID;
		private string userType;


		public int UserID { get => userID; }
		public string UserType { get => userType; }

		public User(int userID, string userType )
		{
			this.userID = userID;
			this.userType = userType;

		}
	}
}
