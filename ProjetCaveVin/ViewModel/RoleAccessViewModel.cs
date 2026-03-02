using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Repositories;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProjetCaveVin.ViewModel
{
    public class RoleAccessViewModel : BaseViewModel
    {
        private string _selectedRoleName;
        public string SelectedRoleName
        {
            get => _selectedRoleName;
            set { _selectedRoleName = value; OnPropertyChanged(); }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        public ICommand SelectRoleCommand { get; }
        public ICommand ValidateRoleCommand { get; }

        public RoleAccessViewModel()
        {
            // recoit text
            SelectRoleCommand = new RelayCommand<string>(ExecuteSelectRole);
            ValidateRoleCommand = new RelayCommand<object>(ExecuteValidateRole);
        }

        private void ExecuteSelectRole(string roleName)
        {
            //
            SelectedRoleName = roleName;
            Message = "";
        }

        private void ExecuteValidateRole(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            string password = passwordBox?.Password;

            // Vérification sur la variable STRING
            if (string.IsNullOrEmpty(SelectedRoleName))
            {
                Message = "Veuillez d'abord sélectionner un rôle (Cliquez sur un bouton).";
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                Message = "Veuillez entrer le mot de passe.";
                return;
            }

            // Vérification du mot de passe en base
            if (RARepository.CheckPassword(SelectedRoleName, password))
            {
                Window targetWindow;
                if (SelectedRoleName == "Administrateur")
                    targetWindow = new ProjetCaveVin.View.LoginAdminWindow();
                else
                    targetWindow = new ProjetCaveVin.View.LoginWindow(SelectedRoleName);

                targetWindow.Show();

                foreach (Window win in Application.Current.Windows)
                {
                    if (win is ProjetCaveVin.View.RoleAccessWindow)
                    {
                        win.Close();
                        break;
                    }
                }
            }
            else
            {
                Message = "Mot de passe incorrect.";
                if (passwordBox != null) passwordBox.Password = "";
            }
        }
    }
}