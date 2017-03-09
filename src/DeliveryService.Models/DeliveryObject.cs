using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models
{
    public enum DeliveryStatus
    {
        Available,
        Expired,
        Taken
    }

    public class DeliveryObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        public DeliveryStatus? Status { get; set; }
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public TimeSpan Lifetime { get; set; }

        public int PersonId { get; set; }
    }
}
