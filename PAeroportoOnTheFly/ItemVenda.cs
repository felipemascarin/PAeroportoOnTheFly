using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAeroportoOnTheFly
{
    internal class ItemVenda
    {
        public string IDItemVenda { get; set; }
        public string IDPassagem { get; set; }
        public float ValorUnitario { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }

        public ItemVenda(string idItemVenda, string idPassagem, float valorUnitario, string cpf, string nome)
        {
            this.IDItemVenda = idItemVenda;
            this.IDPassagem = idPassagem;
            this.ValorUnitario = valorUnitario;
            this.Cpf = cpf;
            this.Nome = nome;
        }
        public override string ToString()
        {
            return "\nDADOS ITEM VENDA: \nID Item Venda: " + IDItemVenda + "\nID Passagem: " + IDPassagem + "\nID Cliente: " + Nome + "\nCPF: " + Cpf;
        }
        public string ObterDados()
        {
            return IDItemVenda + IDPassagem + ValorConverter(ValorUnitario) + Cpf + Nome;
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