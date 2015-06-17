using System.Collections.Generic;
using System.Linq;

namespace FerryLegacy
{
    public static class FerryManager
    {
        public static int GetFerryTurnaroundTime(PortModel destination)
        {
            if (destination.Id == 3)
                return 25;
            if (destination.Id == 2)
                return 20;
            return 15;
        }
    }
}