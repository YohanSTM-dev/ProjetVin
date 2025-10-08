using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model;
using System;
using System.Windows.Input;

namespace ProjetCaveVin.ViewModel
{
    public class testConnexion : BaseViewModel
    {
        private string _resultat;
        public string Resultat
        {
            get { return _resultat; }
            set { _resultat = value; OnPropertyChanged(); }
        }

        public ICommand TestConnexionCommand { get; }

        public testConnexion()
        {
            TestConnexionCommand = new RelayCommand(TestConnexion);
        }

        private void TestConnexion()
        {
            try
            {
                string connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=restaurant;User Id=yohan;Password=1234;Encrypt=False;";

                var db = new DatabaseConnexion(connectionString);
                //var query = "Insert blabla";

                db.Open();

                if (db.IsOpenConnected())
                    Resultat = "Connexion SQL Server OK !";
                    // db.Execute(query);
                else
                    Resultat = "Erreur de connexion à la base !";

                db.Close(); // fermeture manuelle
            }
            catch (Exception ex)
            {
                Resultat = "Erreur : " + ex.Message;
            }
        }


    }
}
