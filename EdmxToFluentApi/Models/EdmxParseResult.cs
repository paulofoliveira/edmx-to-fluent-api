using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;

namespace EdmxToFluentApi.Models
{
    public class EdmxParseResult
    {
        public List<EntitySetMapping> EntitySetMappings { get; set; }
        public Dictionary<string, List<RelationshipDescription>> RelationshipDescriptions { get; set; }
        public Dictionary<string, List<TableColumnDescription>> TableColumnsDescriptions { get; set; }
        public Dictionary<string, CommonEntityInfo> CommonEntityInfos { get; set; }
    }
}