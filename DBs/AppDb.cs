using System;
using MySql.Data.MySqlClient;

namespace testWebAPI.DBs
{
    public class AppDb : IDisposable
    {
        public readonly MySqlConnection Connection;

        public AppDb()
        {
            Connection = new MySqlConnection("server=localhost;user id=testapi;password='';port=8889;database=testAPIdb");
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
