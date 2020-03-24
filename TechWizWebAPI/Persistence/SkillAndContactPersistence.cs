using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechWizWebAPI.Models;

namespace TechWizWebAPI
{
    public class SkillAndContactPersistence
    {
        private MySqlConnection conn;

        public SkillAndContactPersistence()
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

        public ArrayList getSkills() {
            ArrayList skillsArray = new ArrayList();

            MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * FROM skills ORDER BY name";
            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            while (mySQLReader.Read())
            {
                Skill s = new Skill();
                s.id = mySQLReader.GetInt32(0);
                s.name = mySQLReader.GetString(1);
                skillsArray.Add(s);
            }

            return skillsArray;

        }

        public ArrayList getContactMethods()
        {
            ArrayList cmArray = new ArrayList();

            MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * FROM contactMethods ORDER BY MethodName";
            MySqlCommand cmd = new MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            while (mySQLReader.Read())
            {
                ContactMethod s = new ContactMethod();
                s.id = mySQLReader.GetInt32(0);
                s.MethodName = mySQLReader.GetString(1);
                cmArray.Add(s);
            }

            return cmArray;

        }

    }
}