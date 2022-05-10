using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvWordy.Server.Model.Repositories
{
    public class WordRepository : IAsyncDisposable
    {
        private SqlConnection _connection;

        public WordRepository(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }






        private DataSet ExecuteDataSet(string query, SqlParameter[]? parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            DataSet ds = new DataSet();

            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = commandType;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(ds);
                }
            }

            return ds;
        }

        private object ExecuteScalar(string query, SqlParameter[]? parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = commandType;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteScalar();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection?.State != System.Data.ConnectionState.Closed)
                await _connection?.CloseAsync()!;

            _connection?.DisposeAsync();
        }
    }
}
