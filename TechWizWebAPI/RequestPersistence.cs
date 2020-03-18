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

        private const string acceptRequestAsWizardQuery =
            "UPDATE  " +
                "techwizard.workrequests  " +
            "SET  " +
                "user_WizardID = COALESCE(user_WizardID, @ID),   " +
                "AcceptedDate = COALESCE(AcceptedDate, NOW()) " +
            "WHERE  " +
                "RequestId = @RID  ";

        private const string getJobListDataForWizardById =
            "SELECT  " +
                "wr.requestid,  " +
                "wr.title,  " +
                "wr.description,  " +
                "wr.CreatedDate,  " +
                "wr.AcceptedDate, " +
                "wr.CompletedDate,  " +
                "wr.price_in_cents,  " +
                "concat(u.FirstName, ' ', u.LastName) as user,  " +
                "concat(w.FirstName, ' ', w.LastName) as wizard,  " +
                "s.Name as skill,  " +
                "cm.MethodName as contactmethod, " +
                "u.UserID as UserUserID, " +
                "u.UserName as UserUserName, " +
                "u.FirstName as UserFirstName, " +
                "u.LastName as UserLastName, " +
                "u.Address as UserAddress, " +
                "u.City as UserCity, " +
                "u.State as UserState, " +
                "u.Zip as UserZip, " +
                "u.Phone as UserPhone, " +
                "u.Email as UserEmail, " +
                "u.isWizard as UserIsWizard, " +
                "w.UserID as WizardUserID " +
            "FROM  " +
                "( " +
                    "SELECT  " +
                        "* " +
                    "FROM  " +
                        "techwizard.workrequests " +
                    "WHERE " +
                        "user_UserID != @ID " +
                        "AND (user_WizardID is null " +
                        "OR user_WizardID = @ID) " +
                ") wr  " +
                "left join techwizard.skills s on wr.Skills_SkillsId = s.SkillsId  " +
                "left join techwizard.user u on wr.user_UserID = u.UserID  " +
                "left join techwizard.user w on wr.user_WizardID = w.UserID  " +
                "left join techwizard.contactmethods cm on wr.ContactMethodID = cm.ContactMethodID  ";

        private const string getJobListDataForUserById =
            "SELECT  " +
                "wr.requestid,  " +
                "wr.title,  " +
                "wr.description,  " +
                "wr.CreatedDate,  " +
                "wr.AcceptedDate, " +
                "wr.CompletedDate,  " +
                "wr.price_in_cents,  " +
                "concat(u.FirstName, ' ', u.LastName) as user,  " +
                "concat(w.FirstName, ' ', w.LastName) as wizard,  " +
                "s.Name as skill,  " +
                "cm.MethodName as contactmethod, " +
                "w.UserID as WizardUserID, " +
                "w.UserName as WizardUserName, " +
                "w.FirstName as WizardFirstName, " +
                "w.LastName as WizardLastName, " +
                "w.Address as WizardAddress, " +
                "w.City as WizardCity, " +
                "w.State as WizardState, " +
                "w.Zip as WizardZip, " +
                "w.Phone as WizardPhone, " +
                "w.Email as WizardEmail, " +
                "w.isWizard as WizardIsWizard " +
            "FROM  " +
                "( " +
                    "SELECT  " +
                        "* " +
                    "FROM  " +
                        "techwizard.workrequests " +
                    "WHERE " +
                        "user_UserID = @ID " +
                ") wr  " +
                "left join techwizard.skills s on wr.Skills_SkillsId = s.SkillsId " +
                "left join techwizard.user u on wr.user_UserID = u.UserID " +
                "left join techwizard.user w on wr.user_WizardID = w.UserID " +
                "left join techwizard.contactmethods cm on wr.ContactMethodID = cm.ContactMethodID";



        public RequestPersistence()
        {
            string myConnectionString;
            myConnectionString = "server=127.0.0.1;uid=root;pwd=techwizard;database=techwizard";
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
                Request r = new Request()
                {
                    requestID = (long)(int)mySQLReader["RequestID"],
                    title = (string)mySQLReader["title"],
                    description = (string)mySQLReader["Description"],
                    user = (int)mySQLReader["user_UserID"],
                    wizard = (int?)(mySQLReader["user_WizardID"] == System.DBNull.Value ? null : mySQLReader["user_WizardID"]),
                    skill = (int)mySQLReader["Skills_SkillsId"],
                    openDate = (DateTime)mySQLReader["CreatedDate"],
                    completedDate = (DateTime?)(mySQLReader["CompletedDate"] == System.DBNull.Value?null: mySQLReader["CompletedDate"]),
                    acceptDate = (DateTime?)(mySQLReader["AcceptedDate"] == System.DBNull.Value ? null : mySQLReader["AcceptedDate"]),
                    priceInCents = (int)mySQLReader["price_in_cents"],
                    contactMethod = (int)mySQLReader["ContactMethodID"]
                };

                requestArray.Add(r);
            }

            return requestArray;
        }


        public ArrayList getRequestsForDisplayForUserId(long id)
        {
            MySqlDataReader mySQLReader;
 
            MySqlCommand cmd = new MySqlCommand(getJobListDataForUserById, conn);
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@ID", MySqlDbType.Int32).Value = id;

            mySQLReader = cmd.ExecuteReader();

            ArrayList requestArray = new ArrayList();

            // Declare transfer objects
            RequestForDisplay r;
            User w;
            JobListObject jlo;

            while (mySQLReader.Read())
            {
                r = new RequestForDisplay()
                {
                    requestID = (long)(int)mySQLReader["RequestID"],
                    title = (string)mySQLReader["title"],
                    description = (string)mySQLReader["Description"],
                    user = (string)mySQLReader["user"],
                    wizard = (string)(mySQLReader["wizard"] == System.DBNull.Value ? null : mySQLReader["wizard"]),
                    skill = (string)mySQLReader["skill"],
                    openDate = (DateTime)mySQLReader["CreatedDate"],
                    completedDate = (DateTime?)(mySQLReader["CompletedDate"] == System.DBNull.Value ? null : mySQLReader["CompletedDate"]),
                    acceptDate = (DateTime?)(mySQLReader["AcceptedDate"] == System.DBNull.Value ? null : mySQLReader["AcceptedDate"]),
                    priceInCents = (int)mySQLReader["price_in_cents"],
                    contactMethod = (string)mySQLReader["contactmethod"]
                };

                w = new User();

                w.ID = (int?)(mySQLReader["WizardUserID"] == System.DBNull.Value ? null : mySQLReader["WizardUserID"]);

                if (w.ID != null)
                {
                    w.FirstName = (string)mySQLReader["WizardFirstName"];

                    w.isWizard = true;

                    if (r.contactMethod == "Email")
                    {
                        w.Email = (string)mySQLReader["WizardEmail"];
                    }
                    else if (r.contactMethod == "Phone")
                    {
                        w.Phone = (string)mySQLReader["WizardPhone"];
                    }
                    else if (r.contactMethod == "Text")
                    {
                        w.Phone = (string)mySQLReader["WizardPhone"];
                    }
                    // Maybe implement GPS distance here instead of sharing teh full address
                    else if (r.contactMethod == "In-Person")
                    {
                        w.Address = (string)mySQLReader["WizardAddress"];
                        w.City = (string)mySQLReader["WizardCity"];
                        w.State = (string)mySQLReader["WizardState"];
                        w.Zip = (int)mySQLReader["WizardZip"];
                        w.Phone = (string)mySQLReader["WizardPhone"];
                    }
                } 


                jlo = new JobListObject() { r4d = r, contact = w };
                requestArray.Add(jlo);
            }

            return requestArray;
        }

        public ArrayList getRequestsForDisplayForWizardId(long id)
        {
            MySqlDataReader mySQLReader;

            MySqlCommand cmd = new MySqlCommand(getJobListDataForWizardById, conn);
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@ID", MySqlDbType.Int32).Value = id;

            mySQLReader = cmd.ExecuteReader();

            ArrayList requestArray = new ArrayList();

            // Declare transfer objects
            RequestForDisplay r;
            User u;
            JobListObject jlo;

            while (mySQLReader.Read())
            {
                r = new RequestForDisplay()
                {
                    requestID = (int)mySQLReader["RequestID"],
                    title = (string)mySQLReader["title"],
                    description = (string)mySQLReader["Description"],
                    user = (string)mySQLReader["user"],
                    wizard = (string)(mySQLReader["wizard"] == System.DBNull.Value ? null : mySQLReader["wizard"]),
                    skill = (string)mySQLReader["skill"],
                    openDate = (DateTime)mySQLReader["CreatedDate"],
                    completedDate = (DateTime?)(mySQLReader["CompletedDate"] == System.DBNull.Value ? null : mySQLReader["CompletedDate"]),
                    acceptDate = (DateTime ?)(mySQLReader["AcceptedDate"] == System.DBNull.Value ? null : mySQLReader["AcceptedDate"]),
                    priceInCents = (int)mySQLReader["price_in_cents"],
                    contactMethod = (string)mySQLReader["contactmethod"]
                };

                u = new User();

                int? wizardUid = (int?)(mySQLReader["WizardUserID"] == System.DBNull.Value ? null : mySQLReader["WizardUserID"]);

                if ( wizardUid != null && wizardUid == id)
                {
                    u.ID = (int)mySQLReader["UserUserID"];
                    u.FirstName = (string)mySQLReader["UserFirstName"];

                    u.isWizard = false;

                    if (r.contactMethod == "Email")
                    {
                        u.Email = (string)mySQLReader["UserEmail"];
                    }
                    else if (r.contactMethod == "Phone")
                    {
                        u.Phone = (string)mySQLReader["UserPhone"];
                    }
                    else if (r.contactMethod == "Text")
                    {
                        u.Phone = (string)mySQLReader["UserPhone"];
                    }
                    // Maybe implement GPS distance here instead of sharing teh full address
                    else if (r.contactMethod == "In-Person")
                    {
                        u.Address = (string)mySQLReader["UserAddress"];
                        u.City = (string)mySQLReader["UserCity"];
                        u.State = (string)mySQLReader["UserState"];
                        u.Zip = (int)mySQLReader["UserZip"];
                        u.Phone = (string)mySQLReader["UserPhone"];
                    }
                }

                jlo = new JobListObject() { r4d = r, contact = u };
                requestArray.Add(jlo);
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


        public bool acceptRequestAsWizard(long requestID, long wizardID) {
            

            MySqlDataReader mySQLReader;

            MySqlCommand cmd = new MySqlCommand(acceptRequestAsWizardQuery, conn);
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@ID", MySqlDbType.Int32).Value = wizardID;
            cmd.Parameters.AddWithValue("@RID", MySqlDbType.Int32).Value = requestID;

            mySQLReader = cmd.ExecuteReader();

            return (mySQLReader.RecordsAffected > 0);

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