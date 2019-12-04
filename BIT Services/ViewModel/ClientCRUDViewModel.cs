using BIT_Services.Commands;
using BIT_Services.Model;
using BIT_Services.View;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;

namespace BIT_Services.ViewModel
{
	class ClientCRUDViewModel : NotificationClass
	{
		// Variables
		private string _clientName;
		private string _clientAddress;
		private string _clientState;
		private string _clientPhone;
		private string _clientEmail;
		private Suburb _clientSuburb;
		private string _clientNotes;

		private SuburbList _suburbList;

		private ClientList _clientList;
		private Client _selectedClient;

		private string _filterString;
		private ICollectionView _clientListView;

		

		private bool _clientListEnabled;
		private bool _dataEntryEnabled;





		// Data Bindings & Accessors
		public string ClientName
		{
			get { return _clientName; }
			set
			{
				_clientName = value;
				OnPropertyChanged("ClientName");
			}
		}

		public string ClientAddress
		{
			get { return _clientAddress; }
			set
			{
				_clientAddress = value;
				OnPropertyChanged("ClientAddress");
			}
		}

		public string ClientState
		{
			get { return _clientState; }
			set
			{
				_clientState = value;
				OnPropertyChanged("ClientState");
			}
		}

		public string ClientPhone
		{
			get { return _clientPhone; }
			set
			{
				_clientPhone = value;
				OnPropertyChanged("ClientPhone");
			}
		}

		public string ClientEmail
		{
			get { return _clientEmail; }
			set
			{
				_clientEmail = value;
				OnPropertyChanged("ClientEmail");
			}
		}

		public Suburb ClientSuburb
		{
			get { return _clientSuburb; }
			set
			{
				_clientSuburb = value;
				OnPropertyChanged("ClientSuburb");
				OnPropertyChanged("ClientSuburbIndex");
			}
		}

		/// <summary>
		/// Contains the index of suburblist that suburb is of
		/// </summary>
		public int ClientSuburbIndex
		{
			get
			{
				if (ClientSuburb == null) return -1;
				for (int i = 0; i < SuburbList.Count; i++)
				{
					if (ClientSuburb.ToString() == SuburbList[i].ToString())
					{
						return i;
					}
				}
				return -1;
				//return SuburbList.IndexOf(ClientSuburb);
			}
			set
			{
				if (value != -1)
				{
					ClientSuburb = SuburbList[value];
				}
				OnPropertyChanged("ClientSuburb");
				OnPropertyChanged("ClientSuburbIndex");
			}
		}

		public string ClientNotes
		{
			get { return _clientNotes; }
			set
			{
				_clientNotes = value;
				OnPropertyChanged("ClientNotes");
			}
		}



		public SuburbList SuburbList
		{
			get { return _suburbList; }
			set
			{
				_suburbList = value;
				OnPropertyChanged("SuburbList");
			}
		}


		



		public ClientList ClientList
		{
			get { return _clientList; }
			set
			{
				_clientList = value;
				OnPropertyChanged("ClientList");
			}
		}



		public Client SelectedClient
		{
			get { return _selectedClient; }
			set
			{
				_selectedClient = value;
				OnPropertyChanged("SelectedClient");
				// We've changed the Selected client, refill all fields if we haven't set it to null
				if (value != null)
				{
					LoadSelectedClient();
				}
			}
		}



		public string FilterString
		{
			get { return _filterString; }
			set
			{
				_filterString = value;
				OnPropertyChanged("FilterString");
			}
		}

		public bool ClientListEnabled
		{
			get { return _clientListEnabled; }
			set
			{
				_clientListEnabled = value;
				OnPropertyChanged("ClientListEnabled");
			}
		}

		/// <summary>
		/// Whether the user should be able to currently edit data fields.
		/// </summary>
		public bool DataEntryDisallowed
		{
			get { return _dataEntryEnabled; }
			set
			{
				_dataEntryEnabled = value;
				OnPropertyChanged("DataEntryDisallowed");
				OnPropertyChanged("DataEntryAllowed");
			}
		}

		/// <summary>
		/// Inverse of DataEntryDisallowed, used for buttons
		/// </summary>
		public bool DataEntryAllowed
		{
			get { return !_dataEntryEnabled; }
			set
			{
				_dataEntryEnabled = !value;
				OnPropertyChanged("DataEntryAllowed");
				OnPropertyChanged("DataEntryDisallowed");
			}
		}





		// Command Bindings
		public RelayCommand FilterCollection {  get { return new RelayCommand(FilterClientList, true); } }
		public RelayCommand AddButtonCommand { get { return new RelayCommand(AddButton, true); } }
		public RelayCommand UpdateButtonCommand { get { return new RelayCommand(UpdateButton, true); } }
		public RelayCommand DeleteButtonCommand { get { return new RelayCommand(DeleteButton, true); } }
		public RelayCommand SaveButtonCommand { get { return new RelayCommand(SaveButton, true); } }
		public RelayCommand CancelButtonCommand { get { return new RelayCommand(CancelButton, true); } }
	




		// Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public ClientCRUDViewModel()
		{
			SuburbList = DAL.GetSuburbs();
			UpdateClientList();
			this._clientListView = CollectionViewSource.GetDefaultView(_clientList);
			ClientListEnabled = true;
			DataEntryDisallowed = true;
		}
	



		// Methods



		/// <summary>
		/// Filters the client list using the filter string
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private bool ClientFilter(object item)
		{
			Client client = item as Client;
			if (FilterString == null || FilterString == "")
			{
				return true;
			}
			else
			{
				return client.ClientName.Contains(FilterString);
			}
		}
		/// <summary>
		/// Filters client list, bound method
		/// </summary>
		private void FilterClientList()
		{
			this._clientListView.Filter = ClientFilter;
		}
		

		
		private void AddButton()
		{
			SelectedClient = null;
			ResetDataEntry();
			DataEntryDisallowed = false;
			ClientListEnabled = false;
		}



		private void UpdateButton()
		{
			Client temp = SelectedClient;
			DataEntryDisallowed = false;
			ClientListEnabled = false;
			SelectedClient = temp;
		}



		private void DeleteButton()
		{
			DialogResult confirmation = MessageBox.Show("Are you sure you want to delete client " + SelectedClient.ClientName + "? This will likely fail if they have any job requests.", "Confirm Delete", MessageBoxButtons.YesNo);
			if (confirmation == DialogResult.Yes)
			{

				try
				{
					DAL.DeleteClient(SelectedClient);
					SelectedClient = null;
					UpdateClientList();
					ResetDataEntry();
				}
				catch (MySqlException)
				{
					MessageBox.Show("Failed to delete client, ensure they do not have any job requests.", "Failed to delete", MessageBoxButtons.OK);
				}
			}
			
		}



		private void SaveButton()
		{
			string validateMessage = ValidateData();
			if (validateMessage == null)
			{
				try
				{
					if (SelectedClient != null) // Are we in update mode?
					{
						Client updatedClient = new Client(
							SelectedClient.ClientID,
							ClientName,
							ClientAddress,
							ClientSuburb,
							ClientState,
							ClientPhone,
							ClientEmail,
							ClientNotes
							);

						DAL.UpdateClient(updatedClient);
						new EventLogger().Log("Inserted Client into database");

					}
					else // No we are in add mode
					{
						Client newClient = new Client(
							ClientName,
							ClientAddress,
							ClientSuburb,
							ClientState,
							ClientPhone,
							ClientEmail,
							ClientNotes
							);


						DAL.InsertClient(newClient);
						new EventLogger().Log("Updated Client in database");
					}
				}
				catch (MySqlException)
				{

					MessageBox.Show("Failed to save client", "Saving Failed", MessageBoxButtons.OK);
				}

				SelectedClient = null;
				ResetDataEntry();
				ClientListEnabled = true;
				DataEntryDisallowed = true;
				UpdateClientList();
			}
			else
			{
				System.Windows.Forms.MessageBox.Show(validateMessage, "Saving Failed", MessageBoxButtons.OK);
			}
		}



		private void CancelButton()
		{
			SelectedClient = null;
			ResetDataEntry();
			UpdateClientList();
			DataEntryDisallowed = true;
			ClientListEnabled = true;
		}



		private void LoadSelectedClient()
		{
			ClientName = SelectedClient.ClientName;
			ClientAddress = SelectedClient.Address;
			ClientState = SelectedClient.State;
			ClientPhone = SelectedClient.ContactPhone;
			ClientEmail = SelectedClient.Email;
			ClientSuburb = SelectedClient.Suburb;
			ClientNotes = SelectedClient.Notes;
		}



		private void ResetDataEntry()
		{
			ClientName = null;
			ClientAddress = null;
			ClientState = null;
			ClientPhone = null;
			ClientEmail = null;
			ClientSuburb = null;
			ClientNotes = null;
		}


		private void UpdateClientList()
		{
			ClientList = DAL.GetClients();
		}





		/// <summary>
		/// Validates all data entered.
		/// </summary>
		/// <returns>Null if all data was successfully validated, otherwise a string describing the problem.</returns>
		private string ValidateData()
		{
			if (ClientName == "" || ClientName == null)
			{
				return "Missing Name";
			}
			if (ClientAddress == "" || ClientAddress == null)
			{
				return "Missing Address";
			}
			if (ClientState == "" || ClientState == null)
			{
				return "Missing State";
			}
			if (ClientEmail == "" && ClientEmail == null)
			{
				return "Missing Email";
			}
			if (ClientSuburb == null)
			{
				return "Missing Suburb";
			}



			if (ClientState.Length > 3)
			{
				return "State must be no longer than three characters. E.G. NSW, QLD";
			}
			if (ClientPhone.Length != 10)
			{
				return "Phone number must be 10 digits long with no spaces";
			}
			
			
			
			try { System.Net.Mail.MailAddress email = new System.Net.Mail.MailAddress(ClientEmail); }
			catch (FormatException)
			{
				return "Please enter a valid email";
			}


			return null;
		}
	}
}
