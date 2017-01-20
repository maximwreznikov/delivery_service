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
        Expired
    }

    public class DeliveryObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int DeliveryObjectId { get; set; }
        public DeliveryStatus? Status { get; set; }
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ModificationTime { get; set; }

        public int PersonId { get; set; }
    }
}
