using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetCaveVin.View;
using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Helpers;
using System.Windows.Input;



namespace ProjetCaveVin.ViewModel
{
	public class AdministrateurViewModels : BaseViewModel
    {

        public ICommand ShowGestionBouteillesCommand {  get;  }
        public ICommand ShowStatistiquesGrandMonarqueCommand { get; }

        public AdministrateurViewModels()
		{
            ShowGestionBouteillesCommand = new RelayCommand(showGestionBouteilles);
            ShowStatistiquesGrandMonarqueCommand = new RelayCommand(showStatistiquesGrandMonarque);

        }


		public void showGestionBouteilles()
		{
            
            var gestionBouteilleWindow = new GestionBouteilleWindow();
            System.Windows.Application.Current.MainWindow = gestionBouteilleWindow;
            gestionBouteilleWindow.Show();
        }

		public void showStatistiquesGrandMonarque()
		{
			var statistiquesGrandMonarqueWindow = new StatistiquesGrandMonarqueWindow();
            System.Windows.Application.Current.MainWindow = statistiquesGrandMonarqueWindow;
            statistiquesGrandMonarqueWindow.Show();
            
        }
    }

	
}
