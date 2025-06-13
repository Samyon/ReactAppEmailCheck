using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Db.Repository.TaskTbl
{
    public static class Querys
    {

        public static async Task InsertTaskAsync(Db.Repository.TaskTbl.Dtos.TaskInsertDto dto)
        {
            string str = $@" 
                INSERT INTO tasks
                (created_at,          email,  status, change_status_at, code,       ip_client,   'session') VALUES
                (CURRENT_TIMESTAMP, '{dto.Email}', 0,    CURRENT_TIMESTAMP, '{dto.Code}', '{dto.IpClient}',   '{dto.Session}');
                        ";
            await GetDb.ExecuteNonQueryAsync(str);
        }

        public static async Task InsertTaskParamAsync(Db.Repository.TaskTbl.Dtos.TaskInsertDto dto)
        {
            string str = $@" 
                INSERT INTO tasks
                (created_at,          email,  status, change_status_at, code,       ip_client,   'session') VALUES
                (CURRENT_TIMESTAMP, @Email, 0,    CURRENT_TIMESTAMP, @Code, @IpClient,   @Session);
                        ";

            await GetDb.ExecuteNonQueryParamAsync(str, dto);
        }


        public static async Task DeleteTaskAsync(string where)
        {
            if (string.IsNullOrEmpty(where)) throw new ArgumentNullException(nameof(where));
            string str = $@"DELETE FROM tasks WHERE {where};";
            await GetDb.ExecuteNonQueryAsync(str);
        }

        public static async Task DeleteTaskParamAsync(string where, Dictionary<string, string> dictParam)
        {
            if (string.IsNullOrEmpty(where)) throw new ArgumentNullException(nameof(where));
            string str = $@"DELETE FROM tasks WHERE {where};";
            await GetDb.ExecuteNonQueryParamAsync(str, dictParam);
        }

        public static async Task<List<TaskTbl.Entity>> GetTasksAsync(string where)
        {
            string selectStr = $@"SELECT id, created_at, email, status, change_status_at, code, ip_client, 'session'
                FROM tasks";

            var tasks = new List<Db.Repository.TaskTbl.Entity>();
            using (var reader = await GetDb.GetDataReaderAsync(FormatQuery.FormateString(selectStr, where)))
            {
                var task = new Db.Repository.TaskTbl.Entity();
                while (await reader.ReadAsync())
                {
                    task.Id = (int)reader["id"];
                    task.Created_at = reader["created_at"].ToString();
                    task.Email = reader["email"].ToString();
                    task.Status = (int)reader["status"];
                    task.Change_status_at = reader["change_status_at"].ToString();
                    task.Code = reader["code"].ToString();
                    task.Ip_client = reader["ip_client"].ToString();
                    task.Session = reader["session"].ToString();
                }
            }
            return tasks;
        }





    }
}
