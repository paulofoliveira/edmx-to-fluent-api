using EdmxToFluentApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EdmxToFluentApi.Processors
{
    internal class FluentApiProcessor : IProcessor
    {
        private readonly FluentApiBuilder _builder = new();
        private IEnumerable<Type> _entitiesFromAssembly = [];
        public void Process(ProcessorContext context)
        {
            if (!File.Exists(context.AssemblyPath))
                throw new ArgumentException("Assembly specified file is not exist");

            if (!Directory.Exists(context.OutputDirectory))
                Directory.CreateDirectory(context.OutputDirectory);
            else
            {
                var files = Directory.GetFiles(context.OutputDirectory, "*.cs");

                foreach (var file in files)
                    File.Delete(file);
            }

            _entitiesFromAssembly = GetEntitiesFromAssembly(context);

            GenerateFluentApiFiles(context.EdmxParseResult, context);
        }

        private IEnumerable<Type> GetEntitiesFromAssembly(ProcessorContext context)
        {
            var assembly = !string.IsNullOrEmpty(context.AssemblyPath) ? Assembly.LoadFrom(context.AssemblyPath) : null;
            var entityNames = context.EdmxParseResult.EntitySetMappings.Select(x => x.EntitySet.ElementType.Name);

            return assembly != null ? assembly.GetTypes()
                      .Where(t => t.IsClass && entityNames.Contains(t.Name))
                      .ToList() : [];
        }
        private IEnumerable<PropertyInfo> GetReadOnlyProperties(string entityName)
        {
            if (entityName == null)
                throw new ArgumentNullException(nameof(entityName));

            var entityType = _entitiesFromAssembly.FirstOrDefault(x => x.Name == entityName);
            return entityType != null ? entityType.GetProperties()
                    .Where(p => p.CanRead && !p.CanWrite)
                    .ToList() : [];
        }
        private void GenerateFluentApiFiles(EdmxParseResult parseResult, ProcessorContext context)
        {
            // Get entities from assembly file to compare entities from edmx and ignore possible properties.
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

                var configurationClassName = $"{entityName}Configuration";

                _builder.AddDefaultUsings()
                    .AddNamespace(context.NamespaceName)
                    .StartEntityConfiguration(configurationClassName, entityName)
                    .ToTable(commonInfo.Table, commonInfo.Schema)
                    .HasKey(commonInfo.PrimaryKeys.ToArray());

                if (parseResult.TableColumnsDescriptions.TryGetValue(entityName, out var descriptions))
                {
                    foreach (var description in descriptions)
                        _builder.AddProperty(description);
                }

                // Specifying Not to Map a CLR Property to a Column in the Database: https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties#specifying-not-to-map-a-clr-property-to-a-column-in-the-database

                var readOnlyProperties = GetReadOnlyProperties(entityName);

                if (readOnlyProperties.Any())
                {
                    _builder.AddEmptyLine();

                    foreach (var readOnlyProperty in readOnlyProperties)
                    {
                        _builder.AddIgnoreProperty(readOnlyProperty.Name);
                    }
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

                WriteGeneratedFile(configurationClassName, context.OutputDirectory, generatedFileText);
            }
        }
        private void WriteGeneratedFile(string configurationClassName, string outputDirectory, string generatedFileText)
        {
            var filename = $"{configurationClassName}.cs";
            var path = Path.Combine(outputDirectory, filename);
            File.WriteAllText(path, generatedFileText);
        }
    }
}
