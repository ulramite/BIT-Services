using BIT_Services.Commands;
using BIT_Services.Model;
using BIT_Services.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.ViewModel
{
	class LoginViewModel : NotificationClass
	{
		private string outputText;
		private string username;
		private SecureString securePassword;
        private User currentUser;

		
		/// <summary>
		/// Should be bound to output text box
		/// </summary>
		public string OutputText
		{
			get { return outputText; }
			set
			{
				outputText = value;
				OnPropertyChanged("OutputText");
			}
		}
		/// <summary>
		/// Bound to username textbox
		/// </summary>
		public string Username
		{
			get { return username; }
			set
			{
				username = value;
				OnPropertyChanged("Username");
			}
		}
		/// <summary>
		/// Bound to passwordbox
		/// </summary>
		public SecureString SecurePassword { get => securePassword; set =>  securePassword = value; }
		/// <summary>
		/// Contains user information
		/// </summary>
        public User CurrentUser { get; }
		/// <summary>
		/// Contains action provided by view to close the view.
		/// </summary>
		private Action CloseView { get; set; }


		/// <summary>
		/// Constructor takes action to close view.
		/// </summary>
		/// <param name="CloseViewAction">Action that closes view. Should be "this.close()"</param>
		public LoginViewModel(Action CloseViewAction)
		{
			CloseView = CloseViewAction;
		}



		/// <summary>
		/// Links button to TestUsernamePassword()
		/// </summary>
		public RelayCommand Login { get { return new RelayCommand(TestUsernamePassword, true); } }
		/// <summary>
		/// Checks database to get user account type and user id. Then opens the appropriate window and closes the view.
		/// </summary>
		private void TestUsernamePassword()
		{
            currentUser = DAL.CheckLogin(Username, SecurePassword);

            if (currentUser != null)
            {
                if (currentUser.UserType == "Coordinator")
                {
                    CoordinatorMainWindow mainWindow = new CoordinatorMainWindow();
                    mainWindow.Show();
					CloseView();
					
                }
                else if (currentUser.UserType == "Admin")
                {
                    AdminMainWindow mainWindow = new AdminMainWindow();
                    mainWindow.Show();
					CloseView();
                }

            }
            else
            {
                OutputText = "Incorrect username/password";
            }

        }


		
	}
}
