using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Model
{
	public class JobRequest
	{
		private int _jobRequestID;
		private int _clientID;
		private int? _coordinatorID;
		private string _clientName;
		private string _notes;
		private DateTime _timeRequested;
		private int _estimatedHours;
		private int _status;
		private string _address;
		private Suburb _suburb;
		private string _feedback;

		public int JobRequestID { get => _jobRequestID; }
		public int ClientID { get => _clientID; }
		public int? CoordinatorID { get => _coordinatorID; }
		public string ClientName { get => _clientName; }
		public string Notes { get => _notes; }
		public DateTime TimeRequested { get => _timeRequested;  }
		public int EstimatedHours { get => _estimatedHours; }
		public int Status { get => _status; }
		public string DisplayStatus
		{
			get
			{
				switch (Status)
				{
					case (0):
						return "Requested";
					case (1):
						return "Assigned";
					case (2):
						return "Accepted";
					case (3):
						return "Complete";
					case (4):
						return "Paid";
				}
				return null;
			}
		}
		public string Address { get => _address; }
		public Suburb Suburb { get => _suburb; }
		public string Feedback { get => _feedback; }




		public JobRequest(int jobRequestID)
		{
			_jobRequestID = jobRequestID;
		}

		public JobRequest(int jobRequestID, int clientID, int? coordinatorID, string clientName, string notes, DateTime timeRequested, int estimatedHours, int status, string address, Suburb suburb, string feedback)
		{
			_jobRequestID = jobRequestID;
			_clientID = clientID;
			_coordinatorID = coordinatorID;
			_clientName = clientName;
			_notes = notes;
			_timeRequested = timeRequested;
			_estimatedHours = estimatedHours;
			_status = status;
			_address = address;
			_suburb = suburb;
			_feedback = feedback;
		}
	}
}
