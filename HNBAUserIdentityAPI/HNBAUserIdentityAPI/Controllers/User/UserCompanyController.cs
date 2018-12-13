using HNBAUserIdentityAPI.Models.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HNBAUserIdentityAPI.Controllers.User
{
    public class UserCompanyController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();


        [HttpGet]
        [TDABasicAuthenticationFilter(false)]
        public IEnumerable<UserCompany> Get()
        {
            List<UserCompany> list = new List<UserCompany>();
            DataTable dataTable = new DataTable();
            OracleDataReader dataReader = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            string sql = "SELECT " +
                     "CASE WHEN C.COMPANY_ID IS NULL THEN 0  ELSE C.COMPANY_ID END  , " +//0
                     "CASE WHEN C.COMPANY_NAME IS NULL THEN '' ELSE C.COMPANY_NAME END " +//1 
                     " FROM WF_ADMIN_COMPANY C order by C.COMPANY_NAME";


            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);
                dataReader.Close();
                connection.Close();
                list = (from DataRow drow in dataTable.Rows
                        select new UserCompany()
                        {
                            CompanyId = Convert.ToInt32(drow[0]),
                            CompanyName = drow[1].ToString()
                        }).ToList();
            }
            catch (Exception exception)
            {
                if (dataReader != null || connection.State == ConnectionState.Open)
                {
                    dataReader.Close();
                    connection.Close();
                }

            }
            return list;
        }
    }
}
