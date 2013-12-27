using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
namespace Dinesh_Project
{
    class CustomersData
    {


        public string ID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public ListCollectionView OwnedVehicles
        {
            get;
            set;
        }
        
    }
}
