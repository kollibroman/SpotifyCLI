using System.ComponentModel.DataAnnotations;

namespace SpotifyCli.Db.Entities
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