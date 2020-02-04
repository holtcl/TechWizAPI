using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechWizWebAPI.Models;
using MySql.Data;
using System.Collections;


namespace TechWizWebAPI
{
    public class UserPersistence
    {
        private MySql.Data.MySqlClient.MySqlConnection conn;

        public UserPersistence()
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

        public ArrayList getUsers()
        {
            ArrayList userArray = new ArrayList();

            MySql.Data.MySqlClient.MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * FROM user";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            while (mySQLReader.Read())
            {
                User u = new User();
                u.ID = mySQLReader.GetInt32(0);
                u.UserName = mySQLReader.GetString(1);
                u.FirstName = mySQLReader.GetString(2);
                u.LastName = mySQLReader.GetString(3);
                u.Address = mySQLReader.GetString(4);
                u.City = mySQLReader.GetString(5);
                u.State = mySQLReader.GetString(6);
                u.Zip = mySQLReader.GetInt32(7);
                u.Phone = mySQLReader.GetString(8);
                u.Email = mySQLReader.GetString(9);
                u.Password = mySQLReader.GetString(10);
                userArray.Add(u);
            }

            return userArray;

        }
        public User getUser(long ID)
        {
            User u = new User();
            MySql.Data.MySqlClient.MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * FROM user WHERE UserID = @ID;";

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            cmd.Parameters.Add("@ID", MySql.Data.MySqlClient.MySqlDbType.Int32);
            cmd.Parameters["@ID"].Value = ID;

            mySQLReader = cmd.ExecuteReader();
            if (mySQLReader.Read())
            {
                u.ID = mySQLReader.GetInt32(0);
                u.UserName = mySQLReader.GetString(1);
                u.FirstName = mySQLReader.GetString(2);
                u.LastName = mySQLReader.GetString(3);
                u.Address = mySQLReader.GetString(4);
                u.City = mySQLReader.GetString(5);
                u.State = mySQLReader.GetString(6);
                u.Zip = mySQLReader.GetInt32(7);
                u.Phone = mySQLReader.GetString(8);
                u.Email = mySQLReader.GetString(9);
                u.Password = mySQLReader.GetString(10);
                return u;
            }
            else
            {
                return null;
            }


            }

        public bool deleteUser(long ID)
        {
            User u = new User();
            MySql.Data.MySqlClient.MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * FROM user WHERE UserID = " + ID.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            if (mySQLReader.Read())
            {
                mySQLReader.Close();

                sqlString = "DELETE FROM user WHERE UserID = " + ID.ToString();
                
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

                cmd.ExecuteNonQuery();
                return true;

            }
            else
            {
                return false;
            }


        }

        public bool updateUser(long ID, User userToSave)
        {

            MySql.Data.MySqlClient.MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * FROM user WHERE UserID = " + ID.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            if (mySQLReader.Read())
            {
                mySQLReader.Close();

                sqlString = "UPDATE user SET UserName= '" + userToSave.UserName + "', FirstName= '" + userToSave.FirstName + "', LastName='" + userToSave.LastName + "', Address='" + userToSave.Address + "', City='" + userToSave.City + "', State='" + userToSave.State + "', Zip=" + userToSave.Zip + ", Phone='" + userToSave.Phone + "', Email='" + userToSave.Email + "', Password='" + userToSave.Password + "' WHERE UserID = " + ID.ToString();

                cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

                cmd.ExecuteNonQuery();
                return true;

            }
            else
            {
                return false;
            }
        }



            public long saveUser(User userToSave)
        {
            String sqlString = "INSERT INTO user (UserName, FirstName, LastName, Address, City, State, Zip, Phone, Email, Password) VALUES ('" + userToSave.UserName + "','" + userToSave.FirstName + "','" + userToSave.LastName + "','" + userToSave.Address + "','" + userToSave.City + "','" + userToSave.State + "'," + userToSave.Zip + ",'" + userToSave.Phone + "','" + userToSave.Email + "','" + userToSave.Password + "')";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            cmd.ExecuteNonQuery();
            long id = cmd.LastInsertedId;
            return id;
        }
    }
}