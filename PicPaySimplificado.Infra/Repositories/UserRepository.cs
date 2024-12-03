using System.Data;
using Dapper;
using PicPaySimplificado.Domain.Entities;

namespace PicPaySimplificado.Infra.Repositories;

public class UserRepository
{
    private readonly IDbConnection _connection;

    public UserRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Guid> CreateUserAsync(User user)
    {
        var sql = @"INSERT INTO Users (id, fullname, cpf, email, passwordhash, walletbalance, ismerchant)
            VALUES (@Id, @FullName, @CPF, @Email, @PasswordHash, @WalletBalance, @IsMerchant)";
        user.Id = Guid.NewGuid();
        await _connection.ExecuteAsync(sql, user);
        return user.Id;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var sql = "SELECT * FROM Users WHERE email = @Email";

        return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email});
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        var sql = "SELECT * FROM Users WHERE id = @UserId";

        return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { UserId = userId});
    }

    public async Task UpdateWalletBalanceAsync(Guid userId, decimal newBalance)
    {
        var sql = "UPDATE Users SET walletbalance = @NewBalance WHERE id = @UserId";

        await _connection.ExecuteAsync(sql, new { UserId = userId, NewBalance = newBalance });
    }
}