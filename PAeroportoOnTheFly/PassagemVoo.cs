using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAeroportoOnTheFly
{
    internal class PassagemVoo
    {
        public string IDPassagem { get; set; }
        public string IDVoo { get; set; }
        public DateTime DataUltimaOperacao { get; set; }
        public float Valor { get; set; }
        public char Situacao { get; set; }
        public PassagemVoo(string idPassagem, string idVoo, DateTime dataUltimaOperacao, float valor, char Situacao)
        {
            this.IDPassagem = idPassagem;
            this.IDVoo = idVoo;
            this.DataUltimaOperacao = dataUltimaOperacao;
            this.Valor = valor;
            this.Situacao = Situacao; 
        }
        public override string ToString()
        {
            return "\nDADOS PASSAGEM: \nID Passagem: " + IDPassagem + "\nID VOO: " + IDVoo + "\nData Última Operação: " + DataUltimaOperacao.ToString("dd/MM/yyyy HH:mm") + "\nValor: " + Valor + "\nSituação: " + Situacao;
        }
        public string ObterDados()
        {
            return IDPassagem + IDVoo + DataUltimaOperacao.ToString("ddMMyyyyHHmm") + ValorConverter(Valor) + Situacao;
        }

        public string ValorConverter(float valor)
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