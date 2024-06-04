namespace cqrs_mediatr.Behaviors
{
    public interface IGuidGenerator
    {
        Guid GenerateGuid();
    }

    public class DefaultGuidGenerator : IGuidGenerator
    {
        public Guid GenerateGuid()
        {
            return Guid.NewGuid();
        }
    }
}
