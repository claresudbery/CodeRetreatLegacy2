using System;
using System.Collections.Generic;
using System.Linq;

namespace FerryLegacy
{
    public class FerryAvailabilityService
    {
        private readonly Ports _ports;
        private readonly Ferries _ferries;
        public readonly TimeTables _timeTables;
        public readonly PortManager _portManager;

        public FerryAvailabilityService(Ports ports, Ferries ferries, TimeTables timeTables, PortManager portManager)
        {
            _ports = ports;
            _ferries = ferries;
            _timeTables = timeTables;
            _portManager = portManager;
        }

        public Ferry NextFerryAvailableFrom(int portId, TimeSpan time)
        {
            var ports = _portManager.PortModels();
            var allEntries = _timeTables.All().SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();

            Ferry foundFerry = null;

            foreach (var entry in allEntries)
            {
                PortModel origin = ports.Single(x => x.Id == entry.OriginId);
                PortModel destination = ports.Single(x => x.Id == entry.DestinationId);

                var ferryJourney = new FerryJourney(origin, destination, entry.Time);

                var time1 = FerryModule.TimeReady(entry, destination);
                destination.AddBoat(time1, ferryJourney.Ferry);

                if (entry.OriginId == portId)
                {
                    if (entry.Time >= time)
                    {
                        foundFerry = ferryJourney.Ferry;
                        break;
                    }
                }
            }

            if (foundFerry == null)
            {
                throw new CantFindFerryException(portId, time);
            }

            return foundFerry;

            //Ferry ferry;

            //if (_timeTables.All().SelectMany(x => x.Entries).Any(x => x.OriginId == portId && x.Time >= time))
            //{
            //    var ports = _portManager.PortModels();
            //    TimeTableEntry relevantTimetable =
            //        _timeTables.All().SelectMany(x => x.Entries).First(x => x.OriginId == portId && x.Time >= time);

            //    var ferryJourney = new FerryJourney
            //    {
            //        Origin = ports.Single(x => x.Id == relevantTimetable.OriginId),
            //        Destination = ports.Single(x => x.Id == relevantTimetable.DestinationId)
            //    };

            //    BoatReady(relevantTimetable, ferryJourney.Destination, ferryJourney);

            //    ferry = ferryJourney.Ferry;
            //}
            //else
            //{
            //    throw new CantFindFerryException(portId, time);
            //}

            //return ferry;
        }
    }

    public class CantFindFerryException : Exception
    {
        public CantFindFerryException(int portId, TimeSpan time)
            : base(string.Format("Hang on, why can't I find a ferry journey from port Id {0} at {1}", portId, time))
        {
        }
    }
}
