using System;
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
        }


        // Insere uma categoria
        public void InsertCategory(SqlConnection connection) {

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
            
            using(connection = new SqlConnection(connectionString)) {

                var rows = connection.Execute(insertSql, new {
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
        public void ListCategories(SqlConnection connection) {

            using(connection = new SqlConnection(connectionString)) {
                var categories = connection.Query<Category>("SELECT [Id], [Title], [Description] FROM [Category]");

                foreach (var categoryItem in categories)
                {
                    Console.WriteLine($"Id: {categoryItem.Id} -- Title: {categoryItem.Title} -- Description: {categoryItem.Description}");
                }
            };
        }

        // Atualiza uma categoria pelo id
        public void UpdateCategory(SqlConnection connection, string categoryTitle, string categoryId) {
            var updateCategorySql = "UPDATE [Category] SET [Title] = @title WHERE [Id] = @id";

            using(connection = new SqlConnection(connectionString)) {
                var rows = connection.Execute(updateCategorySql, new{
                    title = categoryTitle,
                    id = categoryId
                });
                Console.WriteLine($"{rows} linha(s) afetadas");
            }
        }

        // Deleta uma categoria pelo id
        public void DeleteCategory(SqlConnection connection, string categoryId) {
            var deleteSql = @"DELETE FROM [CATEGORY] WHERE [Category].[Id] = @IdCategoria";

            using(connection = new SqlConnection(connectionString)) {
                var rows = connection.Execute(deleteSql, new {
                    IdCategoria = categoryId
                });

                Console.WriteLine($"{rows} linha(s) afetadas");
            };
        }
    }
}