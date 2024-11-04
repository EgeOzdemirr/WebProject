namespace WebProject.WebUI.Settings
{
    public class ClientSettings
    {
        public Client WebProjectVisitorClient { get; set; }
        public Client WebProjectManagerClient { get; set; }
        public Client WebProjectAdminClient { get; set; }
    }

    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
