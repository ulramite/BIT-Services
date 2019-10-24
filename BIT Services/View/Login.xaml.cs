using BIT_Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BIT_Services.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
			this.DataContext = new LoginViewModel(new Action(this.Close));
        }

		// Required for security reasons
		private void PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (this.DataContext != null)
			{ ((dynamic)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword; }
		}
	}
}
