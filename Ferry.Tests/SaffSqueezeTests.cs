using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FerryLegacy;
using NUnit.Framework;

namespace Ferry.Tests
{
    [TestFixture]
    class SaffSqueezeTests
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
            StringBuilder newOutput = new StringBuilder();
            StringWriter outputWriter = new StringWriter(newOutput);
            System.Console.SetOut(outputWriter);

            var timeTables = new TimeTables();
            var ferries = new Ferries();
            var bookings = new Bookings();
            var _ports = new Ports();
            var _ferryService = new FerryAvailabilityService(_ports, ferries, timeTables, new PortManager(_ports, ferries));
            var _bookingService = new JourneyBookingService(timeTables, bookings, _ferryService);
            var _timeTableService = new TimeTableService(timeTables, bookings, _ferryService);

            Console.WriteLine("Welcome to the Ferry Finding System");
            Console.WriteLine("=======");
            Console.WriteLine("Ferry Time Table");

            var ports = _ports.All();
            var timeTable = _timeTableService.GetTimeTable(ports);

            foreach (var port in ports)
            {
                Console.WriteLine();
                Console.WriteLine("Departures from " + port.Name);
                Console.WriteLine();
                Console.WriteLine(" --------------------------------------------------------------------------");
                Console.WriteLine("| {0} | {1} | {2} | {3} | {4} |",
                                  "Time".PadRight(8),
                                  "Destination".PadRight(13),
                                  "Journey Time".PadRight(13),
                                  "Ferry".PadRight(18),
                                  "Arrives".PadRight(8));
                Console.WriteLine(" --------------------------------------------------------------------------");
                var items = timeTable.Where(x => x.OriginPort == port.Name).OrderBy(x => x.StartTime);

                foreach (var item in items)
                {
                    Console.WriteLine("| {0} | {1} | {2} | {3} | {4} |",
                                      item.StartTime.PadRight(8),
                                      item.DestinationPort.PadRight(13),
                                      item.JourneyLength.PadRight(13),
                                      item.FerryName.PadRight(18),
                                      item.ArrivalTime.PadRight(8));
                }
            }

            Assert.That(newOutput.ToString(), Is.EqualTo(_expectedData));
        }
    }
}
