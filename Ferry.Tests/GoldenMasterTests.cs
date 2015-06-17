using System.IO;
using FerryLegacy;
using NUnit.Framework;

namespace Ferry.Tests
{
    [TestFixture]
    public class GoldenMasterTests
    {
        //[Test]
        public void generate_golden_master()
        {
            WriteToFile("master.txt");
        }

        private static void WriteToFile(string fileName)
        {
            TextWriter file = new StreamWriter(fileName);
            System.Console.SetOut(file);
            Program.MainWithTestData();
            file.Close();
        }

        [Test]
        public void compare_to_golden_master()
        {
            WriteToFile("test-run.txt");
            var master = new StreamReader("master.txt").ReadToEnd();
            var tests = new StreamReader("test-run.txt").ReadToEnd();
            Assert.That(tests, Is.EqualTo(master));
        }
    }
}