using Bingou.Commands;
using Bingou.Database;
using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows;

namespace Bingou.Components.EmitirCartelas
{
    internal class VerificarCartelasRepetidasCommand : CommandBase
    {
        private readonly EmitirCartelasViewModel emitirCartelasViewModel;
        private readonly DBConnect db = new DBConnect();

        public VerificarCartelasRepetidasCommand(EmitirCartelasViewModel emitirCartelasViewModel)
        {
            this.emitirCartelasViewModel = emitirCartelasViewModel;
        }
        public override void Execute(object parameter)
        {
            int qtdRepetidos = Convert.ToInt32(db.VerificarCartelasRepetidas());
            Trace.WriteLine(qtdRepetidos.ToString());
            MessageBox.Show(string.Format("Encontrado {0} números repetidos.", qtdRepetidos), "", MessageBoxButton.OK, MessageBoxImage.Information);

            if (emitirCartelasViewModel.Quantidade > Convert.ToInt32(Properties.Settings.Default.maxValidado))
            {
                Properties.Settings.Default.maxValidado = emitirCartelasViewModel.Quantidade.ToString();
                Properties.Settings.Default.Save();
            }
        }
    }
}
