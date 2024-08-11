using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Database.Models
{
    public class ClientData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string ClientId { get; set; } 
        public required string ClientSecret { get; set; } 
    }
}