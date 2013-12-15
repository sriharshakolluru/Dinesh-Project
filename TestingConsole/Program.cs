using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataOperations;

namespace TestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Utility.WriteLog("Sample");
            //CoreOperations.AddANewOperation("Puncture", "2wheeler");
            //CoreOperations.AddANewCustomer("Neeti", "Neet1234", "9030256352", "Delhi");
            //CoreOperations.AddANewTechnician("Dinesh", "Dinu2134");
            //CoreOperations.AddANewVehicle("AP28D2012313243", "MBenz", 1);
  //          CoreOperations.GetAllVehicles();
            //CoreOperations.GetAllOwners("aet");
//            int outd= CoreOperations.doesVehicleExist("23");

            CoreOperations.StartANewTransactionWithExistingVehicle(1, DateTime.Now.AddDays(-1), "Opened", "AP23D323341231", "HarshaKB", "Card", "Paid", 500, "Check");
            
        }
    }
}
