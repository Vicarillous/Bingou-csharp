using Bingou.Database;
using Bingou.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bingou.Components.EmitirCartelas
{
    internal class EmitirCartelasViewModel : BaseViewModel
    {
        private int offset = 1;
        public int Offset
        {
            get { return offset; }
            set
            {
                offset = value;
                OnPropertyChanged();
            }
        }

        private int quantidade;
        public int Quantidade
        {
            get { return quantidade; }
            set
            {
                quantidade = value;
                OnPropertyChanged();
            }
        }

        public EmitirCartelasViewModel()
        {
            GerarCartelasCommand = new GerarCartelasCommand(this);
            VerificarCartelasRepetidasCommand = new VerificarCartelasRepetidasCommand(this);

            DBConnect db = new DBConnect();
            db.CriarTabelaCartelas();
        }

        public ICommand GerarCartelasCommand { get; }
        public ICommand VerificarCartelasRepetidasCommand { get; }
    }
}
