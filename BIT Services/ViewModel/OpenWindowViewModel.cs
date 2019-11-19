using BIT_Services.Commands;
using BIT_Services.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.ViewModel
{
	class OpenWindowViewModel : NotificationClass
	{
		public RelayCommand OpenClient { get { return new RelayCommand(OpenClientWindow, true); } }
		public RelayCommand OpenContractor { get { return new RelayCommand(OpenContractorWindow, true); } }
		public RelayCommand OpenRequest { get { return new RelayCommand(OpenRequestWindow, true); } }
		public RelayCommand OpenStaff { get { return new RelayCommand(OpenStaffWindow, true); } }



		private void OpenClientWindow()
		{
			new ClientCRUD().Show();
		}

		private void OpenContractorWindow()
		{
			new ContractorCRUD().Show();
		}

		private void OpenRequestWindow()
		{
			new JobRequests().Show();
		}

		private void OpenStaffWindow()
		{
			new StaffCRUD().Show();
		}
	}
}
