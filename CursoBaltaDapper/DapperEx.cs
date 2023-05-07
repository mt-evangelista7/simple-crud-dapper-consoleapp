using System;
using System.Data;
using CursoBaltaDapper.Models;
using Dapper;
using Microsoft.Data.SqlClient;

// ACESSO A DADOS COM O DAPPER
namespace CursoBaltaDapper
{
    class DapperEx
    {
        const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            // // Cria uma nova categoria
            // var category = new Category();
            // category.Id = Guid.NewGuid();
            // category.Title = "Amazon AWS";
            // category.Url = "balta/category/amazon";
            // category.Description = "Categoria destinada a serviços do AWS";
            // category.Order = 8;
            // category.Summary = "AWS Clound";
            // category.Featured = false;

            // // Para se previnir de um SQL Injection, é extritamente proibido, usar concatenação de strings numa query
            // // Ou seja, caso tenha parametros que precisam ser utilizados na query
            // // Em hipotese alguma use concatenação, como por exemplo ${}, ou qualquer outra forma.

            // // SEMPRE USAR OS PARAMETERS E NÃO CONCATENAÇÃO

            // // Cria o insert
            // // o @ é apenas para poder quebrar linha, não é nescessario para o funcionamento
            // var insertSql = @"INSERT INTO 
            //     [Category] 
            // VALUES(
            //     @id,
            //     @title,
            //     @url,
            //     @description,
            //     @order,
            //     @summary,
            //     @featured
            // )";

            // using (var connection = new SqlConnection(connectionString)) 
            // {
            //     // Executa o insert criado e passa os parameters
            //     // o Execute retorna a quantidade de linha afetadas
            //     var rows = connection.Execute(insertSql, new {
            //         id = category.Id,
            //         title = category.Title,
            //         url = category.Url,
            //         description = category.Description,
            //         order = category.Order,
            //         summary = category.Summary,
            //         featured = category.Featured
            //     });

            //     // Mostra a quantidade de linhas inseridas com o insert
            //     Console.WriteLine($"{rows} linha(s) inseridas");

            //     // O .Query executa uma query 
            //     // Definimos que categories será um enumerador de categoria
            //     // Ou seja, vai retornar as categorias do banco
            //     var categories = connection.Query<Category>("SELECT [Id], [Title], [Url], [Description] FROM [Category]"); // Usamos o query pois sabemos que o retorno pode ser mais de um

            //     // Para cada categoria que o banco retornou
            //     foreach(var categoryItem in categories) 
            //     {
            //         // Printa o id e no titulo e etc
            //         Console.WriteLine($"ID: {categoryItem.Id} -- Title: {categoryItem.Title}");
            //     }
            // }


            var con = new SqlConnection();

            var dapperEx = new DapperEx();

            dapperEx.InsertCategory(con);

            Console.WriteLine("Categoria criada!");

            dapperEx.ListCategories(con);

            dapperEx.UpdateCategory(con, "Nuvem", "b4c5af73-7e02-4ff7-951c-f69ee1729cac");

            Console.WriteLine("Categoria atualizada!");

            dapperEx.ListCategories(con);

            dapperEx.DeleteCategory(con, "fc67afb8-3edd-4836-980c-05fcd51a036d");

            Console.WriteLine("Categoria deletada!");

            dapperEx.ListCategories(con);

            dapperEx.InsertManyCategories(con);

            Console.WriteLine("Categorias criadas!");

            dapperEx.ListCategories(con);

            dapperEx.ExecuteProcedure(con);

            Console.WriteLine("Procedure executada!");

            dapperEx.ListCategories(con);

            dapperEx.ExecuteReadProcedure(con);

            dapperEx.InsertCategoryExecuteScalar(con);


        }


        // Insere uma categoria
        public void InsertCategory(SqlConnection connection)
        {

            // Cria uma nova categoria
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Docker";
            category.Url = "balta/category/docker";
            category.Description = "Categoria destinada ao docker";
            category.Order = 9;
            category.Summary = "Docker teste";
            category.Featured = false;

            var insertSql = @"INSERT INTO
                [Category]
                VALUES (
                    @id,
                    @title,
                    @url,
                    @description,
                    @order,
                    @summary,
                    @featured
                )";

            using (connection = new SqlConnection(connectionString))
            {

                var rows = connection.Execute(insertSql, new
                {
                    id = category.Id,
                    title = category.Title,
                    url = category.Url,
                    description = category.Description,
                    order = category.Order,
                    summary = category.Summary,
                    featured = category.Featured
                });

                Console.WriteLine($"{rows} linha(s) inseridas");
            };
        }

        // Lista todas as categorias
        public void ListCategories(SqlConnection connection)
        {

            using (connection = new SqlConnection(connectionString))
            {
                var categories = connection.Query<Category>("SELECT [Id], [Title], [Description] FROM [Category]");

                foreach (var categoryItem in categories)
                {
                    Console.WriteLine($"Id: {categoryItem.Id} -- Title: {categoryItem.Title} -- Description: {categoryItem.Description}");
                }
            };
        }

        // Atualiza uma categoria pelo id
        public void UpdateCategory(SqlConnection connection, string categoryTitle, string categoryId)
        {
            var updateCategorySql = "UPDATE [Category] SET [Title] = @title WHERE [Id] = @id";

            using (connection = new SqlConnection(connectionString))
            {
                var rows = connection.Execute(updateCategorySql, new
                {
                    title = categoryTitle,
                    id = categoryId
                });
                Console.WriteLine($"{rows} linha(s) afetadas");
            }
        }

        // Deleta uma categoria pelo id
        public void DeleteCategory(SqlConnection connection, string categoryId)
        {
            var deleteSql = @"DELETE FROM [CATEGORY] WHERE [Category].[Id] = @IdCategoria";

            using (connection = new SqlConnection(connectionString))
            {
                var rows = connection.Execute(deleteSql, new
                {
                    IdCategoria = categoryId
                });

                Console.WriteLine($"{rows} linha(s) afetadas");
            };
        }

        // Insere várias categorias
        public void InsertManyCategories(SqlConnection connection)
        {

            // Cria uma nova categoria
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Oracle";
            category.Url = "oracle";
            category.Description = "Categoria destinada ao oracle";
            category.Order = 11;
            category.Summary = "oracle teste";
            category.Featured = false;

            // Cria uma nova categoria
            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "AWS";
            category2.Url = "aws";
            category2.Description = "Categoria destinada ao aws";
            category2.Order = 10;
            category2.Summary = "aws teste";
            category2.Featured = true;

            var insertSql = @"INSERT INTO
                [Category]
                VALUES (
                    @id,
                    @title,
                    @url,
                    @description,
                    @order,
                    @summary,
                    @featured
                )";

            using (connection = new SqlConnection(connectionString))
            {

                // No caso de precisar criar varios itens, ao inves de passarmos o new {} e colocar a categoria
                // podemos criar um new array e passarmos quantas novas categorias quisermos
                var rows = connection.Execute(insertSql, new[]
                {
                    new
                    {
                        id = category.Id,
                        title = category.Title,
                        url = category.Url,
                        description = category.Description,
                        order = category.Order,
                        summary = category.Summary,
                        featured = category.Featured
                    },
                    new
                    {
                        id = category2.Id,
                        title = category2.Title,
                        url = category2.Url,
                        description = category2.Description,
                        order = category2.Order,
                        summary = category2.Summary,
                        featured = category2.Featured
                    }

                });

                Console.WriteLine($"{rows} linha(s) inseridas");
            };
        }

        // Executa uma procedure
        public void ExecuteProcedure(SqlConnection connection)
        {

            // Cria o comando que chama a procedure
            var procedure = "[spDeleteStudent]"; // [nome da procedure]

            // Cria uma paramentro, no caso um StudentId
            var pars = new { StudentId = "62792da4-8952-4c70-a183-ef84a9cbe640" };

            using (connection = new SqlConnection(connectionString))
            {
                // Para executar essa procedure, usamos o .Execute
                // e passamos, o comando sql, os paramentros, e o tipo de comando que no caso é um StoredProcedure
                var rows = connection.Execute(procedure, pars, commandType: CommandType.StoredProcedure);

                Console.WriteLine($"{rows} linha(s) afetadas");
            };
        }

        // Realiza uma leitura de procedure
        public void ExecuteReadProcedure(SqlConnection connection)
        {

            // Define a procedure
            var procedure = "[spGetCoursesByCategory]";

            // Cria o paramentro
            var pars = new { CategoryId = "af3407aa-11ae-4621-a2ef-2028b85507c4" };

            using (connection = new SqlConnection(connectionString))
            {
                // Retorna uma lista de cursos
                var courses = connection.Query(procedure, pars, commandType: CommandType.StoredProcedure);

                // Passa por todos os cursos e printa o id
                foreach (var coruseItem in courses)
                {
                    Console.WriteLine(coruseItem.Id);
                }
            }
        }

        // ExecuteScalar = diferente do Execute que retorna um int, com o executescalar você pode definir qual informação você quer que ele retorne
        // Exemplo: Ao invés de retornar as linhas afetadas, você queira ver o id gerado
        public void InsertCategoryExecuteScalar(SqlConnection connection)
        {

            // Cria uma nova categoria
            var category = new Category();
            category.Title = "Teste";
            category.Url = "teste";
            category.Description = "Categoria destinada ao teste";
            category.Order = 12;
            category.Summary = "teste";
            category.Featured = false;

            // Realiza o insert, criado um id pelo sql server
            var insertSql = @"
            INSERT INTO
                [Category]
            OUTPUT inserted.[Id]
                VALUES (
                    NEWID(),
                    @title,
                    @url,
                    @description,
                    @order,
                    @summary,
                    @featured)";

            using (connection = new SqlConnection(connectionString))
            {
                // Passa o execute scalar tipado como guid, pois sabemos que o id é um guid
                var idGerado = connection.ExecuteScalar<Guid>(insertSql, new
                {
                    title = category.Title,
                    url = category.Url,
                    description = category.Description,
                    order = category.Order,
                    summary = category.Summary,
                    featured = category.Featured,

                });

                Console.WriteLine($"ID GERADO:{idGerado}");
            };
        }
    }
}