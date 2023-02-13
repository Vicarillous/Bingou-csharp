using Bingou.Commands;
using Bingou.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bingou.Components.ValidarCartelas
{
    internal class ValidarCommand : CommandBase
    {
        private readonly ValidarCartelasViewModel viewModel;

        public ValidarCommand(ValidarCartelasViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            using (SQLiteConnection conn = DBConnect.CreateConnection())
            {
                CriarTabelaValidacao(conn);

                InserirNumero(conn);

                CarregarTabelaValidacao(conn);
            }
        }

        private void CarregarTabelaValidacao(SQLiteConnection conn)
        {
            Trace.WriteLine("aa");
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Validacoes";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable("Validacoes");
                adapter.Fill(dt);
                viewModel.DataGridValidados.ItemsSource = dt.DefaultView;
            }
        }

        private void InserirNumero(SQLiteConnection conn)
        {
            using (SQLiteCommand inserirValidacaoCmd = conn.CreateCommand())
            {
                try
                {
                    inserirValidacaoCmd.CommandText = string.Format("INSERT OR FAIL INTO Validacoes (id) VALUES ({0})", viewModel.NumeroParaValidar);
                    inserirValidacaoCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    if (ex is SQLiteException)
                    {
                        ExcluirNumero(conn);
                    }
                    else
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void ExcluirNumero(SQLiteConnection conn)
        {
            MessageBoxResult result = MessageBox.Show("Cartela já validada. Deseja excluidar da validação?", "Exclusão de cartela", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Validacoes.ExcluirNumero(viewModel.NumeroParaValidar);
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM Validacoes WHERE id = {0}", viewModel.NumeroParaValidar);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CriarTabelaValidacao(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS Validacoes (id INTEGER PRIMARY KEY NOT NULL, FOREIGN KEY(id) REFERENCES Cartelas(id));";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
        }
    }
}
