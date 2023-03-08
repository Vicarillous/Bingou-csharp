using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Entity;
using System.Windows;
using System.Data;

namespace Bingou.Database
{
    public class DBConnect
    {
        private SQLiteConnection conn;

        public SQLiteConnection Conn
        {
            get { return conn; }
        }

        public DBConnect()
        {
            conn = CreateConnection();
        }
        public SQLiteConnection CreateConnection()
        {
            SQLiteConnection conn;
            // Create a new database connection:
            conn = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Compress = True;");
            // Open the connection:
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return conn;
        }

        public void CriarTabelaCartelas()
        {
            SQLiteCommand cmd;
            cmd = conn.CreateCommand();
            string cmdText = "CREATE TABLE IF NOT EXISTS Cartelas (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, numeros TEXT NOT NULL)";
            cmd.CommandText = cmdText;
            cmd.ExecuteNonQuery();
        }

        #region EmitirCartelas
        public void ExcluirTabelaCartelas()
        {
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DROP TABLE IF EXISTS Cartelas";
            cmd.ExecuteNonQuery();
        }

        public object VerificarCartelasRepetidas()
        {
            SQLiteCommand cmd = conn.CreateCommand();
            string cmdText = "SELECT count(*) FROM Cartelas WHERE numeros IN (SELECT numeros FROM Cartelas GROUP BY numeros HAVING COUNT(*) > 1)";
            cmd.CommandText = cmdText;

            return cmd.ExecuteScalar();
        }
        #endregion

        #region ValidarCartelas
        public void CriarTabelaValidacoes()
        {
            SQLiteCommand cmd = conn.CreateCommand();
            string cmdText = "CREATE TABLE IF NOT EXISTS Validacoes (id INTEGER PRIMARY KEY NOT NULL, FOREIGN KEY(id) REFERENCES Cartelas(id));";
            cmd.CommandText = cmdText;
            cmd.ExecuteNonQuery();
        }

        public void InserirValidacao(int valor)
        {
            SQLiteCommand sqlite_cmd = conn.CreateCommand();
            string cmdText = string.Format("INSERT OR FAIL INTO Validacoes (id) VALUES ({0})", valor);
            sqlite_cmd.CommandText = cmdText;
            sqlite_cmd.ExecuteNonQuery();
        }

        public void ExcluirValidacao(int valor)
        {
            SQLiteCommand cmd = conn.CreateCommand();
            string cmdText = string.Format("DELETE FROM Validacoes WHERE id = {0}", valor);
            cmd.CommandText = cmdText;
            cmd.ExecuteNonQuery();
        }

        public void ExcluirTodasValidacoes()
        {
            SQLiteCommand cmd = conn.CreateCommand();
            string cmdText = "DELETE FROM Validacoes";
            cmd.CommandText = cmdText;
            cmd.ExecuteNonQuery();
        }

        public DataTable CarregarTabelaValidacoes()
        {
            SQLiteCommand cmd = conn.CreateCommand();
            string cmdText = "SELECT * FROM Validacoes";
            cmd.CommandText = cmdText;
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable("Validacoes");
            adapter.Fill(dt);

            return dt;
        }

        public object SelecionarQuantidadeValidados()
        {
            SQLiteCommand cmd = conn.CreateCommand();
            string cmdText = "SELECT COUNT(*) FROM Validacoes";
            cmd.CommandText = cmdText;
            return cmd.ExecuteScalar();
        }
        #endregion
    }
}
