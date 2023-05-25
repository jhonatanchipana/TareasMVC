using System.ComponentModel.DataAnnotations;

namespace TareasMVC.Models
{
    public class PasosCrearDTO
    {
        [Required]
        public string Descripcion { get; set; }
        public bool Realizado { get; set; }
    }
}
