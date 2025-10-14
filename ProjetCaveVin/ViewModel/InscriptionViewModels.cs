using Microsoft.Win32;
using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjetCaveVin.ViewModel
{
    public class InscriptionViewModels : BaseViewModel
    {

        private string _nom;
        private string _prenom;
        private string _email;
        private string _password;
        private string _selectedRole;

        public string Nom { get => _nom; set { _nom = value; OnPropertyChanged(); } }
        public string Prenom { get => _prenom; set { _prenom = value; OnPropertyChanged(); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public string SelectedRole { get => _selectedRole; set { _selectedRole = value; OnPropertyChanged(); } }

        public List<string> Roles { get; set; }

        private string _message;
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }

        public ICommand RegisterCommand { get; }

        private Utilisateur _currentUser;

        private List<string> GetAvailableRolesForCurrentUser(Utilisateur user)
        {
            if (user == null) // si inscription publique
                return new List<string>();

            switch (user.Role.Nom)
            {
                case "Administrateur":
                    return new List<string> { "Administrateur", "Sommelier", "Serveur" };
                case "Sommelier":
                    return new List<string> { "Serveur" };
                default:
                    return new List<string>();
            }
        }

        public InscriptionViewModels(Utilisateur currentUser)
        {
            _currentUser = currentUser;
            Roles = GetAvailableRolesForCurrentUser(currentUser);
            RegisterCommand = new RelayCommand(Register);
        }

        //private List<string> GetAvailableRolesForCurrentUser(Utilisateur user)
        //{
        //    if (user == null) 
        //        return new List<string> { "Serveur" }; 

        //}



        private void Register()
        {
            if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Prenom) ||
                string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(SelectedRole))
            {
                Message = "Tous les champs sont obligatoires.";
                return;
            }

            try
            {
                var selectedRoleObject = Role.GetByName(SelectedRole);
                if (selectedRoleObject == null)
                {
                    Message = "Rôle sélectionné invalide.";
                    return;
                }

                var newUser = new Utilisateur
                {
                    Nom = Nom,
                    Prenom = Prenom,
                    Email = Email,
                    PasswordHash = Password,
                    Role = selectedRoleObject
                };

                Utilisateur.Add(newUser);
                Message = "Utilisateur créé avec succès !";
            }
            catch (Exception ex)
            {
                Message = "Erreur : " + ex.Message;
            }
        }




    }
}
