using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Logging;

namespace WebAppENSEK.Db
{
    public class SqlHelper
    {
        public static string conStr;

      
        public static SqlCommand SplCmd(SqlCommand Command)
        {
            using SqlConnection connection = new SqlConnection(conStr);
            connection.Open();
            try
            {
                Command.Connection = connection;
                Command.ExecuteNonQuery();
                return Command;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public static object GetScalar(string sql)
        {
            using var connection = new SqlConnection(conStr);
            connection.Open();
            try
            {
                using var cmd = new SqlCommand(sql, connection)
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = sql
                };
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
