using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Database.Models
{
    public class ClientData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ClientId { get; set; } 
        public string? ClientSecret { get; set; } 
    }
}