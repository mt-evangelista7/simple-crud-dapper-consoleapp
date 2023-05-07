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
            var con = new SqlConnection();

            var dapperEx = new DapperEx();

            // Chamar os metodos vv
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

        // ** Relacionamento entre tabelas **

        // Um para um
        public void OneToOne(SqlConnection connection)
        {
            var sql = @"
            SELECT 
                * 
            FROM 
                [CareerItem] 
            INNER JOIN 
                [Course] ON [CareerItem].[CourseId] = [Course].[Id]
            ";

            using (connection = new SqlConnection(connectionString))
            {
                // <Tabela A, Tabela B e a junção está dentro da tabela A>(script sql, função para definir como o dapper vai percorer os itens e carregar um course dentro de um careerItem)
                // Essa função recebe dois parametros, um careerItem e um course, e deinimos que a propriedade Course de careerItem recebe o parametro course
                // e retornamos o carrerItem
                var items = connection.Query<CareerItem, Course, CareerItem>(sql, (careerItem, course) =>
                {
                    careerItem.Course = course;
                    return careerItem;
                }, splitOn: "Id"); // Usamos o splitOn para definir aonde ocorre a separação do que é careerItem e Course, que nesse caso é o Id (O primeiro item de cada tabela)


                // Para cada careerItem
                foreach (var item in items)
                {
                    Console.WriteLine($"{item.Title} - Curso: {item.Course.Title}");
                }
            };
        }

        // Um para muitos
        public void OneToMany(SqlConnection connection)
        {
            var sql = @"
                SELECT 
                    [Career].[Id],
                    [Career].[Title],
                    [CareerItem].[CareerId],
                    [CareerItem].[Title]
                FROM 
                    [Career] 
                INNER JOIN 
                    [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]
                ORDER BY
                    [Career].[Title]";

            using (connection = new SqlConnection(connectionString))
            {
                var careers = new List<Career>();

                var items = connection.Query<Career, CareerItem, Career>(
                    sql,
                    (career, item) =>
                {
                    // Verifica se o item já existe na carreira
                    var car = careers.Where(x => x.Id == career.Id).FirstOrDefault();

                    // Se não achar vem nulo
                    if (car == null)
                    {
                        car = career;
                        car.CareerItems.Add(item);
                        careers.Add(car);
                    }
                    // Se achou, adiciona o item na lista de categorias da carreira
                    else
                    {
                        car.CareerItems.Add(item);
                    }

                    return career;
                }, splitOn: "CareerId"); // Onde ocorre a divisão de dados de career e careerItem

                foreach (var career in careers)
                {
                    Console.WriteLine($"{career.Title}");

                    foreach (var item in career.CareerItems)
                    {
                        Console.WriteLine($" - {item.Title}");

                    }
                }
            };
        }

        // Muitos para muitos
        public void QueryMultiple(SqlConnection connection)
        {
            var query = "SELECT * FROM [Category]; SELECT * FROM [Course]";

            using (connection = new SqlConnection(connectionString))
            {
                using (var multi = connection.QueryMultiple(query)) // O queryMultiple permite realizar mais de um comando na mesma query, nesse caso dois select
                {
                    var categories = multi.Read<Category>();
                    var courses = multi.Read<Course>();

                    foreach (var category in categories)
                    {
                        Console.WriteLine(category.Title);
                    }

                    foreach (var course in courses)
                    {
                        Console.WriteLine(course.Title);
                    }
                }
            }
        }

        // SELECT IN
        public void SelectIn(SqlConnection connection)
        {
            // Selecione tudo da tabela de Cursos onde o id está em um desses dois items passados
            // eu deixei esses dois id fixado mas isso pode ser um parametro
            var query = @"
                SELECT * FROM [Course] WHERE [Id] IN(
                    '5d8cf396-e717-9a02-2443-021b00000000',
                    '5c349848-e717-9a7d-1241-0e6500000000' )";

            using (connection = new SqlConnection(connectionString))
            {
                var courses = connection.Query<Course>(query);

                foreach (var course in courses)
                {
                    Console.WriteLine(course.Title);
                }
            }

        }

        // Like
        public void Like(SqlConnection connection)
        {
            // Selecione tudo da tabela de cursos, onde contenha a palavra criando
            var query = @"
                SELECT * FROM [Course] WHERE [Title] LIKE  '%Criando%'";

            using (connection = new SqlConnection(connectionString))
            {
                var courses = connection.Query<Course>(query);

                foreach (var course in courses)
                {
                    Console.WriteLine(course.Title);
                }
            }

        }

        // Transaction
        public void Transaction(SqlConnection connection)
        {

            // Cria uma nova categoria
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Entity";
            category.Url = "Entity";
            category.Description = "Categoria destinada ao Entity";
            category.Order = 9;
            category.Summary = "Entity teste";
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

            connection = new SqlConnection(connectionString);

            connection.Open();

            // Inicia uma transação
            var transaction = connection.BeginTransaction();

            var rows = connection.Execute(insertSql, new
            {
                id = category.Id,
                title = category.Title,
                url = category.Url,
                description = category.Description,
                order = category.Order,
                summary = category.Summary,
                featured = category.Featured
            }, transaction);

            /*transaction.Commit();*/ // Efetua as alterações no banco
            transaction.Rollback(); // Desfaz as alterações

            Console.WriteLine($"{rows} linha(s) inseridas");

            transaction.Dispose();

            connection.Dispose();


        }
    }
}