using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Database.Models
{
    public class Token
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? AccessToken { get; set; } 
        public string? RefreshToken { get; set; } 
        public int ExpiresIn { get; set; } 
        public string? TokenType { get; set; } 
        public DateTime CreatedAt { get; set; } 
    }
}