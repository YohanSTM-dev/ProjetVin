using ProjetCaveVin.Helpers;
using System.Windows.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetCaveVin.Model.Tables;
using System.Windows.Controls;
using ProjetCaveVin.View;


namespace ProjetCaveVin.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {


        private string _email;
        private string _password;
        private string _message;

        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            var user = Utilisateur.GetByCredentials(Email, Password);
            if (user != null)
            {
                Message = $"Connexion réussie. Role : {user.Role}";
            }
            else
            {
                Message = "Email ou mot de passe incorrect";
            }
        }

        private void ChangementFenetre()
        {
            Utilisateur user = Utilisateur.GetByCredentials(Email, Password);
            if (user != null)
            {
                if (user.Role == "serveur")
                {
                    ServeurViewModel serveurVM = new ServeurViewModel();
                    MainWindow mainWindow = new MainWindow(serveurVM);
                    mainWindow.Show();
                    App.Current.MainWindow.Close();

                }

                if (user.Role == "Administrateur")
                {
                    AdministrateurViewModel adminstrateurVM = new AdministrateurViewModel();
                    MainWindow mainWindow = new MainWindow(adminstrateurVM);
                    mainWindow.Show();
                    App.Current.MainWindow.Close();

                }

                else if (user.Role == "Sommelier")
                {
                    SommelierViewModel sommelierVM = new SommelierViewModel();
                    MainWindow mainWindow = new MainWindow(sommelierVM);
                    mainWindow.Show();
                    App.Current.MainWindow.Close();

                }

            }

            else
            {
                {
                    Console.WriteLine("Aucun role a affilé a cette utilisateur ");
                }
            }
        }
    }
}
