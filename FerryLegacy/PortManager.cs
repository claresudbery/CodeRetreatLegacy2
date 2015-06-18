using System;
using System.Collections.Generic;
using System.Linq;

namespace FerryLegacy
{
    public class PortManager
    {
        public readonly Ports _ports;
        public readonly Ferries _ferries;

        public PortManager(Ports ports, Ferries ferries)
        {
            _ports = ports;
            _ferries = ferries;
        }

        public List<PortModel> PortModels()
        {
            var ports = _ports.All().Select(x => new PortModel(x)).ToList();
            foreach (var ferry in _ferries.All())
            {
                var port = ports.Single(x => x.Id == ferry.HomePortId);
                port.AddBoat(new TimeSpan(0, 0, 0), ferry);
            }
            return ports;
        }
    }
}