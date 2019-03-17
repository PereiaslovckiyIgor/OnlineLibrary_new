using LibOnline.Models.Account;
using System.ComponentModel.DataAnnotations;

namespace LibOnline.Models
{
    public class LoginModel: AccountMD5
    {
        [Required(ErrorMessage = "Не указан Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}