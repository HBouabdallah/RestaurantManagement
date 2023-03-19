namespace RestaurantManagement.Services
{
    public interface IHttpService<T> where T : class
    {
        public Task<T> GetById(string endpoint, int id);
    }
}
