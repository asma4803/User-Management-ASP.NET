using BSEF15M039_Assign5.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BSEF15M039_Assign5.Models
{
    public static class DAL
    {
        public static AdminDTO validateAdmin(string username, string password)
        {
            string connString = @"Data Source=.\SQLEXPRESS2012; initial Catalog=Assignment5; User Id=sa;Password=savechange; ";
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    string query = "Select * from dbo.Admin where Login= '" + username + "' AND password = '" + password + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read() == true)
                    {
                        AdminDTO admin = new AdminDTO();
                        admin.name = reader.GetString(0);
                        admin.adminId = reader.GetInt32(3);
                        admin.login = reader.GetString(1);
                        return admin;
                    }
                    return null;
                }
                catch (Exception ex) {
                    return null;
                }
            }
        }
        public static List<UserDTO> getAllUsers()
        {
            string connString = @"Data Source=.\SQLEXPRESS2012; initial Catalog=Assignment5; User Id=sa;Password=savechange; ";
            using (SqlConnection con = new SqlConnection(connString)) {
                try {
                    con.Open();
                    List<UserDTO> userList = new List<UserDTO>();
                    string query = "Select * from dbo.Users";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read()) {
                        UserDTO user = new UserDTO();
                        user.userid = r.GetInt32(0);
                        user.name = r.GetString(1);
                        user.login = r.GetString(2);
                        user.gender = r.GetString(4);
                        user.address = r.GetString(5);
                        user.age = r.GetInt32(6);
                        user.NIC = r.GetString(7);
                        user.DOB = r.GetDateTime(8);
                        user.cricket = r.GetBoolean(9);
                        user.hockey = r.GetBoolean(10);
                        user.chess = r.GetBoolean(11);
                        user.imageName = r.GetString(12);
                        user.createdon = r.GetDateTime(13);
                        user.email = r.GetString(14);
                        userList.Add(user);
                    } 
                    return userList; 
                }
                catch (Exception ex) {
                    return null;
                }
            }
        }

        public static int saveUser(UserDTO user)
        {
            int res = -1;
            string connString = @"Data Source=.\SQLEXPRESS2012; initial Catalog=Assignment5; User Id=sa;Password=savechange; ";
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    string query = "";
                    if (user.userid > 0)
                    {
                        query = "Update dbo.Users Set Name='"+user.name+"', Login='"+user.login+"', Password='"+user.password+"',ImageName='"+user.imageName+"',Gender='"+user.gender+"',Address='"+user.address+"',Age='"+user.age+"',NIC='"+user.NIC+"',DOB='"+user.DOB+"',Cricket='"+user.cricket+"',Hockey='"+user.hockey+"',Chess='"+user.chess+"',Email='"+user.email+"' where userid= '"+user.userid+"'" ;
                        SqlCommand cmd = new SqlCommand(query, con);
                        int r = cmd.ExecuteNonQuery();
                        if (r > -1)
                        {
                            return r;
                        }
                        else
                            return -1;
                    }
                    else if (user.userid <= 0)
                    {
                        DateTime d = DateTime.Today.Date;
                        query = "insert into dbo.users (Name,Login,Password,Gender,Address,Age,NIC,DOB,Cricket,Hockey,Chess,ImageName,CreatedOn,Email) values('" + user.name + "','" + user.login + "','" + user.password + "','" + user.gender + "','" + user.address + "','" + user.age + "','" + user.NIC + "','" + user.DOB + "','" + user.cricket + "','" + user.hockey + "','" + user.chess + "','" + user.imageName + "','" + d + "','" + user.email + "')";
                        query = query + "; Select Scope_Identity()";
                        SqlCommand cmd = new SqlCommand(query, con);
                        int r = Convert.ToInt32(cmd.ExecuteScalar());
                        if (r > -1)
                        {
                            res = r;
                            return res;
                        }
                        else
                        {
                            return res;
                        }
                    }
                    return -1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
           
        }

        public static UserDTO getUser(int id) {
            UserDTO user = new UserDTO();
            string connString = @"Data Source=.\SQLEXPRESS2012; initial Catalog=Assignment5; User Id=sa;Password=savechange; ";
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    string query = "Select * from dbo.Users where userid='"+id+"'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read()) {
                        user.userid = r.GetInt32(0);
                        user.name = r.GetString(1);
                        user.login = r.GetString(2);
                        user.password = r.GetString(3);
                        user.gender = r.GetString(4);
                        user.address = r.GetString(5);
                        user.age = r.GetInt32(6);
                        user.NIC = r.GetString(7);
                        user.DOB = r.GetDateTime(8);
                        user.cricket = r.GetBoolean(9);
                        user.hockey = r.GetBoolean(10);
                        user.chess = r.GetBoolean(11);
                        user.imageName = r.GetString(12);
                        user.createdon = r.GetDateTime(13);
                        user.email = r.GetString(14);
                    }
                    return user;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
                    
        }

        public static UserDTO validateUser(string username, string password)
        {
            string connString = @"Data Source=.\SQLEXPRESS2012; initial Catalog=Assignment5; User Id=sa;Password=savechange; ";
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    string query = "Select * from dbo.Users where Login='" + username + "' AND Password ='" + password + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader r = cmd.ExecuteReader();

                    if (r.Read() == true)
                    {
                        UserDTO user = new UserDTO();
                        user.userid = r.GetInt32(0);
                        user.name = r.GetString(1);
                        user.login = r.GetString(2);
                        user.password = r.GetString(3);
                        user.gender = r.GetString(4);
                        user.address = r.GetString(5);
                        user.age = r.GetInt32(6);
                        user.NIC = r.GetString(7);
                        user.DOB = r.GetDateTime(8);
                        user.cricket = r.GetBoolean(9);
                        user.hockey = r.GetBoolean(10);
                        user.chess = r.GetBoolean(11);
                        user.imageName = r.GetString(12);
                        user.createdon = r.GetDateTime(13);
                        user.email = r.GetString(14);
                        
                        return user;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public static int findUserByEmail(string email) {
            int id = 0;
            string connString = @"Data Source=.\SQLEXPRESS2012; initial Catalog=Assignment5; User Id=sa;Password=savechange; ";
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    string query = "Select * from dbo.Users where Email='"+email+"'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd.Read())
                    {
                        id = rd.GetInt32(0);
                    }
                    return id;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
        public static bool updatePassword(int id,string password) {
            string connString = @"Data Source=.\SQLEXPRESS2012; initial Catalog=Assignment5; User Id=sa;Password=savechange; ";
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    string query = "Update dbo.Users Set password='" + password + "' where UserId='" + id + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    int r = cmd.ExecuteNonQuery();
                    if (r > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }              
        }
        public static int deleteUser(int id) {
            int result = 0;
            string connString = @"Data Source=.\SQLEXPRESS2012; initial Catalog=Assignment5; User Id=sa;Password=savechange; ";
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    string query = "Delete from dbo.Users where userid='"+id+"'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    result = cmd.ExecuteNonQuery();
                    return result;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
        public static int emailAlreadyExists(string email) {
            int result = 0;
            string connString = @"Data Source=.\SQLEXPRESS2012; initial Catalog=Assignment5; User Id=sa;Password=savechange; ";
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    string query = "Select * from dbo.Users where email='"+email+"'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader r = cmd.ExecuteReader();
                    if (r.Read()) {
                        result = 1;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
                  
        }
        public static int loginAlreadyExists(string login)
        {
            int result = 0;
            string connString = @"Data Source=.\SQLEXPRESS2012; initial Catalog=Assignment5; User Id=sa;Password=savechange; ";
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    string query = "Select * from dbo.Users where Login='" + login + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        result = 1;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }

        }
        public static int NICAlreadyExists(string NIC)
        {
            int result = 0;
            string connString = @"Data Source=.\SQLEXPRESS2012; initial Catalog=Assignment5; User Id=sa;Password=savechange; ";
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    string query = "Select * from dbo.Users where NIC='" + NIC + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        result = 1;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }

        }
    }
}