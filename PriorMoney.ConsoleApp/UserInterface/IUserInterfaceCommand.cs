using System.Threading.Tasks;

namespace PriorMoney.ConsoleApp.UserInterface
{
    public interface IUserInterfaceCommand
    {
        Task ExecuteAsync();
    }

    public interface IUserInterfaceCommand<T>
    {
        Task<T> ExecuteAsync();
    }
}