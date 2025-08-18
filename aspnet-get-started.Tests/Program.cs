using System;
using System.IO;
using NUnitLite;

namespace aspnet_get_started.Tests
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // Run tests
            int exitCode = new AutoRun().Execute(args);

            // Attempt to read the NUnitLite generated XML results (default file name)
            try
            {
                string resultsPath = Path.Combine(Directory.GetCurrentDirectory(), "TestResult.xml");
                if (File.Exists(resultsPath))
                {
                    Console.WriteLine();
                    Console.WriteLine("==== BEGIN TEST RESULT XML ====");
                    Console.WriteLine(File.ReadAllText(resultsPath));
                    Console.WriteLine("==== END TEST RESULT XML ====");
                }
                else
                {
                    Console.WriteLine("(TestResult.xml not found to export)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to output test results XML: " + ex.Message);
            }

            return exitCode;
        }
    }
}
