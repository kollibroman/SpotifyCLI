using System.ComponentModel.DataAnnotations;

namespace Core.Data
{
    public class UsrAccount
    {
        public string? UserId { get; set; } = default!;
        public string? DisplayName { get; set; } = default!;
        public string Uri { get; set; } = default!;
    }
}