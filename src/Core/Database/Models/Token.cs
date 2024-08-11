using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Database.Models
{
    public class Token
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string AccessToken { get; set; } 
        public required string RefreshToken { get; set; } 
        public required int ExpiresIn { get; set; } 
        public required string TokenType { get; set; } 
        public DateTime CreatedAt { get; set; } 
    }
}