using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiApp.Model
{
    public class factura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int numero { get; set; }
        public DateTime fecha { get; set; }
        public decimal valor { get; set; }
        public string descripcion { get; set; }

    }
}
