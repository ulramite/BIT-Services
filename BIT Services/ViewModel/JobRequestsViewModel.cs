using BIT_Services.Commands;
using BIT_Services.Model;
using BIT_Services.View;
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
	class JobRequestsViewModel : NotificationClass
	{
		// Variables
		private JobRequest _selectedJobRequest;
		private JobRequestList _jobRequestList;

		// Contains list of both assigned and assignable
		private AssignableContractorList _assignableContractorList;

		private Skill _jobRequestSelectedSkill;
		private SkillList _jobRequestSkillList;

		private Skill _nonRequiredSelectedSkill;
		private SkillList _nonRequiredSkillList;

		private int _estimatedHours;

		private bool _markedAsReady;


		private string _filterString;
		private ICollectionView _jobRequestListView;

		private bool _jobRequestListEnabled;
		private bool _dataEntryAllowed;





		// Data Bindings & Accessors
		public string FilterString
		{
			get { return _filterString; }

			set
			{
				_filterString = value;
				OnPropertyChanged("FilterString");
			}
		}

		public bool JobRequestListEnabled
		{
			get { return _jobRequestListEnabled; }

			set
			{
				_jobRequestListEnabled = value;
				OnPropertyChanged("JobRequestListEnabled");
			}
		}

		public bool DataEntryAllowed
		{
			get { return _dataEntryAllowed; }

			set
			{
				_dataEntryAllowed = value;
				OnPropertyChanged("DataEntryAllowed");
				OnPropertyChanged("DataEntryDisallowed");
				OnPropertyChanged("CanMarkAsReady");
			}
		}
		public bool DataEntryDisallowed
		{
			get { return !_dataEntryAllowed; }
		}

		public bool CanMarkAsReady
		{
			get
			{
				return (DataEntryAllowed && SelectedJobRequest.Status < 2);
			}
		}

		public JobRequest SelectedJobRequest
		{
			get { return _selectedJobRequest; }

			set
			{
				_selectedJobRequest = value;
				OnPropertyChanged("SelectedJobRequest");
				if (value != null)
				{
					LoadSelectedJobRequest();
				}
			}
		}

		public JobRequestList JobRequestList
		{
			get { return _jobRequestList; }

			set
			{
				_jobRequestList = value;
				OnPropertyChanged("JobRequestList");
			}
		}

		// Contains list of both assigned and assignable
		public AssignableContractorList AssignableContractorList
		{
			get { return _assignableContractorList; }

			set
			{
				_assignableContractorList = value;
				OnPropertyChanged("AssignableContractorList");
			}
		}

		public Skill JobRequestSelectedSkill
		{
			get { return _jobRequestSelectedSkill; }

			set
			{
				_jobRequestSelectedSkill = value;
				OnPropertyChanged("JobRequestSelectedSkill");
			}
		}

		public SkillList JobRequestSkillList
		{
			get { return _jobRequestSkillList; }

			set
			{
				_jobRequestSkillList = value;
				OnPropertyChanged("JobRequestSkillList");
			}
		}

		public Skill NonRequiredSelectedSkill
		{
			get { return _nonRequiredSelectedSkill; }

			set
			{
				_nonRequiredSelectedSkill = value;
				OnPropertyChanged("NonRequiredSelectedSkill");
			}
		}

		public SkillList NonRequiredSkillList
		{
			get { return _nonRequiredSkillList; }

			set
			{
				_nonRequiredSkillList = value;
				OnPropertyChanged("NonRequiredSkillList");
			}
		}



		// Returns an int, but will accept a string and attempt to parse it.
		public object EstimatedHours
		{
			get { return _estimatedHours;}
			set
			{
				if (value is string)
				{
					if (int.TryParse((string)value, out int result))
					{
						_estimatedHours = int.Parse((string)value);
					}
				}
				else
				{
					_estimatedHours = (int)value;
				}
				OnPropertyChanged("EstimatedHours");
			}
		}

		public bool MarkedAsReady
		{
			get { return _markedAsReady; }
			set
			{
				_markedAsReady = value;
				OnPropertyChanged("MarkedAsReady");
			}
		}









		// Command Bindings
		public RelayCommand FilterCollection { get { return new RelayCommand(FilterJobRequestList, true); } }
		public RelayCommand AddButtonCommand { get { return new RelayCommand(AddButton, true); } }
		public RelayCommand UpdateButtonCommand { get { return new RelayCommand(UpdateButton, true); } }
		public RelayCommand DeleteButtonCommand { get { return new RelayCommand(DeleteButton, true); } }
		public RelayCommand SaveButtonCommand { get { return new RelayCommand(SaveButton, true); } }
		public RelayCommand CancelButtonCommand { get { return new RelayCommand(CancelButton, true); } }
        public RelayCommand AddSkillCommand { get { return new RelayCommand(AddSkill, true); } }
        public RelayCommand RemoveSkillCommand { get { return new RelayCommand(RemoveSkill, true); } }
        public RelayCommand EditSkillCommand { get { return new RelayCommand(EditSkill, true); } }

		





		// Constructor
		public JobRequestsViewModel()
		{
			UpdateJobRequestList();
			this._jobRequestListView = CollectionViewSource.GetDefaultView(_jobRequestList);
			DataEntryAllowed = false;
			JobRequestListEnabled = true;
		}





		// Methods
		private bool JobRequestFilter(object item)
		{
			JobRequest jobRequest = item as JobRequest;
			if (FilterString == null || FilterString == "")
			{
				return true;
			}
			else
			{
				return jobRequest.ClientName.Contains(FilterString);
			}
		}

		private void FilterJobRequestList()
		{
			this._jobRequestListView.Filter = JobRequestFilter;
		}




		private void UpdateJobRequestList()
		{
			JobRequestList = DAL.GetJobRequests();
		}




		private void AddButton()
		{
			NewJobRequest newJobRequestWindow = new NewJobRequest();
			newJobRequestWindow.Show();
		}

		private void UpdateButton()
		{
			if (SelectedJobRequest != null)
			{
				JobRequest temp = SelectedJobRequest;
				DataEntryAllowed = true;
				JobRequestListEnabled = false;
				SelectedJobRequest = temp;
			}
		}

		private void DeleteButton()
		{
			DialogResult confirmation = MessageBox.Show("Are you sure you want to delete this job request? All assignments for this job request will also be deleted.", "Confirm Delete", MessageBoxButtons.YesNo);
			if (confirmation == DialogResult.Yes)
			{
				try
				{
					DAL.DeleteFullJobRequest(SelectedJobRequest);
					SelectedJobRequest = null;
					UpdateJobRequestList();
					ResetDataEntry();
				}
				catch (MySqlException)
				{
					MessageBox.Show("Failed to delete job request.", "Failed to delete", MessageBoxButtons.OK);
				}
			}
		}

		private void SaveButton()
		{
			string validateMessage = ValidateData();
			// Add and remove assignments
			if (validateMessage == null)
			{
				try
				{
					// Get list of assigned from database
					AssignableContractorList dbAssigned = DAL.GetAssignedContractors(SelectedJobRequest);
					// Compare list of selected against database, if it is selected but does not exist in database, add assignment
					foreach (AssignableContractor listContractor in AssignableContractorList)
					{
						if (listContractor.Assigned == false) continue;


						bool found = false;
						foreach (AssignableContractor dbContractor in dbAssigned)
						{
							if (listContractor.ContractorID == dbContractor.ContractorID)
							{
								found = true;
								break;
							}
						}
						if (!found)
						{
							DAL.InsertContractorAssignment(SelectedJobRequest, listContractor);
						}
					}

					// Compare list of database against selected, if in database but not in selected, delete assignment
					foreach (AssignableContractor dbContractor in dbAssigned)
					{
						bool found = false;
						foreach (AssignableContractor listContractor in AssignableContractorList)
						{
							if (dbContractor.ContractorID == listContractor.ContractorID
								&& listContractor.Assigned == true)
							{
								found = true;
								break;
							}
						}
						if (!found)
						{
							DAL.DeleteContractorAssignment(SelectedJobRequest, dbContractor);
						}

					}

					// Update Estimated Hours
					if ((int)EstimatedHours != SelectedJobRequest.EstimatedHours)
					{
						DAL.UpdateRequestHours(new JobRequest(
							SelectedJobRequest.JobRequestID,
							SelectedJobRequest.ClientID,
							SelectedJobRequest.CoordinatorID,
							SelectedJobRequest.ClientName,
							SelectedJobRequest.Notes,
							SelectedJobRequest.TimeRequested,
							(int)EstimatedHours,
							SelectedJobRequest.Status,
							SelectedJobRequest.Address,
							SelectedJobRequest.Suburb,
							SelectedJobRequest.Feedback
							));
					}

					// Update status to ready
					if (MarkedAsReady == true && SelectedJobRequest.Status == 0)
					{
						DAL.MarkRequestReady(SelectedJobRequest);
					}
					// Rollback status to unready
					if (MarkedAsReady == false && SelectedJobRequest.Status == 1)
					{
						DAL.UnmarkRequestReady(SelectedJobRequest);
					}
					new EventLogger().Log("Updated Job Request in database");
				}
				catch (MySqlException)
				{

					MessageBox.Show("Failed to save Job Request", "Saving Failed", MessageBoxButtons.OK);
				}

				SelectedJobRequest = null;
				ResetDataEntry();
				UpdateJobRequestList();
				DataEntryAllowed = false;
				JobRequestListEnabled = true;
			}
			else
			{
				System.Windows.Forms.MessageBox.Show(validateMessage, "Saving Failed", MessageBoxButtons.OK);
			}
		}

		private void CancelButton()
		{
			SelectedJobRequest = null;
			ResetDataEntry();
			UpdateJobRequestList();
			DataEntryAllowed = false;
			JobRequestListEnabled = true;
		}



        private void AddSkill()
        {
            if (NonRequiredSelectedSkill != null && SelectedJobRequest != null)
            {
				DAL.AddRequiredSkill(SelectedJobRequest, NonRequiredSelectedSkill);
				LoadSelectedJobRequest();
			}
        }

        private void RemoveSkill()
        {
            if (JobRequestSelectedSkill != null && SelectedJobRequest != null)
            {
				DAL.DeleteRequiredSkill(SelectedJobRequest, JobRequestSelectedSkill);
				LoadSelectedJobRequest();
			}
        }

        private void EditSkill()
        {
			SkillEdit skillsWindow = new SkillEdit();
			skillsWindow.ShowDialog();
			LoadSelectedJobRequest();
        }


		private void LoadSelectedJobRequest()
		{
			AssignableContractorList = DAL.GetAssignedContractors(SelectedJobRequest);
			foreach (AssignableContractor contractor in DAL.GetAssignableContractors(SelectedJobRequest))
			{
				AssignableContractorList.Add(contractor);
			}
			JobRequestSkillList = DAL.GetRequiredSkills(SelectedJobRequest);
            NonRequiredSkillList = DAL.GetNonRequiredSkills(SelectedJobRequest);
			EstimatedHours = SelectedJobRequest.EstimatedHours;
			if (SelectedJobRequest.Status == 0)
			{
				MarkedAsReady = false;
			}
			else
			{
				MarkedAsReady = true;
			}
		}



		private void ResetDataEntry()
		{
            AssignableContractorList = null;
            JobRequestSkillList = null;
            NonRequiredSkillList = null;
			EstimatedHours = 0;
			MarkedAsReady = false;
		}



		/// <summary>
		/// Validates all data entered
		/// </summary>
		/// <returns>Null if all data was successfully validated, otherrwise a string describing the problem with validation</returns>
		private string ValidateData()
		{
			if ((int)EstimatedHours < 0 || EstimatedHours == null)
			{
				return "Please enter a non-negative number for Estimated Hours";
			}
			return null;
		}
	}
}
