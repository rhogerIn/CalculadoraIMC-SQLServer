using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace wesallen
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection con;
            SqlDataReader reader;

            int cod_client;
            string acao = "";
            double peso, altura, imc;
            string nome, resultado = "";
            int idade;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("************************************************************************************************************************");
            Console.WriteLine("Development Date: 22/07/2020 \nAuthor: Rogério  Souza \nIDE and Tools: Visual Studio, SQL Server, C# \n");
            Console.WriteLine("*****************************           BMI Calculation Environment          *******************************************");
            Console.WriteLine("  .:: Pressione N / C / S ::.\n");
            Console.WriteLine("N - Novo Calculo / New  ");
            Console.WriteLine("C - Consultar um usuário / Consult ");
            Console.WriteLine("S - Sair / Exit \n");
            Console.WriteLine("************************************************************************************************************************");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Pressione a tecla desejada: ");
            Console.ResetColor();

            acao = Console.ReadLine().ToUpper();
            Console.WriteLine();

            while (acao != "S")
            {   
                /*Inicio da tela de calcular os códigos*/
                if (acao == "N")
                {
                    

                    Console.Clear();

                    Console.WriteLine("************************************************************************************************************************");
                    Console.WriteLine("                               PREENCHA AS INFORMAÇÕES E OBTENHA O CALCULO IMC                                          ");
                    Console.WriteLine("\nOBS: Para casas decimais em peso e altura utilize virgula");
                    Console.WriteLine("************************************************************************************************************************");

                    Console.Write("Informe o nome: ");
                    nome = Console.ReadLine();

                    Console.Write("Informe a idade: ");
                    int.TryParse(Console.ReadLine(), out idade);

                    Console.Write("Informe o peso: ");
                    double.TryParse(Console.ReadLine(), out peso);

                    Console.Write("Informe a altura: ");
                    double.TryParse(Console.ReadLine(), out altura);

                    imc = Math.Round((peso / (altura * altura)));

                    if (imc < 18.5)
                    {
                        resultado = "1";
                    }
                    else if (imc > 18.5 && imc < 25)
                    {
                        resultado = "2";
                    }
                    else if (imc > 25 && imc < 30)
                    {
                        resultado = "3";
                    }
                    else if (imc > 30 && imc < 35)
                    {
                        resultado = "4";
                    }
                    else if (imc > 35 && imc < 40)
                    {
                        resultado = "5";
                    }
                    else if (imc > 40)
                    {
                        resultado = "6";
                    }

                    

                    int resltd = Int32.Parse(resultado);

                    

                    if (resultado != "" && imc > 0 && altura > 0 && peso > 0 && nome.Trim().Length > 2 && idade > 0)
                    {

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("   Operação feita com sucesso ! \nPressione ENTER para verificar o Resultado.");
                        Console.ReadLine();


                        
                        
                        con = new SqlConnection(Properties.Settings.Default.connectionString); // Com esta linha estou apontando para a string de conexão que esta no arquivo App.config no Solution Explorer          
                        using (var command = con.CreateCommand())
                        {
                            command.CommandText = "INSERT INTO TL_CLIENT (CLIENT_NAME, CLIENT_AGE, CLIENT_WEIGH, CLIENT_HEIGHT, CLIENT_RESLT_COD )" +
                                                  "VALUES (@nome, @idade, @peso, @altura, @resultado)";

                            command.Parameters.AddWithValue("@nome", nome);
                            command.Parameters.AddWithValue("@idade",idade);
                            command.Parameters.AddWithValue("@peso",peso);
                            command.Parameters.AddWithValue("@altura",altura);
                            command.Parameters.AddWithValue("@resultado", resltd);

                            con.Open(); // Quando o programa identifica a conexão, com o método Open() estou abrindo a conexão com o banco de dados, para executar a Query acima.

                            command.ExecuteNonQuery(); // o método ExecuteNonQuery() executa a query acima, inserindo os valores atribuidos na variaveis em " command.Parameters.AddWithValue "

                            con.Close(); // Fechar a conexão com o banco de dados

                            
                        }

                        con = new SqlConnection(Properties.Settings.Default.connectionString); // Com esta linha estou apontando para a string de conexão que esta no arquivo App.config no Solution Explorer
                        con.Open(); // Quando o programa identifica a conexão, com o método Open() estou abrindo a conexão com o banco de dados.
                        reader = new SqlCommand("SELECT  IMC_DESC FROM TL_IMC WHERE IMC_ID="+ resultado, con).ExecuteReader(); // Com este SELECT estou capturando o ID do IMC e atribuindo a variavel " resultado ".
                        if (reader.HasRows) // Se haver retorno na consulta acima
                        {
                            while (reader.Read()) 
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(" ************** RESULTADO ************\n");
                                Console.WriteLine("Nome: " + nome);
                                Console.WriteLine("\nIdade: " + idade);
                                Console.WriteLine("\nPeso: " + peso);
                                Console.WriteLine("\nAltura: " + altura);
                                Console.WriteLine("\nSituação: {0}", reader.GetString(0)); // Apenas este campo esta utilizando o valor retornado na tabela TL_IMC
                                Console.WriteLine(" *************************************\n");

                                

                            }
                        }
                        con.Close();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("Dados Digitados incorretamente, operação cancelada.");
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                }
                /* Inicio da tela de buscar os clientes*/
                else if (acao == "C")
                {

                    Console.Clear();

                    Console.WriteLine("************************************************************************************************************************");
                    Console.WriteLine("                               FAÇA A BUSCA POR RESULTADOS JA CALCULADOS                                                ");
                    Console.WriteLine("************************************************************************************************************************");


                    int id;

                    con = new SqlConnection(Properties.Settings.Default.connectionString);
                    con.Open();
                    Console.WriteLine("DIGITE UM CÓDIGO DE UM CLIENTE \n");
                    id = int.Parse(Console.ReadLine());

                    reader = new SqlCommand("SELECT CLIENT_NAME, CLIENT_AGE, CLIENT_WEIGH, CLIENT_HEIGHT, IMC_DESC FROM TL_CLIENT " +
                        "INNER JOIN TL_IMC ON CLIENT_RESLT_COD = IMC_ID WHERE CLI_INT_ID=" + id , con).ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(" \nNome: {0} \n" +
                                "Idade: {1} \n" +
                                "Peso: {2}  \n" +
                                "Altura: {3} \n" +
                                "Situação: {4}",
                                reader.GetString(0), // Nome do Cliente
                                reader.GetString(1), // Idade do Cliente
                                reader.GetDouble(2), // Peso do Cliente
                                reader.GetDouble(3), //  Altura do cliente
                                reader.GetString(4)); // Situação do IMC do cliente. 
                                
                        }
                    }
                    else
                    {
                        Console.WriteLine(" Cliente não Encontrado");
                    }
                    reader.Close();
                }

                Console.WriteLine();
                Console.WriteLine(" Press ENTER to return to MAIN MENU \nPressione ENTER para retornar ao MENU Principal");
                Console.ReadKey();


                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("************************************************************************************************************************");
                Console.WriteLine("Development Date: 22/07/2020 \nAuthor: Rogério Souza \nIDE and Tools: Visual Studio SQL Server, C# \n");
                Console.WriteLine("**********************************           CALCULE SEU IMC          **************************************************");
                Console.WriteLine("*****************************           BMI Calculation Environment          *******************************************");
                Console.WriteLine("  .:: Press N / C / S ::.");
                Console.WriteLine("  .:: Pressione N / C / S ::.\n");
                Console.WriteLine("N - Novo Calculo / New  ");
                Console.WriteLine("C - Consultar um usuário / Consult ");
                Console.WriteLine("S - Sair / Exit \n");
                Console.WriteLine("************************************************************************************************************************");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Press a key to start \n");
                Console.Write("Pressione a tecla desejada: ");
                Console.ResetColor();

                acao = Console.ReadLine().ToUpper();
                Console.WriteLine();
            }

        }
    }
}