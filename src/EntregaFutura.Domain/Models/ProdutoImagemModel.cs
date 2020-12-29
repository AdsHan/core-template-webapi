using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntregaFutura.Domain.Models
{
    [Table("ProdutosImagem")]
    public class ProdutoImagemModel
    {
        [Key]
        [Column(Order = 1)]
        public int ProdutoId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ImagemId { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual ProdutoModel Produto { get; set; }

        [ForeignKey("ImagemId")]
        public virtual ImagemModel Imagem { get; set; }

    }

}
