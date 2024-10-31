using System.Collections.Generic;

namespace EdmxToFluentApi.Models
{
    public class CommonEntityInfo
    {
        public List<string> PrimaryKeys { get; set; } = new List<string>();
        public string Table { get; set; }
        public string Schema { get; set; }
    }
}