using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1TestTask.DAL.Configuration
{
    public class RowContent
    {
        public decimal IncomingActive { get; set; }
        public decimal IncomingPassive { get; set; }
        public decimal TurnoverDebit { get; set; }
        public decimal TurnoverCredit { get; set; }
        public decimal OutgoingActive { get; set; }
        public decimal OutgoingPassive { get; set; }
    }
}
