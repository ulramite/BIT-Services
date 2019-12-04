using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Model
{
	public class AssignableContractor : Contractor
	{
		private bool _assigned;
		private TimeSpan _availabilityStart;
		private TimeSpan _availabilityEnd;
		private int? _status;

		public bool Assigned { get => _assigned; set => _assigned = value; }
		public TimeSpan AvailabilityStart { get => _availabilityStart; }
		public TimeSpan AvailabilityEnd { get => _availabilityEnd; }
		public string AvailabilityString
		{
			get
			{
				return AvailabilityStart.ToString() + "-" + AvailabilityEnd.ToString();
			}
		}
		public string SkillsString
		{
			get
			{
				StringBuilder skillsString = new StringBuilder();
				for (int i = 0; i < HasSkillList.Count; i++)
				{
					skillsString.Append(HasSkillList[i].SkillName);
					if (i != HasSkillList.Count - 1)
					{
						skillsString.Append("\r\n");
					}
				}

				return skillsString.ToString();
			}
		}
		public int? Status { get => _status; }
		public string StatusString
		{
			get
			{
				switch (Status)
				{
					case 0:
						return "Assigned";
					case 1:
						return "Rejected";
					case 2:
						return "Accepted";
					case 3:
						return "Completed";
					default:
						return "Unassigned";
				}
			}
		}

		

		public AssignableContractor(int contractorID, string firstName, string lastName, string address, string state, Suburb suburb, string mobile, string email, SkillList hasSkillList, bool assigned, TimeSpan availabilityStart, TimeSpan availabilityEnd, int? status) : base(contractorID, firstName, lastName, address, state, suburb, mobile, email, null, hasSkillList)
		{
			_assigned = assigned;
			_availabilityStart = availabilityStart;
			_availabilityEnd = availabilityEnd;
			_status = status;
		}

		
	}
}
