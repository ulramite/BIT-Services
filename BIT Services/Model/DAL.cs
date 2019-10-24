using BIT_Services.Commands;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Model
{
	static class DAL
	{
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static User CheckLogin(string username, SecureString password)
		{
			{ 
				MySqlParameter usernameParameter = new MySqlParameter("username", username);
				MySqlParameter passwordParameter = new MySqlParameter("password", Conversions.ConvertToUnsecureString(password));

				MySqlCommand cmd = new MySqlCommand();
				MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

				cmd.Connection = conn;
				cmd.Connection.Open();
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.CommandText = "usp_checkLogin";

				cmd.Parameters.Add(usernameParameter);
				cmd.Parameters.Add(passwordParameter);

				MySqlDataReader result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (result.HasRows)
                {
                    result.Read();
                    return new User((int)result.GetValue(1), (string)result.GetValue(0));
                }
                else
                    return null;
            }
		}
	}
}
