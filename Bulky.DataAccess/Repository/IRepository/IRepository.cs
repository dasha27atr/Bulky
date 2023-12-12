namespace Bulky.DataAccess.Repository.IRepository
{
    internal interface IRepository<T> where T : class
    {
        // T - Category
        IEnumerable<T> GetAll();
        T Get();
    }
}
