using System.Windows;
using System.Windows.Controls;
//using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.ViewModel;

using ProjetCaveVin.View;

namespace ProjetCaveVin.View
{
    public partial class RoleAccessWindow : Window
    {
        private string _selectedRole;

        public RoleAccessWindow()
        {
            InitializeComponent();
            DataContext = new RoleAccessViewModel();
        }

    }
}
