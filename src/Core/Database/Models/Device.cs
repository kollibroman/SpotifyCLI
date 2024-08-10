using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class Device
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string DeviceId { get; set; }
        public required string Name { get; set; }
    }
}