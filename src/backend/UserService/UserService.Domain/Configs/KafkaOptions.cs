namespace UserService.Domain.Configs;

public class KafkaOptions
{
    public string BootstrapServers { get; set; } = string.Empty;
    public KafkaTopics Topics { get; set; } = new KafkaTopics();
}

public class KafkaTopics
{
    public string EmailConfirmation { get; set; } = string.Empty;
    public string ConfirmEmail { get; set; } = string.Empty;
    public string ResetPass { get; set;} = string.Empty;
}