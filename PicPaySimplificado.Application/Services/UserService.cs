using System.Security.Cryptography;
using System.Text;
using PicPaySimplificado.Domain.Entities;
using PicPaySimplificado.Application.Interfaces;
using PicPaySimplificado.Infra.Repositories;
using PicPaySimplificado.Application.DTOs;


namespace PicPaySimplificado.Application.Services;
public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly TransactionRepository _transactionRepository;

    public UserService(UserRepository userRepository, TransactionRepository transactionRepository)
    {
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<Guid> RegisterUser(UserDTO userDto)
    {
        var user = new User
        {
            Fullname = userDto.Fullname,
            CPF = userDto.CPF,
            Email = userDto.Email,
            PasswordHash = HashPassword(userDto.Password),
            WalletBalance = 0,
            IsMerchant = userDto.IsMerchant
        };

        return await _userRepository.CreateUserAsync(user);
    }

    public async Task<User> Authenticate(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Credenciais Inválidas.");
        };
        return user;
    }

    public async Task<decimal> GetWalletBalance(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
       
        return user?.WalletBalance ?? throw new Exception("Usuario não encontrado.");
    }

    public async Task Transfer(Guid payerId, Guid payeeId, decimal amount)
    {
        var payer = await _userRepository.GetByIdAsync(payerId);
        var payee = await _userRepository.GetByIdAsync(payeeId);

        if(payer == null || payee == null)
        {
            throw new Exception("Pagador ou Beneficiário não encontrado.");
        }

        if (payer.IsMerchant)
        {
            throw new Exception("Comerciantes não podem realizar transferencias.");
        }

        if(payer.WalletBalance < amount)
        {
            throw new Exception("Saldo insuficiente.");
        }

        payer.WalletBalance -= amount;
        payee.WalletBalance += amount;

        await _userRepository.UpdateWalletBalanceAsync(payerId, payer.WalletBalance);
        await _userRepository.UpdateWalletBalanceAsync(payeeId, payee.WalletBalance);

        await _transactionRepository.CreateTransactionAsync(new Transaction
        {
            PayerId = payerId,
            PayeeId = payeeId,
            Amount = amount
        });
    }

    private static string HashPassword(string password)
    {

        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

        return Convert.ToBase64String(bytes);
    }

    private static bool VerifyPassword(string password, string passwordHashed)
    {
        return HashPassword(password) == passwordHashed;
    }
}