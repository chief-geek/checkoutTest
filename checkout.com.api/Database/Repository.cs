using checkout.com.api.Data;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Transactions;

namespace checkout.com.api.Database
{
    public class Repository : IRepository
    {
        private readonly ILogger<Repository> _logger;
        private IOptions<DatabaseConnection> _config;

        public Repository(ILogger<Repository> logger, IOptions<DatabaseConnection> config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<TransactionResult> GetTransaction(string transactionId)
        {
            using (var connection = new SqlConnection(_config.Value.ConnectionString))
            {
                connection.Open();
                var parameters = new { TransactionId = transactionId };
                var command = "exec sp_executesql N'SELECT c.CardHolder, c.CardNumber, c.ExpiryMonth, c.ExpiryYear, t.Amount, t.TransactionId FROM Transactions t";
                command += " INNER JOIN Cards c ON t.CardId = c.CardId";
                command += $" WHERE TransactionId = @TransactionId', N'@TransactionId NVARCHAR(100)', {transactionId}";
                
                var result = await connection.QueryAsync<TransactionResult>(command, parameters);
                return result.FirstOrDefault();
            }
        }

        public async Task<string> SavePurchase(Purchase purchase)
        {
            try
            {
                using (var connection = new SqlConnection(_config.Value.ConnectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var maskedCard = new string('*', purchase.CreditCard.CardNumber.Length - 4) + purchase.CreditCard.CardNumber.Substring(purchase.CreditCard.CardNumber.Length - 4, 4);

                        var cardParameters = new { CardNumber = purchase.CreditCard.CardNumber, CardHolder = purchase.CreditCard.CardHolder, ExpiryMonth = purchase.CreditCard.Month, ExpiryYear = purchase.CreditCard.Year };
                        var command = $"IF NOT EXISTS(SELECT CardNumber FROM Cards WHERE CardNumber = '{maskedCard}')";
                        command += " BEGIN";
                        command += "  EXEC sp_executesql N'INSERT INTO Cards (CardNumber, CardHolder, ExpiryMonth, ExpiryYear) OUTPUT INSERTED.Id VALUES (@CardNumber, @CardHolder, @ExpiryMonth, @ExpiryYear)'";
                        command += " ,N'@CardNumber NVARCHAR(100), @CardHolder NVARCHAR(100), @ExpiryMonth NVARCHAR(2), @ExpiryYear NVARCHAR(2)'";
                        command += $" ,@CardNumber = '{maskedCard}', @CardHolder = '{purchase.CreditCard.CardHolder}', @ExpiryMonth = '{purchase.CreditCard.Month}', @ExpiryYear = '{purchase.CreditCard.Year}'";
                        command += " END";
                        command += " ELSE";
                        command += " BEGIN";
                        command += "  exec sp_executesql N'SELECT CardId FROM Card WHERE CardNumber = @CardNumber)'";
                        command += " END";
                        var cardId = await connection.ExecuteScalarAsync<string>(command, cardParameters, transaction);
                        var hiddenParameter = new { CardId = cardId };
                        command = $"exec sp_executesql N'SELECT CardId FROM Cards WHERE Id = @CardId', N'@CardId NVARCHAR(100)', @CardId = '{cardId}'";
                        var cardHiddenId = connection.ExecuteScalar<string>(command, hiddenParameter, transaction);
                        
                        var transactionParameters = new { CardId = cardId, Amount = purchase.Amount, TransactionDate = DateTime.UtcNow, Result = 1 };
                        command = "exec sp_executesql N'INSERT INTO [Transactions] (CardId, Amount, TransactionDate, Result) OUTPUT INSERTED.Id VALUES (@CardId, @Amount, @TransactionDate, @Result)'";
                        command += " ,N'@CardId NVARCHAR(100), @Amount DECIMAL(18,2), @TransactionDate SMALLDATETIME, @Result BIT'";
                        command += $" ,@CardId = '{cardHiddenId}', @Amount = {purchase.Amount}, @TransactionDate = '{DateTime.Now.ToString("yyyy-MM-dd")}', @Result = 1";
                        var transactionId = connection.ExecuteScalar<string>(command, transactionParameters, transaction);
                        var hiddenTransactionParameter = new { Id = transactionId };
                        var hiddenTransactionId = connection.ExecuteScalar<string>($"exec sp_executesql N'SELECT TransactionId FROM [Transactions] WHERE Id = @Id', N'@Id INT', @Id={transactionId}", hiddenTransactionParameter, transaction);
                        transaction.Commit();
                        return hiddenTransactionId;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return string.Empty;
            }
        }
    }
}
