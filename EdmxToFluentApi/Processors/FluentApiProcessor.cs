using EdmxToFluentApi.Models;
using System.IO;

namespace EdmxToFluentApi.Processors
{
    internal class FluentApiProcessor : IProcessor
    {
        private readonly FluentApiBuilder _builder = new();
        public void Process(ProcessorContext context)
        {
            if (!Directory.Exists(context.OutputDirectory))
                Directory.CreateDirectory(context.OutputDirectory);
            else
            {
                var files = Directory.GetFiles(context.OutputDirectory, "*.cs");

                foreach (var file in files)
                    File.Delete(file);
            }

            GenerateFluentApiFiles(context.EdmxParseResult, context);
        }
        private void GenerateFluentApiFiles(EdmxParseResult parseResult, ProcessorContext context)
        {
            // Get the name of the entity.
            // Retrieve common information about the entity.
            // Initialize Fluent API configuration for the entity.
            // Add properties to the entity configuration.
            // Add relationships to the entity configuration.
            // End the entity configuration and retrieve the generated file text.
            // Write the generated Fluent API configuration to a file.

            foreach (var entitySetMapping in parseResult.EntitySetMappings)
            {
                var entityName = entitySetMapping.EntitySet.ElementType.Name;
                var commonInfo = parseResult.CommonEntityInfos[entityName];

                _builder.AddDefaultUsings()
                    .AddNamespace(context.NamespaceName)
                    .StartEntityConfiguration(entityName)
                    .ToTable(commonInfo.Table, commonInfo.Schema)
                    .HasKey(commonInfo.PrimaryKeys.ToArray());

                if (parseResult.TableColumnsDescriptions.TryGetValue(entityName, out var descriptions))
                {
                    foreach (var description in descriptions)
                        _builder.AddProperty(description);
                }


                if (parseResult.RelationshipDescriptions.TryGetValue(entityName, out var relationships))
                {
                    _builder.AddEmptyLine();

                    foreach (var relationship in relationships)
                        _builder.AddRelationship(relationship, commonInfo.PrimaryKeys);
                }

                _builder.EndEntityConfiguration();
                var generatedFileText = _builder.ToString();
                _builder.Clear();

                WriteGeneratedFile(entityName, context.OutputDirectory, generatedFileText);
            }
        }
        private void WriteGeneratedFile(string entityName, string outputDirectory, string generatedFileText)
        {
            var filename = $"{entityName}Configuration.cs";
            var path = Path.Combine(outputDirectory, filename);
            File.WriteAllText(path, generatedFileText);
        }
    }
}
