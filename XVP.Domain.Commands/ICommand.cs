namespace XVP.Domain.Commands
{
    using Newtonsoft.Json;

    public interface ICommand
    {
        [JsonProperty]
        string Tenant { get; }

        void Execute();
    }
}
