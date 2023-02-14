using Bingou.Commands;
using Bingou.Database;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bingou.Components.ValidarCartelas
{
    internal class LimparCommand : CommandBase
    {
        private readonly ValidarCartelasViewModel viewModel;
        private DBConnect db = new DBConnect();

        public LimparCommand(ValidarCartelasViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Quer mesmo excluir todos os números validados e iniciar um novo processo?", "Exclusão de cartela", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                db.ExcluirTodasValidacoes();

                viewModel.DataGridValidados.ItemsSource = db.CarregarTabelaValidacoes().DefaultView;
            }
        }
    }
}
