using BIT_Services.Commands;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BIT_Services.Model
{
	static class DAL
	{
		private static MySqlCommand GetProcedure(string storedProcedureName)
		{
			MySqlCommand cmd = new MySqlCommand();
			MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

			cmd.Connection = conn;
			cmd.Connection.Open();
			cmd.CommandType = System.Data.CommandType.StoredProcedure;
			cmd.CommandText = storedProcedureName;

			return cmd;
		}



		/// <summary>
		/// Given a string of a stored procedure name and a parameter list, calls the stored procedure with the given parameters
		/// </summary>
		/// <param name="storedProcedureName"></param>
		/// <returns>DataReader containing results of procedure</returns>
		private static MySqlDataReader ExecuteReader(string storedProcedureName)
		{
			MySqlCommand cmd = GetProcedure(storedProcedureName);

			return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
		}

		/// <summary>
		/// Given a string of a stored procedure name and a parameter list, calls the stored procedure with the given parameters
		/// </summary>
		/// <param name="storedProcedureName"></param>
		/// <param name="parameterList"></param>
		/// <returns>DataReader containing results of procedure</returns>
		private static MySqlDataReader ExecuteReader(string storedProcedureName, MySqlParameter[] parameterList )
		{
			MySqlCommand cmd = GetProcedure(storedProcedureName);
			cmd.Parameters.AddRange(parameterList);

			return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
		}



		private static int ExecuteNonQuery(string storedProcedureName)
		{
			MySqlCommand cmd = GetProcedure(storedProcedureName);

			return cmd.ExecuteNonQuery();
		}

		private static int ExecuteNonQuery(string storedProcedureName, MySqlParameter[] parameterList)
		{
			MySqlCommand cmd = GetProcedure(storedProcedureName);
			cmd.Parameters.AddRange(parameterList);
			return cmd.ExecuteNonQuery();
		}

		private static object ExecuteScalar(string storedProcedureName)
		{
			MySqlCommand cmd = GetProcedure(storedProcedureName);
			
			return cmd.ExecuteScalar();
		}



		/// <summary>
		/// Takes username and password, and checks the database for an account that matches both.
		/// </summary>
		/// <returns>A user containing the usertype and id of the account found, or null if no account was found.</returns>
		public static User CheckLogin(string username, SecureString password)
		{
			MySqlParameter usernameParameter = new MySqlParameter("username", username);
			MySqlParameter passwordParameter = new MySqlParameter("password", Conversions.ConvertToUnsecureString(password));
			MySqlParameter[] parameterList = { usernameParameter, passwordParameter };
			string storedProcedureText = "usp_checkLogin";

			MySqlDataReader result = ExecuteReader(storedProcedureText, parameterList);

            if (result.HasRows)
            {
                result.Read();
                return new User((int)result.GetValue(1), (string)result.GetValue(0));
            }
            else
                return null;
		}



		/// <summary>
		/// Returns an ObservableCollection of suburbs from database.
		/// </summary>
		/// <returns></returns>
		public static SuburbList GetSuburbs()
		{
			string storedProcedureText = "usp_getSuburbs";

			MySqlDataReader result = ExecuteReader(storedProcedureText);

			SuburbList suburbList = new SuburbList();

			while (result.Read())
			{
				suburbList.Add(new Suburb(result.GetString("suburbName")));
			}

			return suburbList;
		}



		// -------------CLIENT LOGIC------------- //


		/// <summary>
		/// Returns a list of clients from the databsae
		/// </summary>
		/// <returns></returns>
		public static ClientList GetClients()
		{
			string storedProcedureText = "usp_getClients";

			MySqlDataReader result = ExecuteReader(storedProcedureText);

			ClientList clientList = new ClientList();

			while (result.Read())
			{
				clientList.Add(new Client(
					result.GetInt32("clientID"),
					result.GetString("clientName"),
					result.GetString("address"),
					new Suburb(result.GetString("suburb")),
					result.GetString("state"),
					result.GetString("contactPhone"),
					result.GetString("email"),
					result.GetString("notes")
					));
			}

			return clientList;
		}


		/// <summary>
		/// Inserts a client into the databsae with a unique uesrname an and a randomly generated password
		/// </summary>
		/// <param name="clientToInsert"></param>
		/// <returns></returns>
		public static int InsertClient(Client clientToInsert)
		{
			string storedProcedureName = "usp_insertClient";

			string emailPattern = "^(?:(?!@).)*";

			Regex email = new Regex(emailPattern);
			Match result = email.Match(clientToInsert.Email);

			int autoIncrement = GetClientAutoIncrement();

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("clientNameIn", clientToInsert.ClientName),
				new MySqlParameter("addressIn", clientToInsert.Address),
				new MySqlParameter("suburbIn", clientToInsert.Suburb),
				new MySqlParameter("stateIn", clientToInsert.State),
				new MySqlParameter("contactPhoneIn", clientToInsert.ContactPhone),
				new MySqlParameter("emailIn", clientToInsert.Email),
				new MySqlParameter("usernameIn", clientToInsert.ClientName[0] + result.Value + autoIncrement),
				new MySqlParameter("passwordIn", RandomPasswordGenerator.GeneratePassword() + autoIncrement),
				new MySqlParameter("notesIn", clientToInsert.Notes)
			};

			return ExecuteNonQuery(storedProcedureName, parameterList);
		}


		/// <summary>
		/// Updates all field for the client with the same ID with the details provided, wth the expception of username, password or ID
		/// </summary>
		/// <param name="clientToUpdate"></param>
		/// <returns></returns>
		public static int UpdateClient(Client clientToUpdate)
		{
			string storedProcedureName = "usp_updateClient";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("clientIDIn", clientToUpdate.ClientID),
				new MySqlParameter("clientNameIn", clientToUpdate.ClientName),
				new MySqlParameter("addressIn", clientToUpdate.Address),
				new MySqlParameter("suburbIn", clientToUpdate.Suburb),
				new MySqlParameter("stateIn", clientToUpdate.State),
				new MySqlParameter("contactPhoneIn", clientToUpdate.ContactPhone),
				new MySqlParameter("emailIn", clientToUpdate.Email),
				new MySqlParameter("notesIn", clientToUpdate.Notes)
			};

			return ExecuteNonQuery(storedProcedureName, parameterList);
		}



		/// <summary>
		/// Deletes the client with the same ID in the database as the client provided
		/// </summary>
		/// <param name="clientToDelete"></param>
		/// <returns></returns>
		public static int DeleteClient(Client clientToDelete)
		{
			string storedProcedureName = "usp_deleteClient";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("clientIDIn", clientToDelete.ClientID)
			};

			return ExecuteNonQuery(storedProcedureName, parameterList);
		}



		/// <summary>
		/// Returns the number that will be used for the ID for the next inserted client
		/// </summary>
		/// <returns></returns>
		public static int GetClientAutoIncrement()
		{
			string storedProcedureText = "usp_getClientAutoIncrement";

			return ExecuteNonQuery(storedProcedureText);
		}


		// -------------COORDINATOR LOGIC------------- //


		/// <summary>
		/// Returns list of coordinators obtained from databsae
		/// </summary>
		/// <returns></returns>
		public static CoordinatorList GetCoordinators()
		{
			string storedProcedureName = "usp_getCoordinators";

			MySqlDataReader result = ExecuteReader(storedProcedureName);

			CoordinatorList coordinatorList = new CoordinatorList();
			while(result.Read())
			{
				coordinatorList.Add(new Coordinator(
					result.GetInt32("coordinatorID"),
					result.GetString("firstName"),
					result.GetString("lastName"),
					result.GetString("address"),
					result.GetString("state"),
					new Suburb(result.GetString("suburb")),
					result.GetString("mobile"),
					result.GetString("email")));
			}
			return coordinatorList;
		}

		
		/// <summary>
		/// Deletes coordinator with the same ID in the database
		/// </summary>
		/// <param name="coordinatorToDelete"></param>
		/// <returns></returns>
		public static int DeleteCoordinator(Coordinator coordinatorToDelete)
		{
			string storedProcedureName = "usp_deleteCoordinator";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("coordinatorIDIn", coordinatorToDelete.CoordinatorID)
			};

			return ExecuteNonQuery(storedProcedureName, parameterList);
		}


		public static int UpdateCoordinator(Coordinator coordinatorToUpdate)
		{
			string storedProcedureName = "usp_updateCoordinator";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("coordinatorIDIn", coordinatorToUpdate.CoordinatorID),
				new MySqlParameter("firstNameIn", coordinatorToUpdate.FirstName),
				new MySqlParameter("lastNameIn", coordinatorToUpdate.LastName),
				new MySqlParameter("addressIn", coordinatorToUpdate.Address),
				new MySqlParameter("suburbIn", coordinatorToUpdate.Suburb),
				new MySqlParameter("stateIn", coordinatorToUpdate.State),
				new MySqlParameter("mobileIn", coordinatorToUpdate.Mobile),
				new MySqlParameter("emailIn", coordinatorToUpdate.Email)
			};

			return ExecuteNonQuery(storedProcedureName, parameterList);
		}

		/// <summary>
		/// Inserts specified coordinator into the database, does not require ID field
		/// </summary>
		/// <param name="coordinatorToInsert"></param>
		/// <returns></returns>
		public static int InsertCoordinator(Coordinator coordinatorToInsert)
		{
			string storedProcedureName = "usp_insertCoordinator";

			int autoIncrement = GetCoordinatorAutoIncrement();

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("firstNameIn", coordinatorToInsert.FirstName),
				new MySqlParameter("lastNameIn", coordinatorToInsert.LastName),
				new MySqlParameter("addressIn", coordinatorToInsert.Address),
				new MySqlParameter("stateIn", coordinatorToInsert.State),
				new MySqlParameter("suburbIn", coordinatorToInsert.Suburb),
				new MySqlParameter("mobileIn", coordinatorToInsert.Mobile),
				new MySqlParameter("emailIn", coordinatorToInsert.Email),
				new MySqlParameter("usernameIn", coordinatorToInsert.FirstName[0] + coordinatorToInsert.LastName + autoIncrement),
				new MySqlParameter("passwordIn", RandomPasswordGenerator.GeneratePassword() + autoIncrement)

			};
			return ExecuteNonQuery(storedProcedureName, parameterList);
		}

		/// <summary>
		/// Returns the number that will be used for the ID for the next inserted coordinator
		/// </summary>
		/// <returns></returns>
		public static int GetCoordinatorAutoIncrement()
		{
			string storedProcedureName = "usp_getCoordinatorAutoIncrement";

			return ExecuteNonQuery(storedProcedureName);
		}

		// -------------CONTRACTOR LOGIC------------- //

		public static ContractorList GetContractors()
		{
			string storedProcedureName = "usp_getContractors";

			MySqlDataReader result = ExecuteReader(storedProcedureName);

			ContractorList contractorList = new ContractorList();

			while (result.Read())
			{
				contractorList.Add(new Contractor(
					result.GetInt32("contractorID"),
					result.GetString("firstName"),
					result.GetString("lastName"),
					result.GetString("address"),
					result.GetString("state"),
					new Suburb(result.GetString("suburb")),
					result.GetString("mobile"),
					result.GetString("email")
					));
			}

			return contractorList;
		}



		public static int DeleteContractor(Contractor contractorToDelete)
		{
			string storedProcedureName = "usp_deleteContractor";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("contractorIDIn", contractorToDelete.ContractorID)
			};

			return ExecuteNonQuery(storedProcedureName, parameterList);
		}



		public static int UpdateContractor(Contractor contractorToUpdate)
		{
			string storedProcedureName = "usp_updateContractor";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("contractorIDIn", contractorToUpdate.ContractorID),
				new MySqlParameter("firstNameIn", contractorToUpdate.FirstName),
				new MySqlParameter("lastNameIn", contractorToUpdate.LastName),
				new MySqlParameter("addressIn", contractorToUpdate.Address),
				new MySqlParameter("suburbIn", contractorToUpdate.Suburb),
				new MySqlParameter("stateIn", contractorToUpdate.State),
				new MySqlParameter("mobileIn", contractorToUpdate.Mobile),
				new MySqlParameter("emailIn", contractorToUpdate.Email),
			};
			int rowsAffected = ExecuteNonQuery(storedProcedureName, parameterList);

			return rowsAffected;
		}



		public static int InsertContractor(Contractor contractorToInsert)
		{
			string storedProcedureName = "usp_insertContractor";

			int autoIncrement = GetContractorAutoIncrement();

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("firstNameIn", contractorToInsert.FirstName),
				new MySqlParameter("lastNameIn", contractorToInsert.LastName),
				new MySqlParameter("addressIn", contractorToInsert.Address),
				new MySqlParameter("stateIn", contractorToInsert.State),
				new MySqlParameter("suburbIn", contractorToInsert.Suburb),
				new MySqlParameter("mobileIn", contractorToInsert.Mobile),
				new MySqlParameter("emailIn", contractorToInsert.Email),
				new MySqlParameter("usernameIn", contractorToInsert.FirstName[0] + contractorToInsert.LastName + autoIncrement),
				new MySqlParameter("passwordIn", RandomPasswordGenerator.GeneratePassword() + autoIncrement),
			};

			return ExecuteNonQuery(storedProcedureName, parameterList);
		}



		public static SuburbList PreferredSuburbsList(Contractor contractor)
		{
			string storedProcedureName = "usp_getContractorSuburbs";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("contractorIDIn", contractor.ContractorID)
			};

			MySqlDataReader result = ExecuteReader(storedProcedureName, parameterList);

			SuburbList suburbList = new SuburbList();

			while (result.Read())
			{
				suburbList.Add(new Suburb(result.GetString("suburb")));
			}

			return suburbList;
		}

		public static SuburbList UnselectedSuburbsList(Contractor contractor)
		{
			string storedProcedureName = "usp_getContractorUnselectedSuburbs";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("contractorIDIn", contractor.ContractorID)
			};

			MySqlDataReader result = ExecuteReader(storedProcedureName, parameterList);

			SuburbList suburbList = new SuburbList();

			while (result.Read())
			{
				suburbList.Add(new Suburb(result.GetString("SuburbName")));
			}

			return suburbList;
		}



		public static SkillList HasSkillList(Contractor contractor)
		{
			string storedProcedureName = "usp_getContractorSelectedSkills";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("contractorIDIn", contractor.ContractorID)
			};

			MySqlDataReader result = ExecuteReader(storedProcedureName, parameterList);

			SkillList skillList = new SkillList();

			while (result.Read())
			{
				skillList.Add(new Skill(
					result.GetString("skillName"),
					result.GetString("skillDescription")
					));
			}

			return skillList;
		}

		public static SkillList UnselectedSkillList(Contractor contractor)
		{
			string storedProcedureName = "usp_getContractorUnselectedSkills";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("contractorIDIn", contractor.ContractorID)
			};

			MySqlDataReader result = ExecuteReader(storedProcedureName, parameterList);

			SkillList skillList = new SkillList();

			while (result.Read())
			{
				skillList.Add(new Skill(
					result.GetString("skillName"),
					result.GetString("skillDescription")
					));
			}

			return skillList;
		}



		public static int AddPreferredSuburb(Contractor contractor, Suburb suburb)
		{
			string storedProcedureName = "usp_addPreferredSuburb";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("contractorIdIn", contractor.ContractorID),
				new MySqlParameter("suburbNameIn", suburb.SuburbName)
			};

			return ExecuteNonQuery(storedProcedureName, parameterList);
		}

		public static int RemovePreferredSuburb(Contractor contractor, Suburb suburb)
		{
			string storedProcedureName = "usp_removePreferredSuburb";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("contractorIdIn", contractor.ContractorID),
				new MySqlParameter("suburbNameIn", suburb.SuburbName)
				
			};

			return ExecuteNonQuery(storedProcedureName, parameterList);
		}



		public static int AddHasSkill(Contractor contractor, Skill skill)
		{
			string storedProcedureName = "usp_addHasSkill";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("contractorIdIn", contractor.ContractorID),
				new MySqlParameter("skillNameIn", skill.SkillName)
			};

			return ExecuteNonQuery(storedProcedureName, parameterList);
		}

		public static int RemoveHasSkill(Contractor contractor, Skill skill)
		{
			string storedProcedureName = "usp_removeHasSkill";

			MySqlParameter[] parameterList = new MySqlParameter[]
			{
				new MySqlParameter("contractorIdIn", contractor.ContractorID),
				new MySqlParameter("skillNameIn", skill.SkillName)
			};

			return ExecuteNonQuery(storedProcedureName, parameterList);
		}



		public static int GetContractorAutoIncrement()
		{
			string storedProcedureName = "usp_getContractorAutoIncrement";

			return ExecuteNonQuery(storedProcedureName);
		}
	}
}
