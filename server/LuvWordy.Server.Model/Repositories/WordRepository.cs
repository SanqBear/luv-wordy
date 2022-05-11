using LuvWordy.Server.Model.Enums;
using LuvWordy.Server.Model.Models;
using LuvWordy.Server.Model.Utils;
using System.Data;
using System.Data.SqlClient;

namespace LuvWordy.Server.Model.Repositories
{
    public class WordRepository : IAsyncDisposable
    {
        private SqlConnection _connection;

        public const string KEY = "KorWordy";

        public WordRepository(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public (int totalCount, List<WordItemSummary> items) GetWordItems(int size, int offset)
        {
            int totalCount = -1;
            List<WordItemSummary> wordItems = new List<WordItemSummary>();

            var sizeParam = new SqlParameter("Size", DbType.Int32);
            var offsetParam = new SqlParameter("Offset", DbType.Int32);

            sizeParam.Value = size;
            offsetParam.Value = offset;

            using (DataSet ds = ExecuteDataSet("[dbo].[up_LIST_tb_Word]", new SqlParameter[] { sizeParam, offsetParam }))
            {
                if (ds?.Tables?.Count > 1)
                {
                    totalCount = int.TryParse(ds.Tables[0].Rows[0]["ItemCount"]?.ToString(), out int tc) ? tc : -1;

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        wordItems.Add(new WordItemSummary(row));
                    }
                }
            }

            return (totalCount, wordItems);
        }

        public List<WordItem> GetWordItemDetails(Guid id, Guid definitionId)
        {
            List<WordItem> wordItems = new List<WordItem>();

            var idParam = new SqlParameter("Id", DbType.Guid);
            var defIdParam = new SqlParameter("DefinitionId", DbType.Guid);

            idParam.Value = id;
            defIdParam.Value = definitionId != Guid.Empty ? definitionId : DBNull.Value;

            using (DataSet ds = ExecuteDataSet("[dbo].[up_SELECT_tb_Word]", new SqlParameter[] { idParam, defIdParam }))
            {
                if (ds?.Tables?.Count > 0 && ds.Tables[0].Rows?.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        wordItems.Add(new WordItem(row));
                    }
                }
            }

            return wordItems;
        }

        public (int totalCount, List<WordItemSummary> items) SearchWordItems(int size, int offset, string keyword
            , LexicalUnitType lexicalUnit = LexicalUnitType.NotInterested
            , PartOfSpeechType partOfSpeech = PartOfSpeechType.NotInterested
            , VocabularyLevelType vocabularyLevel = VocabularyLevelType.NotInterested)
        {
            int totalCount = -1;
            List<WordItemSummary> wordItems = new List<WordItemSummary>();

            var sizeParam = new SqlParameter("Size", DbType.Int32);
            var offsetParam = new SqlParameter("Offset", DbType.Int32);

            sizeParam.Value = size;
            offsetParam.Value = offset;

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                sizeParam,
                offsetParam,
                
            };
            parameters.AddRange(BuildSearchParams(keyword, lexicalUnit, partOfSpeech, vocabularyLevel));

            using (DataSet ds = ExecuteDataSet("[dbo].[up_SEARCH_tb_Word]", parameters.ToArray()))
            {
                if (ds?.Tables?.Count > 1)
                {
                    totalCount = int.TryParse(ds.Tables[0].Rows[0]["ItemCount"]?.ToString(), out int tc) ? tc : -1;

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        wordItems.Add(new WordItemSummary(row));
                    }
                }
            }

            return (totalCount, wordItems);
        }

        private SqlParameter[] BuildSearchParams(string keyword, LexicalUnitType lexicalUnit, PartOfSpeechType partOfSpeech, VocabularyLevelType vocabularyLevel)
        {
            var keywordParam = new SqlParameter("Keyword", DbType.String);
            var lexicalUnitParam = new SqlParameter("LexicalUnit", DbType.String);
            var partofSpeechParam = new SqlParameter("PartOfSpeech", DbType.String);
            var vocabularyLevelParam = new SqlParameter("VocabularyLevel", DbType.String);

            keywordParam.Value = string.IsNullOrWhiteSpace(keyword) ? DBNull.Value : keyword;
            lexicalUnitParam.Value = lexicalUnit == LexicalUnitType.NotInterested ? DBNull.Value : LexicalUnit.ToString(lexicalUnit);
            partofSpeechParam.Value = partOfSpeech == PartOfSpeechType.NotInterested ? DBNull.Value : PartOfSpeech.ToString(partOfSpeech);
            vocabularyLevelParam.Value = vocabularyLevel == VocabularyLevelType.NotInterested ? DBNull.Value : VocabularyLevel.ToString(vocabularyLevel);

            return new SqlParameter[] { keywordParam, lexicalUnitParam, partofSpeechParam, vocabularyLevelParam };
        }

        private DataSet ExecuteDataSet(string query, SqlParameter[]? parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            if (_connection.State != ConnectionState.Open)
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