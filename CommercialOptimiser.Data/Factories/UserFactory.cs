using System;
using System.Threading.Tasks;
using CommercialOptimiser.Core.Models;
using CommercialOptimiser.Data.Factories.Contracts;
using CommercialOptimiser.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace CommercialOptimiser.Data.Factories
{
    public class UserFactory : IUserFactory
    {
        #region Members

        private readonly DatabaseContext _databaseContext;
        private readonly ITableModelConverter _tableModelConverter;

        #endregion

        #region Constructors

        public UserFactory(
            DatabaseContext dbContext,
            ITableModelConverter tableModelConverter)
        {
            _databaseContext = dbContext;
            _tableModelConverter = tableModelConverter;
        }

        #endregion

        public async Task<User> GetUserAsync(string uniqueUserId)
        {
            var userTable =
                await _databaseContext.Users.FirstOrDefaultAsync(
                value =>
                    value.UniqueUserId == uniqueUserId);
            if (userTable == null) return null;

            return _tableModelConverter.ConvertTableToModel(userTable);
        }

        public async Task AddUserAsync(User user)
        {
            var userTable = _tableModelConverter.ConvertModelToTable(user);
            await _databaseContext.Users.AddAsync(userTable);
            _databaseContext.SaveChanges();

            user.Id = userTable.Id;
        }
    }
}
