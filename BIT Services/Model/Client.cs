using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Model
{
    class Client
    {
        private int _clientID;
        private string _clientName;
        private string _address;
        private Suburb _suburb;
        private string _state;
        private string _contactPhone;
        private string _email;
        private string _notes;

        public int ClientID { get => _clientID;  }
        public string ClientName { get => _clientName;  }
        public string Address { get => _address;  }
        public Suburb Suburb { get => _suburb;  }
        public string State { get => _state;  }
        public string ContactPhone { get => _contactPhone;  }
        public string Email { get => _email;  }
        public string Notes { get => _notes;  }

		public Client(string clientName, string address, Suburb suburb, string state, string contactPhone, string email, string notes)
		{
			_clientName = clientName;
			_address = address;
			_suburb = suburb;
			_state = state;
			_contactPhone = contactPhone;
			_email = email;
			_notes = notes;
		}

		public Client(int clientID, string clientName, string address, Suburb suburb, string state, string contactPhone, string email, string notes)
		{
			_clientID = clientID;
			_clientName = clientName;
			_address = address;
			_suburb = suburb;
			_state = state;
			_contactPhone = contactPhone;
			_email = email;
			_notes = notes;
		}

		
	}
}
