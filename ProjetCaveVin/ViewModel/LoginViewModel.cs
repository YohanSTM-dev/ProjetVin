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
using System.Windows;


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

        public event Action<Utilisateur> LoginSucceeded;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            var user = Utilisateur.GetByCredentials(Email, Password);
            if (user != null)
            {
                Message = $"Connexion réussie. Role : ({user.Role})";
                LoginSucceeded?.Invoke(user);
            }
            else
            {
                Message = "Email ou mot de passe incorrect";
            }
        }

    }
}
