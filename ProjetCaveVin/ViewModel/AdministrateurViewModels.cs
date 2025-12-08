using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetCaveVin.View;
//using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Helpers;
using System.Windows.Input;



namespace ProjetCaveVin.ViewModel
{
	public class AdministrateurViewModels : BaseViewModel
    {

        public ICommand ShowGestionBouteillesCommand {  get;  }
        public ICommand ShowStatistiquesGrandMonarqueCommand { get; }

      


		public void showStatistiquesGrandMonarque()
		{
			var statistiquesGrandMonarqueWindow = new StatistiquesGrandMonarqueWindow();
            System.Windows.Application.Current.MainWindow = statistiquesGrandMonarqueWindow;
            statistiquesGrandMonarqueWindow.Show();
            
        }
    }

	
}
