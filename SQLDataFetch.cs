using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

public class SQLDataFetch
{
    DebugMode _debug = new DebugMode();

    public Task SQLDataFetchAsync(string connectionString)
    {
        DebugMode(connectionString);

        try
        {
            SqlConnection sqlConnection;

            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            Console.WriteLine(sqlConnection.State);
            Console.WriteLine("Press Enter to close the connection.");

            ConnectionCloser(sqlConnection);
        }
        catch (SqlException sqlExceptionInvalidCredentials) when (sqlExceptionInvalidCredentials.Message.Contains("password"))
        {
            Console.WriteLine("Invalid username or password.");

        }
        catch (SqlException sqlExceptionLoginFailed) when (sqlExceptionLoginFailed.Message.Contains("A connection was successfully established with the server, but then an error occurred during the login process."))
        {
            Console.WriteLine("Unable to login.");
        }
        catch (SqlException sqlException)
        {
            Console.WriteLine(sqlException.Message);
        }

        return Task.CompletedTask;
    }

    private void DebugMode(string connectionString)
    {
        if (_debug.Debug)
        {
            Console.WriteLine("SQL DEBUG MODE ON:");
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

            Console.WriteLine($"Data Source: {builder.DataSource}");
            Console.WriteLine($"Initial Catalog: {builder.InitialCatalog}");
            Console.WriteLine($"Username: {builder.UserID}");
            Console.WriteLine($"Password: {builder.Password}");

            Console.WriteLine();
        }
    }

    private static void ConnectionCloser(SqlConnection sqlConnection)
    {
        if (sqlConnection.State == System.Data.ConnectionState.Open && Console.ReadKey().Key == ConsoleKey.Enter)
        {
            sqlConnection.Close();
            Console.WriteLine(sqlConnection.State);
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Connection is not open or you didn't press enter.");
            Console.WriteLine("Press Enter to close the connection.");
            ConnectionCloser(sqlConnection);
        }
    }

}