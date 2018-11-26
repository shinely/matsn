using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    public class MySqlHelper : IDBHelper
    {
        private static String connStr = ConfigurationManager.ConnectionStrings["mysql"].ToString();

        private MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            return conn;
        }

        public T Execute<T>(string sql, Func<DbCommand, T> func, params DbParameter[] parameters)
        {
            using (MySqlConnection conn = GetConnection())
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                if(parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return func.Invoke(cmd);
            }
        }

        public int ExecuteNonQuery(string sql, params DbParameter[] parameters)
        {
            return Execute(sql, cmd => cmd.ExecuteNonQuery(), parameters);
        }

        public DbDataReader ExecuteReader(string sql, params DbParameter[] parameters)
        {
            return Execute(sql, cmd => cmd.ExecuteReader(), parameters);
        }

        public object ExecuteScalar(string sql, params DbParameter[] parameters)
        {
            return Execute(sql, cmd => cmd.ExecuteScalar(), parameters);
        }
    }
}
