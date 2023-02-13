using Bingou.Commands;
using Bingou.Database;
using System;
using System.Data.SQLite;
using System.Diagnostics;

namespace Bingou.Components.EmitirCartelas
{
    internal class GerarCartelasCommand : CommandBase
    {
        private readonly EmitirCartelasViewModel emitirCartelasViewModel;

        public GerarCartelasCommand(EmitirCartelasViewModel emitirCartelasViewModel)
        {
            this.emitirCartelasViewModel = emitirCartelasViewModel;
        }

        public override void Execute(object parameter)
        {
            Trace.WriteLine("Gerando cartelas...");
            using (SQLiteConnection conn = DBConnect.CreateConnection())
            {
                SQLiteCommand dropCartelasCmd = conn.CreateCommand();
                dropCartelasCmd.CommandText = "DROP TABLE IF EXISTS Cartelas";
                dropCartelasCmd.ExecuteNonQuery();
                DBConnect.CreateTable(conn);

                int lastId = 0;
                try
                {
                    SQLiteCommand lastIdCmd = conn.CreateCommand();
                    lastIdCmd.CommandText = "SELECT seq FROM sqlite_sequence WHERE name='Cartelas'";
                    lastId = Convert.ToInt32(lastIdCmd.ExecuteScalar());
                }
                catch (Exception e)
                {
                    //Falhe silenciosamente
                }

                using (SQLiteTransaction sqlTransaction = conn.BeginTransaction())
                {
                    using (SQLiteCommand inserirCmd = new SQLiteCommand(conn))
                    {
                        SQLiteParameter numeros = new SQLiteParameter();

                        inserirCmd.CommandText = "INSERT INTO Cartelas (numeros) VALUES (?)";
                        inserirCmd.Parameters.Add(numeros);

                        for (int i = 0; i < emitirCartelasViewModel.Quantidade; i++)
                        {
                            Random rd = new Random(lastId + emitirCartelasViewModel.Offset);
                            string numerosGerados = GerarCartela(rd);
                            numeros.Value = numerosGerados;
                            inserirCmd.ExecuteNonQuery();
                            lastId++;
                        }
                        sqlTransaction.Commit();

                    }
                }
            }
        }

        private string GerarCartela(Random rd)
        {
            int[,] numeros = new int[5, 5];
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    bool haNumeroRepetido = false;
                    int max = (x + 1) * 15;
                    int min = max - 14;

                    //verifica se número aleatório será único
                    do
                    {
                        haNumeroRepetido = false;

                        numeros[y, x] = rd.Next(min, max);
                        for (int i = 0; i < y; i++)
                        {
                            if (numeros[i, x] == numeros[y, x])
                            {
                                haNumeroRepetido = true;
                            }
                        }
                    } while (haNumeroRepetido);
                }
            }
            numeros[2, 2] = 0;

            string numerosString = Matriz2DParaString(numeros);

            return numerosString;
            //Cartela cartela = new Cartela(numerosString);
        }

        private string Matriz2DParaString(int[,] array)
        {
            string arrayString = "";
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    arrayString += array[y, x] + ",";
                }
            }
            arrayString = arrayString.Remove(arrayString.Length - 1, 1);
            return arrayString;
        }
    }
}
