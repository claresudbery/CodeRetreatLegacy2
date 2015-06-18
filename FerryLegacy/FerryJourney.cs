using System;

namespace FerryLegacy
{
    public class FerryJourney
    {
        public FerryJourney(PortModel origin, PortModel destination, TimeSpan time)
        {
            Ferry = origin.GetNextAvailable(time);
            Origin = origin;
            Destination = destination;
        }

        public Ferry Ferry { get; set; }
        public PortModel Origin { get; private set; }
        public PortModel Destination { get; private set; }
    }
}