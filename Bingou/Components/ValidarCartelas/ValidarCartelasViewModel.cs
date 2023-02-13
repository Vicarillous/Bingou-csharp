using Bingou.ViewModel;
using System;
using System.Collections.Generic;
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
        private int numeroParaValidar;
        public int NumeroParaValidar
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
        }

        public ICommand ValidarCommand { get; }
    }
}
