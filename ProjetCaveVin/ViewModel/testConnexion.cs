using ProjetCaveVin.Model.Connexion;
using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Tables;
using System;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;



namespace ProjetCaveVin.ViewModel
{
    public class testConnexion : BaseViewModel
    {
        private string _resultat;
        private string _resultatUtilisateur;

        public string ResultatUtilisateur
        {
            get { return _resultatUtilisateur; }
            set { _resultatUtilisateur = value; OnPropertyChanged(); }
        }
        public string Resultat
        {
            get { return _resultat; }
            set { _resultat = value; OnPropertyChanged(); }
        }

        public ICommand TestConnexionCommand { get; }

        //tes 
        public ICommand TestConexionUtilisateur { get; }

        public testConnexion()
        {
            TestConnexionCommand = new RelayCommand(TestConnexion);
            TestConexionUtilisateur = new RelayCommand(TestUtilisateur);
        }

        // test 
        private void TestConnexion()
        {
            try
            {
                string connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=restaurant;User Id=yohan;Password=1234;Encrypt=False;";

                var db = new DatabaseConnexion(connectionString);
                //var req = "select * from Utilisateur";
                db.Open();

                if (db.IsOpenConnected())
                {
                    Resultat = "Connexion SQL Server OK !";
                    //Console.WriteLine(db.Execute(GetAllUtilisateur()));
                    //Console.WriteLine(db.Execute(req));
                }


                else
                    Resultat = "Erreur de connexion à la base !";

                db.Close(); 
            }
            catch (Exception ex)
            {
                Resultat = "Erreur : " + ex.Message;
            }
        }

        private void TestUtilisateur()
        {
            try
            {
                var utilisateurs = Utilisateur.GetAllUtilisateur();

                ResultatUtilisateur = ""; 
                foreach (var user in utilisateurs)
                {
                    ResultatUtilisateur +=
                        $"ID: {user.id_utilisateur}, Name: {user.Nom}, Lastname: {user.Prenom}, Email: {user.Email}, Role: {user.Role}\n";
                }

                if (utilisateurs.Count == 0)
                    ResultatUtilisateur = "Aucun utilisateur trouvé.";
            }
            catch (Exception ex)
            {
                ResultatUtilisateur = "Erreur : " + ex.Message;
            }

        }

        }
}
