namespace PicPaySimplificado.Application.DTOs;

public class UserDTO
{ 
    public string Fullname { get; set; }
    public string CPF { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsMerchant { get; set; }
}