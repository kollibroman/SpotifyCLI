using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class Device
    {
        [Key]
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string Name { get; set; }
    }
}