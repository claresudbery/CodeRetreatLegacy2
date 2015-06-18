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
            var newOutput = new StringBuilder();
            var outputWriter = new StringWriter(newOutput);
            Console.SetOut(outputWriter);

            // Program.Start STARTS

            // Program.WireUp STARTS
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var bookings = new Bookings();
            var _ports = new Ports();
            var _ferryService = new FerryAvailabilityService(_ports, ferries, timeTables, new PortManager(_ports, ferries));
            var _timeTableService = new TimeTableService(timeTables, bookings, _ferryService);
            // Program.WireUp ENDS

            var allPorts = _ports.All();
            var timeTable = _timeTableService.GetTimeTable(allPorts);

            // Program.DisplayTimetable STARTS
            foreach (var port in allPorts)
            {
                var items = timeTable.Where(x => x.OriginPort == port.Name).OrderBy(x => x.StartTime);

                foreach (var item in items)
                {
                    Console.WriteLine("| {0} | {1} | {2} | {3} | {4} |",
                                      item.StartTime.PadRight(8),
                                      item.DestinationPort.PadRight(13),
                                      item.JourneyLength.PadRight(13),
                                      item.FerryName.PadRight(18),
                                      item.ArrivalTime.PadRight(8));

                    Assert.That(item.FerryName, Is.Not.Empty);
                }
            }
            // Program.DisplayTimetable ENDS

            // Program.Start ENDS
        }

        [Test]
        public void SaffThree()
        {
            var newOutput = new StringBuilder();
            var outputWriter = new StringWriter(newOutput);
            Console.SetOut(outputWriter);

            // Program.WireUp STARTS
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var bookings = new Bookings();
            var ports = new Ports();
            var ferryService = new FerryAvailabilityService(ports, ferries, timeTables, new PortManager(ports, ferries));
            var timeTableService = new TimeTableService(timeTables, bookings, ferryService);
            // Program.WireUp ENDS

            var allPorts = ports.All();

            // _timeTableService.GetTimeTable STARTS
            var timetables = timeTableService._timeTables.All();

            var allEntries = timetables.SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();

            foreach (var timetable in allEntries)
            {
                var origin = allPorts.Single(x => x.Id == timetable.OriginId);
                var ferry = timeTableService._ferryService.NextFerryAvailableFrom(origin.Id, timetable.Time);
                Assert.That(ferry, Is.Not.Null);
            }
            // _timeTableService.GetTimeTable ENDS
        }

        [Test]
        public void SaffFour()
        {
            var newOutput = new StringBuilder();
            var outputWriter = new StringWriter(newOutput);
            Console.SetOut(outputWriter);

            // Program.WireUp STARTS
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var bookings = new Bookings();
            var ports = new Ports();
            var ferryService = new FerryAvailabilityService(ports, ferries, timeTables, new PortManager(ports, ferries));
            var timeTableService = new TimeTableService(timeTables, bookings, ferryService);
            // Program.WireUp ENDS

            // timeTableService._ferryService.NextFerryAvailableFrom STARTS
            var ports1 = timeTableService._ferryService._portManager.PortModels();
            var allEntries1 = timeTableService._ferryService._timeTables.All().SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();

            foreach (var entry in allEntries1)
            {
                PortModel origin1 = ports1.Single(x => x.Id == entry.OriginId);
                PortModel destination = ports1.Single(x => x.Id == entry.DestinationId);
                var ferryJourney = new FerryJourney(origin1, destination, entry.Time);

                Assert.That(ferryJourney.Ferry, Is.Not.Null);
            }
            // timeTableService._ferryService.NextFerryAvailableFrom ENDS
        }

        [Test]
        public void SaffFive()
        {
            var newOutput = new StringBuilder();
            var outputWriter = new StringWriter(newOutput);
            Console.SetOut(outputWriter);

            // Program.WireUp STARTS
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var bookings = new Bookings();
            var ports = new Ports();
            var ferryService = new FerryAvailabilityService(ports, ferries, timeTables, new PortManager(ports, ferries));
            var timeTableService = new TimeTableService(timeTables, bookings, ferryService);
            // Program.WireUp ENDS

            // timeTableService._ferryService.NextFerryAvailableFrom STARTS
            var ports1 = timeTableService._ferryService._portManager.PortModels();
            var allEntries1 = timeTableService._ferryService._timeTables.All().SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();

            foreach (var entry in allEntries1)
            {
                PortModel origin = ports1.Single(x => x.Id == entry.OriginId);

                // FerryJourney constructor STARTS

                // origin.GetNextAvailable STARTS
                var available = origin._boatAvailability.FirstOrDefault(x => entry.Time >= x.Value);
                Assert.That(available.Key, Is.Not.EqualTo(0));
                // origin.GetNextAvailable ENDS

                // FerryJourney constructor ENDS
            }
            // timeTableService._ferryService.NextFerryAvailableFrom ENDS
        }

        [Test]
        public void SaffSix()
        {
            var newOutput = new StringBuilder();
            var outputWriter = new StringWriter(newOutput);
            Console.SetOut(outputWriter);

            // Program.WireUp STARTS
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var bookings = new Bookings();
            var ports = new Ports();
            var ferryService = new FerryAvailabilityService(ports, ferries, timeTables, new PortManager(ports, ferries));
            var timeTableService = new TimeTableService(timeTables, bookings, ferryService);
            // Program.WireUp ENDS

            // timeTableService._ferryService.NextFerryAvailableFrom STARTS
            var ports1 = timeTableService._ferryService._portManager.PortModels();
            var allEntries1 = timeTableService._ferryService._timeTables.All().SelectMany(x => x.Entries).OrderBy(x => x.Time).ToList();

            foreach (var entry in allEntries1)
            {
                PortModel origin = ports1.Single(x => x.Id == entry.OriginId);
                Assert.That(origin._boatAvailability.Any(x => entry.Time >= x.Value), Is.EqualTo(true));
            }
            // timeTableService._ferryService.NextFerryAvailableFrom ENDS
        }

        [Test]
        public void SaffSeven()
        {
            var newOutput = new StringBuilder();
            var outputWriter = new StringWriter(newOutput);
            Console.SetOut(outputWriter);

            // Program.WireUp STARTS
            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var bookings = new Bookings();
            var ports = new Ports();
            var ferryService = new FerryAvailabilityService(ports, ferries, timeTables, new PortManager(ports, ferries));
            var timeTableService = new TimeTableService(timeTables, bookings, ferryService);
            // Program.WireUp ENDS

            // timeTableService._ferryService.NextFerryAvailableFrom STARTS

            var ports1 = timeTableService._ferryService._portManager.PortModels();
            IEnumerable<KeyValuePair<int, TimeSpan>> allBoatAvailabilities = ports1.SelectMany(x => x._boatAvailability);
            Assert.That(allBoatAvailabilities.Any(x => x.Value.Seconds != 0), Is.EqualTo(false));

            // timeTableService._ferryService.NextFerryAvailableFrom ENDS
        }
    }
}