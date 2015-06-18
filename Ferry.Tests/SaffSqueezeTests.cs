﻿using System;
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
    }
}