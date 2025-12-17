using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace SIG_T.API.Data;

public static class ApplicationDbContextExtensions
{
    /// <summary>
    /// Executes a stored procedure non-query using the DbContext's connection and returns the number of affected rows.
    /// Output parameters can be read from the provided parameters after execution.
    /// </summary>
    public static async Task<int> ExecuteStoredProcNonQueryAsync(this DbContext context, string storedProc, params DbParameter[] parameters)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (string.IsNullOrWhiteSpace(storedProc)) throw new ArgumentNullException(nameof(storedProc));

        var conn = context.Database.GetDbConnection();
        try
        {
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = storedProc;
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null && parameters.Length > 0)
                foreach (var p in parameters) cmd.Parameters.Add(p);

            var result = await cmd.ExecuteNonQueryAsync();
            return result;
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                await conn.CloseAsync();
        }
    }

    /// <summary>
    /// Executes a stored procedure that returns rows and maps each row using the provided mapper function.
    /// </summary>
    public static async Task<List<T>> QueryStoredProcAsync<T>(this DbContext context, string storedProc, Func<DbDataReader, T> map, params DbParameter[] parameters)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (string.IsNullOrWhiteSpace(storedProc)) throw new ArgumentNullException(nameof(storedProc));
        if (map == null) throw new ArgumentNullException(nameof(map));

        var list = new List<T>();
        var conn = context.Database.GetDbConnection();

        try
        {
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = storedProc;
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null && parameters.Length > 0)
                foreach (var p in parameters) cmd.Parameters.Add(p);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(map(reader));
            }

            return list;
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                await conn.CloseAsync();
        }
    }
}