using PAeroportoOnTheFly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace PAeroportoOnTheFly
{
    internal class Program
    {
        static List<Passageiro> listPassageiro = new List<Passageiro>();
        static List<CompanhiaAerea> listCompanhia = new List<CompanhiaAerea>();
        static List<Aeronave> listAeronaves = new List<Aeronave>();
        static List<string> voosrealizados = new List<string>();
        static List<Voo> listVoo = new List<Voo>();
        static List<PassagemVoo> listPassagem = new List<PassagemVoo>();
        static List<Venda> listVenda = new List<Venda>();
        static List<ItemVenda> listItemVenda = new List<ItemVenda>();
        static List<string> listRestritos = new List<string>();
        static List<string> listBloqueados = new List<string>();
        static List<string> listDestino = new List<string>();


        static public DateTime DateConverter(string data)
        {
            char[] datasembarra = data.ToCharArray();
            string datacombarras = (datasembarra[0].ToString() + datasembarra[1].ToString() + "/" + datasembarra[2].ToString() + datasembarra[3].ToString() + "/" + datasembarra[4].ToString() + datasembarra[5].ToString() + datasembarra[6].ToString() + datasembarra[7].ToString());
            return DateTime.Parse(datacombarras);
        }

        static public DateTime DateHourConverter(string datahora)
        {
            char[] datahorastring = datahora.ToCharArray();
            char[] datahoracombarra = new char[] { datahorastring[0], datahorastring[1], '/', datahorastring[2], datahorastring[3], '/', datahorastring[4], datahorastring[5], datahorastring[6], datahorastring[7], ' ', datahorastring[8], datahorastring[9], ':', datahorastring[10], datahorastring[11] };
            string datacombarras = null;
            foreach (var v in datahoracombarra)
            {
                datacombarras = datacombarras + v;
            }
            return DateTime.Parse(datacombarras);
        }

        static bool PausaMensagem()
        {
            bool repetirdo;
            do
            {
                Console.WriteLine("\nPressione S para informar novamente ou C para cancelar:");
                ConsoleKeyInfo op = Console.ReadKey(true);
                if (op.Key == ConsoleKey.S)
                {
                    Console.Clear();
                    return false;
                }
                else
                {
                    if (op.Key == ConsoleKey.C)
                    {
                        Console.Clear();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Escolha uma opção válida!");
                        repetirdo = true;
                    }
                }
            } while (repetirdo == true);
            return true;
        }

        static void Pausa()
        {
            Console.WriteLine("\nAperte 'ENTER' para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        static void Atualizar()
        {
            try
            {
                //Exclui voos já realizados
                for (int j = 0; j < listVoo.Count; j++)
                {
                    if (DateTime.Compare(listVoo[j].DataVoo, System.DateTime.Now) < 0)
                    {
                        for (int i = 0; i < listPassagem.Count; i++)
                        {
                            if (listPassagem[i].IDVoo == listVoo[j].IDVoo)
                            {
                                if (listPassagem.Count > 1)
                                {
                                    PassagemVoo aux = listPassagem[i + 1];
                                    listPassagem.Remove(listPassagem[i]);
                                    listPassagem[i] = aux;
                                    i--;
                                }
                                else
                                {
                                    listPassagem.Remove(listPassagem[i]);
                                }
                            }
                        }

                        voosrealizados.Add(listVoo[j].DadosVooRealizado());
                        GravarVooRealizado();

                        if (listVoo.Count > 1)
                        {
                            Voo auxi = listVoo[j + 1];
                            listVoo.Remove(listVoo[j]);
                            listVoo[j] = auxi;
                            j--;
                        }
                        else
                        {
                            listVoo.Remove(listVoo[j]);
                        }
                    }
                }

                //Altera as passagens reservadas para livres em 2 dias após a compra
                foreach (var passagem in listPassagem)
                {
                    if (passagem.Situacao == 'R' && DateTime.Compare(passagem.DataUltimaOperacao.AddDays(2), System.DateTime.Now) < 0)
                    {
                        passagem.Situacao = 'L';
                    }
                }
                GravarVoo();
                GravarPassagem();
            }
            catch (Exception)
            {
                Console.WriteLine("ERRO de arquivo! Não foi possível atualizar os voos e passagens!");
                Pausa();
            }
        }

        static void GravarPassageiro()
        {
            StreamWriter gravPassageiro = new StreamWriter(@"C:\DBOnTheFly\Passageiro.dat");
            foreach (var passageiro in listPassageiro)
            {
                gravPassageiro.WriteLine(passageiro.ObterDados());
            }
            gravPassageiro.Close();
        }

        static void GravarCompanhiaAerea()
        {
            StreamWriter gravCompAerea = new StreamWriter(@"C:\DBOnTheFly\CompanhiaAerea.dat");
            foreach (var companhiaaerea in listCompanhia)
            {
                gravCompAerea.WriteLine(companhiaaerea.ObterDados());
            }
            gravCompAerea.Close();
        }

        static void GravarAeronaves()
        {
            StreamWriter gravAeronaves = new StreamWriter(@"C:\DBOnTheFly\Aeronaves.dat");
            foreach (var aeronaves in listAeronaves)
            {
                gravAeronaves.WriteLine(aeronaves.ObterDados());
            }
            gravAeronaves.Close();
        }

        static void GravarVoo()
        {
            StreamWriter gravVoo = new StreamWriter(@"C:\DBOnTheFly\Voo.dat");
            foreach (var voo in listVoo)
            {
                gravVoo.WriteLine(voo.ObterDados());
            }
            gravVoo.Close();
        }

        static void GravarPassagem()
        {
            StreamWriter gravPassagem = new StreamWriter(@"C:\DBOnTheFly\PassagemVoo.dat");
            foreach (var passagem in listPassagem)
            {
                gravPassagem.WriteLine(passagem.ObterDados());
            }
            gravPassagem.Close();
        }

        static void GravarVenda()
        {
            StreamWriter gravVenda = new StreamWriter(@"C:\DBOnTheFly\Venda.dat");
            foreach (var venda in listVenda)
            {
                gravVenda.WriteLine(venda.ObterDados());
            }
            gravVenda.Close();
        }

        static void GravarItemVenda()
        {
            StreamWriter gravItemVenda = new StreamWriter(@"C:\DBOnTheFly\ItemVenda.dat");
            foreach (var itemvenda in listItemVenda)
            {
                gravItemVenda.WriteLine(itemvenda.ObterDados());
            }
            gravItemVenda.Close();
        }

        static void GravarRestritos()
        {
            StreamWriter gravRestritos = new StreamWriter(@"C:\DBOnTheFly\Restritos.dat");
            foreach (var restritos in listRestritos)
            {
                gravRestritos.WriteLine(restritos);
            }
            gravRestritos.Close();
        }

        static void GravarBloqueados()
        {
            StreamWriter gravBloqueados = new StreamWriter(@"C:\DBOnTheFly\Bloqueados.dat");
            foreach (var bloqueados in listBloqueados)
            {
                gravBloqueados.WriteLine(bloqueados);
            }
            gravBloqueados.Close();
        }

        static void GravarVooRealizado()
        {
            StreamWriter gravRealizados = new StreamWriter(@"C:\DBOnTheFly\VooRealizado.dat");
            foreach (var voorealizado in voosrealizados)
            {
                gravRealizados.WriteLine(voorealizado);
            }
            gravRealizados.Close();
        }

        static void CarregarArquivos()
        {
            char[] caracteres;
            string[] linhas;
            float Valor;
            string valor = null;
            char Situacao;
            string data = null;
            DateTime DataCadastro;

            try
            {
                DateTime DataNascimento;
                DateTime DataUltimaCompra;

                linhas = System.IO.File.ReadAllLines(@"C:\DBOnTheFly\Passageiro.dat");
                foreach (var linha in linhas)
                {
                    char Sexo;
                    string cpf = null;
                    string nome = null;
                    Valor = 0;
                    valor = null;

                    caracteres = linha.ToCharArray();
                    for (int i = 0; i <= 10; i++)
                    {
                        cpf = cpf + caracteres[i].ToString();
                    }
                    for (int i = 11; i <= 61; i++)
                    {
                        nome = nome + caracteres[i].ToString();
                    }
                    for (int i = 62; i <= 69; i++)
                    {
                        data = data + caracteres[i].ToString();
                    }
                    DataNascimento = DateConverter(data);
                    data = null;
                    Sexo = caracteres[70];
                    for (int i = 71; i <= 82; i++)
                    {
                        data = data + caracteres[i].ToString();
                    }
                    DataUltimaCompra = DateHourConverter(data);
                    data = null;
                    for (int i = 83; i <= 94; i++)
                    {
                        data = data + caracteres[i].ToString();
                    }
                    DataCadastro = DateHourConverter(data);
                    Situacao = caracteres[95];
                    Passageiro P = new Passageiro(cpf, nome, DataNascimento, Sexo, DataUltimaCompra, DataCadastro, Situacao);
                    listPassageiro.Add(P);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Mensagem de Erro: Não foi possível carregar dados do arquivo Passageiro.dat");
            }

            try
            {
                DateTime DataAbertura;
                DateTime UltimoVoo;


                linhas = System.IO.File.ReadAllLines(@"C:\DBOnTheFly\CompanhiaAerea.dat");
                foreach (var linha in linhas)
                {
                    string Cnpj = null;
                    string RazaoSocial = null;
                    data = null;

                    caracteres = linha.ToCharArray();
                    for (int i = 0; i <= 13; i++)
                    {
                        Cnpj = Cnpj + caracteres[i].ToString();
                    }
                    for (int i = 14; i <= 64; i++)
                    {
                        RazaoSocial = RazaoSocial + caracteres[i].ToString();
                    }
                    for (int i = 65; i <= 72; i++)
                    {
                        data = data + caracteres[i].ToString();
                    }
                    DataAbertura = DateConverter(data);
                    data = null;
                    for (int i = 73; i <= 84; i++)
                    {
                        data = data + caracteres[i].ToString();
                    }
                    UltimoVoo = DateHourConverter(data);
                    data = null;
                    for (int i = 85; i <= 96; i++)
                    {
                        data = data + caracteres[i].ToString();
                    }
                    DataCadastro = DateHourConverter(data);
                    Situacao = caracteres[97];
                    CompanhiaAerea CA = new CompanhiaAerea(Cnpj, RazaoSocial, DataAbertura, UltimoVoo, DataCadastro, Situacao);
                    listCompanhia.Add(CA);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Mensagem de Erro: Não foi possível carregar dados do arquivo CompanhiaAerea.dat");
            }

            try
            {
                DateTime UltimaVenda;


                linhas = System.IO.File.ReadAllLines(@"C:\DBOnTheFly\Aeronaves.dat");
                foreach (var linha in linhas)
                {
                    string Inscricao = null;
                    int Capacidade = 0;
                    int Assentos = 0;
                    string cnpj = null;
                    string razaosocial = null;
                    data = null;

                    caracteres = linha.ToCharArray();

                    for (int i = 0; i <= 5; i++)
                    {
                        Inscricao = Inscricao + caracteres[i].ToString();
                    }
                    for (int i = 6; i <= 8; i++)
                    {
                        Capacidade = Capacidade + caracteres[i];
                    }
                    for (int i = 9; i <= 11; i++)
                    {
                        Assentos = Assentos + caracteres[i];
                    }
                    for (int i = 12; i <= 23; i++)
                    {
                        data = data + caracteres[i].ToString();
                    }
                    UltimaVenda = DateHourConverter(data);
                    data = null;
                    for (int i = 24; i <= 35; i++)
                    {
                        data = data + caracteres[i].ToString();
                    }
                    DataCadastro = DateHourConverter(data);
                    Situacao = caracteres[36];
                    for (int i = 37; i <= 50; i++)
                    {
                        cnpj = cnpj + caracteres[i].ToString();
                    }
                    for (int i = 51; i <= 100; i++)
                    {
                        razaosocial = razaosocial + caracteres[i].ToString();
                    }
                    Aeronave Aer = new Aeronave(Inscricao, Capacidade, Assentos, UltimaVenda, DataCadastro, Situacao, cnpj, razaosocial);
                    listAeronaves.Add(Aer);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Mensagem de Erro:  Não foi possível carregar dados do arquivo Aeronaves.dat ");
            }

            try
            {
                DateTime DataVoo;


                linhas = System.IO.File.ReadAllLines(@"C:\DBOnTheFly\Voo.dat");
                foreach (var linha in linhas)
                {
                    string IDVoo = null;
                    string Destino = null;
                    string Aeronave = null;
                    data = null;
                    caracteres = linha.ToCharArray();
                    for (int i = 0; i <= 4; i++)
                    {
                        IDVoo = IDVoo + caracteres[i].ToString();
                    }
                    for (int i = 5; i <= 7; i++)
                    {
                        Destino = Destino + caracteres[i].ToString();
                    }
                    for (int i = 8; i <= 13; i++)
                    {
                        Aeronave = Aeronave + caracteres[i].ToString();
                    }
                    for (int i = 14; i <= 25; i++)
                    {
                        data = data + caracteres[i].ToString();
                    }
                    DataVoo = DateHourConverter(data);
                    data = null;
                    for (int i = 26; i <= 37; i++)
                    {
                        data = data + caracteres[i].ToString();
                    }
                    DataCadastro = DateHourConverter(data);
                    Situacao = caracteres[38];
                    Voo V = new Voo(IDVoo, Destino, Aeronave, DataVoo, DataCadastro, Situacao);
                    listVoo.Add(V);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Mensagem de Erro: Não foi possível carregar dados do arquivo Voo.dat");
            }

            try
            {
                DateTime DataUltimaOperacao;


                linhas = System.IO.File.ReadAllLines(@"C:\DBOnTheFly\PassagemVoo.dat");
                foreach (var linha in linhas)
                {
                    string IDPassagem = null;
                    string IDVoo = null;
                    data = null;
                    valor = null;

                    caracteres = linha.ToCharArray();
                    for (int i = 0; i <= 5; i++)
                    {
                        IDPassagem = IDPassagem + caracteres[i].ToString();
                    }
                    for (int i = 6; i <= 10; i++)
                    {
                        IDVoo = IDVoo + caracteres[i].ToString();
                    }
                    for (int i = 11; i <= 22; i++)
                    {
                        data = data + caracteres[i].ToString();
                    }
                    DataUltimaOperacao = DateHourConverter(data);
                    for (int i = 23; i <= 30; i++)
                    {
                        valor = valor + caracteres[i].ToString();
                    }
                    Valor = float.Parse(valor);
                    Situacao = caracteres[31];
                    PassagemVoo PV = new PassagemVoo(IDPassagem, IDVoo, DataUltimaOperacao, Valor, Situacao);
                    listPassagem.Add(PV);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Mensagem de Erro: Não foi possível carregar dados do arquivo PassagemVoo.dat ");
            }

            try
            {
                DateTime DataVenda;

                linhas = System.IO.File.ReadAllLines(@"C:\DBOnTheFly\Venda.dat");
                foreach (var linha in linhas)
                {
                    string IDVenda = null;
                    string CpfPassageiro = null;
                    data = null;
                    valor = null;

                    caracteres = linha.ToCharArray();
                    for (int i = 0; i <= 4; i++)
                    {
                        IDVenda = IDVenda + caracteres[i].ToString();
                    }
                    for (int i = 5; i <= 16; i++)
                    {
                        data = data + caracteres[i].ToString();
                    }
                    DataVenda = DateHourConverter(data);
                    for (int i = 17; i <= 27; i++)
                    {
                        CpfPassageiro = CpfPassageiro + caracteres[i].ToString();
                    }
                    for (int i = 28; i <= 35; i++)
                    {
                        valor = valor + caracteres[i].ToString();
                    }
                    Valor = float.Parse(valor);
                    Venda Vend = new Venda(IDVenda, DataVenda, CpfPassageiro, Valor);
                    listVenda.Add(Vend);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Mensagem de Erro: Não foi possível carregar dados do arquivo Venda.dat");
            }

            try
            {
                linhas = System.IO.File.ReadAllLines(@"C:\DBOnTheFly\ItemVenda.dat");
                foreach (var linha in linhas)
                {
                    string IDItemVenda = null;
                    string IDPassagem = null;
                    string cpf = null;
                    string nome = null;
                    valor = null;

                    caracteres = linha.ToCharArray();
                    for (int i = 0; i <= 4; i++)
                    {
                        IDItemVenda = IDItemVenda + caracteres[i].ToString();
                    }
                    for (int i = 5; i <= 10; i++)
                    {
                        IDPassagem = IDPassagem + caracteres[i].ToString();
                    }
                    for (int i = 11; i <= 18; i++)
                    {
                        valor = valor + caracteres[i].ToString();
                    }
                    Valor = float.Parse(valor);
                    for (int i = 19; i <= 29; i++)
                    {
                        cpf = cpf + caracteres[i].ToString();
                    }
                    for (int i = 30; i <= 79; i++)
                    {
                        nome = nome + caracteres[i].ToString();
                    }
                    ItemVenda IV = new ItemVenda(IDItemVenda, IDPassagem, Valor, cpf, nome);
                    listItemVenda.Add(IV);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Mensagem de Erro: Não foi possível carregar dados do arquivo ItemVenda.dat");
            }

            //Restritos
            try
            {
                linhas = System.IO.File.ReadAllLines(@"C:\DBOnTheFly\Restritos.dat");
                foreach (var linha in linhas)
                {
                    listRestritos.Add(linha);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Mensagem de Erro: Não foi possível carregar dados do arquivo Restritos.dat");
            }

            //Bloqueados
            try
            {
                linhas = System.IO.File.ReadAllLines(@"C:\DBOnTheFly\Bloqueados.dat");
                foreach (var linha in linhas)
                {
                    listBloqueados.Add(linha);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Mensagem de Erro: Não foi possível carregar dados do arquivo Bloqueados.dat");
            }

            //Destino
            try
            {
                linhas = System.IO.File.ReadAllLines(@"C:\DBOnTheFly\Destino.dat");
                foreach (var linha in linhas)
                {
                    listDestino.Add(linha);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Mensagem de Erro: Não foi possível carregar dados do arquivo Destino.dat");
            }

            //Voos Realizados

            try
            {
                linhas = System.IO.File.ReadAllLines(@"C:\DBOnTheFly\VooRealizado.dat");
                foreach (var linha in linhas)
                {
                    voosrealizados.Add(linha);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Mensagem de Erro: Não foi possível carregar dados dos voos já realizados.dat");
            }
        }

        static List<string> GeradorIdPassagens(int capacidadeassentos)
        {
            try
            {
                Random random = new Random();
                List<string> listaId = new List<string>();
                string id;
                bool encontrado;

                for (int i = 0; i < capacidadeassentos; i++)
                {
                    encontrado = false;

                    id = random.Next(1001, 9999).ToString();

                    foreach (var idnalista in listaId)
                    {
                        if (idnalista == id)
                        {
                            encontrado = true;
                            i--;
                            break;
                        }
                    }
                    if (encontrado == false)
                        listaId.Add("PA" + id.ToString());
                }
                return listaId;
            }
            catch (Exception)
            {
                Console.WriteLine("Erro, não foi possível gerar ID's das passagens!");
                Pausa();
                return null;
            }
        }

        static public string GeradorId(String id)
        {
            switch (id)
            {
                case "idvoo":

                    #region IDVoo

                    Random random = new Random();
                    bool encontrado = false;
                    string idvoogerado;
                    do
                    {
                        try
                        {
                            encontrado = false;
                            idvoogerado = ("V" + random.Next(1000, 9999).ToString());

                            foreach (var voo in listVoo)
                            {
                                if (voo.IDVoo == idvoogerado)
                                {
                                    encontrado = true;
                                    break;
                                }
                            }

                            if (listVoo.Count <= 8999)
                            {
                                if (encontrado == false)
                                    return idvoogerado;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Erro, não foi possível gerar ID do Voo! Lista de vendas está cheia");
                                Pausa();
                                return null;
                            }
                        }
                        catch (Exception)
                        {
                            Console.Clear();
                            Console.WriteLine("Erro, não foi possível gerar ID do Voo!");
                            Pausa();
                            return null;
                        }
                    } while (encontrado == true);
                    return null;
                #endregion

                case "idvenda":


                    #region IDVenda

                    try
                    {
                        string idvenda = (listVenda.Count + 1).ToString();
                        if (idvenda.Length == 1) return ("0000" + idvenda);
                        else if (idvenda.Length == 2) return ("000" + idvenda);
                        else if (idvenda.Length == 3) return ("00" + idvenda);
                        else if (idvenda.Length == 4) return ("0" + idvenda);
                        else if (idvenda.Length == 5) return idvenda;
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Erro, não foi possível gerar ID da venda! Lista de vendas está cheia");
                            Pausa();
                            return null;
                        }
                    }
                    catch (Exception)
                    {
                        Console.Clear();
                        Console.WriteLine("Erro, não foi possível gerar ID da venda!");
                        Pausa();
                        return null;
                    }

                #endregion


                case "iditemvenda":

                    #region IDItemVenda
                    try
                    {
                        string idvenda = (listVenda.Count + 1).ToString();
                        if (idvenda.Length == 1) return ("0000" + idvenda);
                        else if (idvenda.Length == 2) return ("000" + idvenda);
                        else if (idvenda.Length == 3) return ("00" + idvenda);
                        else if (idvenda.Length == 4) return ("0" + idvenda);
                        else if (idvenda.Length == 5) return idvenda;
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Erro, não foi possível gerar ID do Item Venda! Lista está cheia");
                            Pausa();
                            return null;
                        }
                    }
                    catch (Exception)
                    {
                        Console.Clear();
                        Console.WriteLine("Erro, não foi possível gerar ID do Item Venda!");
                        Pausa();
                        return null;
                    }

                #endregion

                default:
                    return null;
            }
        }

        static string ValidarEntrada(string entrada)
        {
            string[] vetorletras = new string[] {"Ç","ç","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S",
            "T","U","V","W","X","Y","Z","Á","É","Í","Ó","Ú","À","È","Ì","Ò","Ù","Â","Ê","Î","Ô","Û","Ã","Õ"," "};
            string[] vetornumeros = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            bool encontrado;
            bool retornar = true;
            int qtdnumerosiguais = 0;


            switch (entrada)
            {
                case "menu":

                    #region menu

                    do
                    {

                        try
                        {
                            char[] vetortecla;
                            Console.CursorVisible = false;
                            ConsoleKeyInfo op = Console.ReadKey(true);
                            vetortecla = op.Key.ToString().ToCharArray();

                            if (vetortecla[0] == 'N')
                            {
                                if (vetornumeros.Contains(vetortecla[6].ToString()) == true)
                                {
                                    return vetortecla[6].ToString();
                                }
                                else
                                {
                                    encontrado = false;
                                }
                            }
                            else
                            {
                                if (vetortecla[0] == 'D')
                                {
                                    if (vetornumeros.Contains(vetortecla[1].ToString()) == true)
                                    {
                                        return vetortecla[1].ToString();
                                    }
                                    else
                                    {
                                        encontrado = false;
                                    }
                                }
                                else
                                {
                                    encontrado = false;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            encontrado = false;
                        }
                    } while (encontrado == false);

                    return null;


                #endregion


                case "cpf":

                    #region CPF;

                    do
                    {
                        //Seta encontrado e validado sempre que retorna o laço do processo:
                        encontrado = true; // seta true para não quebrar o for de primeira
                        retornar = false; // só retorna se o usuário quiser
                        qtdnumerosiguais = 0;
                        string cpf;

                        try
                        {
                            Console.Clear();
                            Console.Write("Informe o CPF: ");

                            cpf = Console.ReadLine();

                            char[] letras = cpf.ToCharArray();

                            //verifica se tem 11 caracteres:
                            if (letras.Length == 11)
                            {
                                //verifica os 11 caracteres se são obrigatóriamente números:
                                for (int i = 0; i < 11 && encontrado != false; i++)
                                {
                                    foreach (var v in vetornumeros)
                                    {
                                        if (letras[i].ToString().ToUpper().Equals(v))
                                        {
                                            encontrado = true;
                                            break; // sai do foreach e volta pro for
                                        }
                                        else encontrado = false;
                                    }
                                }

                                //Verifica se é um cpf válido calculando os 2 últimos digitos, segundo a receita federal:
                                if (encontrado == true)
                                {
                                    int soma = 0;
                                    int resto = 0;
                                    int digito1 = 0;
                                    int digito2 = 0;

                                    //Verifica se os números são iguais

                                    for (int i = 0; i < 9; i++)
                                    {
                                        if (letras[i] == letras[i + 1])
                                            qtdnumerosiguais = qtdnumerosiguais + 1;

                                    }

                                    //Se os 9 primeiros digitos forem todos iguais, invalida o cpf:
                                    if (qtdnumerosiguais != 9)
                                    {
                                        //calcula o primeiro digito verificador do cpf:
                                        for (int i = 1, j = 0; i < 10; i++, j++)
                                            soma = soma + (int.Parse(letras[j].ToString()) * i);

                                        resto = soma % 11;

                                        if (resto >= 10)
                                        {
                                            digito1 = 0;

                                        }
                                        else
                                        {
                                            digito1 = resto;
                                        }

                                        //Verifica se o primeiro digito digitado é igual ao que era pra ser:
                                        if (digito1 == int.Parse(letras[9].ToString()))
                                        {
                                            soma = 0; //seta o soma em 0 para o processo de soma do segundo digito do cpf:

                                            //calcula o segundo digito verificador do cpf:
                                            for (int i = 0, j = 0; i < 10; i++, j++)
                                                soma = soma + (int.Parse(letras[j].ToString()) * i);

                                            resto = soma % 11;

                                            if (resto >= 10)
                                            {
                                                digito2 = 0;
                                            }
                                            else
                                            {
                                                digito2 = resto;
                                            }
                                            //Verifica se o segundo digito digitado é igual ao que era pra ser:
                                            if (digito2 == int.Parse(letras[10].ToString()))
                                            {
                                                //Se digitos validados, procura na lista de cadastro se já existe o cpf cadastrado:
                                                encontrado = false;
                                                foreach (var passageiro in listPassageiro)
                                                {
                                                    //Se achar na lista não deixa prosseguir
                                                    if (passageiro.Cpf == cpf)
                                                    {
                                                        //Se encontrar na lista, invalida o cadastro
                                                        encontrado = true;
                                                        Console.WriteLine("CPF já cadastrado!");
                                                        retornar = PausaMensagem();
                                                        break; //Quando encontrar um cpf igual na lista, quebra o foreach
                                                    }
                                                    else
                                                        encontrado = false; //Mantem encontrado como false enquanto não achar na lista
                                                }

                                                //Ao fim da procura, se não possuir o cpf na lista, encontrado = false e retorna o cpf cadastrado:
                                                if (encontrado == false)
                                                    //////////RETORNA O CPF
                                                    return cpf;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Esse não é um CPF válido!");
                                                retornar = PausaMensagem();
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Esse não é um CPF válido!");
                                            retornar = PausaMensagem();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("CPF com números sequenciais iguais não é válido!");
                                        retornar = PausaMensagem();
                                    }
                                }
                                else
                                {

                                    Console.WriteLine("Só aceita números válidos de 11 digitos");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Só aceita números válidos de 11 digitos");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("ERRO: Só aceita números válidos de 11 digitos");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion;


                case "cnpj":

                    #region CNPJ;

                    do
                    {
                        //Seta encontrado e validado sempre que retorna o laço do processo:
                        encontrado = true; // seta true para não quebrar o for de primeira
                        retornar = false; // só retorna se o usuário quiser
                        qtdnumerosiguais = 0;
                        string cnpj;

                        try
                        {
                            Console.Clear();
                            Console.Write("Informe o CNPJ: ");

                            cnpj = Console.ReadLine();

                            char[] letras = cnpj.ToCharArray();

                            //verifica se tem 14 caracteres:
                            if (letras.Length == 14)
                            {
                                //verifica os 14 caracteres se são obrigatóriamente números:
                                for (int i = 0; i < 14 && encontrado != false; i++)
                                {
                                    foreach (var v in vetornumeros)
                                    {
                                        if (letras[i].ToString().ToUpper().Equals(v))
                                        {
                                            encontrado = true;
                                            break; // sai do foreach e volta pro for
                                        }
                                        else encontrado = false; // se não encontrado, sai do foreach e quebra a condição do for
                                    }
                                }

                                //Qualquer valor que não seja um número invalida o cnpj:
                                if (encontrado == true)
                                {
                                    //Verifica se os números são iguais
                                    for (int i = 0; i < 12; i++)
                                    {
                                        if (letras[i] == letras[i + 1])
                                            qtdnumerosiguais = qtdnumerosiguais + 1;
                                    }

                                    //Se os 12 primeiros digitos forem todos iguais, invalida o cnpj:
                                    if (qtdnumerosiguais != 12)
                                    {
                                        int soma = 0;
                                        int resto = 0;
                                        int digito1 = 0;
                                        int digito2 = 0;

                                        //calcula o primeiro digito verificador do cnpj:
                                        for (int i = 6, j = 0; i < 10; i++, j++)
                                            soma = soma + (int.Parse(letras[j].ToString()) * i);

                                        for (int i = 2, j = 4; i < 10; i++, j++)
                                            soma = soma + (int.Parse(letras[j].ToString()) * i);

                                        resto = soma % 11;

                                        if (resto >= 10)
                                        {
                                            digito1 = 0;

                                        }
                                        else
                                        {
                                            digito1 = resto;
                                        }

                                        //Verifica se o primeiro digito digitado é igual ao que era pra ser:
                                        if (digito1 == int.Parse(letras[12].ToString()))
                                        {
                                            soma = 0; //seta o soma em 0 para o processo de soma do segundo digito do cnpj:

                                            //calcula o segundo digito verificador do cpf:
                                            for (int i = 5, j = 0; i < 10; i++, j++)
                                                soma = soma + (int.Parse(letras[j].ToString()) * i);

                                            for (int i = 2, j = 5; i < 10; i++, j++)
                                                soma = soma + (int.Parse(letras[j].ToString()) * i);

                                            resto = soma % 11;

                                            if (resto >= 10)
                                            {
                                                digito2 = 0;

                                            }
                                            else
                                            {
                                                digito2 = resto;
                                            }
                                            //Verifica se o segundo digito digitado é igual ao que era pra ser:
                                            if (digito2 == int.Parse(letras[13].ToString()))
                                            {
                                                //Se digitos validados, procura na lista de cadastro se já existe o cnpj cadastrado:
                                                encontrado = false;
                                                foreach (var companhia in listCompanhia)
                                                {
                                                    //Se achar na lista não deixa prosseguir
                                                    if (companhia.Cnpj == cnpj)
                                                    {
                                                        //Se encontrar na lista, invalida o cadastro
                                                        encontrado = true;
                                                        Console.WriteLine("CNPJ já cadastrado!");
                                                        retornar = PausaMensagem();
                                                        break; //Quando encontrar um cnpj igual na lista, quebra o foreach
                                                    }
                                                    else
                                                        encontrado = false; //Mantem encontrado como false enquanto não achar na lista
                                                }

                                                //Ao fim da procura, se não possuir o cnpj na lista, encontrado = false e retorna o cnpj cadastrado:
                                                if (encontrado == false)
                                                    //////////RETORNA O CNPJ
                                                    return cnpj;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Esse não é um CNPJ válido!");
                                                retornar = PausaMensagem();
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Esse não é um CNPJ válido!");
                                            retornar = PausaMensagem();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("CNPJ com números sequenciais iguais não é válido!");
                                        retornar = PausaMensagem();
                                    }
                                }
                                else
                                {

                                    Console.WriteLine("Só aceita números válidos de 14 digitos");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Só aceita números válidos de 14 digitos");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("ERRO: Só aceita números válidos de 14 digitos");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion;


                case "nome":

                    #region Nome

                    do
                    {
                        string nome;
                        encontrado = true;
                        retornar = false;

                        Console.Write("Informe o Nome/Razão Social: ");
                        try
                        {
                            nome = Console.ReadLine();

                            char[] letras = nome.ToCharArray();

                            //Verifica se o nome tem no mínimo 3 e no máximo 50 caracteres:
                            if (letras.Length > 3 && letras.Length <= 50)
                            {
                                //Verifica se o nome só tem letras válidas:
                                for (int i = 0; i < letras.Length && encontrado != false; i++)
                                {
                                    foreach (var v in vetorletras)
                                    {
                                        if (letras[i].ToString().ToUpper().Equals(v))
                                        {
                                            encontrado = true;
                                            break;
                                        }
                                        else encontrado = false;
                                    }
                                }

                                //Se possuir somente letras válidas, prossegue:
                                if (encontrado == true)
                                {
                                    int qtdmax = 50;
                                    int qtdescrito = letras.Length;

                                    //Verifica a quantidade de caracteres que falta para 50 caracteres e preenche de espaço, se preciso:
                                    if (qtdescrito < qtdmax)
                                    {
                                        int qtdfaltante = qtdmax - qtdescrito;

                                        for (int i = 0; i <= qtdfaltante; i++)
                                        {
                                            nome = nome + " ";
                                        }
                                        ///////RETORNA O NOME COM 50 CARACTERES (PREENCHIDO COM ESPAÇOS PARA COMPLETAR 50)
                                        return nome;
                                    }
                                    else
                                    {
                                        ///////RETORNA O NOME COM 50 CARACTERES (SE O USUÁRIO UTILIZAR OS 50 CARACTERES)
                                        return nome;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Nome só aceita letras.");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nome informado não é válido! Insira o nome completo, máximo 50 caracteres!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Insira um valor válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion


                case "sexo":

                    #region Sexo
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Informe o sexo:\n[M] - Masculino\n[F] - Feminino\n[N] - Não informar");
                        Console.CursorVisible = false;
                        ConsoleKeyInfo op = Console.ReadKey(true);

                        //Verificar se tecla pressionada foi M / F ou N (independente do CAPSLOCK estar ativado!)
                        if (op.Key == ConsoleKey.M)
                        {
                            Console.Clear();
                            return "M";
                        }
                        else
                        {
                            if (op.Key == ConsoleKey.F)
                            {
                                Console.Clear();
                                return "F";
                            }
                            else
                            {
                                if (op.Key == ConsoleKey.N)
                                {
                                    Console.Clear();
                                    return "N";
                                }
                                else
                                {
                                    Console.WriteLine("Escolha uma opção válida!");
                                    retornar = PausaMensagem();
                                }
                            }
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion


                case "datanascimento":

                    #region DataNascimento

                    do
                    {
                        try
                        {
                            ConsoleKeyInfo teclaData;
                            char[] vetortecla;
                            string DataNascimento = null;
                            encontrado = false;
                            string[] vetordata = new string[] { "_", "_", "_", "_", "_", "_", "_", "_" };

                            static void AtualizarTela(string[] vetordata)
                            {
                                Console.Clear();
                                Console.WriteLine("Insira a Data de Nascimento:");
                                Console.WriteLine(vetordata[0] + vetordata[1] + "/" + vetordata[2] + vetordata[3] + "/" + vetordata[4] + vetordata[5] + vetordata[6] + vetordata[7]);
                                Console.CursorVisible = false;
                            }

                            for (int i = 0; i < 8; i++)
                            {
                                AtualizarTela(vetordata);

                                teclaData = Console.ReadKey(true);

                                vetortecla = teclaData.Key.ToString().ToCharArray();

                                if (vetortecla[0] == 'N')
                                {
                                    if (vetornumeros.Contains(vetortecla[6].ToString()) == true)
                                    {
                                        encontrado = true;
                                        vetordata[i] = vetortecla[6].ToString();
                                        DataNascimento = DataNascimento + vetordata[i];
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }

                                    AtualizarTela(vetordata);
                                }
                                else
                                {

                                    if (vetortecla[0] == 'D')
                                    {
                                        if (vetornumeros.Contains(vetortecla[1].ToString()) == true)
                                        {
                                            encontrado = true;
                                            vetordata[i] = vetortecla[1].ToString();
                                            DataNascimento = DataNascimento + vetordata[i];
                                        }
                                        else
                                        {
                                            encontrado = false;
                                            break;
                                        }

                                        AtualizarTela(vetordata);
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }
                                }
                            }
                            if (encontrado == true)
                            {
                                if (DateTime.Compare(DateConverter(DataNascimento), System.DateTime.Now) < 0)
                                {
                                    return DataNascimento;
                                }
                                else
                                {
                                    Console.WriteLine("Data de nascimento não aceita datas futuras, insira uma data válida!");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Por favor, insira uma data válida!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Erro: Por favor, insira uma data válida!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    return null;


                #endregion


                case "dataabertura":

                    #region DataAberturaCompanhiaAerea

                    do
                    {
                        try
                        {
                            ConsoleKeyInfo teclaData;
                            char[] vetortecla;
                            string DataAbertura = null;
                            encontrado = false;
                            string[] vetordata = new string[] { "_", "_", "_", "_", "_", "_", "_", "_" };

                            static void AtualizarTela(string[] vetordata)
                            {
                                Console.Clear();
                                Console.WriteLine("Insira a Data de abertura da Empresa:");

                                Console.WriteLine(vetordata[0] + vetordata[1] + "/" + vetordata[2] + vetordata[3] + "/" + vetordata[4] + vetordata[5] + vetordata[6] + vetordata[7]);
                                Console.CursorVisible = false;

                            }

                            //Verificar se digitou só nrs válidos
                            for (int i = 0; i < 8; i++)
                            {
                                AtualizarTela(vetordata);

                                teclaData = Console.ReadKey(true);

                                vetortecla = teclaData.Key.ToString().ToCharArray();

                                if (vetortecla[0] == 'N')
                                {
                                    if (vetornumeros.Contains(vetortecla[6].ToString()) == true)
                                    {
                                        encontrado = true;
                                        vetordata[i] = vetortecla[6].ToString();
                                        DataAbertura = DataAbertura + vetordata[i];
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }

                                    AtualizarTela(vetordata);
                                }
                                else
                                {
                                    if (vetortecla[0] == 'D')
                                    {
                                        if (vetornumeros.Contains(vetortecla[1].ToString()) == true)
                                        {
                                            encontrado = true;
                                            vetordata[i] = vetortecla[1].ToString();
                                            DataAbertura = DataAbertura + vetordata[i];
                                        }
                                        else
                                        {
                                            encontrado = false;
                                            break;
                                        }

                                        AtualizarTela(vetordata);
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }
                                }
                            }
                            //Se só digitou números válidos, continua:
                            if (encontrado == true)
                            {
                                //Verificar se é data futura:
                                if (DateTime.Compare(DateConverter(DataAbertura), System.DateTime.Now) < 0)
                                {
                                    //Verificar se a abertura da empresa é maior que 6 meses:
                                    if (DateTime.Compare(DateConverter(DataAbertura), System.DateTime.Now.AddMonths(-6)) < 0)
                                    {
                                        ///////RETORNA A DATA DE ABERTURA
                                        return DataAbertura;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("\nO Aeroporto não aceita cadastrar companhia aérea com menos de 6 meses de existência.");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("\nVocê será redirecionado para o menu anterior.");
                                        Pausa();

                                        //Retorna nullo direto, não tem a opção de digitar a data novamente
                                        return null;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Não aceita datas futuras, insira uma data válida!");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Por favor, insira uma data válida!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Erro: Por favor, insira uma data válida!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);
                    Console.Clear();
                    return null;


                #endregion


                case "datavoo":

                    #region DataVoo

                    do
                    {
                        try
                        {
                            ConsoleKeyInfo teclaData;
                            char[] vetortecla;
                            string DataVoo = null;
                            encontrado = false;
                            string[] vetordata = new string[] { "_", "_", "_", "_", "_", "_", "_", "_", " ", "_", "_", "_", "_" };

                            static void AtualizarTela(string[] vetordata)
                            {
                                Console.Clear();
                                Console.WriteLine("Insira a Data e hora do Voo:");
                                Console.WriteLine(vetordata[0] + vetordata[1] + "/" + vetordata[2] + vetordata[3] + "/" + vetordata[4] + vetordata[5] + vetordata[6] + vetordata[7] + " " + vetordata[9] + vetordata[10] + ":" + vetordata[11] + vetordata[12]);
                                Console.CursorVisible = false;
                            }


                            for (int i = 0; i < 8; i++)
                            {
                                AtualizarTela(vetordata);

                                teclaData = Console.ReadKey(true);

                                vetortecla = teclaData.Key.ToString().ToCharArray();

                                //Verifica se foi teclado realmente um número:
                                if (vetortecla[0] == 'N')
                                {
                                    if (vetornumeros.Contains(vetortecla[6].ToString()) == true)
                                    {
                                        encontrado = true;
                                        vetordata[i] = vetortecla[6].ToString();
                                        DataVoo = DataVoo + vetordata[i];
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }

                                    AtualizarTela(vetordata);
                                }
                                else
                                {
                                    if (vetortecla[0] == 'D')
                                    {
                                        if (vetornumeros.Contains(vetortecla[1].ToString()) == true)
                                        {
                                            encontrado = true;
                                            vetordata[i] = vetortecla[1].ToString();
                                            DataVoo = DataVoo + vetordata[i];
                                        }
                                        else
                                        {
                                            encontrado = false;
                                            break;
                                        }

                                        AtualizarTela(vetordata);
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }
                                }
                            }

                            //Se todas entradas foram números válidos, continua:
                            //A variável DataVoo nesse instante só tem datas sem barras, ex: "12345678":
                            if (encontrado == true)
                            {
                                //Pede os dados da hora separado da data:
                                for (int i = 9; i < 13; i++)
                                {
                                    AtualizarTela(vetordata);

                                    teclaData = Console.ReadKey(true);

                                    vetortecla = teclaData.Key.ToString().ToCharArray();

                                    //Verifica se foi teclado realmente um número:
                                    if (vetortecla[0] == 'N')
                                    {
                                        if (vetornumeros.Contains(vetortecla[6].ToString()) == true)
                                        {
                                            encontrado = true;
                                            vetordata[i] = vetortecla[6].ToString();
                                            DataVoo = DataVoo + vetordata[i];
                                        }
                                        else
                                        {
                                            encontrado = false;
                                            break;
                                        }

                                        AtualizarTela(vetordata);
                                    }
                                    else
                                    {
                                        if (vetortecla[0] == 'D')
                                        {
                                            if (vetornumeros.Contains(vetortecla[1].ToString()) == true)
                                            {
                                                encontrado = true;
                                                vetordata[i] = vetortecla[1].ToString();
                                                DataVoo = DataVoo + vetordata[i];
                                            }
                                            else
                                            {
                                                encontrado = false;
                                                break;
                                            }

                                            AtualizarTela(vetordata);
                                        }
                                    }
                                }

                                //Se dados da hora forem válidos, continua:
                                //A variável DataVoo nesse instante tem a data e a hora sem formatação, ex: "123456781234":
                                if (encontrado == true)
                                {
                                    //Verifica se a data de cadastro do voo não é data antiga:
                                    if (DateTime.Compare(DateHourConverter(DataVoo), System.DateTime.Now) > 0)
                                    {
                                        //////RETORNA A DATA DO VOO "123456781234"
                                        return DataVoo;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Não é possível agendar voo em datas passadas!");
                                        retornar = PausaMensagem();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Insira uma hora válida!");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Por favor, insira uma data válida!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Erro: Por favor, insira uma data válida!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    Console.Clear();
                    return null;

                #endregion


                case "idaeronave":

                    #region IdAeronave

                    //Os prefixos de nacionalidade que identificam aeronaves privadas e comerciais do Brasil são PT, PR, PP, PS e PH.
                    string[] prefixoaeronave = new string[] { "PT", "PR", "PP", "PS", "PH" };

                    //A Agência Nacional de Aviação Civil(Anac) proíbe o registro de marcas de identificação em aeronaves iniciadas com a letra Q
                    //ou que tenham W como segunda letra.Os arranjos SOS, XXX, PAN, TTT, VFR, IFR, VMC e IMC não podem ser utilizados.
                    string[] idproibido = new string[] { "SOS", "XXX", "PAN", "TTT", "VFR", "IFR", "VMC", "IMC" };
                    encontrado = false;
                    string idaeronave;

                    do
                    {
                        Console.Write("Informe o código Nacional de identificação da Aeronave: ");
                        try
                        {
                            idaeronave = Console.ReadLine().ToUpper();

                            char[] letras = idaeronave.ToCharArray();

                            //Verifica se tem 6 caracteres obrigatoriamente:
                            if (letras.Length == 6)
                            {
                                //verifica se foi inserido o traço - na inscrição:
                                if (letras[2] == '-')
                                {
                                    //Verifica se tem Q e W onde não pode na matrícula da aeronave:
                                    if (letras[3] != 'Q' && letras[4] != 'W')
                                    {
                                        //Separa a escrita depois do traço, referente à matrícula do avião:
                                        string matriculaaviao = letras[3].ToString() + letras[4].ToString() + letras[5].ToString();
                                        //Verifica se a matrícula possui um nome proibido, contido no vetor idproibido;
                                        if (idproibido.Contains(matriculaaviao) == false)
                                        {
                                            //Separa os 2 primeiros prefixos e guarda na variável prefixoaviao:
                                            string prefixoaviao = letras[0].ToString() + letras[1].ToString();
                                            //Verifica se os 2 primeiros prefixos são válidos:
                                            if (prefixoaeronave.Contains(prefixoaviao) == true)
                                            {
                                                foreach (var aeronave in listAeronaves)
                                                {
                                                    if (aeronave.Inscricao == idaeronave)
                                                    {
                                                        encontrado = true;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        encontrado = false;
                                                    }
                                                }

                                                if (encontrado == false)
                                                {
                                                    return idaeronave;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Essa Aeronave já possui cadastro!");
                                                    retornar = PausaMensagem();
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Os prefixos devem ser obrigatóriamente PT ou PR ou PP ou PS ou PH ");
                                                retornar = PausaMensagem();
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("As matrículas SOS, XXX, PAN, TTT, VFR, IFR, VMC e IMC não podem ser utilizadas");
                                            retornar = PausaMensagem();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Não é permitido a letra Q como primeira letra e nem a letra W como segunda letra da matrícula da aeronave");
                                        retornar = PausaMensagem();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Digite obrigatóriamente o traço - após prefixos de nacionalidade");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Digite obrigatóriamente o traço - Quantidade incorreta de dígitos de identificação.");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Insira um valor válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion


                case "capacidade":

                    #region Capacidade

                    encontrado = false;
                    string capacidade;
                    string[] vetorcapacidade = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", " " };
                    do
                    {
                        Console.Write("Informe a quantidade de passageiros que a aeronave comporta: ");
                        try
                        {
                            capacidade = Console.ReadLine();

                            //Cria um vetor onde cada casa é um caracter:
                            char[] caracteres = capacidade.ToString().ToCharArray();
                            char[] caraccapacidade = new char[3];

                            //Verifica se tem no máximo 3 caracteres obrigatoriamente:
                            if (caracteres.Length > 0 && caracteres.Length <= 3)
                            {
                                if (caracteres.Length == 1)
                                {
                                    caraccapacidade[0] = ' ';
                                    caraccapacidade[1] = ' ';
                                    caraccapacidade[2] = caracteres[0];
                                }
                                else
                                {
                                    if (caracteres.Length == 2)
                                    {
                                        caraccapacidade[0] = ' ';
                                        caraccapacidade[1] = caracteres[0];
                                        caraccapacidade[2] = caracteres[1];
                                    }
                                    else
                                    {
                                        if (caracteres.Length == 3)
                                        {
                                            caraccapacidade[0] = caracteres[0];
                                            caraccapacidade[1] = caracteres[1];
                                            caraccapacidade[2] = caracteres[2];
                                        }
                                    }
                                }

                                string cap = caraccapacidade[0].ToString() + caraccapacidade[1].ToString() + caraccapacidade[2].ToString();

                                for (int i = 0; i < 3; i++)
                                {
                                    if (vetorcapacidade.Contains(caraccapacidade[i].ToString()))
                                    {
                                        encontrado = true;
                                    }
                                    else
                                    {
                                        encontrado = false;
                                        break;
                                    }
                                }
                                if (encontrado == true)
                                {
                                    return cap;
                                }
                                else
                                {
                                    Console.WriteLine("Só aceita dígitos numéricos válidos!");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Deve ter um valor de quantidade de passageiros, só aceita no máximo 3 dígitos!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Insira apenas números!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;



                #endregion


                case "situacao":

                    #region Situacao


                    do
                    {
                        Console.Clear();
                        Console.WriteLine("A situação ficará no sistema como:\n[A] - Ativa\n[I] - Inativa");
                        Console.CursorVisible = false;
                        ConsoleKeyInfo op = Console.ReadKey(true);

                        //Verificar se tecla pressionada foi A ou I (independente do CAPSLOCK estar ativado!)
                        if (op.Key == ConsoleKey.A)
                        {
                            Console.Clear();
                            return "A";
                        }
                        else
                        {
                            if (op.Key == ConsoleKey.I)
                            {
                                Console.Clear();
                                return "I";
                            }
                            else
                            {
                                Console.WriteLine("Escolha uma opção válida!");
                                retornar = PausaMensagem();
                            }
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;


                #endregion

                case "destino":

                    #region Destino

                    do
                    {
                        retornar = false;

                        Console.Write("Informe o código IATA do aeroporto de destino: ");
                        try
                        {
                            string iata = Console.ReadLine().ToUpper();

                            if (listDestino.Contains(iata))
                            {
                                return iata;
                            }
                            else
                            {
                                Console.WriteLine("Código não encontrado! Insira um código IATA válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Insira um código IATA válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion


                case "aeronave":

                    #region Aeronave
                    encontrado = false;
                    do
                    {
                        retornar = false;
                        Aeronave a = null;

                        Console.Write("Informe o código Nacional de identificação da Aeronave: ");
                        try
                        {
                            idaeronave = Console.ReadLine().ToUpper();

                            foreach (var aeronave in listAeronaves)
                            {
                                if (aeronave.Inscricao == idaeronave)
                                {
                                    a = aeronave;
                                    encontrado = true;
                                    break;
                                }
                                else
                                {
                                    encontrado = false;
                                }
                            }
                            if (encontrado == true)
                            {
                                if (a.Situacao == 'A')
                                {
                                    return idaeronave;
                                }
                                else
                                {
                                    Console.WriteLine("Essa Aeronave encontra-se inativa no sistema.");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Código não encontrado! Insira um código válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("ERRO: Insira um código válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;


                #endregion


                case "aeronaveeditar":

                    #region AeronaveEditar
                    encontrado = false;
                    do
                    {
                        retornar = false;
                        Aeronave a = null;

                        Console.Write("Informe o código Nacional de identificação da Aeronave: ");
                        try
                        {
                            idaeronave = Console.ReadLine().ToUpper();

                            foreach (var aeronave in listAeronaves)
                            {
                                if (aeronave.Inscricao == idaeronave)
                                {
                                    a = aeronave;
                                    encontrado = true;
                                    break;
                                }
                                else
                                {
                                    encontrado = false;
                                }
                            }
                            if (encontrado == true)
                            {
                                return idaeronave;
                            }
                            else
                            {
                                Console.WriteLine("Código não encontrado! Insira um código válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("ERRO: Insira um código válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;


                #endregion




                case "valorpassagem":

                    #region ValorPassagem

                    do
                    {
                        try
                        {
                            Console.Write("insira o valor da passagem: ");
                            float valor = float.Parse(Console.ReadLine());
                            if (valor > 0 && valor < 10000)
                            {
                                return valor.ToString("N2");
                            }
                            else
                            {
                                Console.WriteLine("Não é possível vender passagens desse valor");
                                retornar = PausaMensagem();
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Escolha uma opção válida!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    return null;

                #endregion

                case "cpflogin":

                    #region CpflLogin

                    do
                    {
                        retornar = false;

                        Console.Write("Informe o CPF para prosseguir: ");
                        try
                        {
                            string cpf = Console.ReadLine();

                            //Retira o passageiro escolhido da lista de passageiros:
                            Passageiro p = null;
                            encontrado = false;
                            foreach (var passageiro in listPassageiro)
                            {
                                if (passageiro.Cpf == cpf)
                                {
                                    p = passageiro;
                                    encontrado = true;
                                    break;
                                }
                                else
                                {
                                    encontrado = false;
                                }
                            }
                            if (encontrado == true)
                            {
                                //Se situação estivar ativa, continua
                                if (p.Situacao == 'A')
                                {
                                    //Verifica se é maior de idade:
                                    if (DateTime.Compare(p.DataNascimento, System.DateTime.Now.AddYears(-18)) <= 0)
                                    {
                                        encontrado = false;
                                        //Procura na lista de restritos:
                                        foreach (var restrito in listRestritos)
                                        {
                                            if (p.Cpf == restrito)
                                            {
                                                encontrado = true;
                                                break;
                                            }
                                            else
                                            {
                                                encontrado = false;
                                            }
                                        }
                                        //Se não estiver na lista de restritos, retorna o cpf.
                                        if (encontrado == false)
                                        {
                                            return cpf;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Impossível prosseguir! Esse Passageiro se encontra restrito!");
                                            retornar = PausaMensagem();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Impossível prosseguir! Esse Passageiro é menor de idade!");
                                        retornar = PausaMensagem();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Passageiro Inativo no sistema!");
                                    retornar = PausaMensagem();
                                }
                            }
                            else
                            {
                                Console.WriteLine("CPF não encontrado! Insira um CPF válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Insira um CPF válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;



                #endregion


                case "cpfexiste":

                    #region CpfExiste

                    do
                    {
                        retornar = false;

                        Console.Write("Informe o CPF para prosseguir: ");
                        try
                        {
                            string cpf = Console.ReadLine();

                            //Consulta o cpf do passageiro escolhido da lista de passageiros:
                            encontrado = false;
                            foreach (var passageiro in listPassageiro)
                            {
                                if (passageiro.Cpf == cpf)
                                {
                                    encontrado = true;
                                    break;
                                }
                                else
                                {
                                    encontrado = false;
                                }
                            }
                            if (encontrado == true)
                            {
                                return cpf;
                            }
                            else
                            {
                                Console.WriteLine("CPF não encontrado! Insira um CPF válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("CPF Inválido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion

                case "idvoo":

                    #region IDVoo

                    do
                    {
                        retornar = false;

                        Console.Write("Informe o ID do Voo para prosseguir: ");
                        try
                        {
                            string idvoo = Console.ReadLine().ToUpper();

                            //Consulta o id do voo escolhido da lista de voos:
                            encontrado = false;
                            foreach (var voo in listVoo)
                            {
                                if (voo.IDVoo == idvoo)
                                {
                                    encontrado = true;
                                    break;
                                }
                                else
                                {
                                    encontrado = false;
                                }
                            }

                            if (encontrado == true)
                            {
                                return idvoo;
                            }
                            else
                            {
                                Console.WriteLine("ID do Voo não encontrado! Insira um código válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("ID do Voo inválido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;


                #endregion


                case "idvenda":

                    #region idvenda

                    do
                    {
                        encontrado = false;
                        retornar = false;

                        Console.Write("Informe o ID da venda: ");
                        try
                        {
                            string idvenda = Console.ReadLine().ToUpper();

                            foreach (var venda in listVenda)
                            {
                                if (venda.IDVenda == idvenda)
                                {
                                    encontrado = true;
                                    break;
                                }
                                else
                                {
                                    encontrado = false;
                                }
                            }

                            if (encontrado == true)
                            {
                                return idvenda;
                            }
                            else
                            {
                                Console.WriteLine("Código não encontrado! Insira um ID Venda válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Insira um código ID Venda válido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);

                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;

                #endregion

                case "cnpjexiste":

                    #region cnpjogin
                    do
                    {
                        retornar = false;
                        encontrado = false;
                        CompanhiaAerea c = null;
                        Console.Write("Informe o CNPJ para prosseguir: ");
                        try
                        {
                            string cnpj = Console.ReadLine();

                            foreach (var companhia in listCompanhia)
                            {
                                if (companhia.Cnpj == cnpj)
                                {
                                    c = companhia;
                                    encontrado = true;
                                    break;
                                }
                                else
                                {
                                    encontrado = false;
                                }
                            }

                            if (encontrado == true)
                            {
                                return cnpj;
                            }
                            else
                            {
                                Console.WriteLine("CNPJ não encontrado! Insira um CNPJ válido!");
                                retornar = PausaMensagem();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("CPF Inválido!");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == false);
                    //Retorna nulo se o usuário quiser cancelar no meio do cadastro;
                    return null;
                #endregion

                default:
                    return null;
            }
        }

        static string ValorConverter(float valor)
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

        static void TelaInicial()
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("Bem vindo à On The Fly!");
                Console.WriteLine("\nPor Favor, informe a Opção Desejada:\n");
                Console.WriteLine(" 1 - Companhia Aérea\n");
                Console.WriteLine(" 2 - Passageiro\n");
                Console.WriteLine(" 3 - Compras de Passagens\n");
                Console.WriteLine(" 4 - Acesso a Lista de CPF Restritos\n");
                Console.WriteLine(" 5 - Acesso a Lista de CNPJ Restritos\n");
                Console.WriteLine(" 6 - Aeronaves\n");
                Console.WriteLine(" 7 - Voos Realizados\n");
                Console.WriteLine(" 8 - Voos Disponíveis");
                Console.WriteLine("\n 0 - Encerrar Sessão\n");
                opc = int.Parse(ValidarEntrada("menu"));
                Console.Clear();

                switch (opc)
                {
                    case 0:
                        Console.WriteLine("Encerrado");
                        GravarPassageiro();
                        GravarCompanhiaAerea();
                        GravarAeronaves();
                        GravarPassagem();
                        GravarVenda();
                        GravarVoo();
                        GravarItemVenda();
                        GravarBloqueados();
                        GravarRestritos();
                        GravarVooRealizado();
                        Environment.Exit(0);
                        break;

                    case 1:
                        TelaInicialCompanhiasAereas();
                        break;

                    case 2:
                        TelaInicialPassageiros();
                        break;

                    case 3:

                        TelaVendas();

                        break;

                    case 4:
                        TelaInicialCpfRestritos();
                        break;

                    case 5:
                        TelaInicialCnpjRestritos();
                        break;

                    case 6:
                        TelaVerAeronavesCadastradas();
                        break;

                    case 7:
                        foreach (var voorealizado in voosrealizados)
                        {
                            Console.WriteLine(voorealizado);
                        }
                        Pausa();
                        break;

                    case 8:
                        foreach (var Voo in listVoo)
                        {
                            if (Voo.Situacao == 'A')
                            {
                                Console.WriteLine("IDVoo: " + Voo.IDVoo + " Destino: " + Voo.Destino + " Data e Hora do Voo: " + Voo.DataVoo.ToString("dd/MM/yyyy HH:mm"));
                            }
                        }
                        Pausa();
                        break;
                }

            } while (opc != 0);

        }

        static void TelaInicialPassageiros()
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("\nInforme a Opção Desejada:\n");
                Console.WriteLine(" 1 - Passageiro já cadastrado\n");
                Console.WriteLine(" 2 - Cadastrar um novo Passageiro\n");
                Console.WriteLine("\n 0 - SAIR\n");
                opc = int.Parse(ValidarEntrada("menu"));
                Console.Clear();

                switch (opc)
                {
                    case 0:
                        TelaInicial();
                        break;

                    case 1:
                        TelaLoginPassageiro();
                        break;

                    case 2:
                        TelaCadastrarPassageiro();
                        break;
                }

            } while (opc != 0);
        } 

        static void TelaLoginPassageiro()
        {
            string cpf;
            Passageiro passageiroAtivo;
            Console.Clear();
            cpf = ValidarEntrada("cpfexiste");
            if (cpf == null) TelaInicialPassageiros();
            foreach (Passageiro passageiro in listPassageiro)
            {
                if (passageiro.Cpf == cpf)
                {
                    passageiroAtivo = passageiro;
                    TelaEditarPassageiro(passageiroAtivo);
                }
            }
        }

        static void TelaCadastrarPassageiro()
        {
            do
            {
                string nome, cpf;
                string dataNascimento;
                char sexo;

                nome = ValidarEntrada("nome");
                if (nome == null) TelaInicialPassageiros();

                cpf = ValidarEntrada("cpf");
                if (cpf == null) TelaInicialPassageiros();

                dataNascimento = ValidarEntrada("datanascimento");
                if (dataNascimento == null) TelaInicialPassageiros();

                sexo = char.Parse(ValidarEntrada("sexo"));
                if (sexo.Equals(null)) TelaInicialPassageiros();


                Passageiro passageiro = new Passageiro(cpf, nome, DateConverter(dataNascimento), sexo, System.DateTime.Now, System.DateTime.Now, 'A');
                listPassageiro.Add(passageiro);
                GravarPassageiro();
                Console.WriteLine("\nPassageiro Cadastrado com Sucesso!");
                Pausa();
                TelaInicialPassageiros();

            } while (true);
        }

        static void TelaEditarPassageiro(Passageiro passageiroAtivo)
        {
            int opc;
            string novoNome;
            string novaDataNascimento;
            DateTime data;
            char novoSexo;
            char novaSituacao;

            do
            {
                Console.Clear();
                Console.WriteLine("\nEDTAR DADOS");
                Console.WriteLine("\nEscolha qual Dado deseja Editar: ");
                Console.Write("\n 1 - Nome");
                Console.Write("\n 2 - Data de Nascimento");
                Console.Write("\n 3 - Sexo");
                Console.Write("\n 4 - Situação (Ativo / Inativo)");
                Console.Write("\n\n 0 - Sair");
                opc = int.Parse(ValidarEntrada("menu"));

                switch (opc)
                {
                    case 0:
                        TelaInicial();
                        break;

                    case 1:

                        Console.Clear();
                        Console.WriteLine("\nNome Atual: " + passageiroAtivo.Nome);
                        Console.Write("\n\nInforme o Novo Nome");
                        Pausa();
                        novoNome = ValidarEntrada("nome");
                        if (novoNome == null) TelaEditarPassageiro(passageiroAtivo);

                        passageiroAtivo.Nome = novoNome;
                        GravarPassageiro();
                        Console.Clear();
                        Console.WriteLine("\nNome Alterado com Sucesso!");
                        Pausa();
                        TelaEditarPassageiro(passageiroAtivo);

                        break;

                    case 2:

                        Console.Clear();
                        Console.WriteLine("\nData de nascimento Atual: " + passageiroAtivo.DataNascimento.ToShortDateString());
                        Console.Write("\n\nInforme a Nova Data de Nascimento");
                        Pausa();
                        novaDataNascimento = ValidarEntrada("datanascimento");
                        if (novaDataNascimento == null) TelaEditarPassageiro(passageiroAtivo);

                        data = DateConverter(novaDataNascimento);
                        passageiroAtivo.DataNascimento = data;
                        GravarPassageiro();
                        Console.Clear();
                        Console.WriteLine("\nData de Nascimento Alterada com Sucesso!");
                        Pausa();
                        TelaEditarPassageiro(passageiroAtivo);

                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("\nSexo Atual: " + passageiroAtivo.Sexo);
                        Console.Write("\n\nInforme o Novo Sexo");
                        Pausa();
                        novoSexo = char.Parse(ValidarEntrada("sexo"));
                        if (novoSexo.Equals(null)) TelaInicialPassageiros();

                        passageiroAtivo.Sexo = novoSexo;
                        GravarPassageiro();
                        Console.Clear();
                        Console.WriteLine("\nSexo Alterado com Sucesso!");
                        Pausa();
                        TelaEditarPassageiro(passageiroAtivo);
                        break;


                    case 4:

                        Console.Clear();
                        Console.WriteLine("\nPASSAGEIRO: " + passageiroAtivo.Nome);
                        if (passageiroAtivo.Situacao == 'A')
                        { Console.WriteLine("\nSituação Atual: ATIVO"); }

                        if (passageiroAtivo.Situacao == 'I')
                        { Console.WriteLine("\nSituação Atual: INATIVO"); }

                        Pausa();

                        novaSituacao = char.Parse(ValidarEntrada("situacao"));
                        if (novaSituacao.Equals(null)) TelaInicialPassageiros();

                        passageiroAtivo.Situacao = novaSituacao;
                        GravarPassageiro();
                        Console.Clear();
                        Console.WriteLine("\nSituação de Cadastro Alterada com Sucesso!");
                        Pausa();
                        TelaEditarPassageiro(passageiroAtivo);
                        break;
                }

            } while (true);
        }

        static void TelaEditarCompanhiaAerea(CompanhiaAerea companhiaAerea)
        {
            DateTime datanova;
            int opc;
            char novaSituacao;
            string novadata;

            do
            {
                Console.Clear();
                Console.WriteLine("\nEDTAR DADOS");
                Console.WriteLine("\nEscolha qual Dado deseja Editar: ");
                Console.Write("\n 1 - Data de Abertura\n");
                Console.Write("\n 2 - Situação (Ativo / Inativo)\n\n");
                Console.Write("\n 0 - Voltar");
                opc = int.Parse(ValidarEntrada("menu"));
                Console.Clear();

                switch (opc)
                {
                    case 0:
                        TelaOpcoesCompanhiaAerea(companhiaAerea);
                        break;

                    case 1:
                        Console.WriteLine("\nData de abertura Atual: " + companhiaAerea.DataAbertura.ToString("dd/MM/yyyy"));
                        Console.Write("\n\nInforme a nova data");
                        Pausa();
                        novadata = ValidarEntrada("dataabertura");
                        if (novadata == null) TelaEditarCompanhiaAerea(companhiaAerea);
                        datanova = DateConverter(novadata);

                        companhiaAerea.DataAbertura = datanova;
                        GravarCompanhiaAerea();
                        Console.Clear();
                        Console.WriteLine("\nData de abertura alterada com Sucesso!");
                        Pausa();
                        TelaEditarCompanhiaAerea(companhiaAerea);
                        break;

                    case 2:
                        Console.WriteLine("\nCompanhia Aérea: " + companhiaAerea.RazaoSocial);
                        if (companhiaAerea.Situacao == 'A')
                        { Console.WriteLine("\nSituação Atual: ATIVA"); }

                        if (companhiaAerea.Situacao == 'I')
                        { Console.WriteLine("\nSituação Atual: INATIVA"); }

                        Pausa();

                        novaSituacao = char.Parse(ValidarEntrada("situacao"));
                        if (novaSituacao.Equals(null)) TelaEditarCompanhiaAerea(companhiaAerea);

                        companhiaAerea.Situacao = novaSituacao;
                        GravarCompanhiaAerea();
                        Console.Clear();
                        Console.WriteLine("\nSituação de Cadastro Alterada com Sucesso!");
                        Pausa();
                        TelaEditarCompanhiaAerea(companhiaAerea);
                        break;

                }

            } while (true);
        }

        static void TelaInicialCpfRestritos()
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("\n'CPF' RESTRITOS");
                Console.WriteLine("\nInforme a Opção Desejada:\n");
                Console.WriteLine(" 1 - Ver a Lista de 'CPF' Restritos\n");
                Console.WriteLine(" 2 - Adicionar um 'CPF' à Lista de Restritos\n");
                Console.WriteLine(" 3 - Remover um 'CPF' da Lista de Restritos\n");
                Console.WriteLine("\n 0 - Sair\n");

                opc = int.Parse(ValidarEntrada("menu"));
                Console.Clear();

                switch (opc)
                {
                    case 0:

                        TelaInicial();

                        break;

                    case 1:
                        if (listRestritos.Count == 0)
                        {
                            Console.WriteLine("LISTA VAZIA!");
                            Pausa();
                            TelaInicialCpfRestritos();
                        }
                        else
                        {
                            foreach (var cpfRest in listRestritos)
                            {
                                Console.WriteLine(cpfRest);
                            }
                            Pausa();
                            TelaInicialCpfRestritos();
                        }
                        break;
                    case 2:
                        Console.WriteLine("Só é possível adicionar na lista de restritos um CPF cadastrado no sistema!\n");
                        Pausa();
                        string adcCpf = ValidarEntrada("cpfexiste");
                        if (adcCpf == null) TelaInicialCpfRestritos();
                        listRestritos.Add(adcCpf);
                        GravarRestritos();
                        Console.Clear();
                        Console.WriteLine("Cpf adiconado com sucesso");
                        Pausa();
                        TelaInicialCpfRestritos();
                        break;

                    case 3:
                        if (listRestritos.Count == 0)
                        {
                            Console.WriteLine("LISTA VAZIA!");
                            Console.ReadKey();
                            TelaInicialCpfRestritos();
                        }
                        else
                        {
                            string Cpf = ValidarEntrada("cpfexiste");
                            if (Cpf == null) TelaInicialCpfRestritos();
                            foreach (var cpfRestrito in listRestritos)
                            {
                                if (cpfRestrito == Cpf)
                                {
                                    listRestritos.Remove(cpfRestrito);
                                    GravarRestritos();
                                    Console.Clear();
                                    Console.WriteLine("Cpf Removido com sucesso!");
                                    Pausa();
                                    TelaInicialCpfRestritos();
                                }
                            }
                            Console.Clear();
                            Console.WriteLine("CPF não encontrado!");
                            Pausa();
                            TelaInicialCpfRestritos();
                        }


                        break;
                }

            } while (true);
        }

        static void TelaInicialCnpjRestritos()
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("\n'CNPJ' RESTRITOS");
                Console.WriteLine("\nInforme a Opção Desejada:\n");
                Console.WriteLine(" 1 - Ver a Lista de 'CNPJ' Bloqueados\n");
                Console.WriteLine(" 2 - Adicionar um 'CNPJ' à Lista de Bloqueados\n");
                Console.WriteLine(" 3 - Remover um 'CNPJ' da Lista de Bloqueados\n");
                Console.WriteLine("\n 0 - Sair\n");

                opc = int.Parse(ValidarEntrada("menu"));
                Console.Clear();

                switch (opc)
                {
                    case 0:

                        TelaInicial();
                        break;

                    case 1:
                        if (listBloqueados.Count == 0)
                        {
                            Console.WriteLine("LISTA VAZIA!");
                            Pausa();
                            TelaInicialCnpjRestritos();
                        }
                        else
                        {
                            foreach (var cnpjbloq in listBloqueados)
                            {
                                Console.WriteLine(cnpjbloq);
                            }
                            Pausa();
                            TelaInicialCnpjRestritos();
                        }
                        break;

                    case 2:
                        Console.WriteLine("Só é possível adicionar na lista de bloqueados um CNPJ cadastrado no sistema!\n");
                        Pausa();
                        string adcCnpj = ValidarEntrada("cnpjexiste");
                        if (adcCnpj == null) TelaInicialCnpjRestritos();
                        listBloqueados.Add(adcCnpj);
                        GravarBloqueados();
                        Console.WriteLine("CNPJ adiconado com sucesso!");
                        Pausa();
                        TelaInicialCnpjRestritos();
                        break;

                    case 3:
                        string Cnpj = ValidarEntrada("cnpjexiste");
                        if (Cnpj == null) TelaInicialCnpjRestritos();

                        if (listBloqueados.Count == 0)
                        {
                            Console.WriteLine("LISTA VAZIA!");
                            Pausa();
                            TelaInicialCnpjRestritos();
                        }
                        else
                        {
                            foreach (var cnpjBloqueado in listBloqueados)
                            {
                                if (cnpjBloqueado == Cnpj)
                                {
                                    listBloqueados.Remove(cnpjBloqueado);
                                    GravarBloqueados();
                                    Console.Clear();
                                    Console.WriteLine("CNPJ Removido com sucesso!");
                                    Pausa();
                                    TelaInicialCnpjRestritos();
                                }
                            }
                            Console.Clear();
                            Console.WriteLine(Cnpj + " não encontrado!");
                            Pausa();
                            TelaInicialCnpjRestritos();
                        }
                        break;
                }

            } while (true);
        }

        static void TelaInicialCompanhiasAereas()
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("\nInforme a Opção Desejada:\n");
                Console.WriteLine(" 1 - Companhia Aérea já cadastrada\n");
                Console.WriteLine(" 2 - Cadastrar uma Nova Companhia Aérea\n");
                Console.WriteLine("\n 0 - SAIR\n");
                opc = int.Parse(ValidarEntrada("menu"));
                Console.Clear();

                switch (opc)
                {
                    case 0:

                        TelaInicial();

                        break;

                    case 1:

                        TelaLoginCompanhiaAerea();

                        break;

                    case 2:

                        TelaCadastrarCompanhiaAerea();

                        break;
                }

            } while (opc != 0);
        }

        static void TelaLoginCompanhiaAerea()
        {
            string cnpj;
            CompanhiaAerea compAtivo;
            Console.Clear();
            Console.WriteLine("\nInforme o 'CNPJ' para Entrar\n");
            Pausa();
            cnpj = ValidarEntrada("cnpjexiste");
            if (cnpj == null) TelaInicialCompanhiasAereas();

            foreach (CompanhiaAerea companhia in listCompanhia)
            {
                if (companhia.Cnpj == cnpj)
                {
                    compAtivo = companhia;
                    TelaOpcoesCompanhiaAerea(compAtivo);
                }
            }

        }

        static void TelaCadastrarCompanhiaAerea()
        {
            string nomeComp;
            string cnpj;
            string dataAbertura;

            nomeComp = ValidarEntrada("nome");
            if (nomeComp == null) TelaInicialCompanhiasAereas();

            cnpj = ValidarEntrada("cnpj");
            if (cnpj == null) TelaInicialCompanhiasAereas();

            dataAbertura = ValidarEntrada("dataabertura");
            if (dataAbertura == null) TelaInicialCompanhiasAereas();

            CompanhiaAerea novaComp = new CompanhiaAerea(cnpj, nomeComp, DateConverter(dataAbertura), System.DateTime.Now, System.DateTime.Now, 'A');
            listCompanhia.Add(novaComp);
            GravarCompanhiaAerea();
            TelaInicialCompanhiasAereas();


        }

        static void TelaOpcoesCompanhiaAerea(CompanhiaAerea compAtivo)
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("\nCompanhia Aérea " + compAtivo.RazaoSocial);
                Console.WriteLine("\nPor Favor, informe a Opção Desejada:\n");
                Console.WriteLine(" 1 - Cadastrar uma nova Aeronave\n");
                Console.WriteLine(" 2 - Programar um novo Voo\n");
                Console.WriteLine(" 3 - Cancelar um voo\n");
                Console.WriteLine(" 4 - Ativar/Inativar Aeronave\n");
                Console.WriteLine(" 5 - Editar dados da Companhia Aerea\n");

                Console.WriteLine("\n 0 - Encerrar Sessão\n");
                opc = int.Parse(ValidarEntrada("menu"));
                Console.Clear();

                switch (opc)
                {
                    case 0:

                        TelaInicial();

                        break;

                    case 1:

                        TelaCadastrarAeronave(compAtivo);

                        break;

                    case 2:

                        TelaCadastrarVoo(compAtivo);

                        break;


                    case 3:
                        foreach (var Voo in listVoo)
                        {
                            if (Voo.Situacao == 'A')
                            {
                                Console.WriteLine("IDVoo: " + Voo.IDVoo + " Destino: " + Voo.Destino + " Data e Hora do Voo: " + Voo.DataVoo.ToString("dd/MM/yyyy HH:mm"));
                            }
                        }
                        Console.WriteLine("\n----------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n1 - Escolher o Voo Desejado: ");
                        Console.WriteLine("0 - Voltar");
                        opc = int.Parse(ValidarEntrada("menu"));
                        Console.Clear();

                        switch (opc)
                        {
                            case 0:
                                TelaOpcoesCompanhiaAerea(compAtivo);
                                break;
                            case 1:
                                Console.Clear();
                                string idvoo = ValidarEntrada("idvoo");
                                if (idvoo == null) TelaOpcoesCompanhiaAerea(compAtivo);

                                foreach (var voo in listVoo)
                                {
                                    if (voo.IDVoo == idvoo)
                                    {
                                        voo.Situacao = 'C';
                                        Console.WriteLine("Voo CANCELADO!! Um novo Voo deve ser cadastrado.");
                                        Pausa();
                                        TelaOpcoesCompanhiaAerea(compAtivo);
                                    }

                                }

                                break;
                        }

                        break;


                    case 4:

                        TelaEditarAeronave(compAtivo);


                        break;

                    case 5:

                        TelaEditarCompanhiaAerea(compAtivo);

                        break;
                }


            } while (true);
        }

        static void TelaCadastrarAeronave(CompanhiaAerea compAtivo)
        {
            string idAeronave;
            int capacidade;

            char situacao;
            Aeronave novaAeronave;

            idAeronave = ValidarEntrada("idaeronave");
            if (idAeronave == null) TelaOpcoesCompanhiaAerea(compAtivo);

            capacidade = int.Parse(ValidarEntrada("capacidade"));
            if (capacidade.Equals(null)) TelaOpcoesCompanhiaAerea(compAtivo);

            situacao = char.Parse(ValidarEntrada("situacao"));
            if (situacao.Equals(null)) TelaOpcoesCompanhiaAerea(compAtivo);

            novaAeronave = new Aeronave(idAeronave, capacidade, 0, System.DateTime.Now, System.DateTime.Now, situacao, compAtivo.Cnpj, compAtivo.RazaoSocial);
            listAeronaves.Add(novaAeronave);
            GravarAeronaves();
            Console.WriteLine("\nCadastro Realizado com Sucesso!");
            Pausa();
            TelaOpcoesCompanhiaAerea(compAtivo);
        }

        static void TelaCadastrarVoo(CompanhiaAerea compAtivo)
        {
            Console.Clear();
            string idVoo;
            string destino;
            string idAeronave;
            string auxData;
            DateTime dataVoo;
            float valor;

            if (compAtivo.Situacao == 'A')
            {
                destino = ValidarEntrada("destino");
                if (destino == null) TelaOpcoesCompanhiaAerea(compAtivo);

                idAeronave = ValidarEntrada("aeronave");
                if (idAeronave == null) TelaOpcoesCompanhiaAerea(compAtivo);

                auxData = ValidarEntrada("datavoo");
                if (auxData == null) TelaOpcoesCompanhiaAerea(compAtivo);
                dataVoo = DateHourConverter(auxData);

                valor = float.Parse(ValidarEntrada("valorpassagem"));
                if (valor.Equals(null)) TelaOpcoesCompanhiaAerea(compAtivo);

                idVoo = GeradorId("idvoo");
                if (idVoo == null) TelaOpcoesCompanhiaAerea(compAtivo);

                Voo novoVoo = new Voo(idVoo, destino, idAeronave, dataVoo, System.DateTime.Now, 'A');
                listVoo.Add(novoVoo);
                GravarVoo();
                Aeronave a = null;
                foreach (var aeronave in listAeronaves)
                {
                    if (aeronave.Inscricao == idAeronave)
                    {
                        a = aeronave;
                        break;
                    }
                }
                //Gerador de passagens
                List<string> idsPassagem = GeradorIdPassagens(a.Capacidade);
                for (int i = 0; i < a.Capacidade; i++)
                {
                    PassagemVoo passagem = new PassagemVoo(idsPassagem[i], idVoo, System.DateTime.Now, valor, 'L');
                    listPassagem.Add(passagem);
                    GravarPassagem();
                }
                Console.WriteLine("\nCadastro Realizado com Sucesso!");
                Pausa();
                TelaOpcoesCompanhiaAerea(compAtivo);
            }
            else
            {
                Console.WriteLine("\nCompanhia INATIVA no sistema, não pode cadastrar voo!");
                Pausa();
                TelaOpcoesCompanhiaAerea(compAtivo);
            }
        }

        static void TelaVerAeronavesCadastradas()
        {
            Console.Clear();

            foreach (var aeronave in listAeronaves)
            {
                Console.WriteLine(aeronave.ToString() + "\n");
            }
            Pausa();
            TelaInicial();
        }

        static void TelaEditarAeronave(CompanhiaAerea compAtivo)
        {
            string idaeronave = ValidarEntrada("aeronaveeditar");
            if (idaeronave == null) TelaOpcoesCompanhiaAerea(compAtivo);

            string situacao = ValidarEntrada("situacao");
            if (situacao == null) TelaOpcoesCompanhiaAerea(compAtivo);
            char s = char.Parse(situacao);

            foreach (var aeronave in listAeronaves)
            {
                if (aeronave.Inscricao == idaeronave)
                {
                    aeronave.Situacao = s;
                }
            }
            Console.Clear();
            Console.WriteLine("Situação alterada com sucesso!");
            Pausa();
            TelaOpcoesCompanhiaAerea(compAtivo);
        }

        static void TelaVendas()
        {

            int opc;
            Console.WriteLine("Informe a opção desejada: \n");
            Console.WriteLine("1 - Compra de Passagem\n");
            Console.WriteLine("2 - Ver Passagens Vendidas\n");
            Console.WriteLine("3 - Ver Passagens Reservadas\n");
            Console.WriteLine("\n0 -  SAIR\n");
            opc = int.Parse(ValidarEntrada("menu"));
            Console.Clear();

            switch (opc)
            {
                case 0:
                    TelaInicialPassageiros();
                    break;
                case 1:
                    string cpfLogin = ValidarEntrada("cpflogin");
                    if (cpfLogin == null) TelaInicial();
                    Passageiro passageiroAtivo = null;
                    foreach (var passageiro in listPassageiro)
                    {
                        if (passageiro.Cpf == cpfLogin)
                        {
                            passageiroAtivo = passageiro;
                            break;
                        }
                    }
                    Console.Clear();
                    TelaVoosDisponiveis(passageiroAtivo);
                    break;
                case 2:
                    TelaHistoricoVendas();
                    break;
                case 3:
                    TelaHistoricoReservadas();
                    break;
            }
        }

        static void TelaVoosDisponiveis(Passageiro passageiroAtivo)
        {
            int opc;
            foreach (var Voo in listVoo)
            {
                if (Voo.Situacao == 'A')
                {
                    Console.WriteLine("IDVoo: " + Voo.IDVoo + " Destino: " + Voo.Destino + " Data e Hora do Voo: " + Voo.DataVoo.ToString("dd/MM/yyyy HH:mm"));
                }
            }
            Console.WriteLine("\n----------------------------------------------------------------------------------------------");
            Console.WriteLine("\n1 - Escolher o Voo Desejado: ");
            Console.WriteLine("0 - Voltar");
            opc = int.Parse(ValidarEntrada("menu"));
            Console.Clear();

            switch (opc)
            {
                case 0:
                    TelaVendas();
                    break;
                case 1:
                    Console.Clear();
                    string idvoo = ValidarEntrada("idvoo");
                    if (idvoo == null) TelaVoosDisponiveis(passageiroAtivo);
                    TelaDescricaoVoo(idvoo, passageiroAtivo);
                    break;
            }
        }

        static void TelaDescricaoVoo(string idvoo, Passageiro passageiroAtivo)
        {

            int opc;
            Voo vooatual = null;
            foreach (var voo in listVoo)
            {
                if (voo.IDVoo == idvoo)
                {
                    vooatual = voo;
                    break;
                }
                else
                {
                    vooatual = null;
                }
            }
            Console.WriteLine(vooatual.ToString());
            Console.WriteLine("\n----------------------------------------------------------------------------------------------");
            Console.WriteLine("1 - Comprar: ");
            Console.WriteLine("2 - Reservar: ");
            Console.WriteLine("0 - Voltar: ");
            opc = int.Parse(ValidarEntrada("menu"));
            Console.Clear();

            switch (opc)
            {
                case 0:
                    TelaVoosDisponiveis(passageiroAtivo);
                    break;
                case 1:
                    int cont = 0;
                    bool retornar = false;
                    int quantPassagem;
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("\nInforme a quantidade de passagens (máximo 4): \n1  2  3  4");
                        quantPassagem = int.Parse(ValidarEntrada("menu"));
                        if (quantPassagem > 0 && quantPassagem <= 4)
                        {
                            foreach (var passagem in listPassagem)
                            {
                                if (passagem.IDVoo == idvoo && passagem.Situacao == 'L')
                                {
                                    cont++;
                                }
                            }
                            if (cont >= quantPassagem)
                            {
                                cont = 0;
                                PassagemVoo p = null;
                                foreach (var passagem in listPassagem)
                                {
                                    if (passagem.IDVoo == idvoo && passagem.Situacao == 'L')
                                    {
                                        p = passagem;
                                        passagem.Situacao = 'P';
                                        passagem.DataUltimaOperacao = System.DateTime.Now;
                                        GravarPassagem();
                                        ItemVenda item = new ItemVenda(GeradorId("iditemvenda"), passagem.IDPassagem, passagem.Valor, passageiroAtivo.Cpf, passageiroAtivo.Nome);
                                        listItemVenda.Add(item);
                                        GravarItemVenda();
                                        cont++;
                                    }

                                    if (cont == quantPassagem)
                                    {
                                        retornar = true;
                                        Venda venda = new Venda(GeradorId("idvenda"), System.DateTime.Now, passageiroAtivo.Cpf, (p.Valor * quantPassagem));
                                        listVenda.Add(venda);
                                        GravarVenda();
                                        passageiroAtivo.DataUltimaCompra = System.DateTime.Now;
                                        GravarPassageiro();

                                        string idAeronave = null;
                                        foreach (var voo in listVoo)
                                        {
                                            if (idvoo == voo.IDVoo)
                                            {
                                                idAeronave = voo.IDVoo;
                                                break;
                                            }
                                        }
                                        foreach (var aeronave in listAeronaves)
                                        {
                                            if (aeronave.Inscricao == idAeronave)
                                            {
                                                aeronave.AssentosOcupados = aeronave.AssentosOcupados + quantPassagem;
                                                aeronave.UltimaVenda = System.DateTime.Now;
                                            }
                                        }
                                        GravarAeronaves();

                                        Console.WriteLine("Compra realizada com sucesso!");
                                        Pausa();
                                        TelaVendas();
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Não possui esta quantidade de passagens disponíveis: ");
                                retornar = PausaMensagem();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Só é possível comprar [4] passagens por venda");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == true);
                    TelaHistoricoVendas();
                    break;
                case 2:

                    cont = 0;
                    retornar = false;
                    do
                    {
                        Console.WriteLine("\nInforme a quantidade de passagens para reserva (máximo 4): \n1  2  3  4");
                        quantPassagem = int.Parse(ValidarEntrada("menu"));
                        if (quantPassagem > 0 && quantPassagem <= 4)
                        {
                            foreach (var passagem in listPassagem)
                            {
                                if (passagem.IDVoo == idvoo && passagem.Situacao == 'L')
                                {
                                    cont++;
                                }
                            }
                            if (cont >= quantPassagem)
                            {
                                cont = 0;
                                PassagemVoo p = null;
                                foreach (var passagem in listPassagem)
                                {
                                    if (passagem.IDVoo == idvoo && passagem.Situacao == 'L')
                                    {
                                        p = passagem;
                                        passagem.Situacao = 'R';
                                        passagem.DataUltimaOperacao = System.DateTime.Now;
                                        GravarPassagem();
                                        cont++;
                                    }
                                    if (cont == quantPassagem) break;
                                }
                                Console.Clear();
                                Console.WriteLine("Reserva realizada com sucesso!");
                                Pausa();
                                TelaVendas();
                            }
                            else
                            {
                                Console.WriteLine("Não possui esta quantidade de passagens disponíveis: ");
                                retornar = PausaMensagem();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Só é possível reservar [4] passagens por venda");
                            retornar = PausaMensagem();
                        }
                    } while (retornar == true);
                    TelaHistoricoReservadas();
                    break;
            }
        }

        static void TelaHistoricoVendas()
        {
            int opc;
            foreach (var venda in listVenda)
            {
                Console.WriteLine("ID Venda: " + venda.IDVenda + " Valor: " + venda.ValorTotal + " Data da Venda: " + venda.DataVenda);
            }
            Console.WriteLine("\n----------------------------------------------------------------------------------------------");
            Console.WriteLine("\n1 - Detalhes da Venda: ");
            Console.WriteLine("0 - Voltar");
            opc = int.Parse(ValidarEntrada("menu"));
            Console.Clear();

            switch (opc)
            {
                case 0:
                    TelaVendas();
                    break;
                case 1:
                    TelaDescricaoItemVenda();
                    break;
            }
        }

        static void TelaDescricaoItemVenda()
        {

            string idvenda = ValidarEntrada("idvenda");
            if (idvenda == null) TelaVendas();
            foreach (var itemVenda in listItemVenda)
            {
                if (itemVenda.IDItemVenda == idvenda)
                {
                    Console.WriteLine(itemVenda.ToString());
                }
            }
            Pausa();
            TelaVendas();

        }

        static void TelaHistoricoReservadas()
        {
            foreach (var passagem in listPassagem)
            {
                if (passagem.Situacao == 'R')
                {
                    Console.WriteLine("ID Passagem: " + passagem.IDPassagem + " ID Voo: " + passagem.IDVoo + " Valor: " + passagem.Valor + " Data da Reserva: " + passagem.DataUltimaOperacao);
                }

            }
            Console.WriteLine("\n---------------------------------------------------------------------------------------------");

            Pausa();
            TelaVendas();
        }


        static void Main(string[] args)
        {
            System.IO.Directory.CreateDirectory(@"C:\DBOnTheFly");
            CarregarArquivos();
            Atualizar();
            Console.WriteLine("Carregado base de dados");
            Pausa();
            TelaInicial();
        }
    }
}