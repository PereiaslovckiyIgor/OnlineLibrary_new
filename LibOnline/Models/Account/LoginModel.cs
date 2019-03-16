using System.ComponentModel.DataAnnotations;

namespace LibOnline.Models
{
    public class LoginModel
    {
    //    [Required(ErrorMessage = "Не указан Email")]
    //    public string Email { get; set; }

        [Required(ErrorMessage = "Не указан Login")]
        public string Ligin { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
