using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Db.Repository.EmailTasks
{
    public static class Querys
    {
        [Obsolete("Warning! SQL Injection possible. Use method with Param")]
        public static async Task InsertTaskAsync(Db.Repository.EmailTasks.Dtos.EmailTaskInsertDto dto)
        {
            string str = $@" 
                INSERT INTO email_tasks
                (created_at,          email,  status, change_status_at, code,       ip_client,   web_session) VALUES
                (CURRENT_TIMESTAMP, '{dto.Email}', 0,    CURRENT_TIMESTAMP, '{dto.Code}', '{dto.IpClient}',   '{dto.WebSession}');
                        ";
            await GetDb.ExecuteNonQueryAsync(str);
        }

        public static async Task InsertTaskParamAsync(Db.Repository.EmailTasks.Dtos.EmailTaskInsertDto dto)
        {
            string str = $@" 
                INSERT INTO email_tasks
                (email,  status, change_status_at, code,       ip_client,   web_session) VALUES
                (@Email, 0,    CURRENT_TIMESTAMP, @Code, @IpClient,   @web_session);
                        ";

            await GetDb.ExecuteNonQueryParamAsync(str, dto);
        }

        [Obsolete]
        public static async Task DeleteTaskAsync(string where)
        {
            if (string.IsNullOrEmpty(where)) throw new ArgumentNullException(nameof(where));
            string str = $@"DELETE FROM email_tasks WHERE {where};";
            await GetDb.ExecuteNonQueryAsync(str);
        }

        public static async Task DeleteTaskParamAsync(string where, Dictionary<string, string> dictParam)
        {
            if (string.IsNullOrEmpty(where)) throw new ArgumentNullException(nameof(where));
            string deleteStr = $@"DELETE FROM email_tasks";
            await GetDb.ExecuteNonQueryParamAsync(FormatQuery.FormateString(deleteStr, where), dictParam);
        }

        public static async Task<List<EmailTasks.Entity>> GetTasksAsync(string where)
        {
            string selectStr = $@"SELECT id, created_at, email, status, change_status_at, code, ip_client, web_session
                FROM email_tasks";

            var tasks = new List<Db.Repository.EmailTasks.Entity>();
            await GetDb.GetDataReaderAsync(FormatQuery.FormateString(selectStr, where),
                async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        var task = new Db.Repository.EmailTasks.Entity()
                        {
                            Id = (long)reader["id"],
                            Created_at = reader["created_at"].ToString(),
                            Email = reader["email"].ToString(),
                            Status = (long)reader["status"],
                            Change_status_at = reader["change_status_at"].ToString(),
                            Code = reader["code"].ToString(),
                            Ip_client = reader["ip_client"].ToString(),
                            WebSession = reader["web_session"].ToString(),
                        };
                        tasks.Add(task);
                    }
                });


            return tasks;
        }

    }





}

