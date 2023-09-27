namespace ApiApplication.Models
{
    public class ResultBase
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public object ReturnedObject { get; set; }
    }

    public class ResultBase<T> : ResultBase
    {
        public new T ReturnedObject { get; set; }
    }

    public class Result : ResultBase
    {
        
    }
}
