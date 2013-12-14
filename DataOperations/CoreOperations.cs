using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
namespace DataOperations
{
    public class CoreOperations
    {
       static  ConnectionStringSettings coreConnectionstring = ConfigurationManager.ConnectionStrings["CoreDbConnectionString"];

        //public DataSet GetData(ConnectionStringSettings connection, string command)
        //{
        //    SqlConnection currentConnection = new SqlConnection(coreConnectionstring.ToString());
        //    SqlCommand cmd= currentConnection.CreateCommand();
        //    cmd.CommandText = command;
        //    cmd.CommandType = CommandType.Text;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    adapter.SelectCommand = cmd;
        //    DataSet resultset = new DataSet();
            //return resultset;
        //}
        public static int InsertData(ConnectionStringSettings connectin, string command)
        {
            int returnCode;
            using (SqlCeConnection con = new SqlCeConnection(connectin.ConnectionString))
            {
                con.Open();
                using (SqlCeCommand com = new SqlCeCommand(command, con))
                {
                    returnCode= com.ExecuteNonQuery();
                }
            }
            return returnCode;
        }

        public static bool AddANewOperation(string OperationName, string VehicalClass)
        {
            String InsertCommand = string.Format("INSERT INTO OPERATIONS (Name,VehicleClass) VALUES ('{0}','{1}')", OperationName, VehicalClass);
            int returnValue = InsertData(coreConnectionstring, InsertCommand);
            if (returnValue > -1)
                return true;
            else
                return false;


        }
        public static bool AddANewCustomer(string CustomerName, string RegistrationID,string  phone,string address)
        {
            String InsertCommand = string.Format("INSERT INTO CUSTOMERS(Name,RegistrationID,Phone,Address) VALUES ('{0}','{1}','{2}','{3}')", CustomerName, RegistrationID,phone,address);
            int returnValue = InsertData(coreConnectionstring, InsertCommand);
            if (returnValue > -1)
                return true;
            else
                return false;


        }
        public static bool AddANewTechnician(string TechnicianName, string RegistrationID)
        {
            String InsertCommand = string.Format("INSERT INTO Technicians(Name,RegistrationID) VALUES ('{0}','{1}')", TechnicianName, RegistrationID);
            int returnValue = InsertData(coreConnectionstring, InsertCommand);
            if (returnValue > -1)
                return true;
            else
                return false;


        }
        public static bool AddANewVehicle(string RegisrtrationId, string VehicleType, int OwnerId)
        {
            String InsertCommand = string.Format("INSERT INTO Vehicles(RegistrationNumber,VehicleType,Ownerid) VALUES ('{0}','{1}',{2})", RegisrtrationId, VehicleType, OwnerId);
            int returnValue = InsertData(coreConnectionstring, InsertCommand);
            if (returnValue > -1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// substr is the vehicle Registration Number like AP28BHJ1234
        /// </summary>
        /// <param name="substr"></param>
        /// <returns></returns>
        public static DataSet GetAllVehicles(string substr)
        {
            try
            {
                SqlCeConnection currentConnection = new SqlCeConnection(coreConnectionstring.ToString());
                SqlCeCommand cmd = currentConnection.CreateCommand();
                if (!string.IsNullOrEmpty(substr))
                    cmd.CommandText = string.Format("SELECT VehicleId,RegistrationNumber,VehicleType,Ownerid FROM Vehicles where RegistrationNumber like '%{0}%'", substr);
                else
                    cmd.CommandText = string.Format("SELECT VehicleId,RegistrationNumber,VehicleType,Ownerid FROM Vehicles");

                //cmd.CommandText = string.Format("SELECT VehicleId,RegistrationNumber,VehicleType,Ownerid FROM Vehicles where RegistrationNumber like @partialId", substr);
                //cmd.Parameters.Add("@partialId", SqlDbType.NVarChar);
                //cmd.Parameters["partialId"].Value =string.Format(" '%{0}%'", substr);
                Console.WriteLine(cmd);
                cmd.CommandType = CommandType.Text;
                SqlCeDataAdapter adapter = new SqlCeDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet resultset = new DataSet();
                adapter.Fill(resultset);
                return resultset;
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception Occurred while Getting Vehicles List" + ex.ToString());
            }
            return null;
        }
        public static DataSet GetAllOwners(string Name)
        {
            try
            {
                SqlCeConnection currentConnection = new SqlCeConnection(coreConnectionstring.ToString());
                SqlCeCommand cmd = currentConnection.CreateCommand();
                if(!string.IsNullOrEmpty(Name))
                    cmd.CommandText = string.Format("SELECT CustomerId,Name,RegistrationId,Phone,Address FROM Customers where Name like '%{0}%'", Name);
                else
                    cmd.CommandText = string.Format("SELECT CustomerId,Name,RegistrationId,Phone,Address FROM Customers");

                //cmd.CommandText = string.Format("SELECT VehicleId,RegistrationNumber,VehicleType,Ownerid FROM Vehicles where RegistrationNumber like @partialId", substr);
                //cmd.Parameters.Add("@partialId", SqlDbType.NVarChar);
                //cmd.Parameters["partialId"].Value =string.Format(" '%{0}%'", substr);
                Console.WriteLine(cmd);
                cmd.CommandType = CommandType.Text;
                SqlCeDataAdapter adapter = new SqlCeDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet resultset = new DataSet();
                adapter.Fill(resultset);
                return resultset;
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception Occurred while Getting Vehicles List"+ ex.ToString());
            }
            return null;
        }
        public static DataSet GetAllTechnicians(string name)
        {
            try
            {
                SqlCeConnection currentConnection = new SqlCeConnection(coreConnectionstring.ToString());
                SqlCeCommand cmd = currentConnection.CreateCommand();
                if (!string.IsNullOrEmpty(name))
                    cmd.CommandText = string.Format("SELECT CustomerId,Name,RegistrationId,Phone,Address FROM Customers where Name like '%{0}%'", name);
                else
                    cmd.CommandText = string.Format("SELECT CustomerId,Name,RegistrationId,Phone,Address FROM Customers");

                //cmd.CommandText = string.Format("SELECT VehicleId,RegistrationNumber,VehicleType,Ownerid FROM Vehicles where RegistrationNumber like @partialId", substr);
                //cmd.Parameters.Add("@partialId", SqlDbType.NVarChar);
                //cmd.Parameters["partialId"].Value =string.Format(" '%{0}%'", substr);
                Console.WriteLine(cmd);
                cmd.CommandType = CommandType.Text;
                SqlCeDataAdapter adapter = new SqlCeDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet resultset = new DataSet();
                adapter.Fill(resultset);
                return resultset;
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception Occurred while Getting Vehicles List" + ex.ToString());
            }
            return null;
        }

        public static int doesVehicleExist(string RegistrationId)
        {
            DataSet vehiclesList= GetAllVehicles(RegistrationId);
            try
            {
                if (vehiclesList != null && vehiclesList.Tables.Count > 0)
                {
                    var vehicId = (from DataRow row in vehiclesList.Tables[0].Rows
                                   select row["VehicleID"]).First();

                    return int.Parse(vehicId.ToString());
                }
                else
                {
                    Utility.WriteLog("The vehicle list Result does not contain any rows");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception occurred in doesVehicleExist " + ex.ToString());
            }
            return -1;
        }
        public static int doesOwnerExists(string Name)
        {
            DataSet vehiclesList = GetAllOwners(Name);
            try
            {
                if (vehiclesList != null && vehiclesList.Tables.Count > 0)
                {
                    var vehicId = (from DataRow row in vehiclesList.Tables[0].Rows
                                   select row["CustomerID"]).First();

                    return int.Parse(vehicId.ToString());
                }
                else
                {
                    Utility.WriteLog("The Customer/Owner list Result does not contain any rows");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception occurred in doesVehicleExist " + ex.ToString());
            }
            return -1;
        }
        public static int doesTechnicianExists(string Name)
        {
            DataSet techList = GetAllTechnicians(Name);
            try
            {
                if (techList != null && techList.Tables.Count > 0)
                {
                    var techId = (from DataRow row in techList.Tables[0].Rows
                                   select row["Id"]).First();

                    return int.Parse(techId.ToString());
                }
                else
                {
                    Utility.WriteLog("The Customer/Owner list Result does not contain any rows");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception occurred in doesVehicleExist " + ex.ToString());
            }
            return -1;
        }

        public static bool StartANewTransaction(int OperationID, DateTime StartDate,string status,string VehicleRegisrationNumber,int technicianName,string PaymentType,string PaymentStatus,double paymentAmount,string Remarks)
        {
            SqlCeTransaction transac;
            string InsertCommand=string.Empty;//= string.Format("INSERT INTO Vehicles(OperationId,StartDate,Status,VehicleId,PaymentType,PaymentStatus,Remarks,TechnicianId) VALUES ({0},'{1}','{2}','{3}','{4}','{5}','{6}',{7})",OperationID,DateTime.Now,status,VehicleRegisrationNumber, );
            int returnValue = InsertData(coreConnectionstring, InsertCommand);
            if (returnValue > -1)
                return true;
            else
                return false;
        }   

        
    }
}
