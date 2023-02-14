using Bingou.Database;
using Bingou.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bingou.Components.ValidarCartelas
{
    internal class ValidarCartelasViewModel : BaseViewModel
    {
        private int quantidadeValidados;
        public int QuantidadeValidados
        {
            get { return quantidadeValidados; }
            set
            {
                quantidadeValidados = value;
                OnPropertyChanged();
            }
        }

        private string numeroParaValidar;
        public string NumeroParaValidar
        {
            get { return numeroParaValidar; }
            set
            {
                numeroParaValidar = value;
                OnPropertyChanged();
            }
        }

        private DataGrid dataGridValidados = new DataGrid();
        public DataGrid DataGridValidados
        {
            get { return dataGridValidados; }
            set
            {
                dataGridValidados = value;
                OnPropertyChanged();
            }
        }

        public ValidarCartelasViewModel()
        {
            ValidarCommand = new ValidarCommand(this);
            LimparCommand = new LimparCommand(this);

            DBConnect db = new DBConnect();
            DataGridValidados.ItemsSource = db.CarregarTabelaValidacoes().DefaultView;
        }

        public ICommand ValidarCommand { get; }
        public ICommand LimparCommand { get; }
    }
}
