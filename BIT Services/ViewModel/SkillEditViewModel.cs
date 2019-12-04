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
	class SkillEditViewModel : NotificationClass
	{
		// Variables
		private string _filterString;
		private ICollectionView _skillListView;

		private Skill _selectedSkill;
		private SkillList _skillList;

		private string _skillName;
		private string _skillDescription;







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



		public Skill SelectedSkill
		{
			get { return _selectedSkill; }
			set
			{
				_selectedSkill = value;
				OnPropertyChanged("SelectedSkill");
				if (value != null)
				{
					LoadSelectedSkill();
				}
			}
		}

		public SkillList SkillList
		{
			get { return _skillList; }
			set
			{
				_skillList = value;
				OnPropertyChanged("SkillList");
			}
		}

		public string SkillName
		{
			get { return _skillName; }
			set
			{
				_skillName = value;
				OnPropertyChanged("SkillName");
			}
		}

		public string SkillDescription
		{
			get { return _skillDescription; }
			set
			{
				_skillDescription = value;
				OnPropertyChanged("SkillDescription");
			}
		}







		// Command Bindings
		public RelayCommand FilterCollection{ get { return new RelayCommand(FilterSkillList, true); } }
		public RelayCommand DeleteButtonCommand{ get { return new RelayCommand(DeleteButton, true); } }
		public RelayCommand SaveButtonCommand{ get { return new RelayCommand(SaveButton, true); } }
		public RelayCommand CancelButtonCommand{ get { return new RelayCommand(CancelButton, true); } }
		public Action CloseAction;





		// Constructor
		public SkillEditViewModel()
		{
			LoadSkillList();
			this._skillListView = CollectionViewSource.GetDefaultView(_skillList);
		}





		//Methods

		private bool SkillFilter(object item)
		{
			Skill skill = item as Skill;
			if (FilterString == null || FilterString == "")
			{
				return true;
			}
			else
			{
				return skill.SkillName.Contains(FilterString);
			}
		}

		private void FilterSkillList()
		{
			this._skillListView.Filter = SkillFilter;
		}



		private void DeleteButton()
		{
			DialogResult confirmation = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete this skill? This will likely fail unless this skill is completely unused.", "Confirm Delete", MessageBoxButtons.YesNo);
			if (confirmation == DialogResult.Yes)
			{
				try
				{
					DAL.DeleteSkill(SelectedSkill);
				}
				catch (MySqlException)
				{
					MessageBox.Show("Failed to delete skill, ensure it is not used anywhere.", "Failed to delete", MessageBoxButtons.OK);
				}

				
				ResetDataEntry();
				LoadSkillList();
			}
		}

		private void SaveButton()
		{
			string validateMessage = ValidateData();
			if (validateMessage == null)
			{
				try
				{
					Skill skill = new Skill(SkillName, SkillDescription);

					DAL.InsertUpdateSkill(skill);
					new EventLogger().Log("Inserted Skill in database");
				}
				catch (MySqlException)
				{

					MessageBox.Show("Failed to save skill", "Saving Failed", MessageBoxButtons.OK);
				}
				ResetDataEntry();
				LoadSkillList();
			}
			else
			{
				System.Windows.Forms.MessageBox.Show(validateMessage, "Saving Failed", MessageBoxButtons.OK);
			}
		}

		private void CancelButton()
		{
			CloseAction();
		}



		private void LoadSkillList()
		{
			SkillList = DAL.GetSkills();
		}



		private void LoadSelectedSkill()
		{
			SkillName = SelectedSkill.SkillName;
			SkillDescription = SelectedSkill.SkillDescription;
		}



		private void ResetDataEntry()
		{
			SkillName = "";
			SkillDescription = "";
		}

		

		private string ValidateData()
		{	
			if (SkillName.Length == 0 || SkillName == null)
			{
				return "Please enter a skill name";
			}
			if (SkillDescription.Length == 0 || SkillDescription == null)
			{
				return "Please enter a skill description";
			}
			return null;
		}
	}
}
