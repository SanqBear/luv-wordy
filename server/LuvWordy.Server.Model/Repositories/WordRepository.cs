using LuvWordy.Server.Model.Models;
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


        public List<WordItemSummary> GetWordItems(int size, int offset)
        {
            List<WordItemSummary> wordItems = new List<WordItemSummary>();

            var sizeParam = new SqlParameter("Size", DbType.Int32);
            var offsetParam = new SqlParameter("Offset", DbType.Int32);

            sizeParam.Value = size;
            offsetParam.Value = offset;

            using(DataSet ds = ExecuteDataSet("[dbo].[up_LIST_tb_Word]", new SqlParameter[] { sizeParam, offsetParam }))
            {
                if(ds?.Tables?.Count > 0 && ds.Tables[0].Rows?.Count > 0)
                {
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        wordItems.Add(new WordItemSummary(row));
                    }
                }
            }

            return wordItems;
        }

        public WordItem? GetWordItem(Guid id, Guid definitionId)
        {
            var idParam = new SqlParameter("Id", DbType.Guid);
            var defIdParam = new SqlParameter("DefinitionId", DbType.Guid);

            idParam.Value = id;
            defIdParam.Value = definitionId;

            using(DataSet ds = ExecuteDataSet("[dbo].[up_SELECT_tb_Word]", new SqlParameter[] {idParam, defIdParam }))
            {
                if(ds?.Tables?.Count > 0 && ds.Tables[0].Rows?.Count > 0)
                {
                    return new WordItem(ds.Tables[0].Rows[0]);
                }
            }

            return null;
        }


        private DataSet ExecuteDataSet(string query, SqlParameter[]? parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            if(_connection.State != ConnectionState.Open)
                _connection.Open();

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
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

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
