namespace CqrsApiExample.Models
{
    public class ResultViewModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public T Data { get; set; }

        public ResultViewModel()
        {

        }
        public ResultViewModel(bool success, string message, T Data)
        {
            Success = success;
            Message = message;
            this.Data = Data;
        }
    }
}