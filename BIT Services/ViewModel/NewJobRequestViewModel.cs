using BIT_Services.Commands;
using BIT_Services.Model;
using BIT_Services.View;
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
	class NewJobRequestViewModel : NotificationClass
	{
		// Variables
		private Client _selectedClient;
		private ClientList _clientList;

		private string _jobRequestNotes;
		private DateTime _jobRequestDateRequested;
		private TimeSpan _jobRequestTimeRequested;
		private int _jobRequestEstimatedHours;
		private Suburb _jobRequestSuburb;
		private string _jobRequestAddress;

        private SuburbList _suburbList;

		private Skill _selectedSkill;
		private SkillList _selectedSkillList;

		private Skill _unselectedSkill;
		private SkillList _unselectedSkillList;

		private string _filterString;
		private ICollectionView _clientListView;

		
		





		// Data Bindings & Accessors
		public string JobRequestNotes
		{
			get { return _jobRequestNotes; }

			set
			{
				_jobRequestNotes = value;
				OnPropertyChanged("JobRequestNotes");
			}
		}

		public DateTime JobRequestDateRequested
		{
			get { return _jobRequestDateRequested; }

			set
			{
				_jobRequestDateRequested = value;
				OnPropertyChanged("JobRequestDateRequested");
			}
		}

		public TimeSpan JobRequestTimeRequested
		{
			get { return _jobRequestTimeRequested; }

			set
			{
				_jobRequestTimeRequested = value;
				OnPropertyChanged("JobRequestTimeRequested");
			}
		}
		public TimeSpan[] TimeRequestedOptions
		{
			get
			{
				return new TimeSpan[]
				{
					TimeSpan.FromHours(9),
					TimeSpan.FromHours(10),
					TimeSpan.FromHours(11),
					TimeSpan.FromHours(12),
					TimeSpan.FromHours(13),
					TimeSpan.FromHours(14),
					TimeSpan.FromHours(15),
					TimeSpan.FromHours(16),
					TimeSpan.FromHours(17),
			};
			}
		}

		// Returns an int, but will accept a string and attempt to parse it.
		public object JobRequestEstimatedHours
		{
			get { return _jobRequestEstimatedHours; }
			set
			{
                if (value == null)
                {
                    _jobRequestEstimatedHours = 0;
                }
				else if (value is string)
				{
					if (int.TryParse((string)value, out int result))
					{
						_jobRequestEstimatedHours = int.Parse((string)value);
					}
				}
				else
				{
					_jobRequestEstimatedHours = (int)value;
				}
				OnPropertyChanged("JobRequestEstimatedHours");
			}
		}

		public Suburb JobRequestSuburb
		{
			get { return _jobRequestSuburb; }

			set
			{
				_jobRequestSuburb = value;
				OnPropertyChanged("JobRequestSuburb");
			}
		}

		public string JobRequestAddress
		{
			get { return _jobRequestAddress; }

			set
			{
				_jobRequestAddress = value;
				OnPropertyChanged("JobRequestAddress");
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





		public string FilterString
		{
			get { return _filterString; }

			set
			{
				_filterString = value;
				OnPropertyChanged("FilterString");
			}
		}

		public ICollectionView ClientListView
		{
			get { return _clientListView; }

			set
			{
				_clientListView = value;
				OnPropertyChanged("JobRequestListView");
			}
		}




		public Client SelectedClient
		{
			get { return _selectedClient; }

			set
			{
				_selectedClient = value;
				OnPropertyChanged("SelectedClient");
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




		public Skill SelectedSkill
		{
			get { return _selectedSkill; }

			set
			{
				_selectedSkill = value;
				OnPropertyChanged("SelectedSkill");
			}
		}

		public SkillList SelectedSkillList
		{
			get { return _selectedSkillList; }

			set
			{
				_selectedSkillList = value;
				OnPropertyChanged("SelectedSkillList");
			}
		}




		public Skill UnselectedSkill
		{
			get { return _unselectedSkill; }

			set
			{
				_unselectedSkill = value;
				OnPropertyChanged("UnselectedSkill");
			}
		}

		public SkillList UnselectedSkillList
		{
			get { return _unselectedSkillList; }

			set
			{
				_unselectedSkillList = value;
				OnPropertyChanged("UnselectedSkillList");
			}
		}








		// Command Bindings
		public RelayCommand FilterCollection { get { return new RelayCommand(FilterClientList, true); } }
		public RelayCommand SaveButtonCommand { get { return new RelayCommand(SaveButton, true); } }
		public RelayCommand CancelButtonCommand { get { return new RelayCommand(CancelButton, true); } }
		public RelayCommand AddSkillCommand { get { return new RelayCommand(AddSkill, true); } }
		public RelayCommand RemoveSkillCommand { get { return new RelayCommand(RemoveSkill, true); } }
		public RelayCommand EditSkillCommand { get { return new RelayCommand(EditSkill, true); } }
        public Action CloseAction { get; set; }







		// Constructor
		public NewJobRequestViewModel()
		{
			UpdateClientList();
			UpdateUnselectedSkillList();
			SelectedSkillList = new SkillList();
            SuburbList = DAL.GetSuburbs();
			JobRequestDateRequested = DateTime.Now;
			this._clientListView = CollectionViewSource.GetDefaultView(_clientListView);
		}

		










		// Methods
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

		private void FilterClientList()
		{
			this._clientListView.Filter = ClientFilter;
		}



		private void SaveButton()
		{
			string validateMessage = ValidateData();
			if (validateMessage == null)
			{
				try
				{
					JobRequest jobRequest = new JobRequest(
								DAL.GetJobRequestAutoIncrement(),
								SelectedClient.ClientID,
								null,
								SelectedClient.ClientName,
								JobRequestNotes,
								JobRequestDateRequested.Add(JobRequestTimeRequested),
								(int)JobRequestEstimatedHours,
								0,
								JobRequestAddress,
								JobRequestSuburb,
								null
								);
					DAL.InsertJobRequest(jobRequest);
					new EventLogger().Log("Inserted Job Request in database");

					foreach (Skill skill in SelectedSkillList)
					{
						DAL.AddRequiredSkill(jobRequest, skill);
					}

					CloseAction();
				}
				catch (MySql.Data.MySqlClient.MySqlException)
				{

					MessageBox.Show("Failed to save job request", "Saving Failed", MessageBoxButtons.OK);
				}
			}
			else
			{
				System.Windows.Forms.MessageBox.Show(validateMessage, "Saving Failed", MessageBoxButtons.OK);
			}
		}

		private void CancelButton()
		{
			ResetDataEntry();
			UpdateClientList();
			UpdateUnselectedSkillList();
		}

		
		
		private void AddSkill()
		{
			if (UnselectedSkill != null)
			{
				SelectedSkillList.Add(UnselectedSkill);
				UnselectedSkillList.Remove(UnselectedSkill);
			}
		}

		private void RemoveSkill()
		{
			if (SelectedSkill != null)
			{
				UnselectedSkillList.Add(SelectedSkill);
				SelectedSkillList.Remove(SelectedSkill);
			}
		}

		private void EditSkill()
		{
			SkillEdit skillsWindow = new SkillEdit();
			skillsWindow.ShowDialog();
			UpdateUnselectedSkillList();
		}



		private void ResetDataEntry()
		{
			JobRequestNotes = null;
			JobRequestDateRequested = DateTime.Today;
			JobRequestTimeRequested = TimeSpan.FromHours(9);
			JobRequestEstimatedHours = null;
			JobRequestSuburb = null;
			JobRequestAddress = null;
			SelectedSkill = null;
			UnselectedSkill = null;
			UpdateUnselectedSkillList();
			SelectedSkillList = new SkillList();
		}



		private void UpdateClientList()
		{
			ClientList = DAL.GetClients();
		}


		
		private void UpdateUnselectedSkillList()
		{
			UnselectedSkillList = DAL.GetSkills();
		}




		private string ValidateData()
		{
			if (JobRequestNotes == "" || JobRequestNotes == null)
			{
				return "Missing Request Details";
			}
			if (JobRequestAddress == "" || JobRequestAddress == null)
			{
				return "Missing Request Address";
			}
			if (JobRequestSuburb == null)
			{
				return "Missing Suburb";
			}
			if (JobRequestDateRequested == null)
			{
				return "Missing Date Requested";
			}
			if (JobRequestTimeRequested == null)
			{
				return "Missing Time Requested";
			}
            if (SelectedClient == null)
            {
                return "Missing Client";
            }



			if ((int)JobRequestEstimatedHours < 0 || JobRequestEstimatedHours == null)
			{
				return "Please enter a non-negative number for Estimated Hours";
			}
			if (JobRequestDateRequested < DateTime.Today)
			{
				return "Date Requested cannot be in the past";
			}
			return null;
		}
	}
}
