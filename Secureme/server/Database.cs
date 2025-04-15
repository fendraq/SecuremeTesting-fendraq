namespace DefaultNamespace;

using Npgsql;
using DotNetEnv;
using System;
using System.Threading.Tasks;

public class Database
{
    private readonly string _host;
    private readonly string _port;
    private readonly string _user;
    private readonly string _password;
    private readonly string _database;
    private readonly string _connectionString;

    public Database()
    {
        // Read environment variables loaded by Env.Load(...)
        _host = Environment.GetEnvironmentVariable("DB_HOST");
        _port = Environment.GetEnvironmentVariable("DB_PORT");
        _user = Environment.GetEnvironmentVariable("DB_USER");
        _password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        _database = Environment.GetEnvironmentVariable("DB_NAME");

        // Build the PostgreSQL connection string
        _connectionString = 
            $"Host={_host};Port={_port};Username={_user};Password={_password};Database={_database}";
    }

    public NpgsqlConnection GetConnection()
    {
        // Create and return a fresh connection
        return new NpgsqlConnection(_connectionString);
    }

    public void TestConnection()
    {
        Console.WriteLine("Starting database connection test...");
        Console.WriteLine($"Attempting to connect to: Host={_host}, Port={_port}, Database={_database}, User={_user}");
        
        using var connection = GetConnection();
        try
        {
            var timeout = TimeSpan.FromSeconds(10);
            var connectTask = Task.Run(() => connection.Open());
            
            if (Task.WaitAny(new[] { connectTask }, timeout) == -1)
            {
                throw new TimeoutException($"Database connection timed out after {timeout.TotalSeconds} seconds");
            }

            // Test the connection with a simple query
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT 1";
            cmd.ExecuteScalar();

            Console.WriteLine("Database connection test successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connection error: {ex.GetType().Name} - {ex.Message}");
            Console.WriteLine($"Connection string (without password): Host={_host};Port={_port};Username={_user};Database={_database}");
            
            // In CI environment, we want to fail fast
            throw new Exception("Database connection test failed. See above logs for details.", ex);
        }
    }
}