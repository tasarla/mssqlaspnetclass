using System;
using System.Data;
using System.Data.SqlClient;

public class MSSQLDatabase
{
    private readonly string connectionString;

    public MSSQLDatabase(string connectionStr)
    {
        connectionString = connectionStr;
    }

    public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
    {
        DataTable dataTable = new DataTable();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataTable);
            }
        }
        return dataTable;
    }

    public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteNonQuery();
            }
        }
    }

    public SqlParameter CreateParameter(string name, object value)
    {
        return new SqlParameter(name, value);
    }
}




string connectionString = "Server=myServerAddress;Database=myDatabase;User Id=myUsername;Password=myPassword;";
MSSQLDatabase db = new MSSQLDatabase(connectionString);

// Sorgu örneği:
string query = "SELECT * FROM MyTable WHERE ColumnName = @Value";
SqlParameter[] parameters = { db.CreateParameter("@Value", "SomeValue") };
DataTable result = db.ExecuteQuery(query, parameters);

// Sorgu çalıştırma örneği:
string insertQuery = "INSERT INTO MyTable (ColumnName) VALUES (@Value)";
SqlParameter[] insertParameters = { db.CreateParameter("@Value", "NewValue") };
int rowsAffected = db.ExecuteNonQuery(insertQuery, insertParameters);
