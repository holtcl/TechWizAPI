using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using TechWizWebAPI.Models;
using MySql.Data;

namespace TechWizWebAPI
{
    public class InvoicePersistence
    {
        private MySql.Data.MySqlClient.MySqlConnection conn;

        public InvoicePersistence()
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

        public ArrayList getInvoices()
        {
            ArrayList invoiceArray = new ArrayList();
            MySql.Data.MySqlClient.MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * From invoice";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            while (mySQLReader.Read())
            {
                Invoice i = new Invoice();
                i.invoiceID = mySQLReader.GetInt32(0);
                i.user = mySQLReader.GetInt32(1);
                i.wizard = mySQLReader.GetInt32(2);
                i.hoursWorked = mySQLReader.GetDouble(3);
                i.rate = mySQLReader.GetDouble(4);
                i.amountDue = mySQLReader.GetDouble(5);
                i.openDate = mySQLReader.GetDateTime(6);
                i.dueDate = mySQLReader.GetDateTime(7);
                i.status = mySQLReader.GetInt32(8);
                invoiceArray.Add(i);
            }
            return invoiceArray;
        }


        public Invoice getInvoice(long ID)
        {
            Invoice i = new Invoice();
            MySql.Data.MySqlClient.MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * From invoice WHERE InvoiceID = " + ID.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            if (mySQLReader.Read())
            {
                i.invoiceID = mySQLReader.GetInt32(0);
                i.user = mySQLReader.GetInt32(1);
                i.wizard = mySQLReader.GetInt32(2);
                i.hoursWorked = mySQLReader.GetDouble(3);
                i.rate = mySQLReader.GetDouble(4);
                i.amountDue = mySQLReader.GetDouble(5);
                i.openDate = mySQLReader.GetDateTime(6);
                i.dueDate = mySQLReader.GetDateTime(7);
                i.status = mySQLReader.GetInt32(8);
                return i;
            }
            else
            {
                return null;
            }
        }

        public bool deleteInvoice(long ID)
        {
            Invoice i = new Invoice();
            MySql.Data.MySqlClient.MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * From invoice WHERE InvoiceID = " + ID.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            if (mySQLReader.Read())
            {
                mySQLReader.Close();
                sqlString = "DELETE From invoice WHERE InvoiceID = " + ID.ToString();
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

                cmd.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool updateInvoice(long ID, Invoice invoiceToSave)
        {

            MySql.Data.MySqlClient.MySqlDataReader mySQLReader = null;

            String sqlString = "SELECT * From invoice WHERE InvoiceID = " + ID.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            mySQLReader = cmd.ExecuteReader();
            if (mySQLReader.Read())
            {
                mySQLReader.Close();
                sqlString = "UPDATE invoice SET UserID= " + invoiceToSave.user + ", WizardID= " + invoiceToSave.wizard + ", HoursWorked= " + invoiceToSave.hoursWorked + ", Rate= " + invoiceToSave.rate + ", AmountDue= " + invoiceToSave.amountDue + ", OpenDate= '" + invoiceToSave.openDate.ToString("yyyy-MM-dd HH:mm:ss") + "' , DueDate = '" + invoiceToSave.dueDate.ToString("yyyy-MM-dd HH:mm:ss") + "', Status = " + invoiceToSave.status + " WHERE InvoiceID = " + ID.ToString();
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

                cmd.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            }
        }

        public long saveInvoice(Invoice invoiceToSave)
        {
            String sqlString = "INSERT INTO invoice (UserID, WizardID, HoursWorked, Rate, AmountDue, OpenDate, DueDate, Status) VALUES ( " + invoiceToSave.user + ", " + invoiceToSave.wizard + ", " + invoiceToSave.hoursWorked + ", " + invoiceToSave.rate + ", " + invoiceToSave.amountDue + ", '" + invoiceToSave.openDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + invoiceToSave.dueDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " + invoiceToSave.status + ")";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            cmd.ExecuteNonQuery();
            long id = cmd.LastInsertedId;
            return id;
        }

    }
}