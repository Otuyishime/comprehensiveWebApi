using System;
using MySql.Data.MySqlClient;

namespace testWebAPI.DBs
{
    public class AppDb : IDisposable
    {
        public readonly MySqlConnection Connection;

        public AppDb()
        {
            Connection = new MySqlConnection("server=127.0.0.1;user id=testapi;password='';port=8889;database=testAPIdb");
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
