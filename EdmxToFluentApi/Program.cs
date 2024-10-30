using EdmxToFluentApi.Models;
using EdmxToFluentApi.Processors;
using System;
using System.Collections.Generic;
using System.IO;

namespace EdmxToFluentApi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var edmxFilePath = string.Empty;
            var fluentMappingOutputPath = string.Empty;

            while (string.IsNullOrEmpty(edmxFilePath) || !File.Exists(edmxFilePath))
            {
                Console.Write("EDMX file path: ");
                edmxFilePath = Console.ReadLine();
            }

            while (string.IsNullOrEmpty(fluentMappingOutputPath) || !Directory.Exists(fluentMappingOutputPath))
            {
                Console.Write("Fluent Mapping output path: ");
                fluentMappingOutputPath = Console.ReadLine();
            }

            var context = new ProcessorContext()
            {
                EdmxFilePath = edmxFilePath,
            };

            var processors = new List<IProcessor>();

            foreach (var processor in processors)
            {
                processor.Process(context);
            }

            Console.WriteLine("Press some key to exit >>>");
            Console.ReadKey();
        }
    }
}
