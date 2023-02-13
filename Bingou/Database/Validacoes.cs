using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bingou.Database
{
    public class Validacoes
    {
        public static void ExcluirNumero(int value)
        {
            using(SQLiteConnection conn = DBConnect.CreateConnection())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM Validacoes WHERE id = {0}", value);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
