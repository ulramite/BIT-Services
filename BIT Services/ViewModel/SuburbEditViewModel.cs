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
    class SuburbEditViewModel : NotificationClass
    {
		// Variables
		private string _filterString;
		private ICollectionView _suburbListView;

		private Suburb _selectedSuburb;
		private SuburbList _suburbList;

		private string _suburbName;







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



		public string SuburbName
		{
			get { return _suburbName; }

			set
			{
				_suburbName = value;
				OnPropertyChanged("SuburbName");
			}
		}



		public Suburb SelectedSuburb
		{
			get { return _selectedSuburb; }

			set
			{
				_selectedSuburb = value;
				OnPropertyChanged("SelectedSuburb");
				if (SelectedSuburb != null)
				{
					LoadSelectedSuburb();
				}
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





		// Command Bindings
		public RelayCommand FilterCollection { get { return new RelayCommand(FilterSuburbList, true); } }
		public RelayCommand DeleteButtonCommand { get { return new RelayCommand(DeleteButton, true); } }
		public RelayCommand SaveButtonCommand { get { return new RelayCommand(SaveButton, true); } }
		public RelayCommand CancelButtonCommand { get { return new RelayCommand(CancelButton, true); } }
		public Action CloseAction;








		// Constructor
		public SuburbEditViewModel()
		{
			LoadSuburbList();
			this._suburbListView = CollectionViewSource.GetDefaultView(_suburbList);
		}









		// Methods
		private bool SuburbFilter(object item)
		{
			Suburb Suburb = item as Suburb;
			if (FilterString == null || FilterString == "")
			{
				return true;
			}
			else
			{
				return Suburb.SuburbName.Contains(FilterString);
			}
		}

		private void FilterSuburbList()
		{
			this._suburbListView.Filter = SuburbFilter;
		}



		private void DeleteButton()
		{
			DialogResult confirmation = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete this suburb? This will likely fail unless this suburb is completely unused.", "Confirm Delete", MessageBoxButtons.YesNo);
			if (confirmation == DialogResult.Yes)
			{
				try
				{
					DAL.DeleteSuburb(SelectedSuburb);
					ResetDataEntry();
					LoadSuburbList();
				}
				catch (MySqlException)
				{
					MessageBox.Show("Failed to delete suburb, ensure it is not used anywhere.", "Failed to delete", MessageBoxButtons.OK);
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
					Suburb suburb = new Suburb(SuburbName);

					DAL.InsertSuburb(suburb);
					new EventLogger().Log("Inserted Suburb in database");
				}
				catch (MySqlException)
				{

					MessageBox.Show("Failed to save suburb", "Saving Failed", MessageBoxButtons.OK);
				}
				ResetDataEntry();
				LoadSuburbList();
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



		private void LoadSuburbList()
		{
			SuburbList = DAL.GetSuburbs();
		}



		private void LoadSelectedSuburb()
		{
			SuburbName = SelectedSuburb.SuburbName;
		}



		private void ResetDataEntry()
		{
			SuburbName = "";
		}



		private string ValidateData()
		{
			if (SuburbName.Length == 0 || SuburbName == null)
			{
				return "Please enter a suburb Name";
			}
			if (SuburbList.Contains(new Suburb(SuburbName)))
			{
				return "Selected suburb already exists";
			}
			return null;
		}
	}
}
