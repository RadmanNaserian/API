using System.Data;

using Microsoft.Data.SqlClient;
namespace WebApplication1.Service
{
    

public class DataService : IDisposable
{
    public readonly string _connectionString;
    private readonly SqlConnection _connection;
    public DataService()
    {
        _connectionString = @$"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SchoolProject;Integrated Security=True;TrustServerCertificate=True";
        _connection = new SqlConnection(_connectionString);
    }

    // Open Connection
    private async Task EnsureConnectionAsync()
    {
        if (_connection.State == ConnectionState.Closed)
        {
            await _connection.OpenAsync();
        }
    }

    // Execute Non-Query (INSERT, UPDATE, DELETE)
    public async Task<int> ExecuteNonQueryAsync(string query, Dictionary<string, object> parameters)
    {
        await EnsureConnectionAsync();
        using SqlCommand cmd = new SqlCommand(query, _connection);

        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
        }

        return await cmd.ExecuteNonQueryAsync();
    }

    // Execute Scalar (Returns a single value)
    public async Task<object?> ExecuteScalarAsync(string query, Dictionary<string, object> parameters)
    {
        await EnsureConnectionAsync();
        using SqlCommand cmd = new SqlCommand(query, _connection);

        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
        }

        return await cmd.ExecuteScalarAsync();
    }

    // Execute Query and Return DataTable (SELECT Queries)
    public async Task<DataTable> ExecuteQueryAsync(string query, Dictionary<string, object> parameters)
    {
        await EnsureConnectionAsync();
        using SqlCommand cmd = new SqlCommand(query, _connection);

        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
        }

        using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        return dt;
    }

    // public DataView GetAllStudents()
    // {
    //     SqlConnection con = new SqlConnection(_connectionString);
    //     string query = "select * from students";

    //     SqlDataAdapter adpt = new SqlDataAdapter(query, con);
    //     DataSet ds = new DataSet();
    //     adpt.Fill(ds);
    //     DataView dv = new DataView();
    //     dv = ds.Tables[0].DefaultView;
    //     return dv;
    // }
    
    // public DataView GetStudentById(int selectedId)
    // {
    //     SqlConnection con = new SqlConnection(_connectionString);
    //     string query = $"select * from students where Id = {selectedId}";

    //     SqlDataAdapter adpt = new SqlDataAdapter(query, con);
    //     DataSet ds = new DataSet();
    //     adpt.Fill(ds);

    //     DataView dv = new DataView();
    //     dv = ds.Tables[0].DefaultView;
    //     return dv;
    // }
    public async Task<bool> UpdateStudent(string id , string firstName , string lastName)
    {
        string query = "update students set FirstName = @firstName , LastName = @lastName where Id = @Id";

        var parameters = new Dictionary<string, object>
            {
                { "@Id",id },
                {"@firstName",firstName},
                {"@lastName",lastName}
            };
             int result = await ExecuteNonQueryAsync(query, parameters);

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;   
            }
    }


    // Dispose Connection
    public void Dispose()
    {
        if (_connection != null)
        {
            _connection.Dispose();
        }
    }

}
}