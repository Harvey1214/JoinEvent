using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Web;
using System.Net;

namespace JoinEventDataAccessLibrary
{
    class Database
    {
        // for testing this should be 0, for release it should be 1, for release on freehosting should be 2
        public static int live = 1;

        private static string[] connectionStrings = new string[] { @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\mikuh\source\repos\Dynacon\Dynacon\App_Data\Database.mdf;Integrated Security=True", @"Data Source=81.2.195.160;Integrated Security=False;User ID=f139218;Connect Timeout=15;password=faT39C2F;Encrypt=False;Packet Size=4096", @"server=free.aspify.com;uid=db1477;pwd=2560Db;database=db1477" };
        private SqlConnection con = new SqlConnection(connectionStrings[live]);

        private SqlDataReader rdr;

        public void SignUp(Credentials credentials)
        {
            int newUserID = GetHighestID("Id", "UsersTable") + 1;

            CheckForOpenDatabaseConnections();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "INSERT INTO UsersTable VALUES('" + newUserID + "','" + credentials.Email + "','" + credentials.Password + "','" + 0 + "')";
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public bool NewOrder(Order order)
        {
            try
            {
                int newOrderID = GetHighestID("Id", "OrdersTable") + 1;

                CheckForOpenDatabaseConnections();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "INSERT INTO OrdersTable VALUES('" + newOrderID + "','" + order.UserWhoOrdered + "','" + null + "','" + 0 + "','" + order.Links + "','" + order.Date.ToString() + "','" + order.Street + "','" + order.City + "','" + order.PostalCode + "','" + order.PhoneNumber + "','" + order.Country + "','" + order.FullName + "','" + order.Price + "','" + order.ShippingBudget + "')";
                cmd.ExecuteNonQuery();
                con.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Authenticate(Credentials credentials)
        {
            if (credentials != null)
            {
                List<string> trueCredentials = GetRowInformation("Id", GetUserID(credentials.Email), "UsersTable");

                Credentials auth = new Credentials();
                if (trueCredentials != null)
                {
                    if (trueCredentials.Count > 3)
                    {
                        auth.Email = trueCredentials[1];
                        auth.Password = trueCredentials[2];
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                if (auth.Email == credentials.Email)
                {
                    if (auth.Password == credentials.Password)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public bool UpdateOrderStatus(int orderId, int status, int deliveryUser)
        {
            try
            {
                CheckForOpenDatabaseConnections();

                SqlCommand cmd = new SqlCommand($"UPDATE OrdersTable SET Status='" + status + "',UserWhoDelivers='" + deliveryUser + "' where Id='" + orderId + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateOrderStatus(int orderId, int status)
        {
            try
            {
                CheckForOpenDatabaseConnections();

                SqlCommand cmd = new SqlCommand($"UPDATE OrdersTable SET Status={status} where Id={orderId}", con);
                cmd.ExecuteNonQuery();
                con.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateEarnedAmount(int userId, float amount)
        {
            try
            {
                CheckForOpenDatabaseConnections();

                SqlCommand cmd = new SqlCommand($"UPDATE UsersTable SET Earned={amount} where Id={userId}", con);
                cmd.ExecuteNonQuery();
                con.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<List<string>> GetAllRows(string column, string ID, string table)
        {
            CheckForOpenDatabaseConnections();

            SqlCommand cmd = new SqlCommand($"SELECT * FROM {table} WHERE {column} = @value", con);
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = "@value";
            param.Value = ID;
            cmd.Parameters.Add(param);

            CheckForReaderState();

            rdr = cmd.ExecuteReader();

            List<List<string>> returnLists = new List<List<string>>();

            while (rdr.Read())
            {
                List<string> returnList = new List<string>();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    returnList.Add(rdr[i].ToString());
                }
                returnLists.Add(returnList);
            }

            rdr.Close();
            con.Close();

            return returnLists;
        }

        public List<List<string>> GetAllRows(string column, int ID, string table)
        {
            CheckForOpenDatabaseConnections();

            SqlCommand cmd = new SqlCommand($"SELECT * FROM {table} WHERE {column} = @value", con);
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = "@value";
            param.Value = ID;
            cmd.Parameters.Add(param);

            CheckForReaderState();

            rdr = cmd.ExecuteReader();

            List<List<string>> returnLists = new List<List<string>>();

            while (rdr.Read())
            {
                List<string> returnList = new List<string>();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    returnList.Add(rdr[i].ToString());
                }
                returnLists.Add(returnList);
            }

            rdr.Close();
            con.Close();

            return returnLists;
        }

        public List<List<string>> GetAllRows(string column, int ID, string table, string condition)
        {
            CheckForOpenDatabaseConnections();

            SqlCommand cmd = new SqlCommand($"SELECT * FROM {table} WHERE {column} {condition} @value", con);
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = "@value";
            param.Value = ID;
            cmd.Parameters.Add(param);

            CheckForReaderState();

            rdr = cmd.ExecuteReader();

            List<List<string>> returnLists = new List<List<string>>();

            while (rdr.Read())
            {
                List<string> returnList = new List<string>();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    returnList.Add(rdr[i].ToString());
                }
                returnLists.Add(returnList);
            }

            rdr.Close();
            con.Close();

            return returnLists;
        }

        public List<string> GetRowInformation(string column, int ID, string table)
        {
            CheckForOpenDatabaseConnections();

            SqlCommand cmd = new SqlCommand($"SELECT * FROM {table} WHERE {column} = @value", con);
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = "@value";
            param.Value = ID;
            cmd.Parameters.Add(param);

            CheckForReaderState();

            rdr = cmd.ExecuteReader();
            rdr.Read();

            List<string> returnList = new List<string>();

            for (int i = 0; i < rdr.FieldCount; i++)
            {
                returnList.Add(rdr[i].ToString());
            }

            rdr.Close();
            con.Close();

            return returnList;
        }

        public List<string> GetRowInformation(string column, string ID, string table)
        {
            CheckForOpenDatabaseConnections();

            SqlCommand cmd = new SqlCommand($"SELECT * FROM {table} WHERE {column} = @value", con);
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = "@value";
            param.Value = ID;
            cmd.Parameters.Add(param);

            CheckForReaderState();

            rdr = cmd.ExecuteReader();
            rdr.Read();

            List<string> returnList = new List<string>();

            for (int i = 0; i < rdr.FieldCount; i++)
            {
                returnList.Add(rdr[i].ToString());
            }

            rdr.Close();
            con.Close();

            return returnList;
        }

        private void DeleteRow(string column, int ID, string table)
        {
            CheckForOpenDatabaseConnections();

            SqlCommand cmd = new SqlCommand($"DELETE FROM {table} WHERE {column} = @value", con);
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = "@value";
            param.Value = ID;
            cmd.Parameters.Add(param);
            cmd.ExecuteNonQuery();

            con.Close();
        }

        public int GetUserID(string email)
        {
            CheckForOpenDatabaseConnections();

            SqlCommand cmd = new SqlCommand("SELECT * FROM UsersTable WHERE Email = @Email", con);
            cmd.Parameters.Add(new SqlParameter("@Email", System.Data.SqlDbType.VarChar));
            cmd.Parameters["@Email"].Value = email;

            CheckForReaderState();

            rdr = cmd.ExecuteReader();
            rdr.Read();
            int userID = Convert.ToInt32(rdr[0]);

            rdr.Close();
            con.Close();

            return userID;
        }

        private int GetHighestID(string column, string table)
        {
            CheckForOpenDatabaseConnections();

            SqlCommand cmd = new SqlCommand($"SELECT MAX({column}) FROM {table}", con);

            CheckForReaderState();

            rdr = cmd.ExecuteReader();
            rdr.Read();
            int highestID = Convert.ToInt32(rdr[0]);

            rdr.Close();
            con.Close();

            return highestID;
        }

        private void CheckForOpenDatabaseConnections()
        {
            if (con.State != System.Data.ConnectionState.Open)
            {
                con.Open();
            }
        }

        private void CheckForReaderState()
        {
            if (rdr != null)
            {
                if (rdr.IsClosed == false)
                {
                    rdr.Close();
                }
            }
        }
    }
}
