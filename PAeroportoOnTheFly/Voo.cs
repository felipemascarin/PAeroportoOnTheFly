using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAeroportoOnTheFly
{
    internal class Voo
    {
        public string IDVoo { get; set; }
        public string Destino { get; set; }
        public string IDAeronave { get; set; }
        public DateTime DataVoo { get; set; }
        public DateTime DataCadastro { get; set; }
        public char Situacao { get; set; }
        public Voo(string idVoo, string destino, string idAeronave, DateTime dataVoo, DateTime DataCadastro, char Situacao)
        {
            this.IDVoo = idVoo;
            this.Destino = destino;
            this.IDAeronave = idAeronave;
            this.DataVoo = dataVoo;
            this.DataCadastro = System.DateTime.Now;
            this.Situacao = Situacao; //Ativo,Cancelado
        }
        public override string ToString()
        {
            return "\nDADOS VOO: \nID Voo: " + IDVoo + "\nDestino: " + Destino + "\nID Aeronave: " + IDAeronave + "\nData Voo: " + DataVoo.ToString("dd/MM/yyyy HH:mm") + "\nData Cadastro: " + DataCadastro.ToString("dd/MM/yyyy HH:mm") + "\nSituação: " + Situacao;
        }
        public string ObterDados()
        {
            return this.IDVoo + Destino + IDAeronave + DataVoo.ToString("ddMMyyyyHHmm") + DataCadastro.ToString("ddMMyyyyHHmm") + Situacao;
        }

        public string DadosVooRealizado()
        {
            return "ID Voo:" + IDVoo + "   Destino:" + Destino + "   ID Aeronave:" + IDAeronave + "   Data e hora que o Voo foi inciado:" + DataVoo.ToString("dd/MM/yyyy HH:mm");
        }
    }
}