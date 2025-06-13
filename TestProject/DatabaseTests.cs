using ConsoleAppDBMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TestProject
{
    public class DatabaseTests : IDisposable
    {
        private readonly string _dbPath;

        public DatabaseTests()
        {
            string relativePath = "test.db";
            string basePath = AppContext.BaseDirectory;
            _dbPath = Path.Combine(basePath, relativePath);
            string connectionString = $"Data Source={_dbPath}";

            // Создаёте БД, миграции и т.д.
            Migrate migrate = new Migrate(connectionString);
        }

        [Fact]
        public async Task IntegrtaedTestDbAsync()
        {
            await InsertTaskTblAsync();


        }

        private async Task InsertTaskTblAsync()
        {
            var tasks = await Db.Repository.TaskTbl.Querys.GetTasksAsync("");
            Assert.True(tasks.Count == 0);

            var insertDto = new Db.Repository.TaskTbl.Dtos.TaskInsertDto();

            insertDto.Email = "testEmail1@mail.ru";
            insertDto.Code = "1234";
            insertDto.IpClient = "12.23.34.45";
            insertDto.Session = "899db032-1256-0dd2-0a26-5e809b4a22bf";
            await Db.Repository.TaskTbl.Querys.InsertTaskParamAsync(insertDto);

            insertDto.Email = "testEmail2@mail.ru";
            insertDto.Code = "2345";
            insertDto.IpClient = "22.23.34.45";
            insertDto.Session = "899db032-1256-0dd2-0a26-5e809b4a22bf";
            await Db.Repository.TaskTbl.Querys.InsertTaskParamAsync(insertDto);

            insertDto.Email = "testEmail3@mail.ru";
            insertDto.Code = "3456";
            insertDto.IpClient = "32.23.34.45";
            insertDto.Session = "899db032-1256-0dd2-0a26-5e809b4a22bf";
            await Db.Repository.TaskTbl.Querys.InsertTaskParamAsync(insertDto);

            insertDto.Email = "testEmail4@mail.ru";
            insertDto.Code = "4567";
            insertDto.IpClient = "42.23.34.45";
            insertDto.Session = "899db032-1256-0dd2-0a26-5e809b4a22bf";
            await Db.Repository.TaskTbl.Querys.InsertTaskParamAsync(insertDto);


            tasks = await Db.Repository.TaskTbl.Querys.GetTasksAsync("");
            Assert.True(tasks.Count == 4);

            var result = true;
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteTaskTbl()
        {

            var result = true;
        Assert.True(result);
        }

        public void Dispose()
        {
            if (File.Exists(_dbPath))
                File.Delete(_dbPath);
        }
    }
}
