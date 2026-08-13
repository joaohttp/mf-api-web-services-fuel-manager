using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mf_api_web_services_fuel_manager.Models
{
    [Table("consumo")]
    public class Consumo
    {
        [Key]
        public int Id { get; set;  }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }
        [Required]
        public TipoCombustivel Tipo { get; set; }
        [Required]
        public int veiculoId { get; set; }
        public Veiculo veiculo { get; set; }

    }

    public enum TipoCombustivel
    {
        Diessel,
        Etanol,
        Gasolina
    }
}
