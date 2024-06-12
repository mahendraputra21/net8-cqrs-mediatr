namespace DewaEShop.Contract
{
    public class SendEmailRequestDto
    {
        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? TemplateId { get; set; }
    }
}
