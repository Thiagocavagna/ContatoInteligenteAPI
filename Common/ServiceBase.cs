using ContatoInteligenteAPI.Models;

namespace ContatoInteligenteAPI.Common
{
    public abstract class ServiceBase
    {
        protected Result Successful()
        {
            return new Result();
        }

        protected (Result result, T data) Successful<T>()
        {
            return (new Result(), default(T))!;
        }

        protected (Result result, T data) Successful<T>(T data)
        {
            return (new Result(), data);
        }

        protected (Result result, T data) Unsuccessful<T>(string message)
        {
            return (Unsuccessful(message), default(T))!;
        }

        protected Result Unsuccessful(string message)
        {
            return new Result() { Message = message, Success = false };
        }
        protected (Result result, T data) Unsuccessful<T>(Result result)
        {
            return (result, default(T))!;
        }
    }
}
