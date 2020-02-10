using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using TechWizWebAPI.Models;
using System.Collections;


namespace TechWizWebAPI
{
    public class RequestPersistence
    {
        private MySql.Data.MySqlClient.MySqlConnection conn;
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

        public ArrayList getRequests()
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
                r.supportType = mySQLReader.GetString(7);
                r.status = mySQLReader.GetInt32(8);
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
                r.supportType = mySQLReader.GetString(7);
                r.status = mySQLReader.GetInt32(8);
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
                sqlString = "UPDATE workrequests SET Description= '" + requestToSave.description + "', UserID= " + requestToSave.user + ", WizardID= " + requestToSave.wizard + ", OpenDate= '" + requestToSave.openDate.ToString("yyyy-MM-dd HH:mm:ss") + "' , AcceptDate = '" + requestToSave.acceptDate.ToString("yyyy-MM-dd HH:mm:ss") + "', CompleteDate = '" + requestToSave.completedDate.ToString("yyyy-MM-dd HH:mm:ss") + "', SupportType = '" + requestToSave.supportType + "', Status = " + requestToSave.status + " WHERE RequestID = " + ID.ToString(); 
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

                cmd.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            }
        }


        public long saveRequest(Request requestToSave)
        {
            String sqlString = "INSERT INTO workrequests (Description, UserID, WizardID, OpenDate, AcceptDate, CompleteDate, SupportType, Status) VALUES ('" + requestToSave.description + "', " + requestToSave.user + ", " + requestToSave.wizard + ", '" + requestToSave.openDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + requestToSave.acceptDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + requestToSave.completedDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + requestToSave.supportType + "', '" + requestToSave.status + "')";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            cmd.ExecuteNonQuery();
            long id = cmd.LastInsertedId;
            return id;
        }
    }
}