using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ape.Entities
{
    [Table("Usuario", Schema = "acesso")]
    public class Usuario
    {
        [Key]
        [Column("IdUsuario")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Column("Login")]
        public string Login { get; set; }

        [Column("Senha")]
        public string Senha { get; set; }
    }
}