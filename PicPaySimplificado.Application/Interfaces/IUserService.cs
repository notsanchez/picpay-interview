using PicPaySimplificado.Application.DTOs;
using PicPaySimplificado.Domain.Entities;

namespace PicPaySimplificado.Application.Interfaces
{
    public interface IUserService
    {
        Task<Guid> RegisterUser(UserDTO id);
        Task<User> Authenticate(string email, string password);
        Task<decimal> GetWalletBalance(Guid userId);
        Task Transfer(Guid payerId, Guid payeeId, decimal amount);
    }
}