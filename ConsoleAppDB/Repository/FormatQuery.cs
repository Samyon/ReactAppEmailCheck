using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Repository
{
    public static class FormatQuery
    {
       public static string FormateString(string selectStr, string where)
        {
            if (where == "") return selectStr + ";";
            return selectStr += $@" WHERE {where};";
        }
    }
}
