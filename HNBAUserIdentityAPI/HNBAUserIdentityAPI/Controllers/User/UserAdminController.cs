using HNBAUserIdentityAPI.Models;
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

    public class UserAdminController : ApiController
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();



        [HttpPost]
        [ActionName("UpdateUserPassword")]
        public HttpResponseMessage UpdateUserPassword(UserMini user)
        {

            if (ValidatePasswordHistory(user.UserName, user.Company, user.NewPassword) == "0")
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "This password used recently. please choose a different one");
            }


            PasswordAdvisor passwordAdvisor = new PasswordAdvisor();
            if (passwordAdvisor.CheckStrength(user.NewPassword)!= PasswordScore.Medium)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "This password is too simple");
            }


            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {

                connection.Open();
                OracleCommand cmd = null;

                cmd = new OracleCommand("UPDATE_USER_PASSWORD");

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = connection;

                cmd.Parameters.Add("V_USER_CODE", OracleType.VarChar).Value = user.UserName;
                cmd.Parameters.Add("V_PASSWORD", OracleType.VarChar).Value = user.Password;
                cmd.Parameters.Add("V_NEW_PASSWORD", OracleType.VarChar).Value = user.NewPassword;
                cmd.Parameters.Add("V_COMPANY", OracleType.VarChar).Value = user.Company;



                cmd.ExecuteNonQuery();
                connection.Close();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                connection.Close();

                return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }
        }




        [HttpPost]
        [ActionName("LockUser")]
        public HttpResponseMessage LockUser(UserMini user)
        {
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;
            try
            {

                connection.Open();
                OracleCommand cmd = null;

                cmd = new OracleCommand("WF_ADMIN_LOCK_USER");

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = connection;

                cmd.Parameters.Add("V_USER_CODE", OracleType.VarChar).Value = user.UserName;

                cmd.Parameters.Add("V_COMPANY", OracleType.VarChar).Value = user.Company;



                cmd.ExecuteNonQuery();
                connection.Close();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                connection.Close();

                return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }
        }



        public string ValidatePasswordHistory(string userCode, string company, string password)
        {

            try
            {

                string validationResult = "0";
                OracleConnection connection = new OracleConnection(ConnectionString);
                connection.Open();
                OracleCommand command;
                command = new OracleCommand("WF_ADMIN_USER_PWD_IS_VALID");



                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.Parameters.Add("V_USER_CODE", OracleType.VarChar).Value = userCode;
                command.Parameters.Add("V_COMPANY_CODE", OracleType.VarChar).Value = company;
                command.Parameters.Add("V_PASSWORD", OracleType.VarChar).Value = password;


                command.Parameters.Add("V_IS_PASSWORD_VALID", OracleType.Number);
                command.Parameters["V_IS_PASSWORD_VALID"].Direction = ParameterDirection.Output;


                command.ExecuteNonQuery();


                validationResult = Convert.ToString(command.Parameters["V_IS_PASSWORD_VALID"].Value);

                connection.Close();

                return validationResult;
            }
            catch (Exception ex)
            {
                return "0";
            }


        }



        [HttpGet]
        [ActionName("GetHNBASystems")]
        public IEnumerable<HNBASystem> GetHNBASystems()
        {
            List<HNBASystem> list = new List<HNBASystem>();
            DataTable dataTable = new DataTable();
            OracleDataReader dr = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;


            string sql = "SELECT " +
                 "CASE WHEN S.SYSTEM_CODE IS NULL THEN 0  ELSE S.SYSTEM_CODE END  , " +//0
                     "CASE WHEN S.SYSTEM_NAME IS NULL THEN '' ELSE S.SYSTEM_NAME END, " +//1
                     "CASE WHEN S.IS_ACTIVE IS NULL THEN 0  ELSE S.IS_ACTIVE END " +//2
                    " FROM WF_ADMIN_SYSTEM S ";


            command = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                dr = command.ExecuteReader();
                dataTable.Load(dr);
                dr.Close();
                connection.Close();
                list = (from DataRow drow in dataTable.Rows
                        select new HNBASystem()
                        {
                            SystemCode = Convert.ToInt32(drow[0]),
                            SystemName = drow[1].ToString(),
                            IsActive = Convert.ToInt32(drow[2].ToString())
                        }).ToList();
            }
            catch (Exception exception)
            {
                if (dr != null || connection.State == ConnectionState.Open)
                {
                    dr.Close();
                    connection.Close();
                }

            }
            return list;
        }

        [HttpGet]
        [ActionName("GetUserSystemRoles")]
        public IEnumerable<UserSystemRole> GetUserSystemRoles(string userName, string company)
        {
            List<UserSystemRole> list = new List<UserSystemRole>();
            DataTable dataTable = new DataTable();
            OracleDataReader dr = null;
            OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command;


            string sql = "SELECT " +
                 "CASE WHEN S.USER_CODE IS NULL THEN ''  ELSE S.USER_CODE END  , " +//0
                   "CASE WHEN S.COMPANY_CODE IS NULL THEN '' ELSE S.COMPANY_CODE END, " +//1
                 "CASE WHEN S.SYSTEM_CODE IS NULL THEN 0  ELSE S.SYSTEM_CODE END  , " +//2
                     "CASE WHEN S.USER_ROLE_CODE IS NULL THEN 0  ELSE S.USER_ROLE_CODE END, " +//3
                     "CASE WHEN S.IS_ALLOWED IS NULL THEN 0  ELSE S.IS_ALLOWED END " +//4
                    " FROM WF_ADMIN_USER_SYSTEM S " +
                    " WHERE S.USER_CODE=:V_USER_CODE AND S.COMPANY_CODE=:V_COMPANY_CODE";



            command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("V_USER_CODE", userName));
            command.Parameters.Add(new OracleParameter("V_COMPANY_CODE", company));



            try
            {
                connection.Open();
                dr = command.ExecuteReader();
                dataTable.Load(dr);
                dr.Close();
                connection.Close();
                list = (from DataRow drow in dataTable.Rows
                        select new UserSystemRole()
                        {
                            UserName = drow[0].ToString(),
                            CompanyCode = drow[1].ToString(),
                            SystemCode = Convert.ToInt32(drow[2].ToString()),
                            UserRoleCode = Convert.ToInt32(drow[3].ToString()),
                            IsAllowed = Convert.ToInt32(drow[4].ToString())


                        }).ToList();
            }
            catch (Exception exception)
            {
                if (dr != null || connection.State == ConnectionState.Open)
                {
                    dr.Close();
                    connection.Close();
                }

            }
            return list;
        }

    }
}
