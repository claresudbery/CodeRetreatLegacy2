using System;
using System.Collections.Generic;
using System.Linq;

namespace FerryLegacy
{
    public class PortModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public readonly Dictionary<int, TimeSpan> _boatAvailability = new Dictionary<int, TimeSpan>();
        public readonly List<Ferry> _boats = new List<Ferry>();

        public PortModel(Port port)
        {
            Id = port.Id;
            Name = port.Name;
        }

        public void AddBoat(TimeSpan available, Ferry boat)
        {
            if (boat != null)
            {
                _boats.Add(boat);
                _boatAvailability.Add(boat.Id, available);
            }
        }

        public Ferry GetNextAvailable(TimeSpan time)
        {
            var available = _boatAvailability.FirstOrDefault(x => time >= x.Value);
            Ferry toReturn = null;
            if (available.Key != 0)
            {

                _boatAvailability.Remove(available.Key);
                 toReturn = _boats.Single(x => x.Id == available.Key);
                _boats.Remove(toReturn);
            }
            return toReturn;
        }
    }
}