using System;
using System.Configuration;
using System.Data.SqlClient;

namespace AlanDJackson.Util.Sql
{
    public class SqlScalarCmdUtil
    {
        protected string _connectionString = "";
        public string ConnectionString { get { return _connectionString; } set { _connectionString = value; } }

        public SqlScalarCmdUtil WithConnectionStringName(string cnnStrName)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[cnnStrName].ConnectionString;
            return this;
        }

        public SqlConnection GetOpenConnection()
        {
            SqlConnection cnn = new SqlConnection(ConnectionString);
            cnn.Open();
            return cnn;
        }

        public string GetScalarAsString(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection cnn = GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                foreach (SqlParameter parameter in parameters)
                    cmd.Parameters.Add(parameter);
                return SafeToString(cmd.ExecuteScalar());
            }
        }

        public static string SafeToString(object o)
        {
            if (o == null || o == DBNull.Value)
                return null;
            return o.ToString();
        }
    }
}
