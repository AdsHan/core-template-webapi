using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntregaFutura.Domain.Models
{
    [Table("Imagens")]
    public class ImagemModel
    {
        [Key]
        public int ImagemId { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string ImagemUrl { get; set; }

    }

}
