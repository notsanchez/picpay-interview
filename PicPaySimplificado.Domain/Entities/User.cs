namespace PicPaySimplificado.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public decimal WalletBalance { get; set; }
        public bool IsMerchant { get; set; }
    }
}
