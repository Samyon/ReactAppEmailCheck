using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db;

namespace EmailSenderConsole
{
    public class Logic
    {
        private static readonly Random _random = new Random();

        public static async Task MakeAllAsync()
        {
            //Ищем записи с статусом 0 - самые свежие
            string sqlStr = $@"SELECT id, status, change_status_at FROM email_tasks WHERE status=0 ORDER BY created_at DESC";

            var values = await Db.GetDb.GetRawQueryResultAsync(sqlStr);

            //Организовываем по ним цикл
            //Каждой status присваиваем 1 и новое время присвоения
            //Выводим код в консоль
            foreach (var value in values)
            {
                value["status"] = 1;
                value["change_status_at"] = Db.Helper.GetCurrentTimeForDB();

                Console.WriteLine($"Для Email {value["email"]}   Код{value["code"]} ");

                //Записываем данные в БД
                string updateStr = $@" UPDATE email_tasks SET status=@status, change_status_at=@change_status_at
                    WHERE id={value["id"]};";

                await Db.GetDb.ExecuteNonQueryParamAsync(updateStr, value);
            }


            //Ищем записи с статусом 1 - которые в работе, и с вероятностью 0,1 присваиваем им код 2 - значит письмо дошло
            sqlStr = $@"SELECT id, status, change_status_at FROM email_tasks WHERE status=1 ORDER BY created_at DESC";

            values = await Db.GetDb.GetRawQueryResultAsync(sqlStr);
            double chance = 0.1;
            //Организовываем по ним цикл
            foreach (var value in values)
            {
                if (_random.NextDouble() < chance)
                {
                    value["status"] = 2;
                    value["change_status_at"] = Db.Helper.GetCurrentTimeForDB();
                    //Записываем данные в БД
                    string updateStr1 = $@" UPDATE email_tasks  SET status=@status, change_status_at=@change_status_at
                         WHERE id={value["id"]};";

                    await Db.GetDb.ExecuteNonQueryParamAsync(updateStr1, value);
                }
            }

            //Удаляем старые записи из таблицы за сутки
            var param = new Dictionary<string, object>();
            param.Add("created_at", Helper.TimeUntil(dayToExpire:1));

            sqlStr = $@" DELETE FROM email_tasks WHERE created_at<@created_at;";
            await Db.GetDb.ExecuteNonQueryParamAsync(sqlStr, param);




        }



    }
}
