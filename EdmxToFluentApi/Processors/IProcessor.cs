using EdmxToFluentApi.Models;

namespace EdmxToFluentApi.Processors
{
    public interface IProcessor
    {
        void Process(ProcessorContext context);
    }
}
