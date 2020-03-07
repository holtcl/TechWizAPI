using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using TechWizWebAPI.Models;
using System.Collections;
using MySql.Data.MySqlClient;

namespace TechWizWebAPI
{
    public class RequestPersistence
    {
        private MySql.Data.MySqlClient.MySqlConnection conn;
        public RequestPersistence()
        {
            string myConnectionString;
            myConnectionString = "server=127.0.0.1;uid=root;pwd=password;database=techwizard";
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {

            }
        }

        public ArrayList getAllRequests()
        {

            ArrayList requestArray = new ArrayList();
            MySql.Data.MySqlClient.MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * From workrequests";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            while (mySQLReader.Read())
            {
                Request r = new Request();
                r.requestID = mySQLReader.GetInt32(0);
                r.description = mySQLReader.GetString(1);
                r.user = mySQLReader.GetInt32(2);
                r.wizard = mySQLReader.GetInt32(3);
                r.openDate = mySQLReader.GetDateTime(4);
                r.acceptDate = mySQLReader.GetDateTime(5);
                r.completedDate = mySQLReader.GetDateTime(6);
                //r.supportType = mySQLReader.GetString(7);
                //r.status = mySQLReader.GetInt32(8);
                requestArray.Add(r);
            }
            return requestArray;
        }

        public ArrayList getRequestsForUserId(long id) {
            User u = new User();
            MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * FROM workrequests WHERE user_UserID = @ID;";

            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            cmd.Prepare();

            cmd.Parameters.AddWithValue("@ID", id);

            mySQLReader = cmd.ExecuteReader();

            ArrayList requestArray = new ArrayList();

            while (mySQLReader.Read()) {
                Request r = new Request();
                //{
                r.requestID = (long)(int)mySQLReader["RequestID"];
                r.title = (string)mySQLReader["title"];
                r.description = (string)mySQLReader["Description"];
                r.user = (int)mySQLReader["user_UserID"];
                r.wizard = (int?)(mySQLReader["user_WizardID"] == System.DBNull.Value ? null : mySQLReader["user_WizardID"]);
                r.skill = (int)mySQLReader["Skills_SkillsId"];
                r.openDate = (DateTime)mySQLReader["CreatedDate"];
                r.completedDate = (DateTime?)(mySQLReader["CompletedDate"] == System.DBNull.Value?null: mySQLReader["CompletedDate"]) ;
                //acceptDate = mySQLReader.GetDateTime("AcceptedDate"),
                r.priceInCents = (int)mySQLReader["price_in_cents"];
                r.contactMethod = (int)mySQLReader["ContactMethodID"];
                
                //};

                requestArray.Add(r);
            }

            return requestArray;
        }


        public ArrayList getRequestsForDisplayForUserId(long id)
        {
            User u = new User();
            MySqlDataReader mySQLReader = null;

            String sqlString =
                "SELECT " +
                    "wr.requestid, " +
                    "wr.title, " +
                    "wr.description, " +
                    "wr.CreatedDate, " +
                    "wr.CompletedDate, " +
                    "wr.price_in_cents, " +
                    "concat(u.FirstName, ' ', u.LastName) as user, " +
                    "concat(w.FirstName, ' ', w.LastName) as wizard, " +
                    "s.Name as skill, " +
                    "cm.MethodName as contactmethod " +
                "FROM " +
                    "techwizard.workrequests wr " +
                    "left join techwizard.skills s on wr.Skills_SkillsId = s.SkillsId " +
                    "left join techwizard.user u on wr.user_UserID = u.UserID " +
                    "left join techwizard.user w on wr.user_WizardID = w.UserID " +
                    "left join techwizard.contactmethods cm on wr.ContactMethodID = cm.ContactMethodID " +
                "WHERE " +
                    "u.UserID=@ID ";

            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            cmd.Prepare();

            cmd.Parameters.AddWithValue("@ID", id);

            mySQLReader = cmd.ExecuteReader();

            ArrayList requestArray = new ArrayList();

            while (mySQLReader.Read())
            {
                RequestForDisplay r = new RequestForDisplay();
                //{
                r.requestID = (long)(int)mySQLReader["RequestID"];
                r.title = (string)mySQLReader["title"];
                r.description = (string)mySQLReader["Description"];
                r.user = (string)mySQLReader["user"];
                r.wizard = (string)(mySQLReader["wizard"] == System.DBNull.Value ? null : mySQLReader["wizard"]);
                r.skill = (string)mySQLReader["skill"];
                r.openDate = (DateTime)mySQLReader["CreatedDate"];
                r.completedDate = (DateTime?)(mySQLReader["CompletedDate"] == System.DBNull.Value ? null : mySQLReader["CompletedDate"]);
                //acceptDate = mySQLReader.GetDateTime("AcceptedDate"),
                r.priceInCents = (int)mySQLReader["price_in_cents"];
                r.contactMethod = (string)mySQLReader["contactmethod"];

                //};

                requestArray.Add(r);
            }

            return requestArray;
        }

        public ArrayList getRequestsForWizardId()
        {

            ArrayList requestArray = new ArrayList();
            MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * From workrequests";
            MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            while (mySQLReader.Read())
            {
                Request r = new Request();
                r.requestID = mySQLReader.GetInt32(0);
                r.description = mySQLReader.GetString(1);
                r.user = mySQLReader.GetInt32(2);
                r.wizard = mySQLReader.GetInt32(3);
                r.openDate = mySQLReader.GetDateTime(4);
                r.acceptDate = mySQLReader.GetDateTime(5);
                r.completedDate = mySQLReader.GetDateTime(6);
                //r.supportType = mySQLReader.GetString(7);
                //r.status = mySQLReader.GetInt32(8);
                requestArray.Add(r);
            }
            return requestArray;
        }

        public Request getRequest(long ID)
        {
            Request r = new Request();
            MySql.Data.MySqlClient.MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * From workrequests WHERE RequestID = " + ID.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            if (mySQLReader.Read())
            {
                r.requestID = mySQLReader.GetInt32(0);
                r.description = mySQLReader.GetString(1);
                r.user = mySQLReader.GetInt32(2);
                r.wizard = mySQLReader.GetInt32(3);
                r.openDate = mySQLReader.GetDateTime(4);
                r.acceptDate = mySQLReader.GetDateTime(5);
                r.completedDate = mySQLReader.GetDateTime(6);
                //r.supportType = mySQLReader.GetString(7);
                //r.status = mySQLReader.GetInt32(8);
                return r;
            }
            else
            {
                return null;
            }
        }

        public bool  deleteRequest(long ID)
        {
            Request r = new Request();
            MySql.Data.MySqlClient.MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * From workrequests WHERE RequestID = " + ID.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            if (mySQLReader.Read())
            {
                mySQLReader.Close();
                sqlString = "DELETE From workrequests WHERE RequestID = " + ID.ToString();
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

             cmd.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool updateRequest(long ID, Request requestToSave)
        {
            
            MySql.Data.MySqlClient.MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * From workrequests WHERE RequestID = " + ID.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            if (mySQLReader.Read())
            {
                mySQLReader.Close();
                sqlString = "UPDATE workrequests SET Description= '"; //+ requestToSave.description + "', UserID= " + requestToSave.user + ", WizardID= " + requestToSave.wizard + ", OpenDate= '" + requestToSave.openDate.ToString("yyyy-MM-dd HH:mm:ss") + "' , AcceptDate = '" + requestToSave.acceptDate.ToString("yyyy-MM-dd HH:mm:ss") + "', CompleteDate = '" + requestToSave.completedDate.ToString("yyyy-MM-dd HH:mm:ss");// + "', SupportType = '" + requestToSave.supportType + "', Status = " + requestToSave.status + " WHERE RequestID = " + ID.ToString(); 
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

                cmd.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            }
        }


        public long createNewRequest(Request requestToSave)
        {
            String sqlString = "INSERT INTO workrequests (" +
                "Description, user_UserID, Skills_SkillsId, CreatedDate, price_in_cents, title, ContactMethodID" +
                ") VALUES (" +
                "@Description, @UserID, @SkillsId, @CreatedDate, @price_in_cents, @title, @ContactMethodID)";

            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            cmd.Prepare();

            cmd.Parameters.AddWithValue("@Description", requestToSave.description);
            cmd.Parameters.AddWithValue("@UserID", requestToSave.user);
            cmd.Parameters.AddWithValue("@SkillsId", requestToSave.skill);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@price_in_cents", requestToSave.priceInCents);
            cmd.Parameters.AddWithValue("@title", requestToSave.title);
            cmd.Parameters.AddWithValue("@ContactMethodID", requestToSave.contactMethod);


            //requestToSave.openDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + requestToSave.acceptDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + requestToSave.completedDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + requestToSave.supportType + "', '" + requestToSave.status + "')";
            //MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            cmd.ExecuteNonQuery();
            long id = cmd.LastInsertedId;
            return id;
        }
    }
}