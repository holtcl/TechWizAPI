using System;
using TechWizWebAPI.Models;
using System.Collections;
using MySql.Data.MySqlClient;


namespace TechWizWebAPI
{
    public class UserPersistence
    {
        private MySqlConnection conn;

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
            catch (MySqlException ex)
            {

            }
        }

        public ArrayList getUsers()
        {
            ArrayList userArray = new ArrayList();

            MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * FROM user";
            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

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
            MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * FROM user WHERE UserID = @ID;";

            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            cmd.Prepare();

            cmd.Parameters.AddWithValue("@ID", ID);

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

        public User GetUserByCredentials(String username, String password)
        {
            User u = new User();
            MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * FROM user WHERE UserName = @UserName AND Password = @Password;";

            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            cmd.Prepare(); 

            cmd.Parameters.AddWithValue("@UserName", username);
            cmd.Parameters.AddWithValue("@Password", password);

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
            //return true if the user existed, false if it did not

            String sqlString = "DELETE FROM user WHERE UserID = @ID";

            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            cmd.Prepare();

            cmd.Parameters.AddWithValue ("@ID", ID);

            int rowsDeleted = cmd.ExecuteNonQuery();

            if (rowsDeleted > 0) 
            {
                return true; 
            }
            else
            {
                return false;
            }
        }

        public bool updateUser(long ID, User userToSave)
        {

            MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * FROM user WHERE UserID = @ID";
            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            cmd.Prepare();

            cmd.Parameters.AddWithValue("@ID", ID);

            mySQLReader = cmd.ExecuteReader();
            if (mySQLReader.Read())
            {
                mySQLReader.Close();

                sqlString = "UPDATE user SET UserName=@UserName, FirstName=@FirstName, LastName=@LastName, Address=@Address, City=@City, State=@State, Zip=@Zip, Phone=@Phone, Email=@Email, Password=@Password WHERE UserID = @ID";

                cmd = new MySqlCommand(sqlString, conn);

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
            String sqlString = "INSERT INTO user (UserName, FirstName, LastName, Address, City, State, Zip, Phone, Email, Password) VALUES (@UserName,@FirstName,@LastName,@Address,@City,@State,@Zip,@Phone,@Email,@Password)";
            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            cmd.Prepare();

            cmd.Parameters.AddWithValue("@UserName", userToSave.UserName);
            cmd.Parameters.AddWithValue("@FirstName", userToSave.FirstName);
            cmd.Parameters.AddWithValue("@LastName", userToSave.LastName);
            cmd.Parameters.AddWithValue("@Address", userToSave.Address);
            cmd.Parameters.AddWithValue("@City", userToSave.City);
            cmd.Parameters.AddWithValue("@State", userToSave.State);
            cmd.Parameters.AddWithValue("@Zip", userToSave.Zip);
            cmd.Parameters.AddWithValue("@Phone", userToSave.Phone);
            cmd.Parameters.AddWithValue("@Email", userToSave.Email);
            cmd.Parameters.AddWithValue("@Password", userToSave.Password);

            cmd.ExecuteNonQuery();
            long id = cmd.LastInsertedId;
            return id;
        }
    }
}