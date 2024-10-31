using System.Data.Entity.Core.Metadata.Edm;

namespace EdmxToFluentApi.Models
{
    public class RelationshipEntityDescription
    {
        public string EntityName { get; set; }
        public string NavigationPropertyName { get; set; }
        public RelationshipMultiplicity RelationshipType { get; set; }
        public string JoinKeyName { get; set; }
    }
}