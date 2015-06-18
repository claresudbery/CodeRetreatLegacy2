using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FerryLegacy;
using NUnit.Framework;

namespace Ferry.Tests
{
    [TestFixture]
    internal class SaffSqueezeTests
    {
        #region expectedData
        private string _expectedData = @"Welcome to the Ferry Finding System
=======
Ferry Time Table

Departures from Port Ellen

 --------------------------------------------------------------------------
| Time     | Destination   | Journey Time  | Ferry              | Arrives  |
 --------------------------------------------------------------------------
| 00:00    | Mos Eisley    | 00:30         | Titanic            | 00:30    |
| 00:10    | Tarsonis      | 00:45         | Hyperion           | 00:55    |
| 00:20    | Mos Eisley    | 00:30         | Millenium Falcon   | 00:50    |
| 00:40    | Mos Eisley    | 00:30         | Golden Hind        | 01:10    |
| 01:00    | Mos Eisley    | 00:30         | Enterprise         | 01:30    |
| 01:10    | Tarsonis      | 00:45         | Hood               | 01:55    |
| 01:20    | Mos Eisley    | 00:30         | Tempest            | 01:50    |
| 01:40    | Mos Eisley    | 00:30         | Dreadnaught        | 02:10    |

Departures from Mos Eisley

 --------------------------------------------------------------------------
| Time     | Destination   | Journey Time  | Ferry              | Arrives  |
 --------------------------------------------------------------------------
| 00:10    | Port Ellen    | 00:30         | Enterprise         | 00:40    |
| 00:30    | Port Ellen    | 00:30         | Tempest            | 01:00    |
| 00:40    | Tarsonis      | 00:35         | Black Pearl        | 01:15    |
| 00:50    | Port Ellen    | 00:30         | Titanic            | 01:20    |
| 01:10    | Port Ellen    | 00:30         | Millenium Falcon   | 01:40    |
| 01:30    | Port Ellen    | 00:30         | Golden Hind        | 02:00    |
| 01:40    | Tarsonis      | 00:35         | Defiant            | 02:15    |
| 01:50    | Port Ellen    | 00:30         | Enterprise         | 02:20    |

Departures from Tarsonis

 --------------------------------------------------------------------------
| Time     | Destination   | Journey Time  | Ferry              | Arrives  |
 --------------------------------------------------------------------------
| 00:25    | Port Ellen    | 00:45         | Dreadnaught        | 01:10    |
| 00:40    | Mos Eisley    | 00:35         | Defiant            | 01:15    |
| 01:25    | Port Ellen    | 00:45         | Hyperion           | 02:10    |
| 01:40    | Mos Eisley    | 00:35         | Black Pearl        | 02:15    |
";
        #endregion expectedData

        [Test]
        public void SaffOne()
        {
            var newOutput = new StringBuilder();
            var outputWriter = new StringWriter(newOutput);
            Console.SetOut(outputWriter);

            Program.Start();

            Assert.That(newOutput.ToString(), Is.EqualTo(_expectedData));
        }

        [Test]
        public void SaffTwo()
        {
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var bookings = new Bookings();
            var _ports = new Ports();
            var _ferryService = new FerryAvailabilityService(_ports, ferries, timeTables,
                                                             new PortManager(_ports, ferries));
            var _timeTableService = new TimeTableService(timeTables, bookings, _ferryService);

            List<Port> ports = _ports.All();
            List<TimeTableViewModelRow> timeTable = _timeTableService.GetTimeTable(ports);

            foreach (Port port in ports)
            {
                IOrderedEnumerable<TimeTableViewModelRow> items =
                    timeTable.Where(x => x.OriginPort == port.Name).OrderBy(x => x.StartTime);

                foreach (TimeTableViewModelRow item in items)
                {
                    Assert.That(item.FerryName, Is.EqualTo("Titanic"));
                }
            }
        }

        [Test]
        public void SaffThree()
        {
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var _ports = new Ports();
            var _ferryService = new FerryAvailabilityService(_ports, ferries, timeTables,
                                                             new PortManager(_ports, ferries));

            List<Port> ports = _ports.All();
            List<TimeTable> allTimetables = timeTables.All();

            List<TimeTableEntry> allEntries = allTimetables.SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();
            var rows = new List<TimeTableViewModelRow>();

            foreach (TimeTableEntry timetable in allEntries)
            {
                Port origin = ports.Single(x => x.Id == timetable.OriginId);
                Port destination = ports.Single(x => x.Id == timetable.DestinationId);
                string destinationName = destination.Name;
                string originName = origin.Name;
                FerryLegacy.Ferry ferry = _ferryService.NextFerryAvailableFrom(origin.Id, timetable.Time);
                TimeSpan arrivalTime = timetable.Time.Add(timetable.JourneyTime);
                var row = new TimeTableViewModelRow
                {
                    DestinationPort = destinationName,
                    FerryName = ferry == null ? "" : ferry.Name,
                    JourneyLength = timetable.JourneyTime.ToString("hh':'mm"),
                    OriginPort = originName,
                    StartTime = timetable.Time.ToString("hh':'mm"),
                    ArrivalTime = arrivalTime.ToString("hh':'mm"),
                };
                rows.Add(row);
            }
            List<TimeTableViewModelRow> timeTable = rows;

            foreach (Port port in ports)
            {
                IOrderedEnumerable<TimeTableViewModelRow> items =
                    timeTable.Where(x => x.OriginPort == port.Name).OrderBy(x => x.StartTime);

                foreach (TimeTableViewModelRow item in items)
                {
                    Assert.That(item.FerryName, Is.EqualTo("Titanic"));
                }
            }
        }

        [Test]
        public void SaffFour()
        {
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var _ports = new Ports();
            var _ferryService = new FerryAvailabilityService(_ports, ferries, timeTables,
                                                             new PortManager(_ports, ferries));

            List<Port> ports = _ports.All();
            List<TimeTable> allTimetables = timeTables.All();

            List<TimeTableEntry> allEntries = allTimetables.SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();
            var timeTable = new List<TimeTableViewModelRow>();

            foreach (TimeTableEntry timetable in allEntries)
            {
                Port origin = ports.Single(x => x.Id == timetable.OriginId);
                Port destination = ports.Single(x => x.Id == timetable.DestinationId);
                string destinationName = destination.Name;
                string originName = origin.Name;
                FerryLegacy.Ferry ferry = _ferryService.NextFerryAvailableFrom(origin.Id, timetable.Time);
                TimeSpan arrivalTime = timetable.Time.Add(timetable.JourneyTime);
                var row = new TimeTableViewModelRow
                {
                    DestinationPort = destinationName,
                    FerryName = ferry == null ? "" : ferry.Name,
                    JourneyLength = timetable.JourneyTime.ToString("hh':'mm"),
                    OriginPort = originName,
                    StartTime = timetable.Time.ToString("hh':'mm"),
                    ArrivalTime = arrivalTime.ToString("hh':'mm"),
                };
                timeTable.Add(row);
            }

            foreach (Port port in ports)
            {
                IOrderedEnumerable<TimeTableViewModelRow> items =
                    timeTable.Where(x => x.OriginPort == port.Name).OrderBy(x => x.StartTime);

                Assert.That(items.First().FerryName, Is.EqualTo("Titanic"));
            }
        }

        [Test]
        public void SaffFive()
        {
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var _ports = new Ports();
            var _ferryService = new FerryAvailabilityService(_ports, ferries, timeTables,
                                                             new PortManager(_ports, ferries));

            List<Port> ports = _ports.All();
            List<TimeTable> allTimetables = timeTables.All();

            List<TimeTableEntry> allEntries = allTimetables.SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();

            foreach (TimeTableEntry timetable in allEntries)
            {
                Port origin = ports.Single(x => x.Id == timetable.OriginId);
                FerryLegacy.Ferry ferry = _ferryService.NextFerryAvailableFrom(origin.Id, timetable.Time);

                Assert.That(ferry, Is.Not.Null);
            }
        }

        [Test]
        public void SaffSix()
        {
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var _ports = new Ports();
            var _portManager = new PortManager(_ports, ferries);

            List<Port> ports = _ports.All();
            List<TimeTable> allTimetables = timeTables.All();

            List<TimeTableEntry> allEntries = allTimetables.SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();

            foreach (TimeTableEntry timetable in allEntries)
            {
                Port origin = ports.Single(x => x.Id == timetable.OriginId);
                List<PortModel> ports1 = _portManager.PortModels();
                List<TimeTableEntry> allEntries1 =
                    timeTables.All().SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();
                FerryLegacy.Ferry resultingFerry = null;

                foreach (TimeTableEntry entry in allEntries1)
                {
                    FerryJourney ferry1 = new FerryJourney
                    {
                        Origin = ports1.Single(x => x.Id == entry.OriginId),
                        Destination = ports1.Single(x => x.Id == entry.DestinationId)
                    };
                    if (ferry1 != null)
                    {
                        PortModel destination = ferry1.Destination;
                        if (ferry1.Ferry == null)
                        {
                            ferry1.Ferry = ferry1.Origin.GetNextAvailable(entry.Time);
                        }

                        FerryLegacy.Ferry ferry2 = ferry1.Ferry;

                        TimeSpan time = FerryModule.TimeReady(entry, destination);
                        destination.AddBoat(time, ferry2);
                    }
                    if (entry.OriginId == origin.Id)
                    {
                        if (entry.Time >= timetable.Time)
                        {
                            if (ferry1 != null)
                            {
                                resultingFerry = ferry1.Ferry;
                                break;
                            }
                        }
                    }
                }
                FerryLegacy.Ferry ferry = resultingFerry;

                Assert.That(ferry, Is.Not.Null);
            }
        }

        [Test]
        public void SaffSeven()
        {
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var _ports = new Ports();
            var _portManager = new PortManager(_ports, ferries);


            List<PortModel> ports1 = _portManager.PortModels();
            List<TimeTableEntry> allEntries1 = timeTables.All().SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();

            TimeTableEntry entry = allEntries1.First();
            FerryJourney ferry1 = new FerryJourney
                {
                    Origin = ports1.Single(x => x.Id == entry.OriginId),
                    Destination = ports1.Single(x => x.Id == entry.DestinationId)
                };
            if (ferry1 != null)
            {
                if (ferry1.Ferry == null)
                {
                    ferry1.Ferry = ferry1.Origin.GetNextAvailable(entry.Time);
                }

                Assert.That(ferry1.Ferry, Is.Not.EqualTo(null));
            }
        }

        [Test]
        public void SaffEight()
        {
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var _ports = new Ports();
            var _portManager = new PortManager(_ports, ferries);

            List<PortModel> ports1 = _portManager.PortModels();
            List<TimeTableEntry> allEntries1 = timeTables.All().SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();

            TimeTableEntry entry = allEntries1.First();
            FerryJourney ferry1 = new FerryJourney
            {
                Origin = ports1.Single(x => x.Id == entry.OriginId),
                Destination = ports1.Single(x => x.Id == entry.DestinationId)
            };
            if (ferry1 != null)
            {
                if (ferry1.Ferry == null)
                {
                    PortModel tempQualifier = ferry1.Origin;
                    KeyValuePair<int, TimeSpan> available =
                        tempQualifier._boatAvailability.FirstOrDefault(x => entry.Time >= x.Value);
                    FerryLegacy.Ferry toReturn = null;
                    if (available.Key != 0)
                    {
                        tempQualifier._boatAvailability.Remove(available.Key);
                        toReturn = tempQualifier._boats.Single(x => x.Id == available.Key);
                        tempQualifier._boats.Remove(toReturn);
                    }
                    ferry1.Ferry = toReturn;
                }

                Assert.That(ferry1.Ferry, Is.Not.EqualTo(null));
            }
        }

        [Test]
        public void SaffNine()
        {
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var _ports = new Ports();
            var _portManager = new PortManager(_ports, ferries);

            List<PortModel> ports1 = _portManager.PortModels();
            List<TimeTableEntry> allEntries1 = timeTables.All().SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();

            TimeTableEntry entry = allEntries1.First();
            FerryJourney ferry1 = new FerryJourney
            {
                Origin = ports1.Single(x => x.Id == entry.OriginId),
                Destination = ports1.Single(x => x.Id == entry.DestinationId)
            };
            Assert.AreNotEqual(ferry1.Origin._boatAvailability.FirstOrDefault(x => entry.Time >= x.Value).Key, 0);
        }

        [Test]
        public void SaffTen()
        {
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var _ports = new Ports();
            FerryLegacy.Ferry ferry = ferries.All()[0];
            var portModelFred = new PortModel(_ports.All()[0]);

            if (ferry != null)
            {
                portModelFred._boatAvailability.Add(ferry.Id, new TimeSpan(0, 0, 0));
            }

            List<TimeTableEntry> allEntries1 = timeTables.All().SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();
            TimeTableEntry entry = allEntries1.First();

            int expected = portModelFred._boatAvailability.FirstOrDefault(x => entry.Time >= x.Value).Key;
            Assert.AreNotEqual(expected, 0);
        }
    }
}