using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Database.Models
{
    public class Device
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? DeviceId { get; set; }
        public string? Name { get; set; }
    }
}