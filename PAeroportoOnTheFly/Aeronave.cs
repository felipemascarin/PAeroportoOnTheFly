using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAeroportoOnTheFly
{
    internal class Aeronave
    {
        public string Inscricao { get; set; }
        public int Capacidade { get; set; }
        public int AssentosOcupados { get; set; }
        public DateTime UltimaVenda { get; set; }
        public DateTime DataCadastro { get; set; }
        public char Situacao { get; set; }
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }

        public Aeronave(string inscricao, int capacidade, int assentosOcupados, DateTime UltimaVenda, DateTime Cadastro, char situacao, string cnpj, string razaoSocial)
        {
            this.Inscricao = inscricao;
            this.Capacidade = capacidade;
            this.AssentosOcupados = assentosOcupados;
            this.UltimaVenda = System.DateTime.Now;
            this.DataCadastro = System.DateTime.Now;
            this.Situacao = situacao;
            this.Cnpj = cnpj;
            this.RazaoSocial = razaoSocial;
        }
        public override string ToString()
        {
            string s;
            if (Situacao == 'A') s = "ATIVA";
            else s = "INATIVA";

            return "\nDADOS AERONAVE: \nInscrição: " + Inscricao + "\nCapacidade: " + Capacidade + "\nAssentos Ocupados: " + AssentosOcupados + "\nData Cadastro: " + DataCadastro.ToString("dd/MM/yyyy HH:mm") + "\nÚltima Venda: " + UltimaVenda.ToString("dd/MM/yyyy HH:mm") + "\nSituação: " + s + "\nEmpresa Aérea: " + RazaoSocial + "\nCNPJ: " + Cnpj;
        }
        public string ObterDados()
        {
            return Inscricao + Capacidade + string.Format("{0:000}", AssentosOcupados) + UltimaVenda.ToString("ddMMyyyyHHmm") + DataCadastro.ToString("ddMMyyyyHHmm") + Situacao + Cnpj + RazaoSocial;
        }
    }
}