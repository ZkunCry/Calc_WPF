using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using System.IO;

namespace CalculatorTRPO
{
    internal class DataBase : IMemory
    {
        static string DBconnect = "server = 92.246.214.15; user=ais_aksentev1933_calculator;" +
           " password=TeV6yiiquyjUxe1s4BfehQTi; database=ais_aksentev1933_calculator";
        private readonly IDbConnection _db = new MySqlConnection(DBconnect);
      
        public string ReadSomewhere()
        {
            var id = 1;
            var num = _db.Query<string>("SELECT memorynumber FROM `calculator` WHERE id = @id", new { id = id });
            return num.First();
        }

        public void WriteSomewhere(string number)
        {
            var id = 1;
            _db.Query<string>("UPDATE `calculator` SET memorynumber = @number WHERE id = @id", new { number = number,id=id });
        }
    }
}
