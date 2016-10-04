namespace TrueMoney.Infrastructure.Repositories
{
    using System.Collections.Generic;

    public interface IRepository<T>
    {
        T GetById(int Id);

        IList<T> GetAll();

        /// <summary>
        /// Not closed entities
        /// </summary>
        /// <returns></returns>
        IList<T> GetAllActive();
    }
}
