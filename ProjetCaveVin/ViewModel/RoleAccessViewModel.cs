using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Tables;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ProjetCaveVin.ViewModel;
using ProjetCaveVin.View;

namespace ProjetCaveVin.ViewModel
{
    public class RoleAccessViewModel : BaseViewModel
    {
        private RoleAccess _selectedRole;
        private string _password;
        private string _message;

        public ObservableCollection<RoleAccess> Roles { get; set; }

        public RoleAccess SelectedRole
        {
            get => _selectedRole;
            set { _selectedRole = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        public ICommand ValidateRoleCommand { get; }

        public RoleAccessViewModel()
        {
            Roles = new ObservableCollection<RoleAccess>(RoleAccess.GetAllRoles());
            ValidateRoleCommand = new RelayCommand(ValidateRole);
        }

        private void ValidateRole()
        {
            if (SelectedRole == null || string.IsNullOrEmpty(Password))
            {
                Message = "Veuillez sélectionner un rôle et entrer le mot de passe.";
                return;
            }

            if (RoleAccess.CheckPassword(SelectedRole.RoleName, Password))
            {
                if (SelectedRole.RoleName == "Administrateur")
                {
                    var loginAdminWindow = new LoginAdminWindow();
                    loginAdminWindow.Show();
                }
                else
                {
                    var loginWindow = new LoginWindow("Sommelier"); // role par défaut pour Sommelier/Serveur
                    loginWindow.Show();
                }

                Application.Current.Windows[0]?.Close(); // Ferme la fenêtre actuelle
            }
            else
            {
                Message = "Mot de passe incorrect";
            }
        }
    }
}
