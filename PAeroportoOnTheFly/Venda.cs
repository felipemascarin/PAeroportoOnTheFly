using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAeroportoOnTheFly
{
    internal class Venda
    {
        public string IDVenda { get; set; }
        public DateTime DataVenda { get; set; }
        public string Passageiro { get; set; }
        public float ValorTotal { get; set; }
        public Venda(string idVenda, DateTime dataVenda, string passageiro, float valorTotal)
        {
            this.IDVenda = idVenda;
            this.DataVenda = dataVenda;
            this.Passageiro = passageiro;
            this.ValorTotal = valorTotal;
        }
        public override string ToString()
        {
            return "\nDADOS VENDA: \nID Venda: " + IDVenda + "\nData Venda: " + DataVenda.ToString("dd/MM/yyyy HH:mm") + "\nPassageiro: " + Passageiro + "\nValor Total: " + ValorTotal;
        }
        public string ObterDados()
        {
            return IDVenda + DataVenda.ToString("ddMMyyyyHHmm") + Passageiro + ValorConverter(ValorTotal);
        }

        static public string ValorConverter(float valor)
        {
            try
            {
                string[] valorpassagem = new string[] { "0", ".", "0", "0", "0", ",", "0", "0" };
                string valorp = null;
                string valorstring = valor.ToString("N2");
                char[] vetorvalorstring = valorstring.ToCharArray();
                if (valorstring.Length == 4)
                {
                    valorpassagem[4] = vetorvalorstring[0].ToString();
                }
                else
                {
                    if (valorstring.Length == 5)
                    {
                        valorpassagem[3] = vetorvalorstring[0].ToString();
                        valorpassagem[4] = vetorvalorstring[1].ToString();
                    }
                    else
                    {
                        if (valorstring.Length == 6)
                        {
                            valorpassagem[2] = vetorvalorstring[0].ToString();
                            valorpassagem[3] = vetorvalorstring[1].ToString();
                            valorpassagem[4] = vetorvalorstring[2].ToString();
                        }
                        else
                        {
                            if (valorstring.Length == 8)
                            {
                                return valorstring;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                for (int i = 0; i < valorpassagem.Length; i++)
                {
                    valorp = valorp + valorpassagem[i];
                }
                return valorp;
            }
            catch
            {
                return null;
            }
        }
    }
}