namespace Test.Models
{
    public class RabbitOptions
    {
        public string Hostname { get; set; }
        public string VirtualHost { get; set; }
        public int Port { get; set; }
        public string Exchange { get; set; }
        public string UserName  { get; set; }
        public string Password  { get; set; }
    }
}