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
    /// Interaction logic for SuburbEdit.xaml
    /// </summary>
    public partial class SuburbEdit : Window
    {
        public SuburbEdit()
        {
            InitializeComponent();
			SuburbEditViewModel vm = new SuburbEditViewModel();
			this.DataContext = vm;
			if (vm.CloseAction == null)
			{
				vm.CloseAction = this.Close;
			}
        }
    }
}
