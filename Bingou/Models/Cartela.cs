using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bingou.Models
{
    public class Cartela
    {
        //public int[,] Numeros { get; set; } = new int[5, 5];

        /*public Cartela(int[,] numeros)
        {
            Numeros = numeros;
        }*/
        public string Numeros { get; set; }

        public Cartela(string numeros)
        {
            Numeros = numeros;
        }

        public void ExibirCartela()
        {
            Trace.WriteLine(Numeros);
        }
    }
}
