namespace MoTimeAPI.Helpers
{
    public class EmailModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public EmailModel(string to, string sub, string con)
        {
            To = to;
            Subject = sub;
            Content = con;
        }
    }


}
