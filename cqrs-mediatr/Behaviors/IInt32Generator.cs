namespace cqrs_mediatr.Behaviors
{
    public interface IInt32Generator
    {
        int GenerateInt32();
    }

    public class Int32Generator : IInt32Generator
    {
        public int GenerateInt32()
        {
            return 50;
        }
    }
}
