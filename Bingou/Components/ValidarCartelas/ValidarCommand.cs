using Bingou.Commands;
using Bingou.Components.EmitirCartelas;
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
        private DBConnect db = new DBConnect();

        public ValidarCommand(ValidarCartelasViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            string numeroParaValidar = viewModel.NumeroParaValidar.ToString();
            db.CriarTabelaValidacoes();

            if(viewModel.NumeroParaValidar == null || viewModel.NumeroParaValidar.Equals(0))
            {
                MessageBox.Show("Digite um número para validar.");
                return;
            }

            if (numeroParaValidar.Contains(" "))
            {
                MessageBox.Show("Remova os espaços.");
                return;
            }

            if (numeroParaValidar.Contains("-"))
            {
                try
                {
                    InserirIntervaloDeNumeros(numeroParaValidar);
                } catch (Exception ex)
                {

                }
            }
            else
            {
                InserirNumero();
            }

            viewModel.QuantidadeValidados = Convert.ToInt32(db.SelecionarQuantidadeValidados());
            viewModel.NumeroParaValidar = null;
            viewModel.DataGridValidados.ItemsSource = db.CarregarTabelaValidacoes().DefaultView;
        }

        private void InserirIntervaloDeNumeros(string numeroParaValidar)
        {
            int numeroMin;
            int numeroMax;

            String[] separador = { "-" };
            String[] numeroParaValidarSeparado = numeroParaValidar.Split(separador, 2, StringSplitOptions.RemoveEmptyEntries);

            numeroMin = Convert.ToInt32(numeroParaValidarSeparado[0]);
            numeroMax = Convert.ToInt32(numeroParaValidarSeparado[1]);

            using (SQLiteTransaction sqlTransaction = db.Conn.BeginTransaction())
            {
                using (SQLiteCommand inserirCmd = new SQLiteCommand(db.Conn))
                {
                    SQLiteParameter numeros = new SQLiteParameter();

                    inserirCmd.CommandText = "INSERT OR FAIL INTO Validacoes (id) VALUES (?)";
                    inserirCmd.Parameters.Add(numeros);

                    for (int i = numeroMin; i <= numeroMax; i++)
                    {
                        try
                        {
                            numeros.Value = i;
                            inserirCmd.ExecuteNonQuery();
                        } catch (Exception ex)
                        {

                        }
                        
                    }
                    sqlTransaction.Commit();
                }
            }
        }

        private void InserirNumero()
        {

            try
            {
                db.InserirValidacao(Convert.ToInt32(viewModel.NumeroParaValidar));
            }
            catch (Exception ex)
            {
                if (ex is SQLiteException)
                {
                    MessageBoxResult result = MessageBox.Show("Cartela já validada. Deseja excluidar da validação?", "Exclusão de cartela", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        db.ExcluirValidacao(Convert.ToInt32(viewModel.NumeroParaValidar));
                    }
                }
                else
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
