using System.Linq;
using System.Windows.Input;
using ProjetCaveVin.Helpers;
using ProjetCaveVin.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using ProjetCaveVin.Model.Classes;


namespace ProjetCaveVin.ViewModel
{
    public class LoginAdminViewModel : BaseViewModel
    {
        public ICommand OpenLoginCommand { get; }
        public ICommand OpenRegisterCommand { get; }

        public LoginAdminViewModel()
        {
            OpenLoginCommand = new RelayCommand(OpenLogin);
            OpenRegisterCommand = new RelayCommand(OpenRegister);
        }

        // Note :  a utiliser plutard (fonction non reusssi pour l'instant) -> pour l'instant c'est dans le behind du LoginAdminWindow.xaml.cs
        private void OpenLogin()
        {
            var loginWindow = new LoginWindow("Serveur"); // role non important ici
            System.Windows.Application.Current.MainWindow = loginWindow;
            loginWindow.Show();
            CloseCurrentWindow();
        }

        private void OpenRegister()
        {
            var inscriptionWindow = new InscriptionWindow(null);
            inscriptionWindow.Show();
            CloseCurrentWindow();
        }

        private void CloseCurrentWindow()
        {
            var current = System.Windows.Application.Current.Windows
                .OfType<System.Windows.Window>()
                .SingleOrDefault(w => w.IsActive);
            current?.Close();
        }
    }
}
