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
            //CoreOperations.GetAllVehicles("2334");
            //CoreOperations.GetAllOwners("aet");
            int outd= CoreOperations.doesVehicleExist("233");
            
        }
    }
}
