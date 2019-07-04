using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace QuickReach.ECommerce.API.Controllers.Utilities
{
    public class ConnectionHelper
    {
        public static SqlConnection GetConnection()
        {
            var connectionString = "Server=.;Database=QuickReachDb;Integrated Security=true;";
            return new SqlConnection(connectionString);
        }
    }
}
