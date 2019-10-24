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
        private string _suburb;
        private string _state;
        private string _contactPhone;
        private string _email;
        private string _notes;

        public int ClientID { get => _clientID; set => _clientID = value; }
        public string ClientName { get => _clientName; set => _clientName = value; }
        public string Address { get => _address; set => _address = value; }
        public string Suburb { get => _suburb; set => _suburb = value; }
        public string State { get => _state; set => _state = value; }
        public string ContactPhone { get => _contactPhone; set => _contactPhone = value; }
        public string Email { get => _email; set => _email = value; }
        public string Notes { get => _notes; set => _notes = value; }
    }
}
