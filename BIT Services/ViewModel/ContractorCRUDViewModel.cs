using BIT_Services.Commands;
using BIT_Services.Model;
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
    class ContractorCRUDViewModel : NotificationClass
    {
		// Variables
		private string _contractorFirstName;
		private string _contractorLastName;
		private string _contractorAddress;
		private string _contractorState;
		private Suburb _contractorSuburb;
		private string _contractorMobile;
		private string _contractorEmail;
		
		

		private Contractor _selectedContractor;
		private ContractorList _contractorList;

		private SuburbList _suburbList;

		private Suburb _selectedSuburbToAdd;
		private SuburbList _unselectedSuburbList;
		private Suburb _selectedSuburbToRemove;
		private SuburbList _contractorPreferredSuburbList;

		private Skill _selectedSkillToAdd;
		private SkillList _unselectedSkillList;
		private Skill _selectedSkillToRemove;
		private SkillList _contractorHasSkillList;

		private string _filterString;
		private ICollectionView _contractorListView;

		private bool _contractorListEnabled;
		private bool _dataEntryAllowed;


		// Data Bindings & Accessors
		public string ContractorFirstName
		{
			get { return _contractorFirstName; }

			set
			{
				_contractorFirstName = value;
				OnPropertyChanged("ContractorFirstName");
			}
		}

		public string ContractorLastName
		{
			get { return _contractorLastName; }

			set
			{
				_contractorLastName = value;
				OnPropertyChanged("ContractorLastName");
			}
		}

		public string ContractorAddress
		{
			get { return _contractorAddress; }

			set
			{
				_contractorAddress = value;
				OnPropertyChanged("ContractorAddress");
			}
		}

		public string ContractorState
		{
			get { return _contractorState; }

			set
			{
				_contractorState = value;
				OnPropertyChanged("ContractorState");
			}
		}


		public string ContractorMobile
		{
			get { return _contractorMobile; }

			set
			{
				_contractorMobile = value;
				OnPropertyChanged("ContractorMobile");
			}
		}

		public string ContractorEmail
		{
			get { return _contractorEmail; }

			set
			{
				_contractorEmail = value;
				OnPropertyChanged("ContractorEmail");
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



		public bool ContractorListEnabled
		{
			get { return _contractorListEnabled; }

			set
			{
				_contractorListEnabled = value;
				OnPropertyChanged("ContractorListEnabled");
			}
		}
		public bool DataEntryAllowed
		{
			get { return _dataEntryAllowed; }

			set
			{
				_dataEntryAllowed = value;
                OnPropertyChanged("DataEntryAllowed");
                OnPropertyChanged("DataEntryDisllowed");

            }
		}
		public bool DataEntryDisallowed
		{
			get { return !_dataEntryAllowed; }

			set
			{
				_dataEntryAllowed = !value;
                OnPropertyChanged("DataEntryAllowed");
                OnPropertyChanged("DataEntryDisallowed");
            }
		}

		

		public Contractor SelectedContractor
		{
			get { return _selectedContractor; }

			set
			{
				_selectedContractor = value;
				OnPropertyChanged("SelectedContractor");

				if (value != null)
				{
					LoadSelectedContractor();
				}
			}
		}
		public ContractorList ContractorList
		{
			get { return _contractorList; }

			set
			{
				_contractorList = value;
				OnPropertyChanged("ContractorList");
			}
		}



		// Contractors suburb
		public Suburb ContractorSuburb
		{
			get { return _contractorSuburb; }

			set
			{
				_contractorSuburb = value;
				OnPropertyChanged("ContractorSuburb");
				OnPropertyChanged("ContractorSuburbIndex");
			}
		}
		// index of contractors suburb in suburb list
		public int ContractorSuburbIndex
		{
			get
			{
				if (ContractorSuburb == null) return -1;
				for (int i = 0; i < SuburbList.Count; i++)
				{
					if (ContractorSuburb.ToString() == SuburbList[i].ToString())
					{
						return i;
					}
				}
				return -1;
			}

			set
			{
				if (value != -1)
				{
					ContractorSuburb = SuburbList[value];
				}
				else
				{
					ContractorSuburb = null;
				}
				OnPropertyChanged("ContractorSuburb");
				OnPropertyChanged("ContractorSuburbIndex");
			}
		}
		// List of all suburbs in databsae
		public SuburbList SuburbList
		{
			get { return _suburbList; }

			set
			{
				_suburbList = value;
				OnPropertyChanged("SuburbList");
			}
		}



		public Suburb SelectedSuburbToAdd
		{
			get { return _selectedSuburbToAdd; }
			set
			{
				_selectedSuburbToAdd = value;
				OnPropertyChanged("SelectedSuburbToAdd");
			}
		}
		public SuburbList UnselectedSuburbList
		{
			get { return _unselectedSuburbList; }

			set
			{
				_unselectedSuburbList = value;
				OnPropertyChanged("UnselectedSuburbList");
			}
		}



		public Suburb SelectedSuburbToRemove
		{
			get { return _selectedSuburbToRemove; }
			set
			{
				_selectedSuburbToRemove = value;
				OnPropertyChanged("SelectedSuburbToRemove");
			}
		}
		public SuburbList ContractorPreferredSuburbList
		{
			get { return _contractorPreferredSuburbList; }

			set
			{
				_contractorPreferredSuburbList = value;
				OnPropertyChanged("ContractorPreferredSuburbList");
			}
		}



		public Skill SelectedSkillToAdd
		{
			get { return _selectedSkillToAdd; }
			set
			{
				_selectedSkillToAdd = value;
				OnPropertyChanged("SelectedSkillToAdd");
			}
		}
		public SkillList UnselectedSkillList
		{
			get{ return _unselectedSkillList; }
			set
			{
				_unselectedSkillList = value;
				OnPropertyChanged("UnselectedSkillList");
			}
		}



		public Skill SelectedSkillToRemove
		{
			get { return _selectedSkillToRemove; }
			set
			{
				_selectedSkillToRemove = value;
				OnPropertyChanged("SelectedSkillToRemove");
			}
		}
		public SkillList ContractorHasSkillList
		{
			get { return _contractorHasSkillList; }

			set
			{
				_contractorHasSkillList = value;
				OnPropertyChanged("ContractorHasSkillList");
			}
		}

		// Command Bindings
		public RelayCommand AddButtonCommand { get { return new RelayCommand(AddButton, true); } }
		public RelayCommand UpdateButtonCommand { get { return new RelayCommand(UpdateButton, true); } }
		public RelayCommand DeleteButtonCOmmand { get { return new RelayCommand(DeleteButton, true); } }
		public RelayCommand SaveButtonCommand { get { return new RelayCommand(SaveButton, true); } }
		public RelayCommand CancelButtonCommand { get { return new RelayCommand(CancelButton, true); } }
		public RelayCommand AddSuburbToPreferredCommand { get { return new RelayCommand(AddSuburbToPreferred, true); } }
		public RelayCommand RemoveSuburbFromPreferredCommand { get { return new RelayCommand(RemoveSuburbFromPreferred, true); } }
		public RelayCommand EditSuburbCommand { get { return new RelayCommand(EditSuburb, true); } }
		public RelayCommand AddSkillToPreferredCommand { get { return new RelayCommand(AddSkillToPreferred, true); } }
		public RelayCommand RemoveSkillFromPreferredCommand { get { return new RelayCommand(RemoveSkillFromPreferred, true); } }
		public RelayCommand EditSkillCommand { get { return new RelayCommand(EditSkill, true); } }




		// Constructor
		public ContractorCRUDViewModel()
		{
			SuburbList = DAL.GetSuburbs();
			UpdateContractorList();
			this._contractorListView = CollectionViewSource.GetDefaultView(_contractorList);
			DataEntryAllowed = false;
			ContractorListEnabled = true;
		}




		// Methods

		private bool ContractorFilter(object item)
		{
			Contractor contractor = item as Contractor;
			if (FilterString == null || FilterString == "")
			{
				return true;
			}
			else
			{
				return contractor.FullName.Contains(FilterString);
			}
		}

		private void FilterContractorList()
		{
			this._contractorListView.Filter = ContractorFilter;
		}




		private void UpdateContractorList()
		{
			ContractorList = DAL.GetContractors();
		}



		private void UpdateBothSuburbsList()
		{
			ContractorPreferredSuburbList = DAL.PreferredSuburbsList(SelectedContractor);
			UnselectedSuburbList = DAL.UnselectedSuburbsList(SelectedContractor);
		}

		private void UpdateBothSkillsList()
		{
			ContractorHasSkillList = DAL.HasSkillList(SelectedContractor);
			UnselectedSkillList = DAL.UnselectedSkillList(SelectedContractor);
		}



		private void AddButton()
		{
			SelectedContractor = null;
			ResetDataEntry();
			DataEntryAllowed = true;
			ContractorListEnabled = false;
		}

		private void UpdateButton()
		{
			Contractor temp = SelectedContractor;
			DataEntryAllowed = true;
			ContractorListEnabled = false;
			SelectedContractor = temp;
		}

		private void DeleteButton()
		{
			DialogResult confirmation = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete coordinator " + SelectedContractor.FullName + "?", "Confirm Delete", MessageBoxButtons.YesNo);
			if (confirmation == DialogResult.Yes)
			{
				DAL.DeleteContractor(SelectedContractor);
				SelectedContractor = null;
				UpdateContractorList();
				ResetDataEntry();
			}
		}

		private void SaveButton()
		{
			string validateMessage = ValidateData();
			if (validateMessage == null)
			{
				if (SelectedContractor != null)// Are we  in update mode?
				{
					Contractor updatedContractor = new Contractor(
						SelectedContractor.ContractorID,
						ContractorFirstName,
						ContractorLastName,
						ContractorAddress,
						ContractorState,
						ContractorSuburb,
						ContractorMobile,
						ContractorEmail,
						ContractorPreferredSuburbList,
						ContractorHasSkillList
						);
					DAL.UpdateContractor(updatedContractor);


					// Grab the list of preferred suburbs from database
					SuburbList DBPreferredSuburbList = DAL.PreferredSuburbsList(updatedContractor);
					// Compare each entry from contractor to DB, if it does not exist in DB, add it
					foreach (Suburb contractorSuburb in updatedContractor.PreferredSuburbList)
					{
						bool found = false;
						foreach (Suburb dbSuburb in DBPreferredSuburbList)
						{
							if (contractorSuburb.SuburbName == dbSuburb.SuburbName)
							{
								found = true;
								break;
							}
						}
						if (!found)
						{
							DAL.AddPreferredSuburb(updatedContractor, contractorSuburb);
						}
					}
					// Compare each entry from db to contractor, if it doesn't exist in contractor, remove it
					foreach (Suburb dbSuburb in DBPreferredSuburbList)
					{
						bool found = false;
						foreach (Suburb contractorSuburb in updatedContractor.PreferredSuburbList)
						{
							if (dbSuburb.SuburbName == contractorSuburb.SuburbName)
							{
								found = true;
								break;
							}
						}
						if (!found)
						{
							DAL.RemovePreferredSuburb(updatedContractor, dbSuburb);
						}
					}

					// TODO Code for updating skills and suburbs
				}
				else // No we are in add mode
				{
					Contractor newContractor = new Contractor(
						SelectedContractor.ContractorID,
						ContractorFirstName,
						ContractorLastName,
						ContractorAddress,
						ContractorState,
						ContractorSuburb,
						ContractorMobile,
						ContractorEmail,
						ContractorPreferredSuburbList,
						ContractorHasSkillList
						);
					DAL.InsertContractor(newContractor);

					// Insert all suburbs
					foreach (Suburb suburb in newContractor.PreferredSuburbList)
					{
						DAL.AddPreferredSuburb(newContractor, suburb);
					}

					// Insert all skills
					foreach (Skill skill in newContractor.HasSkillList)
					{
						DAL.AddHasSkill(newContractor, skill);
					}
				}
				SelectedContractor = null;
				ResetDataEntry();
				UpdateContractorList();
				DataEntryAllowed = false;
				ContractorListEnabled = true;
			}
		}

		private void CancelButton()
		{
			SelectedContractor = null;
			ResetDataEntry();
			UpdateContractorList();
			DataEntryAllowed = false;
			ContractorListEnabled = true;
		}



		private void AddSuburbToPreferred()
		{
            if (SelectedSuburbToAdd != null)
            {
                ContractorPreferredSuburbList.Add(SelectedSuburbToAdd);
                UnselectedSuburbList.Remove(SelectedSuburbToAdd);
                SelectedSuburbToAdd = null;
                SelectedSuburbToRemove = null;
            }
		}

		private void RemoveSuburbFromPreferred()
		{
            if (SelectedSuburbToRemove != null)
            {
                UnselectedSuburbList.Add(SelectedSuburbToRemove);
                ContractorPreferredSuburbList.Remove(SelectedSuburbToRemove);
                SelectedSuburbToAdd = null;
                SelectedSuburbToRemove = null;
            }
		}

		private void EditSuburb()
		{

		}



		private void AddSkillToPreferred()
		{
            if (SelectedSkillToAdd != null)
            {
                ContractorHasSkillList.Add(SelectedSkillToAdd);
                UnselectedSkillList.Remove(SelectedSkillToAdd);
                SelectedSkillToAdd = null;
                SelectedSkillToRemove = null;
            }
		}

        private void RemoveSkillFromPreferred()
        {
            if (SelectedSkillToRemove != null)
            {
                UnselectedSkillList.Add(SelectedSkillToAdd);
                ContractorHasSkillList.Remove(SelectedSkillToAdd);
                SelectedSkillToAdd = null;
                SelectedSkillToRemove = null;
            }
		}

		private void EditSkill()
		{

		}



		private void LoadSelectedContractor()
		{
			ContractorFirstName = SelectedContractor.FirstName;
			ContractorLastName = SelectedContractor.LastName;
			ContractorAddress = SelectedContractor.Address;
			ContractorState = SelectedContractor.State;
			ContractorSuburb = SelectedContractor.Suburb;
			ContractorState = SelectedContractor.State;
			ContractorMobile = SelectedContractor.Mobile;
			ContractorEmail = SelectedContractor.Email;
			UpdateBothSuburbsList();
			UpdateBothSkillsList();
		}



		private void ResetDataEntry()
		{
			ContractorFirstName = null;
			ContractorLastName = null;
			ContractorAddress = null;
			ContractorState = null;
			ContractorSuburb = null;
			ContractorState = null;
			ContractorMobile = null;
			ContractorEmail = null;
			ContractorPreferredSuburbList = null;
			UnselectedSuburbList = null;
			ContractorHasSkillList = null;
			UnselectedSkillList = null;
		}



		/// <summary>
		/// Validates all data entered
		/// </summary>
		/// <returns>Null if all data was successfully validated, otherrwise a string describing the problem with validation</returns>
		private string ValidateData()
		{
			if (ContractorState.Length > 3)
			{
				return "State must be no longer than three characters. E.G. NSW, QLD";
			}
			return null;
		}
	}
}
