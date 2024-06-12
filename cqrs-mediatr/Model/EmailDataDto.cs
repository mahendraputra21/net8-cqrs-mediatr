namespace cqrs_mediatr.Model
{
    public class EmailDataDto
    {
        public Dictionary<string, string> EmailData { get; set; }

        public EmailDataDto()
        {
            EmailData = [];
        }
    }
}
