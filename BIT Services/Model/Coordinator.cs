using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Model
{
	public class Coordinator
	{
		private int _coordinatorID;
		private string _firstName;
		private string _lastName;
		private string _address;
		private string _state;
		private Suburb _suburb;
		private string _mobile;
		private string _email;



		public int CoordinatorID { get => _coordinatorID; }
		public string FirstName { get => _firstName; }
		public string LastName { get => _lastName;  }
		public string FullName { get => FirstName + " " + LastName; }
		public string Address { get => _address; }
		public string State { get => _state; }
		public Suburb Suburb { get => _suburb; }
		public string Mobile { get => _mobile; }
		public string Email { get => _email; }



		public Coordinator(int coordinatorID, string firstName, string lastName, string address, string state, Suburb suburb, string mobile, string email)
		{
			_coordinatorID = coordinatorID;
			_firstName = firstName;
			_lastName = lastName;
			_address = address;
			_state = state;
			_suburb = suburb;
			_mobile = mobile;
			_email = email;
		}
		public Coordinator( string firstName, string lastName, string address, string state, Suburb suburb, string mobile, string email)
		{
			_firstName = firstName;
			_lastName = lastName;
			_address = address;
			_state = state;
			_suburb = suburb;
			_mobile = mobile;
			_email = email;
		}
	}

}
