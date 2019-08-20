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

    public interface IParameterizableUserInterfaceCommand<T>
    {
        Task ExecuteAsync(T parameter);
    }

    public interface IParameterizableUserInterfaceCommand<TIn, TOut>
    {
        Task<TOut> ExecuteAsync(TIn parameter);
    }
}