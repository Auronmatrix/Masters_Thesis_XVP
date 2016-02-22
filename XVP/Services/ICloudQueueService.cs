namespace XVP.Presentation.MVC.Services
{
    using System.Threading.Tasks;

    using XVP.Domain.Commands;

    public interface ICloudQueueService
    {
        Task AddMessageAsync(ICommand command);
    }
}