
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class GetStr
    {

        public static string GetPath()
        {
            //return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            //string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            //UriBuilder uri = new UriBuilder(codeBase);
            //string path = Uri.UnescapeDataString(uri.Path);
            //return Path.GetDirectoryName(path);

            // Get a Type object.
            Type t = typeof(GetStr);
            // Instantiate an Assembly class to the assembly housing the Integer type.
            Assembly assem = Assembly.GetAssembly(t);

            return assem.Location;
        }

        public async Task<string> GetStr1(string rec)
        {
         

            return "Ok";
        }


    }
}
