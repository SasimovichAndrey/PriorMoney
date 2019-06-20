using System.Threading.Tasks;

namespace PriorMoney.ConsoleApp.UserInterface
{
    public interface IUserInterfaceCommand
    {
        int MenuLevel { get; set; }
        Task ExecuteAsync();
    }

    public interface IUserInterfaceCommand<T>
    {
        Task<T> ExecuteAsync();
    }
}