using System.ComponentModel.DataAnnotations;

namespace TareasMVC.Models
{
    public class TareaEditarDTO
    {
        [Required]
        [StringLength(25)]
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
    }
}
