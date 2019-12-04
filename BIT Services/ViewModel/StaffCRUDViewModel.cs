using BIT_Services.Commands;
using BIT_Services.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;

namespace BIT_Services.ViewModel
{
    class StaffCRUDViewModel : NotificationClass
    {
		// Variables
		private string _coordinatorFirstName;
		private string _coordinatorLastName;
		private string _coordinatorPhone;
		private string _coordinatorEmail;
		private string _coordinatorAddress;
		private string _coordinatorState;
		private Suburb _coordinatorSuburb;

		private SuburbList _suburbList;

		private CoordinatorList _coordinatorList;
		private Coordinator _selectedCoordinator;

		private string _filterString;
		private ICollectionView _coordinatorListView;

		private bool _coordinatorListEnabled;
		private bool _dataEntryAllowed;


		// Data Bindings & Accessors
		public string CoordinatorFirstName
		{
			get
			{
				return _coordinatorFirstName;
			}

			set
			{
				_coordinatorFirstName = value;
				OnPropertyChanged("CoordinatorFirstName");
			}
		}

		public string CoordinatorLastName
		{
			get
			{
				return _coordinatorLastName;
			}

			set
			{
				_coordinatorLastName = value;
				OnPropertyChanged("CoordinatorLastName");
			}
		}

		public string CoordinatorPhone
		{
			get
			{
				return _coordinatorPhone;
			}

			set
			{
				_coordinatorPhone = value;
				OnPropertyChanged("CoordinatorPhone");
			}
		}

		public string CoordinatorEmail
		{
			get
			{
				return _coordinatorEmail;
			}

			set
			{
				_coordinatorEmail = value;
				OnPropertyChanged("CoordinatorEmail");
			}
		}

		public string CoordinatorAddress
		{
			get
			{
				return _coordinatorAddress;
			}

			set
			{
				_coordinatorAddress = value;
				OnPropertyChanged("CoordinatorAddress");
			}
		}

		public string CoordinatorState
		{
			get
			{
				return _coordinatorState;
			}

			set
			{
				_coordinatorState = value;
				OnPropertyChanged("CoordinatorState");
			}
		}

		public string FilterString
		{
			get
			{
				return _filterString;
			}

			set
			{
				_filterString = value;
			}
		}

		public bool CoordinatorListEnabled
		{
			get
			{
				return _coordinatorListEnabled;
			}

			set
			{
				_coordinatorListEnabled = value;
				OnPropertyChanged("CoordinatorListEnabled");
			}
		}

		public bool DataEntryAllowed
		{
			get
			{
				return _dataEntryAllowed;
			}

			set
			{
				_dataEntryAllowed = value;
				OnPropertyChanged("DataEntryAllowed");
				OnPropertyChanged("DataEntryDisallowed");
			}
		}
		public bool DataEntryDisallowed
		{
			get
			{
				return !_dataEntryAllowed;
			}

			set
			{
				_dataEntryAllowed = !value;
				OnPropertyChanged("DataEntryAllowed");
				OnPropertyChanged("DataEntryDisallowed");
			}
		}

		public Suburb CoordinatorSuburb
		{
			get
			{
				return _coordinatorSuburb;
			}

			set
			{
				_coordinatorSuburb = value;
				OnPropertyChanged("CoordinatorSuburb");
				OnPropertyChanged("CoordinatorSuburbIndex");
			}
		}

		public int CoordinatorSuburbIndex
		{
			get
			{
				if (CoordinatorSuburb == null) return -1;
				for (int i = 0; i < SuburbList.Count; i++)
				{
					if (CoordinatorSuburb.ToString() == SuburbList[i].ToString())
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
					CoordinatorSuburb = SuburbList[value];
				}
				else
				{
					CoordinatorSuburb = null;
				}
				OnPropertyChanged("CoordinatorSuburb");
				OnPropertyChanged("CoordinatorSuburbIndex");
			}
		}

		public SuburbList SuburbList
		{
			get
			{
				return _suburbList;
			}

			set
			{
				_suburbList = value;
				OnPropertyChanged("SuburbList");
			}
		}

		public CoordinatorList CoordinatorList
		{
			get
			{
				return _coordinatorList;
			}

			set
			{
				_coordinatorList = value;
				OnPropertyChanged("CoordinatorList");
			}
		}

		public Coordinator SelectedCoordinator
		{
			get
			{
				return _selectedCoordinator;
			}
			set
			{
				_selectedCoordinator = value;
				OnPropertyChanged("SelectedClient");
				// We've changed the Selected client, refill all field if we haven't set it to null;
				if (value != null)
				{
					CoordinatorFirstName = SelectedCoordinator.FirstName;
					CoordinatorLastName = SelectedCoordinator.LastName;
					CoordinatorPhone = SelectedCoordinator.Mobile;
					CoordinatorEmail = SelectedCoordinator.Email;
					CoordinatorAddress = SelectedCoordinator.Address;
					CoordinatorSuburb = SelectedCoordinator.Suburb;
					CoordinatorState = SelectedCoordinator.State;
				}
			}
			
		}



		// Command Bindings
		public RelayCommand FilterCollection { get { return new RelayCommand(FilterCoordinatorList, true); } }
		public RelayCommand AddButtonCommand { get { return new RelayCommand(AddButton, true); } }
		public RelayCommand UpdateButtonCommand { get { return new RelayCommand(UpdateButton, true); } }
		public RelayCommand DeleteButtonCommand { get { return new RelayCommand(DeleteButton, true); } }
		public RelayCommand SaveButtonCommand { get { return new RelayCommand(SaveButton, true); } }
		public RelayCommand CancelButtonCommand { get { return new RelayCommand(CancelButton, true); } }


		// Constructor
		public StaffCRUDViewModel()
		{
			SuburbList = DAL.GetSuburbs();
			UpdateCoordinatorList();
			this._coordinatorListView = CollectionViewSource.GetDefaultView(_coordinatorList);
			DataEntryAllowed = false;
			CoordinatorListEnabled = true;
		}






		// Methods

		/// <summary>
		/// Filters the Coordinator list using the filter string
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private bool CoordinatorFilter(object item)
		{
			Coordinator coordinator = item as Coordinator;
			if (FilterString == null || FilterString == "")
			{
				return true;
			}
			else
			{
				return coordinator.FullName.Contains(FilterString);
			}
		}
		/// <summary>
		/// Filters Coordinator list, bound method
		/// </summary>
		private void FilterCoordinatorList()
		{
			this._coordinatorListView.Filter = CoordinatorFilter;
		}



		private void UpdateCoordinatorList()
		{
			CoordinatorList = DAL.GetCoordinators();
		}



		private void AddButton()
		{
			SelectedCoordinator = null;
			ResetDataEntry();
			DataEntryAllowed = true;
			CoordinatorListEnabled = false;

		}

		private void UpdateButton()
		{
			Coordinator temp = SelectedCoordinator;
			DataEntryAllowed = true;
			CoordinatorListEnabled = false;
			SelectedCoordinator = temp;
		}

		private void DeleteButton()
		{
			DialogResult confirmation = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete coordinator " + SelectedCoordinator.FullName + "?", "Confirm Delete", MessageBoxButtons.YesNo);
			if (confirmation == DialogResult.Yes)
			{
				try
				{
					DAL.DeleteCoordinator(SelectedCoordinator);
					SelectedCoordinator = null;
					UpdateCoordinatorList();
					ResetDataEntry();
				}
				catch (MySqlException)
				{
					MessageBox.Show("Failed to delete coordinator.", "Failed to delete", MessageBoxButtons.OK);
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
					if (SelectedCoordinator != null) // Are we  in update mode?
					{
						Coordinator updatedCoordinator = new Coordinator(
							SelectedCoordinator.CoordinatorID,
							CoordinatorFirstName,
							CoordinatorLastName,
							CoordinatorAddress,
							CoordinatorState,
							CoordinatorSuburb,
							CoordinatorPhone,
							CoordinatorEmail
							);
						DAL.UpdateCoordinator(updatedCoordinator);
						new EventLogger().Log("Updated Coordinator in database");
					}
					else // No we are in add mode
					{
						Coordinator newCoordinator = new Coordinator(
							CoordinatorFirstName,
							CoordinatorLastName,
							CoordinatorAddress,
							CoordinatorState,
							CoordinatorSuburb,
							CoordinatorPhone,
							CoordinatorEmail
							);
						DAL.InsertCoordinator(newCoordinator);

						new EventLogger().Log("Inserted Coordinator into database");
					}
				}
				catch (MySqlException)
				{

					MessageBox.Show("Failed to save coordinator", "Saving Failed", MessageBoxButtons.OK);
				}

				SelectedCoordinator = null;
				ResetDataEntry();
				DataEntryAllowed = false;
				CoordinatorListEnabled = true;
				UpdateCoordinatorList();
			}
			else
			{
				System.Windows.Forms.MessageBox.Show(validateMessage, "Saving Failed", MessageBoxButtons.OK);
			}
		}

		private void CancelButton()
		{
			SelectedCoordinator = null;
			ResetDataEntry();
			UpdateCoordinatorList();
			DataEntryAllowed = false;
			CoordinatorListEnabled = true;
		}

		private void ResetDataEntry()
		{
			CoordinatorFirstName = null;
			CoordinatorLastName = null;
			CoordinatorPhone = null;
			CoordinatorEmail = null;
			CoordinatorAddress = null;
			CoordinatorSuburbIndex = -1;
			CoordinatorState = null;

		}

		/// <summary>
		/// Validates all data entered
		/// </summary>
		/// <returns>Null if all data was successfully validated, otherrwise a string describing the problem with validation</returns>
		private string ValidateData()
		{
			if (CoordinatorFirstName == "" || CoordinatorFirstName == null)
			{
				return "Missing First Name";
			}
			if (CoordinatorLastName == "" || CoordinatorLastName == null)
			{
				return "Missing Last Name";
			}
			if (CoordinatorEmail == "" || CoordinatorPhone == null)
			{
				return "Missing Email";
			}
			if (CoordinatorAddress == "" || CoordinatorAddress == null)
			{
				return "Missing Address";
			}
			if (CoordinatorSuburb == null)
			{
				return "Missing Suburb";
			}


			if (CoordinatorState.Length > 3)
			{
				return "State must be no longer than three characters. E.G. NSW, QLD";
			}
			if (CoordinatorPhone.Length != 10)
			{
				return "Phone number must be 10 digits long with no spaces";
			}


			try { System.Net.Mail.MailAddress email = new System.Net.Mail.MailAddress(CoordinatorEmail); }
			catch (FormatException)
			{
				return "Please enter a valid email";
			}


			return null;
		}
	}
}
