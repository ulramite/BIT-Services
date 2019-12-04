using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Model
{
	public class Contractor
	{
		private int _contractorID;
		private string _firstName;
		private string _lastName;
		private string _address;
		private string _state;
		private Suburb _suburb;
		private string _mobile;
		private string _email;
		private SuburbList _preferredSuburbList;
		private SkillList _hasSkillList;

		public int ContractorID { get => _contractorID; }
		public string FirstName { get => _firstName; }
		public string LastName { get => _lastName; }
		public string FullName { get => _firstName + " " + LastName; }
		public string Address { get => _address; }
		public string State { get => _state; }
		public Suburb Suburb { get => _suburb; }
		public string Mobile { get => _mobile; }
		public string Email { get => _email; }
		public SuburbList PreferredSuburbList { get => _preferredSuburbList; }
		public SkillList HasSkillList { get => _hasSkillList; }



		public Contractor(int contractorID)
		{
			_contractorID = contractorID;
		}

		public Contractor(string firstName, string lastName, string address, string state, Suburb suburb, string mobile, string email)
		{
			_firstName = firstName;
			_lastName = lastName;
			_address = address;
			_state = state;
			_suburb = suburb;
			_mobile = mobile;
			_email = email;
		}

		public Contractor(string firstName, string lastName, string address, string state, Suburb suburb, string mobile, string email, SuburbList preferredSuburbList, SkillList hasSkillList)
		{
			_firstName = firstName;
			_lastName = lastName;
			_address = address;
			_state = state;
			_suburb = suburb;
			_mobile = mobile;
			_email = email;
			_preferredSuburbList = preferredSuburbList;
			_hasSkillList = hasSkillList;
		}

		public Contractor(int contractorID, string firstName, string lastName, string address, string state, Suburb suburb, string mobile, string email)
		{
			_contractorID = contractorID;
			_firstName = firstName;
			_lastName = lastName;
			_address = address;
			_state = state;
			_suburb = suburb;
			_mobile = mobile;
			_email = email;
		}



		public Contractor(int contractorID, string firstName, string lastName, string address, string state, Suburb suburb, string mobile, string email, SuburbList preferredSuburbList, SkillList hasSkillList)
		{
			_contractorID = contractorID;
			_firstName = firstName;
			_lastName = lastName;
			_address = address;
			_state = state;
			_suburb = suburb;
			_mobile = mobile;
			_email = email;
			_preferredSuburbList = preferredSuburbList;
			_hasSkillList = hasSkillList;
		}


	}
}
