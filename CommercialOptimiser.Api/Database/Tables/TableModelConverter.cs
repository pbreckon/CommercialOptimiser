using System;
using System.Linq;
using CommercialOptimiser.Core.Models;

namespace CommercialOptimiser.Api.Database.Tables
{
    public interface ITableModelConverter
    {
        #region Public Methods

        UserReportBreakTable ConvertModelToTable(UserReportBreak model);
        UserReportBreakCommercialTable ConvertModelToTable(UserReportBreakCommercial model);
        UserTable ConvertModelToTable(User model);

        UserReportBreak ConvertTableToModel(UserReportBreakTable table);
        UserReportBreakCommercial ConvertTableToModel(UserReportBreakCommercialTable table);
        Break ConvertTableToModel(BreakTable table);
        BreakDemographic ConvertTableToModel(BreakDemographicTable table);
        Commercial ConvertTableToModel(CommercialTable table);
        Demographic ConvertTableToModel(DemographicTable table);
        User ConvertTableToModel(UserTable table);

        #endregion
    }

    public class TableModelConverter : ITableModelConverter
    {
        #region Public Methods

        public UserReportBreakTable ConvertModelToTable(UserReportBreak model)
        {
            var userReportBreakTable =
                new UserReportBreakTable
                {
                    Id = model.Id,
                    User = ConvertModelToTable(model.User),
                    BreakTitle = model.BreakTitle,
                    UserReportBreakCommercials =
                        model.UserReportBreakCommercials.Select(ConvertModelToTable).ToList()
                };
            return userReportBreakTable;
        }

        public UserReportBreakCommercialTable ConvertModelToTable(UserReportBreakCommercial model)
        {
            var userTable =
                new UserReportBreakCommercialTable
                {
                    Id = model.Id,
                    CommercialTitle = model.CommercialTitle,
                    Rating = model.Rating
                };
            return userTable;
        }

        public UserTable ConvertModelToTable(User model)
        {
            var userTable =
                new UserTable
                {
                    Id = model.Id,
                    UserUniqueId = model.UniqueUserId
                };
            return userTable;
        }

        public UserReportBreakCommercial ConvertTableToModel(UserReportBreakCommercialTable table)
        {
            var userBreakCommercial =
                new UserReportBreakCommercial
                {
                    CommercialTitle = table.CommercialTitle,
                    Rating = table.Rating
                };
            return userBreakCommercial;
        }

        public UserReportBreak ConvertTableToModel(UserReportBreakTable table)
        {
            var userBreak =
                new UserReportBreak
                {
                    BreakTitle = table.BreakTitle,
                    UserReportBreakCommercials = table.UserReportBreakCommercials.Select(ConvertTableToModel).ToList()
                };
            return userBreak;
        }

        public User ConvertTableToModel(UserTable table)
        {
            var userBreak =
                new User
                {
                    Id = table.Id,
                    UniqueUserId = table.UserUniqueId
                };
            return userBreak;
        }

        public Break ConvertTableToModel(BreakTable table)
        {
            return
                new Break
                {
                    Id = table.Id,
                    BreakDemographics =
                        table.BreakDemographics.Select(ConvertTableToModel).ToList(),
                    Capacity = table.Capacity,
                    Title = table.Title,
                    InvalidCommercialTypes = table.InvalidCommercialTypes?.Split('|')?.ToList()
                };
        }

        public BreakDemographic ConvertTableToModel(BreakDemographicTable table)
        {
            return
                new BreakDemographic
                {
                    Id = table.Id,
                    Demographic = ConvertTableToModel(table.Demographic),
                    Rating = table.Rating
                };
        }

        public Commercial ConvertTableToModel(CommercialTable table)
        {
            return
                new Commercial
                {
                    Id = table.Id,
                    Demographic = ConvertTableToModel(table.Demographic),
                    CommercialType = table.CommercialType,
                    Title = table.Title
                };
        }

        public Demographic ConvertTableToModel(DemographicTable table)
        {
            return
                new Demographic
                {
                    Id = table.Id,
                    Title = table.Title
                };
        }

        #endregion
    }
}