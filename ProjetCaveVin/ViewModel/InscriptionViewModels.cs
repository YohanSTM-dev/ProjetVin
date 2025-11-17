using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.Helpers;
using System;
using System.Windows.Input;
using ProjetCaveVin.Model.Classes;


namespace ProjetCaveVin.ViewModel
{
    public class InscriptionViewModel : BaseViewModel
    {
        private string _nom;
        private string _prenom;
        private string _email;
        private string _password;
        private string _message;
        private string _role;
        public List<string> Roles { get; } = new List<string> { "Administrateur", "Sommelier", "Serveur" };


        public string Nom
        {
            get => _nom;
            set { _nom = value; OnPropertyChanged(); }
        }

        public string Prenom
        {
            get => _prenom;
            set { _prenom = value; OnPropertyChanged(); }
        }

        public string Email
        {

            get => _email;
            set { _email = value; OnPropertyChanged(); }
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

        public string Role
        {
            get => _role;
            set { _role = value; OnPropertyChanged(); }
        }

        public ICommand InscriptionCommand { get; }

        public InscriptionViewModel(string role)
        {
            Role = role; 
            InscriptionCommand = new RelayCommand(InscrireUtilisateur);
        }

        private void InscrireUtilisateur()
        {
            if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Prenom) ||
                string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                Message = "Veuillez remplir tous les champs.";
                return;
            }

            try
            {
                Utilisateur.InsertUtilisateur(Nom, Prenom, Email, Password, Role);
                Message = "Utilisateur créé avec succès !";

                // Reset champs
                Nom = Prenom = Email = Password = string.Empty;
            }
            catch (Exception ex)
            {
                Message = $"Erreur lors de l'insertion : {ex.Message}";
            }
        }
    }
}
