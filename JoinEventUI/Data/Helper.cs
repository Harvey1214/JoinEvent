using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;

namespace JoinEventUI.Data
{
    public static class Helper
    {
        public static string GetConnectionString()
        {
            return "Data Source=MSI\\SQLSERVER;Initial Catalog=JoinEventDatabase;Persist Security Info=True;User ID=sa;Password=55025105";
        }
    }
}
