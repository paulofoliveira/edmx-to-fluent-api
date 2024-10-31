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
            var outputDirectory = string.Empty;

            while (string.IsNullOrEmpty(edmxFilePath) || !File.Exists(edmxFilePath))
            {
                Console.Write("EDMX File path: ");
                edmxFilePath = Console.ReadLine();
            }

            while (string.IsNullOrEmpty(outputDirectory) || !Directory.Exists(outputDirectory))
            {
                Console.Write("Output Directory path: ");
                outputDirectory = Console.ReadLine();
            }

            Console.Write("Namespace Name: ");
            var namespaceName = Console.ReadLine();

            var context = new ProcessorContext()
            {
                EdmxFilePath = edmxFilePath,
                OutputDirectory = outputDirectory,
                NamespaceName = namespaceName
            };

            var processors = new List<IProcessor>() { new EdmxFileProcessor(), new FluentApiProcessor() };

            foreach (var processor in processors)
            {
                processor.Process(context);
            }

            Console.WriteLine("Press some key to exit >>>");
            Console.ReadKey();
        }
    }
}
