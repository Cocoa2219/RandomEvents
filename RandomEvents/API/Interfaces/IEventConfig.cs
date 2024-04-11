using YamlDotNet.Serialization;

namespace RandomEvents.API.Interfaces;

public interface IEventConfig
{
    public string Name { get; }
}