using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Core
{
    public class MachineClockDateTime : IDateTime
    {
        public DateTime Now { get { return DateTime.Now; } }
    }
}
