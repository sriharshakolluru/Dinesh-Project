using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataOperations;
using System.Configuration;
using System.Data.SqlServerCe;

namespace TestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
//            Utility.WriteLog("Sample");
            //CoreOperations.AddANewOperation("Puncture", "2wheeler");
            //CoreOperations.AddANewCustomer("Neeti", "Neet1234", "9030256352", "Delhi");
            //CoreOperations.AddANewTechnician("Dinesh", "Dinu2134");
            //CoreOperations.AddANewVehicle("AP28D2012313243", "MBenz", 1);
  //          CoreOperations.GetAllVehicles();
            //CoreOperations.GetAllOwners("aet");
//            int outd= CoreOperations.doesVehicleExist("23");

      //      CoreOperations.StartANewTransactionWithExistingVehicle(1, DateTime.Now.AddDays(-1), "Opened", "AP23D323341231", "HarshaKB", "Card", "Paid", 500, "Check");
            
            ConnectionStringSettings coreConnectionstring = ConfigurationManager.ConnectionStrings["CoreDbConnectionString"];
            string getMD5DataOperations= Utility.GetMD5HashData("Haule@3241");

            string command = string.Format("INSERT INTO PasswordDetails (LoginID,Password) values ('{0}','{1}')","DineshKumar",getMD5DataOperations);
            InsertData(coreConnectionstring, command);

        }

        public static int InsertData(ConnectionStringSettings connectin, string command)
        {
            int returnCode;
            try
            {
                Utility.WriteLogDebug(string.Format("The Command Received to execute is {0}. The connection string is {1}", command, connectin.ConnectionString));
                using (SqlCeConnection con = new SqlCeConnection(connectin.ConnectionString))
                {
                    con.Open();
                    //command=command.Replace("\'\'", "NULL");
                    using (SqlCeCommand com = new SqlCeCommand(command, con))
                    {
                        returnCode = com.ExecuteNonQuery();
                    }
                }
                Utility.WriteLogDebug(string.Format("Finished The Command Received to execute : {0}. The connection string is {1}", command, connectin.ConnectionString));
                return returnCode;
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception occurred in inserting/deleting data " + ex.ToString());
                return -1;
            }
        }
    }
}
