﻿using System;
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
            String InsertCommand = string.Format("INSERT INTO Vehicles(RegistrationNumber,VehicleType,Ownerid) VALUES ('{0}','{1}',{2})", RegisrtrationId , VehicleType,OwnerId);
            int returnValue = InsertData(coreConnectionstring, InsertCommand);
            if (returnValue > -1)
                return true;
            else
                return false;
        }

    }
}
