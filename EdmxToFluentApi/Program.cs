using System;
using System.IO;

namespace EdmxToFluentApi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var edmxInputPath = string.Empty;
            var fluentMappingOutputPath = string.Empty;

            while (string.IsNullOrEmpty(edmxInputPath) || !File.Exists(edmxInputPath))
            {
                Console.Write("EDMX input path: ");
                edmxInputPath = Console.ReadLine();
            }

            while (string.IsNullOrEmpty(fluentMappingOutputPath) || !Directory.Exists(fluentMappingOutputPath))
            {
                Console.Write("Fluent Mapping output path: ");
                fluentMappingOutputPath = Console.ReadLine();
            }

            Console.WriteLine("Press some key to exit >>>");
            Console.ReadKey();
        }
    }
}
