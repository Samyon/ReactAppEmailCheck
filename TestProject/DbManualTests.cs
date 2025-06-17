

using ConsoleAppDBMigrator;
using Db;
using System.Threading.Tasks;

namespace TestProject
{
    public class DbManualTests
    {
        private string connectionString = "";
        private string dbPath = "";

        private void SetConnectionString()
        {
            string relativePath = "test.db";
            string basePath = AppContext.BaseDirectory;
            dbPath = Path.Combine(basePath, relativePath);
            connectionString = $"Data Source={dbPath}";
            GetDb.ChangePath(connectionString);
        }



        [Fact]
        public async Task AllTests()
        {
            CreateDb();
            await InsertTaskTblAsync();
            await DeletePartialTaskTbl();
            DeleteDb();
        }

        [Fact]
        [Trait("Category", "Manual")]
        public void CreateDb()
        {
            SetConnectionString();
            // Создаёте БД, миграции и т.д.
            Migrate migrate = new Migrate(connectionString);
            Assert.True(true);
        }


        [Fact]
        [Trait("Category", "Manual")]
        public async Task InsertTaskTblAsync()
        {
            SetConnectionString();
            
            var tasks = await Db.Repository.EmailTasks.Querys.GetTasksAsync("");
            int currentTaskCount = tasks.Count;

            var insertDto = new Db.Repository.EmailTasks.Dtos.EmailTaskInsertDto();

            insertDto.Email = "testEmail1@mail.ru";
            insertDto.Code = "1234";
            insertDto.IpClient = "12.23.34.45";
            insertDto.WebSession = "899db032-1256-0dd2-0a26-5e809b4a22bf";
            await Db.Repository.EmailTasks.Querys.InsertTaskParamAsync(insertDto);

            insertDto.Email = "testEmail2@mail.ru";
            insertDto.Code = "2345";
            insertDto.IpClient = "22.23.34.45";
            insertDto.WebSession = "899db032-1256-0dd2-0a26-5e809b4a22bf";
            await Db.Repository.EmailTasks.Querys.InsertTaskParamAsync(insertDto);

            insertDto.Email = "testEmail3@mail.ru";
            insertDto.Code = "3456";
            insertDto.IpClient = "32.23.34.45";
            insertDto.WebSession = "899db032-1256-0dd2-0a26-5e809b4a22bf";
            await Db.Repository.EmailTasks.Querys.InsertTaskParamAsync(insertDto);

            insertDto.Email = "testEmail4@mail.ru";
            insertDto.Code = "4567";
            insertDto.IpClient = "42.23.34.45";
            insertDto.WebSession = "899db032-1256-0dd2-0a26-5e809b4a22bf";
            await Db.Repository.EmailTasks.Querys.InsertTaskParamAsync(insertDto);


            tasks = await Db.Repository.EmailTasks.Querys.GetTasksAsync("");
            int insertTaskCount = tasks.Count - currentTaskCount;
            Assert.True(insertTaskCount == 4);

            var result = true;
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Manual")]
        public async Task RawQueryResult()
        {
            SetConnectionString();
            var data = await  Db.GetDb.GetRawQueryResultAsync("SELECT * FROM tasks");

            foreach (var row in data)
            {
                Console.WriteLine($"Email: {row["email"]}, Code: {row["code"]}");
            }
        }

        [Fact]
        [Trait("Category", "Manual")]
        public async Task DeletePartialTaskTbl()
        {
            SetConnectionString();
            string where = "Code<3456";
            await Db.Repository.EmailTasks.Querys.DeleteTaskAsync(where);
            var tasks = await Db.Repository.EmailTasks.Querys.GetTasksAsync("");
            Assert.True(tasks.Count < 4);
        }

        //[Fact]
        //[Trait("Category", "Manual")]
        //public async Task DeletePartialParamTaskTbl()
        //{
        //    SetConnectionString();
        //    string where = "@Code<3456";
        //    await Db.Repository.TaskTbl.Querys.DeleteTaskParamAsync(where,);
        //    var tasks = await Db.Repository.TaskTbl.Querys.GetTasksAsync("");
        //    Assert.True(tasks.Count < 4);
        //}

        [Fact]
        [Trait("Category", "Manual")]
        public async Task DeleteAllTaskTbl()
        {
            SetConnectionString();
            string where = "Code<3456";
            await Db.Repository.EmailTasks.Querys.DeleteTaskAsync("");
            var tasks = await Db.Repository.EmailTasks.Querys.GetTasksAsync("");
            Assert.True(tasks.Count < 4);
        }

        [Fact]
        [Trait("Category", "Manual")]
        public async Task GetTasksExpiredAsync()
        {
            SetConnectionString();          
            var tasks = await Db.Repository.EmailTasks.Logic.GetTasksExpiredAsync(10000);
            Assert.True(true);
        }



        [Fact]
        [Trait("Category", "Manual")]
        public void DeleteDb()
        {
            SetConnectionString();
            if (File.Exists(dbPath))
                File.Delete(dbPath);
            Assert.True(true);
        }


    }
}
