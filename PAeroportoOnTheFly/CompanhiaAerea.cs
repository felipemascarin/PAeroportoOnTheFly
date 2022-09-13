using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAeroportoOnTheFly
{
    internal class CompanhiaAerea
    {
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime UltimoVoo { get; set; }
        public DateTime DataCadastro { get; set; }
        public char Situacao { get; set; }

        public CompanhiaAerea(string cnpj, string razaoSocial, DateTime DataAbertura, DateTime UltimoVoo, DateTime DataCadastro, char Situacao)
        {
            this.Cnpj = cnpj;
            this.RazaoSocial = razaoSocial;
            this.DataAbertura = DataAbertura;
            this.UltimoVoo = System.DateTime.Now;
            this.DataCadastro = System.DateTime.Now;
            this.Situacao = Situacao; //Ativo,Inativo
        }
        public override string ToString()
        {
            return "\nDADOS COMPANHIA AÉREA: \nRazão Social: " + RazaoSocial + "\nCNPJ: " + Cnpj + "\nData Abertura: " + DataAbertura.ToString("dd/MM/yyyy") + "\nData Cadastro: " + DataCadastro.ToString("dd/MM/yyyy HH:mm") + "\nÚltimo Voo: " + UltimoVoo.ToString("dd/MM/yyyy HH:mm") + "\nSituação: " + Situacao;
        }
        public string ObterDados()
        {
            return Cnpj + RazaoSocial + DataAbertura.ToString("ddMMyyyy") + UltimoVoo.ToString("ddMMyyyyHHmm") + DataCadastro.ToString("ddMMyyyyHHmm") + Situacao;
        }

    }
}