using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserIdentityScheduledValidator
{
    class ProcessUsers
    {
        public void ExpireUserPassword()
        {
            OracleConnection connection = new OracleConnection(Properties.Settings.Default.ConnStr);
            OracleCommand command;
            try
            {

                connection.Open();
                OracleCommand cmd = null;

                cmd = new OracleCommand("WF_ADMIN_EXPIRE_PASSWORDS");

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = connection;
                
                cmd.ExecuteNonQuery();
                connection.Close();

                
            }
            catch (Exception ex)
            {
                connection.Close();
                
            }
        }
    }
}
