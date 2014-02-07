﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.Objects;
using System.Data.Common;
using System.Data;
using System.Data.SqlServerCe;
using DataOperations;
namespace Dinesh_Project
{
    public class CoreOperations
    {
       static  ConnectionStringSettings coreConnectionstring = ConfigurationManager.ConnectionStrings["CoreDbConnectionString"];
       static List<Vehicle> Vechicles;
       static List<Customer> Owners;
       static List<CustomerData> customerViewData;
       static List<Technician> Technicians;
       static List<Transaction> Transactions;
       static bool isVehicleDirty = true;
       static bool isOwnerDirty = true;
       static bool isTechnicianDirty = true;
       static bool iscustomerDataDirty=true;
       static bool isTransacDirty = true; 


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
            try
            {

                String InsertCommand = string.Format("INSERT INTO OPERATIONS (Name,VehicleClass) VALUES ('{0}','{1}')", OperationName, VehicalClass);
                int returnValue = InsertData(coreConnectionstring, InsertCommand);
                if (returnValue > -1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception Occurred in COre Operations::AddNewOperation. " + ex.ToString());
            }
            return false;
        }
        public static int AddANewCustomer(string CustomerName, string RegistrationID,string  phone,string address)
        {
            try
            {
                //using (CoreDbEntities db = new CoreDbEntities())
                //{
                //    Customer newCustomer = db.CreateObject<Customer>();
                //    newCustomer.Name = CustomerName;
                //    newCustomer.CustomerID = new Random().Next();
                //    newCustomer.Phone = phone;
                //    newCustomer.RegistrationID = RegistrationID;
                //    newCustomer.Address = address;
                //    db.Customers.AddObject(newCustomer);
                //    db.SaveChanges();
                //    return true;
                //}
                isOwnerDirty = true;
                iscustomerDataDirty = true;
                String InsertCommand = string.Format("INSERT INTO CUSTOMERS(Name,RegistrationID,Phone,Address) VALUES ('{0}','{1}','{2}','{3}')", CustomerName, RegistrationID, phone, address);
                int returnValue = InsertData(coreConnectionstring, InsertCommand);
                return returnValue;
            }
            catch (Exception  ex)
            {
                Utility.WriteLogError("Exception occurred in adding a new customer " +CustomerName+"   " + ex.ToString());
            }

            return -1;
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
            iscustomerDataDirty = true;
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
        public static bool EditACustomer(int ID, string regisrtationID, string Name, String phone, string address)
        {
            try
            {                
                using (CoreDbEntities db = new CoreDbEntities())
                {
                    Customer customers = db.Customers.First(c => c.CustomerID == ID);
                    if (customers != null)
                    {
                        db.ObjectStateManager.ChangeObjectState(customers, System.Data.EntityState.Unchanged);
                        db.Customers.Attach(customers);
                        customers.Address = address;
                        customers.Phone = phone;
                        customers.Name = Name;
                        customers.RegistrationID = regisrtationID;
                        db.ObjectStateManager.ChangeObjectState(customers, System.Data.EntityState.Modified);
                        int returnStatus =db.SaveChanges();
                    }
                    iscustomerDataDirty = true;
                    isOwnerDirty = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception  occurred in adding a customer" + ex.ToString());
                
            }
            return false;
        }
        public static bool EditAVehicle(int ID, string RegistrationNumber, string vehicleType, int ownerID)
        {
            try
            {
                using (CoreDbEntities db = new CoreDbEntities())
                {
                    Vehicle vehicle = db.Vehicles.First(c => c.VehicleID== ID);
                    if (vehicle != null)
                    {
                        db.ObjectStateManager.ChangeObjectState(vehicle, System.Data.EntityState.Unchanged);
                        db.Vehicles.Attach(vehicle);
                        vehicle.RegistrationNumber = RegistrationNumber;
                        vehicle.VehicleType = vehicleType;
                        vehicle.Customer= db.Customers.FirstOrDefault(e =>e.CustomerID== ownerID);
                        db.ObjectStateManager.ChangeObjectState(vehicle, System.Data.EntityState.Modified);
                        int returnStatus = db.SaveChanges();
                    }
                    iscustomerDataDirty = true;
                    isOwnerDirty = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception  occurred in adding a Vehicle" + ex.ToString());

            }
            return false;
        }

        public static bool EditATransaction(string ID, string RegistrationNumber, int  ownerID,int  techID)
        {
            try
            {
                isTransacDirty = true;
                isVehicleDirty = true;
                iscustomerDataDirty = true;
                isTechnicianDirty = true;
                using (CoreDbEntities db = new CoreDbEntities())
                {
                    Transaction trans = db.Transactions.First(c => c.ServiceId.Equals(ID));
                    if (trans != null)
                    {
                        db.ObjectStateManager.ChangeObjectState(trans, System.Data.EntityState.Unchanged);
                        db.Transactions.Attach(trans);
                        Vehicle vehic= CoreOperations.GetVehiclesByRegistration(RegistrationNumber).First();
                        vehic.Customer = db.Customers.First(c => c.CustomerID == ownerID);
                        trans.Vehicle = vehic;
                        trans.Technician = db.Technicians.First(t => t.Id == techID);
                        db.ObjectStateManager.ChangeObjectState(trans, System.Data.EntityState.Modified);
                        int returnStatus = db.SaveChanges();
                    }
                    iscustomerDataDirty = true;
                    isOwnerDirty = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception  occurred in adding a Vehicle" + ex.ToString());

            }
            return false;
        }

        /// <summary>
        /// substr is the vehicle Registration Number like AP28BHJ1234
        /// </summary>
        /// <param name="substr"></param>
        /// <returns></returns>
        public static List<Vehicle> GetVehiclesByRegistration(string substr)
        {   
                try
                {
                    Vechicles= GetAllVehicles();
                    var matchedVehicles = (from Vehicle row in Vechicles
                                           where (row.RegistrationNumber.ToLower().ToString().Contains(substr.ToLower()))
                                           select row);
                    
                    return matchedVehicles.ToList();
                    
                }
                catch (Exception ex)
                {
                    Utility.WriteLogError("Exception Occurred while Getting Vehicles List" + ex.ToString());
                }
            return null;
        }
        public static List<Vehicle> GetAllVehicles()
        {
            if (isVehicleDirty == true)
            {
                try
                {
                    using (CoreDbEntities db = new CoreDbEntities())
                    {
                        List<Vehicle> currentVehicles= db.Vehicles.Include("Customer").ToList();
                        Vechicles  = currentVehicles;
                        return Vechicles;
                    }
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
        public static List<Customer> GetAllOwnersByName(string Name,string phone)
        {
            try
            {
                Owners = GetAllOwners();

                var matchedOwners = (from Customer cust in Owners
                                           where (cust.Name.ToLower().Contains(Name.ToLower()) && cust.Phone.ToLower().Contains(phone.ToLower()))
                                           select cust);
                
                return matchedOwners.ToList();
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
                    using (CoreDbEntities db = new CoreDbEntities())
                    {
                         List<Customer> customers= db.Customers.ToList();
                         Owners = customers;
                         return Owners;
                    }
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
                    
                    if (Vechicles != null && Vechicles.Count > 0)
                    {
                        var ownerId = (from Vehicle row in Vechicles
                                       where row.VehicleID.ToString().Equals(vehicId.ToString())
                                       select row.Ownerid).First();

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
        public static List<Technician> GetAllTechnicians(string name,string RegID)
        {
            try
            {
                if (isTechnicianDirty)
                    Technicians = GetAllTechnicians();

                var MatchedRows = (from Technician row in Technicians
                                         where row.Name.ToLower().Contains(name.ToLower()) && row.RegistrationID.Contains(RegID)
                                         select row
                                         ).ToList();
                
                

                return MatchedRows;
            }
            catch (Exception ex)
            {
                Utility.WriteLogError(string.Format("Exception occurred in Getall Technicians with criteria. Inputs: Name = {0} , RegID = {1}. Exception message {2}", name, RegID,ex.ToString()));
            }
            return null;
        }
        public static List<Technician> GetAllTechnicians()
        {
            if (isTechnicianDirty)
            {
                try
                {
                    
                    
                    using (CoreDbEntities db = new CoreDbEntities())
                    {
                        List<Technician> techLIst = db.Technicians.ToList();
                        Technicians = techLIst;
                        isTechnicianDirty = false;
                        return Technicians;
                    }
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
        public static List<Transaction> GetAllTransactions()
        {
            if (isTransacDirty)
            {
                try
                {
                    using (CoreDbEntities db = new CoreDbEntities())
                    {
                        List<Transaction> transacList = db.Transactions.Include("Vehicle").Include("Vehicle.Customer").Include("Technician").Include("Operation").ToList();
                        Transactions= transacList;
                        
                        isTransacDirty= false;
                        return Transactions;
                    }
                }
                catch (Exception ex)
                {
                    Utility.WriteLogError("Exception Occurred while Getting transactions List" + ex.ToString());
                }
            }
            else
            {
                Utility.WriteLog("No new data to fetch in Transactions");
                return Transactions;
            }
            return null;
        }
        public static List<Transaction> GetAllTransactions(DateTime startTimeofTransac, DateTime endTimeofTransac, string OwnerName,string Technician,string RegistrationID,string ServiceID,string Phone)
        {
            try
            {
                if (isTransacDirty)
                    Transactions = GetAllTransactions();

                var MatchedRows = (from Transaction currentTransaction in Transactions
                                   where 
                                   currentTransaction.Vehicle.Customer.Name.Contains(OwnerName)&&
                                   currentTransaction.Vehicle.RegistrationNumber.Contains(RegistrationID)&&
                                   currentTransaction.Technician.Name.Contains(Technician)&&
                                   Utility.DateInBetween((DateTime)currentTransaction.StartDate,startTimeofTransac,endTimeofTransac)
                                   &&(string.IsNullOrEmpty(ServiceID)||currentTransaction.ServiceId.Equals(ServiceID))
                                   &&currentTransaction.Vehicle.Customer.Phone.Contains(Phone)
                                   select currentTransaction
                                         ).ToList();
                return MatchedRows;
            }
            catch (Exception ex)
            {
                Utility.WriteLogError(string.Format("Exception occurred in Getall Technicians with criteria. Inputs: Name = {0} , RegID = {1}. Exception is {2} ", OwnerName, RegistrationID,ex.ToString()));
            }
            return null;
        }
        public static List<Operation> GetAllOPerations()
        {
            using (CoreDbEntities db = new CoreDbEntities())
            {
                return db.Operations.ToList();
            }
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
            List<Vehicle> vehiclesList= GetVehiclesByRegistration(RegistrationId);
            try
            {
                if (vehiclesList != null)
                {
                    var vehicId = (from Vehicle row in vehiclesList
                                   select row.VehicleID).First();
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
            List<Customer> vehiclesList = GetAllOwnersByName(Name, string.Empty);
            try
            {
                if (vehiclesList != null && vehiclesList.Count > 0)
                {
                    var vehicId = (from Customer row in vehiclesList
                                   select row.CustomerID).First();

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
        public static int doesOwnerExists(Int32 id)
        {
            List<Customer> vehiclesList = GetAllOwners();
            try
            {
                if (vehiclesList != null && vehiclesList.Count> 0)
                {
                    var vehicId = (from Customer row in vehiclesList
                                   where row.CustomerID==id
                                   select row.CustomerID).First();
                    if (vehicId >-1)
                        return int.Parse(vehicId.ToString());
                    else
                        return -1;
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
            List<Technician> techList = GetAllTechnicians(Name,string.Empty);
            try
            {
                if (techList != null && techList.Count > 0)
                {
                    var techId = (from Technician row in techList
                                   select row.Id).First();

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
        public static int doesOperationExists(string Name)
        {
            using (CoreDbEntities db = new CoreDbEntities())
            {
                var listofOps = db.Operations.Where(x => x.Name.Contains(Name));
                if (listofOps.Any())
                    return listofOps.First().OperationId;
            }
            return -1;
        }

        public static bool StartANewTransactionWithExistingVehicle(int OperationID, DateTime StartDate,string status,int  vehicleID,string technicianName,string PaymentType,string PaymentStatus,double paymentAmount,string Remarks)
        {
            string serviceID;
            isTransacDirty = true;
            isTechnicianDirty = true;
            isVehicleDirty = true;
            isOwnerDirty = true;
            serviceID = Utility.CreateRandomID(technicianName);
            #region Add technician Part
            int techId= doesTechnicianExists(technicianName);
            if (techId == -1)
            {
                AddANewTechnician(technicianName, Utility.CreateRandomID(technicianName));
                techId = doesTechnicianExists(technicianName);
            }

            #endregion

            //Command Data
            string InsertCommand = string.Format("INSERT INTO Transactions(ServiceId,OperationId,StartDate,Status,VehicleId,PaymentType,PaymentStatus,PaymentAmount,Remarks,TechnicianId) VALUES ('{9}',{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8})",OperationID,StartDate,status,vehicleID,PaymentType,PaymentStatus, paymentAmount,Remarks,techId,serviceID);
            Utility.WriteLog("The insert Command for Transaction is " + InsertCommand);
            int returnValue = InsertData(coreConnectionstring, InsertCommand);
            if (returnValue > -1)
                return true;
            else
                return false;
            

        }



        #region Join Methods
        public static List<CustomerData> GetCustomizedCustomerdData()
        {
            List<Customer> allowners = CoreOperations.GetAllOwners();
            List<Vehicle> allvehicles = CoreOperations.GetAllVehicles();
            if (iscustomerDataDirty)
            {
                List<CustomerData> data = new List<CustomerData>();
                
                var registeredvehicles = (from Vehicle vehicle in allvehicles
                                          group vehicle by vehicle.Ownerid into vehicleList
                                          select new { OwnerID = vehicleList.Key, listofVehicles = vehicleList });
                foreach (Customer CurrentOwner in allowners)
                {
                    CustomerData newCustomer = new CustomerData();
                    newCustomer.Address = CurrentOwner.Address;
                    newCustomer.CustomerID = CurrentOwner.CustomerID;
                    newCustomer.Name = CurrentOwner.Name;
                    newCustomer.Phone = CurrentOwner.Phone;
                    newCustomer.RegistrationID = CurrentOwner.RegistrationID;

                    List<string> vehiclesList = new List<string>();

                    var vehicList = (from registeredvehicle in registeredvehicles
                                     where registeredvehicle.OwnerID == newCustomer.CustomerID
                                     select registeredvehicle.listofVehicles).FirstOrDefault();
                    if (vehicList != null )
                    {
                        foreach (Vehicle vehico in vehicList)
                        {
                            vehiclesList.Add(vehico.RegistrationNumber);
                        }
                        newCustomer.vehiclesID = vehiclesList;
                    }
                    data.Add(newCustomer);
                }
                customerViewData = data;
            }
            else
            {
                Utility.WriteLog("Customer view data is not dirty");
            }
            return customerViewData;

            
        }
        public static List<CustomerData> GetCustomizedCustomerdData(List<Customer> GivenCustomers)
        {
            List<Customer> allowners = GivenCustomers;
            List<Vehicle> allvehicles = CoreOperations.GetAllVehicles();
            if (allowners.Count < 0)
            {
                Utility.WriteLogError("The given List of Customers is Nulll");
                return null;
            }
            if (iscustomerDataDirty)
            {
                List<CustomerData> data = new List<CustomerData>();

                var registeredvehicles = (from Vehicle vehicle in allvehicles
                                          group vehicle by vehicle.Ownerid into vehicleList
                                          select new { OwnerID = vehicleList.Key, listofVehicles = vehicleList });
                foreach (Customer CurrentOwner in allowners)
                {
                    CustomerData newCustomer = new CustomerData();
                    newCustomer.Address = CurrentOwner.Address;
                    newCustomer.CustomerID = CurrentOwner.CustomerID;
                    newCustomer.Name = CurrentOwner.Name;
                    newCustomer.Phone = CurrentOwner.Phone;
                    newCustomer.RegistrationID = CurrentOwner.RegistrationID;

                    List<string> vehiclesList = new List<string>();

                    var vehicList = (from registeredvehicle in registeredvehicles
                                     where registeredvehicle.OwnerID == newCustomer.CustomerID
                                     select registeredvehicle.listofVehicles).FirstOrDefault();
                    if (vehicList != null &&vehicList.Count()>0)
                    {
                        foreach (Vehicle vehico in vehicList)
                        {
                            vehiclesList.Add(vehico.RegistrationNumber);
                        }
                        newCustomer.vehiclesID = vehiclesList;
                    }
                    data.Add(newCustomer);
                }
                customerViewData = data;
            }
            else
            {
                Utility.WriteLog("Customer view data is not dirty");
            }
            return customerViewData;


        }
        #endregion

        public static List<T> RemoveAdditionalData<T>(List<T> inputLIst, int startID, int numberofItems)
        {
            try
            {
                if (inputLIst.Count < numberofItems)
                    return inputLIst;
                if (startID < 0)
                    startID = inputLIst.Count - startID - 1;
                if (startID != 0)
                    inputLIst.RemoveRange(0, startID);
                if(inputLIst.Count>numberofItems)
                    inputLIst.RemoveRange(startID + numberofItems, inputLIst.Count - numberofItems);
            }
            catch (Exception ex)
            {
                Utility.WriteLogError("Exception Occurred in Paging data"+ex.ToString());
            }
            finally
            {
                
            }
            return inputLIst;
        }
    }
}
