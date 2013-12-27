﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using DataOperations;
namespace Dinesh_Project
{
    public class CoreOperations
    {
       static  ConnectionStringSettings coreConnectionstring = ConfigurationManager.ConnectionStrings["CoreDbConnectionString"];
       static DataTable Vechicles;
       static DataTable Owners;
       static DataTable Technicians;
       static bool isVehicleDirty = true;
       static bool isOwnerDirty = true;
       static bool isTechnicianDirty = true;


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
            try
            {
                using (SqlCeConnection con = new SqlCeConnection(connectin.ConnectionString))
                {
                    con.Open();
                    using (SqlCeCommand com = new SqlCeCommand(command, con))
                    {
                        returnCode = com.ExecuteNonQuery();
                    }
                }
                return returnCode;
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception occurred in inserting/deleting data " + ex.ToString());
                return -1;
            }
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
            isOwnerDirty = true;
            String InsertCommand = string.Format("INSERT INTO CUSTOMERS(Name,RegistrationID,Phone,Address) VALUES ('{0}','{1}','{2}','{3}')", CustomerName, RegistrationID,phone,address);
            int returnValue = InsertData(coreConnectionstring, InsertCommand);
            if (returnValue > -1)
                return true;
            else
                return false;


        }
        public static bool AddANewTechnician(string TechnicianName, string RegistrationID)
        {
            isTechnicianDirty = true;
            String InsertCommand = string.Format("INSERT INTO Technicians(Name,RegistrationID) VALUES ('{0}','{1}')", TechnicianName, RegistrationID);
            int returnValue = InsertData(coreConnectionstring, InsertCommand);
            if (returnValue > -1)
                return true;
            else
                return false;


        }
        public static bool AddANewVehicle(string RegisrtrationId, string VehicleType, int OwnerId)
        {
            isVehicleDirty = true;
            String InsertCommand = string.Format("INSERT INTO Vehicles(RegistrationNumber,VehicleType,Ownerid) VALUES ('{0}','{1}',{2})", RegisrtrationId, VehicleType, OwnerId);
            int returnValue = InsertData(coreConnectionstring, InsertCommand);
            if (returnValue > -1)
                return true;
            else
                return false;
        }


        public static bool EditaTechnician(int Id,string TechnicianName, string RegistrationID)
        {
            isTechnicianDirty = true;
            String InsertCommand = string.Format("Update Technicians SET Name='{0}' , RegistrationID ='{1}'  WHERE Id= {2}", TechnicianName, RegistrationID,Id);
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
        public static DataTable GetVehiclesByRegistration(string substr)
        {   
                try
                {
                    Vechicles= GetAllVehicles();
                    DataRow[] matchedVehicles = (from DataRow row in  Vechicles.Rows
                                           where (row["RegistrationNumber"].ToString().Contains(substr))
                                           select row).ToArray();
                    DataTable resultTable = Vechicles.Clone();
                    foreach (DataRow row in matchedVehicles)
                    {
                        resultTable.ImportRow(row);
                    }
                    return resultTable;
                    
                }
                catch (Exception ex)
                {
                    Utility.WriteLogError("Exception Occurred while Getting Vehicles List" + ex.ToString());
                }
            return null;
        }
        public static DataTable GetAllVehicles()
        {
            if (isVehicleDirty == true)
            {
                try
                {
                    SqlCeConnection currentConnection = new SqlCeConnection(coreConnectionstring.ToString());
                    SqlCeCommand cmd = currentConnection.CreateCommand();
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
                    Vechicles = resultset.Tables[0];
                    isVehicleDirty = false;
                    return Vechicles;
                }
                catch (Exception ex)
                {
                    Utility.WriteLogError("Exception Occurred while Getting Vehicles List" + ex.ToString());
                }
                
            }
            else
            {
                Utility.WriteLogError("No new data to Fetch");
                return Vechicles;
            }

            return null;
        }
        public static DataTable GetAllOwnersByName(string Name)
        {
            try
            {
                Owners = GetAllOwners();

                DataRow[] matchedOwners = (from DataRow row in Vechicles.Rows
                                             where (row["Name"].ToString().Contains(Name))
                                             select row).ToArray();
                DataTable resultTable = Owners.Clone();
                foreach (DataRow row in matchedOwners)
                {
                    resultTable.ImportRow(row);
                }
                return resultTable;
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception Occurred while Getting Vehicles List"+ ex.ToString());
            }
            return null;
        }
        public static List<Customer> GetAllOwners()
        {
            if (isOwnerDirty)
            {
                try
                {
                    SqlCeConnection currentConnection = new SqlCeConnection(coreConnectionstring.ToString());
                    SqlCeCommand cmd = currentConnection.CreateCommand();
                    cmd.CommandText = string.Format("SELECT CustomerId,Name,RegistrationId,Phone,Address FROM Customers");
                    Console.WriteLine(cmd);
                    cmd.CommandType = CommandType.Text;
                    SqlCeDataAdapter adapter = new SqlCeDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataSet resultset = new DataSet();
                    adapter.Fill(resultset);
                    Owners = resultset.Tables[0];
                    return Owners;
                }
                catch (Exception ex)
                {
                    Utility.WriteLogError("Exception Occurred while Getting Vehicles List" + ex.ToString());
                }
                finally
                {
                    isOwnerDirty = false;
                }

            }
            else
                return Owners;
            return null;
        }
        public static int GetOwnerByVehicle(string registrationNumber)
        {
            int vehicId = doesVehicleExist(registrationNumber);
            if (vehicId != -1)
            {
                try
                {

                    Vechicles = GetAllVehicles();
                    
                    if (Vechicles != null && Vechicles.Rows.Count > 0)
                    {
                        var ownerId = (from DataRow row in Vechicles.Rows
                                       where row["VehicleID"].ToString().Equals(vehicId.ToString())
                                       select row["Ownerid"]).First();

                        return int.Parse(ownerId.ToString());
                    }
                    else
                    {
                        Utility.WriteLogError("Dataset is empty in result to query on vehicles table");
                        return -1;
                    }
                }
                catch (Exception ex)
                {
                    Utility.WriteLogError("Exception Occurred while Getting Vehicles List" + ex.ToString());
                }
                return -1;
            }
            else
            {
                Utility.WriteLogError(" Error occurred while getting vehicle witi given registration Number");
                return -2;
            }
        }
        public static DataTable GetAllTechnicians(string name,string RegID)
        {
            try
            {
                if (isTechnicianDirty)
                    Technicians = GetAllTechnicians();

                DataRow[] MatchedRows = (from DataRow row in Technicians.Rows
                                         where row["Name"].ToString().Contains(name) && row["RegistrationID"].ToString().Contains(RegID)
                                         select row
                                         ).ToArray();
                DataTable resultTable = Technicians.Clone();
                foreach (DataRow row in MatchedRows)
                {
                    resultTable.ImportRow(row);
                }

                return resultTable;
            }
            catch (Exception ex)
            {
                Utility.WriteLogError(string.Format("Exception occurred in Getall Technicians with criteria. Inputs: Name = {0} , RegID = {1}", name, RegID));
            }
            return null;
        }
        public static DataTable GetAllTechnicians()
        {
            if (isTechnicianDirty)
            {
                try
                {
                    SqlCeConnection currentConnection = new SqlCeConnection(coreConnectionstring.ToString());
                    SqlCeCommand cmd = currentConnection.CreateCommand();
                    cmd.CommandText = string.Format("SELECT Id,Name,RegistrationId FROM Technicians");
                    cmd.CommandType = CommandType.Text;
                    SqlCeDataAdapter adapter = new SqlCeDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataSet resultset = new DataSet();
                    adapter.Fill(resultset);
                    Technicians = resultset.Tables[0];
                    isTechnicianDirty = false;
                    return Technicians;
                }
                catch (Exception ex)
                {
                    Utility.WriteLogError("Exception Occurred while Getting Vehicles List" + ex.ToString());
                }
            }
            else
            {
                Utility.WriteLog("No new data to fetch in technicians");
                return Technicians;
            }
            return null;
        }

        public static bool DeleteTechnician(string RegID)
        {
            isTechnicianDirty = true;
            String InsertCommand = string.Format("DELETE FROM Technicians WHERE RegistrationId='{0}'",RegID);
            int returnValue = InsertData(coreConnectionstring, InsertCommand);
            if (returnValue > -1)
                return true;
            else
                return false;
        }

        public static int doesVehicleExist(string RegistrationId)
        {
            DataTable vehiclesList= GetVehiclesByRegistration(RegistrationId);
            try
            {
                if (vehiclesList != null)
                {
                    var vehicId = (from DataRow row in vehiclesList.Rows
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
            DataTable vehiclesList = GetAllOwnersByName(Name);
            try
            {
                if (vehiclesList != null && vehiclesList.Rows.Count > 0)
                {
                    var vehicId = (from DataRow row in vehiclesList.Rows
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
            DataTable techList = GetAllTechnicians(Name,string.Empty);
            try
            {
                if (techList != null && techList.Rows.Count > 0)
                {
                    var techId = (from DataRow row in techList.Rows
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

        public static bool StartANewTransactionWithExistingVehicle(int OperationID, DateTime StartDate,string status,string VehicleRegisrationNumber,string technicianName,string PaymentType,string PaymentStatus,double paymentAmount,string Remarks)
        {
            SqlCeTransaction transac;
            int ownerId,vehicleId;
            string serviceID;
           
            if ((ownerId=GetOwnerByVehicle(VehicleRegisrationNumber)) > -1)// Owner exists
            {
                #region vehicle Verification
                vehicleId = doesVehicleExist(VehicleRegisrationNumber);
                serviceID = Utility.CreateRandomID(VehicleRegisrationNumber);
                #endregion


                #region Add technician Part
                int techId= doesTechnicianExists(technicianName);
                if (techId == -1)
                {
                    AddANewTechnician(technicianName, Utility.CreateRandomID(technicianName));
                    techId = doesTechnicianExists(technicianName);
                }

                #endregion

                //Command Data
                string InsertCommand = string.Format("INSERT INTO Transactions(ServiceId,OperationId,StartDate,Status,VehicleId,PaymentType,PaymentStatus,PaymentAmount,Remarks,TechnicianId) VALUES ('{9}',{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8})",OperationID,DateTime.Now,status,vehicleId,PaymentType,PaymentStatus, paymentAmount,Remarks,techId,serviceID);
                Utility.WriteLog("The insert Command for Transaction is " + InsertCommand);
                int returnValue = InsertData(coreConnectionstring, InsertCommand);
                if (returnValue > -1)
                    return true;
                else
                    return false;
            }
            else if(ownerId==-2)// Vehicle does not exist
            {
                Utility.WriteLogError("Vehicle Does Not Exist.. Registration ID Entered ... "+VehicleRegisrationNumber);
                return false;
            }
            else if (ownerId==-1)
                Utility.WriteLogError("Vehicle exists but owner data is missing ");

            return false;
            
        }   

        
    }
}