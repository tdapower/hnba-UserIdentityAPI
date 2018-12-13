using HNBAUserIdentityAPI.Models.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;
using Newtonsoft.Json;

namespace HNBAUserIdentityAPI.Controllers.User
{
    public class UserAccountController : ApiController
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConString"].ToString();
        public UserAccount AuthenticateAndLoad(string userName, string password, string company)
        {
            UserAccount u = null;
            //if (_userName == "tda" && _password == "tda")
            //{
            //    u = new UserAccount();
            //    u.userName = _userName;
            //    u.password = _password;
            //}
            u = checkAndLoadUser(userName, password, company);


            return u;
        }

        [HttpGet]
        [ActionName("GetWindowsUserName")]
        [TDABasicAuthenticationFilter(false)]
        public string GetWindowsUserName()
        {
            string UserName = "";
            UserName = User.Identity.Name;
            if (UserName.Length > 0)
            {
                UserName = Right(UserName, (UserName.Length) - 5);
            }
            return UserName;
        }


        [HttpGet]
        [ActionName("checkAndLoadWindowsUser")]
        [TDABasicAuthenticationFilter(false)]
        public UserAccount checkAndLoadWindowsUser(String userName)
        {


            UserAccount u = new UserAccount();



            OracleConnection con = new OracleConnection(ConnectionString);
            OracleDataReader dr;

            con.Open();

            String sql = "";



            sql = "   SELECT T.USER_CODE,T.USER_NAME,T.COMPANY_CODE,T.EPF_NO,T.USER_ROLE_CODE,T.BRANCH_ID,T.PASSWORD FROM WF_ADMIN_USERS_VW T  " +
               " WHERE T.USER_CODE=:V_USER_NAME  AND T.STATUS=1 ";



            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add(new OracleParameter("V_USER_NAME", userName));



            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                u.UserName = userName;
                u.Password = dr[6].ToString();

                u.UserDisplayName = dr[1].ToString();
                u.Company = dr[2].ToString();


                u.Epf = dr[3].ToString();
                u.UserRoleCode = Convert.ToInt32(dr[4].ToString());
                u.BranchCode = dr[5].ToString();



            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            con.Close();
            con.Dispose();

            return u;


        }


        [HttpGet]
        [ActionName("checkAndLoadUser")]
        [TDABasicAuthenticationFilter(false)]
        public UserAccount checkAndLoadUser(string userName, string password, string company)
        {


            UserAccount u = new UserAccount();



            OracleConnection con = new OracleConnection(ConnectionString);
            OracleDataReader dr;

            con.Open();

            String sql = "";



            sql = "   SELECT " +
                 " T.USER_CODE," + //0
                " T.USER_NAME," +//1
                " T.COMPANY_CODE," +//2
                " T.EPF_NO," +//3
                " T.USER_ROLE_CODE," +//4
                " T.BRANCH_ID " +//5
                " FROM WF_ADMIN_USERS_VW T  " +
               " WHERE T.USER_CODE=:V_USER_NAME  AND T.COMPANY_CODE=:V_COMPANY_CODE  AND   T.PASSWORD =:V_PASSWORD  AND T.STATUS=1 ";



            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add(new OracleParameter("V_USER_NAME", userName));
            cmd.Parameters.Add(new OracleParameter("V_PASSWORD", password));
            cmd.Parameters.Add(new OracleParameter("V_COMPANY_CODE", company));


            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                u.UserName = userName;
                u.Password = password;

                u.UserDisplayName = dr[1].ToString();
                u.Company = dr[2].ToString();


                u.Epf = dr[3].ToString();
                u.UserRoleCode = Convert.ToInt32(dr[4].ToString());
                u.BranchCode = dr[5].ToString();



            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            con.Close();
            con.Dispose();

            return u;


        }






        [HttpPost]
        [ActionName("checkAndLoadUser")]
        [TDABasicAuthenticationFilter(false)]
        public UserAccount checkAndLoadUser(dynamic userObj)
        {



            UserAccount us = new UserAccount();
            us.UserName = userObj.UserName;
            us.Company = userObj.Company;
            us.Password = userObj.Password;
            



            UserAccount u = new UserAccount();

            try
            {

                OracleConnection con = new OracleConnection(ConnectionString);
                OracleDataReader dr;

                con.Open();

                String sql = "";



                sql = "   SELECT T.USER_CODE,T.USER_NAME,T.COMPANY_CODE,T.EPF_NO,T.USER_ROLE_CODE,T.BRANCH_ID,T.PWD_STATUS,T.PWD_LAST_UPDATED_DATE  " +
                    " FROM WF_ADMIN_USERS_VW T  " +
                   " WHERE T.USER_CODE=:V_USER_NAME  AND T.COMPANY_CODE=:V_COMPANY_CODE  AND  T.PASSWORD =:V_PASSWORD  AND T.STATUS=1 ";



                OracleCommand cmd = new OracleCommand(sql, con);

                cmd.Parameters.Add(new OracleParameter("V_USER_NAME", us.UserName));
                cmd.Parameters.Add(new OracleParameter("V_PASSWORD", us.Password));
                cmd.Parameters.Add(new OracleParameter("V_COMPANY_CODE", us.Company));



                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    u.UserName = us.UserName;
                    u.Password = us.Password;

                    u.UserDisplayName = dr[1].ToString();
                    u.Company = dr[2].ToString();


                    u.Epf = dr[3].ToString();
                    u.UserRoleCode = Convert.ToInt32(dr[4].ToString());
                    u.BranchCode = dr[5].ToString();
                    u.PasswordStatus = dr[6].ToString();
                    u.PasswordLastUpdatedDate = dr[7].ToString();

                }
                dr.Close();
                dr.Dispose();
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }catch(Exception ex)
            {

            }

            return u;


        }



        public UserAccount getUserFromUserName(String userName)
        {


            UserAccount u = null;
            OracleConnection con = new OracleConnection(ConnectionString);
            OracleDataReader dr;

            con.Open();

            String sql = "";
            sql = "   SELECT T.USER_CODE,T.USER_NAME,T.COMPANY_CODE,T.BRANCH_ID,T.EPF_NO,T.USER_ROLE_CODE FROM WF_ADMIN_USERS_VW T  " +
               " WHERE T.USER_CODE=:V_USER_NAME   AND T.STATUS=1 ";



            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add(new OracleParameter("V_USER_NAME", userName));



            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                u = new UserAccount();
                u.UserName = userName;

                u.UserDisplayName = dr[1].ToString();
                u.Company = dr[2].ToString();
                u.BranchCode = dr[3].ToString();

                u.Epf = dr[4].ToString();
                u.UserRoleCode = Convert.ToInt32(dr[5].ToString());


            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            con.Close();
            con.Dispose();

            return u;


        }

        [HttpPost]
        [ActionName("ValidateUser")]
        // [TDABasicAuthenticationFilter(false)]
        public string ValidateUser(dynamic userObj)
        {
            UserAccount us = new UserAccount();
            us.UserName = userObj.UserName;
            us.Company = userObj.Company;
            us.Password = userObj.Password;



            string returnValue = "false";


            OracleConnection con = new OracleConnection(ConnectionString);
            OracleDataReader dr;

            con.Open();

            String sql = "";



            sql = "   SELECT T.USER_CODE,T.USER_NAME,T.COMPANY_CODE,T.EPF_NO,T.USER_ROLE_CODE FROM WF_ADMIN_USERS_VW T  " +
               " WHERE T.USER_CODE=:V_USER_NAME  AND T.COMPANY_CODE=:V_COMPANY_CODE AND  T.PASSWORD =:V_PASSWORD  AND T.STATUS=1 ";



            OracleCommand cmd = new OracleCommand(sql, con);

            cmd.Parameters.Add(new OracleParameter("V_USER_NAME", us.UserName));
            cmd.Parameters.Add(new OracleParameter("V_PASSWORD", us.Password));
            cmd.Parameters.Add(new OracleParameter("V_COMPANY_CODE", us.Company));


            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                returnValue = "true";

            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            con.Close();
            con.Dispose();

            return returnValue;


        }




        public string Left(string text, int length)
        {
            return text.Substring(0, length);
        }

        public string Right(string text, int length)
        {
            return text.Substring(text.Length - length, length);
        }


    }
}