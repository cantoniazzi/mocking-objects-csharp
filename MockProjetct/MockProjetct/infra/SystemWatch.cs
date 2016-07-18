using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockProjetct.infra
{
    public class SystemWatch : IWatch
    {
        public DateTime Today()
        {
            return DateTime.Today;
        }
    }
}
