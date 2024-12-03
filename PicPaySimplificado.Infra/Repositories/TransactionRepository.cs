using System.Data;
using Dapper;
using PicPaySimplificado.Domain.Entities;

namespace PicPaySimplificado.Infra.Repositories;

public class TransactionRepository
{
    private readonly IDbConnection _connection;

    public TransactionRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task CreateTransactionAsync(Transaction transaction)
    {
        var sql = @"INSERT INTO Transactions (id, payerId, payeeId, amount, createdAt)
                    VALUES (@Id, @PayerId, @PayeeId, @Amount, @CreatedAt)";

        transaction.Id = Guid.NewGuid();
        transaction.CreatedAt = DateTime.UtcNow;

        await _connection.ExecuteAsync(sql, transaction); 
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(Guid userId)
    {
        var sql = @"SELECT * FROM Transactions 
                    WHERE payerId = @UserId OR payeeId = @UserId";

        return await _connection.QueryAsync<Transaction>(sql, new { UserId = userId});
    }
}