using BIT_Services.Commands;
using BIT_Services.Model;
using BIT_Services.View;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BIT_Services.ViewModel
{
	class LoginViewModel : NotificationClass
	{
		// <!------------ VARIABLES ------------!>
		private string _outputText;
		private string _username;
		private SecureString _securePassword;
        private User _currentUser;
		/// <summary>
		/// Contains information about current user, obtained after logging in.
		/// </summary>
        private User CurrentUser { get; }
		


		// <!------------ BINDINGS ------------!>

		/// <summary>
		/// Bound to output text box
		/// </summary>
		public string OutputText
		{
			get { return _outputText; }
			set
			{
				_outputText = value;
				OnPropertyChanged("OutputText");
			}
		}
		/// <summary>
		/// Bound to username textbox
		/// </summary>
		public string Username
		{
			get { return _username; }
			set
			{
				_username = value;
				OnPropertyChanged("Username");
			}
		}
		/// <summary>
		/// Bound to passwordbox
		/// </summary>
		public SecureString SecurePassword { get => _securePassword; set =>  _securePassword = value; }

		/// <summary>
		/// Contains method from view to close the view.
		/// </summary>
		public Action CloseAction { get; set; }



		// <!------------ CONSTRUCTORS ------------!>

		/// <summary>
		/// Constructor
		/// </summary>
		public LoginViewModel()
		{

		}



		// <!------------ METHODS ------------!>

		/// <summary>
		/// Links button to TestUsernamePassword()
		/// </summary>
		public RelayCommand Login { get { return new RelayCommand(TestUsernamePassword, true); } }

		/// <summary>
		/// Checks database to get user account type and user id. Then opens the appropriate window and closes the view.
		/// </summary>
		private void TestUsernamePassword()
		{
			try
			{
				_currentUser = DAL.CheckLogin(Username, SecurePassword);


				if (_currentUser != null)
				{
					if (_currentUser.UserType == "Coordinator")
					{
						CoordinatorMainWindow mainWindow = new CoordinatorMainWindow();
						mainWindow.Show();
						CloseAction();

					}
					else if (_currentUser.UserType == "Admin")
					{
						AdminMainWindow mainWindow = new AdminMainWindow();
						mainWindow.Show();
						CloseAction();
					}

				}
				else
				{
					OutputText = "Incorrect username/password";
				}
			}
			catch (MySqlException)
			{
				OutputText = "Error logging in";
			}
        }
	}
}
