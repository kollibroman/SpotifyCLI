using System.ComponentModel.DataAnnotations;

namespace SpotifyCli.Db.Entities
{
    public class Device
    {
        [Key]
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string Name { get; set; }
    }
}