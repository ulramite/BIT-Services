using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Model
{
	class JobRequest
	{
		private int _jobRequestID;
		private int _clientID;
		private int _coordinatorID;
		private string _notes;
		private DateTime _timeRequested;
		private char _status;
		private string _address;
		private string _suburb;
		private string _feedback;

		public int JobRequestID { get => _jobRequestID; }
		public int ClientID { get => _clientID; }
		public int CoordinatorID { get => _coordinatorID; }
		public string Notes { get => _notes; }
		public DateTime TimeRequested { get => _timeRequested;  }
		public char Status { get => _status; }
		public string Address { get => _address; }
		public string Suburb { get => _suburb; }
		public string Feedback { get => _feedback; }

		public JobRequest(int jobRequestID, int clientID, int coordinatorID, string notes, DateTime timeRequested, char status, string address, string suburb, string feedback)
		{
			_jobRequestID = jobRequestID;
			_clientID = clientID;
			_coordinatorID = coordinatorID;
			_notes = notes;
			_timeRequested = timeRequested;
			_status = status;
			_address = address;
			_suburb = suburb;
			_feedback = feedback;
		}
	}
}
