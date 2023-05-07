// using System;
// using Microsoft.Data.SqlClient;

// // ACESSO A DADOS COM O ADO.NET

// namespace CursoBaltaDapper 
// {
//     class Program 
//     {
//         static void Main(string[] args) 
//         {
//             // Server=nome servidor, porta;Database=nome do banco;User ID=nome usuario;Password=senha do banco
//             const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True;";

//             // Para se conectar, usar o pacote
//             // Microsoft.Data.SqlClient - Pacote Nuget para acessar dados com o sql server

//             // SqlConnection é o objeto de conecção
//             // Com este objeto podemos utilizar a lista de procesimentos de um acesso a dados
//             // Abre a conecção, executa o desejado, fecha a conecção
//             // var connection = new SqlConnection();

//             // connection.Open(); // Abre

//             // execução no banco

//             // connection.Close(); // Fecha

//             // Destroi o obj, e obviamente fecha a conecção, porém só usar quando de fato não precisar mais
//             // pois após usar o dispose, vai ser preciso dar um new new SqlConnection(); novamente
//             // connection.Dispose();

//             // Uma maneira mais otimizada e simples é usando o using
//             // Fecha a conecção ao sair desse bloco de código
//             using (var connection = new SqlConnection(connectionString)) // SqlConnection(string de conecção)
//             {
//                 // Abre a conecção
//                 connection.Open();

//                 Console.WriteLine("Conectado!");

//                 // SqlCommando, serve para criar comandos SQL
//                 // É bom usar dentro de um using pois também pode ficar aberto
//                 using(var command = new SqlCommand())
//                 {
//                     // Define qual é a conecção
//                     command.Connection=connection;

//                     // Define o tipo de comando
//                     command.CommandType = System.Data.CommandType.Text;

//                     // Cria a query
//                     command.CommandText = "SELECT [Id], [Title] FROM [Category]";

//                     // Cria uma variavel que executa um comando de leitura no banco, que no caso é um select (se não for uma leitura, usar o command.ExecuteNonQuery())
//                     var reader = command.ExecuteReader();

//                     // Percorre as linha
//                     // Enquanto tiver linhas a serem lidas
//                     while(reader.Read()) 
//                     {
                            // Printa o Id e o titulo
//                         Console.WriteLine($"{reader.GetGuid(0)} - {reader.GetString(1)}"); 0 e 1 é a ordem das colunas
//                     }
//                 }

                
//             }

//         }
//     }
// }