using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class UsrAccount
    {
        [Key] 
        public int Id { get; set; }
        public string? UserId { get; set; } = default!;
        public string? DisplayName { get; set; } = default!;
        public string Uri { get; set; } = default!;
    }
}