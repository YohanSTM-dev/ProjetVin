using System.Windows.Input;
using ProjetCaveVin.Helpers;
using ProjetCaveVin.View;

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

        private void OpenLogin()
        {
            var loginWindow = new LoginWindow();
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
