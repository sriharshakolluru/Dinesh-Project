using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
namespace DataOperations
{
    public class CoreOperations
    {
        ConnectionStringSettings coreConnectionstring = ConfigurationManager.ConnectionStrings["CoreDbConnectionString"];

        public DataSet GetData(ConnectionStringSettings connection, string command)
        {
            SqlConnection currentConnection = new SqlConnection(coreConnectionstring.ToString());
            SqlCommand cmd= currentConnection.CreateCommand();
            cmd.CommandText = command;
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet resultset = new DataSet();
            return resultset;
        }



    }
}
