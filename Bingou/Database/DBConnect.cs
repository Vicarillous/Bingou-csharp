using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Entity;

namespace Bingou.Database
{
    public class DBConnect
    {
        public static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Compress = True;");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception e)
            {
                //Falhe silenciosamente
            }
            return sqlite_conn;
        }

        public static void CreateTable(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS Cartelas (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, numeros TEXT NOT NULL)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
        }

        public static void InsertData(SQLiteConnection conn, string cmd_text)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = cmd_text;
            sqlite_cmd.ExecuteNonQuery();
        }

        public static void ReadData(SQLiteConnection conn, string cmd_text)
        {
            throw new NotImplementedException();
        }
    }
}
