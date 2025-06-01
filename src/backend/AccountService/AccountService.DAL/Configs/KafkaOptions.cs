namespace AccountService.Domain.Configs;

public class KafkaOptions
{
    public string BootstrapServers { get; set; } = string.Empty;
    public KafkaTopics Topics { get; set; } = new KafkaTopics();
}

public class KafkaTopics
{
    public string SendNotification { get; set; } = string.Empty;

}