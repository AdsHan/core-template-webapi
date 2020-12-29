using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntregaFutura.Domain.Models
{
    [Table("Observacoes")]
    public class ObservacaoModel
    {
        [Key]
        public int ObservacaoId { get; set; }

        [Required(ErrorMessage = "O texto da observação não foi informado")]
        [StringLength(65000)]
        public string Observacao { get; set; }
    }

}
