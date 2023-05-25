using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TareasMVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Error.Requerido")]
        [EmailAddress(ErrorMessage = "Error.Email")]
        [Display(Name = "Display.Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Error.Requerido")]
        [DataType(DataType.Password)]
        [Display(Name = "Display.Password")]
        public string Password { get; set; }

        [Display(Name = "Display.Recuerdame")]
        public bool Recuerdame { get; set; }
    }
}
