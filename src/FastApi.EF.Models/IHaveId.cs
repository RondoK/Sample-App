namespace FastApi.EF.Models
{
    //Since I wrote this code, a lot of time has passed until this comment.
    //I probably wanted Id property for "get paged"
    public interface IHaveId<T>
    {
        public T Id { get; set; }
    }
}