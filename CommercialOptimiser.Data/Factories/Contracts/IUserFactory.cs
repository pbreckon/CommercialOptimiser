using System;
using System.Threading.Tasks;
using CommercialOptimiser.Core.Models;

namespace CommercialOptimiser.Data.Factories.Contracts
{
    public interface IUserFactory
    {
        #region Public Methods

        Task AddUserAsync(User user);
        Task<User> GetUserAsync(string uniqueUserId);

        #endregion
    }
}