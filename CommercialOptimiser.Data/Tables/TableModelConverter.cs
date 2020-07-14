using System;
using System.Collections.Generic;
using System.Linq;
using CommercialOptimiser.Core.Models;
using Newtonsoft.Json;

namespace CommercialOptimiser.Data.Tables
{
    public interface ITableModelConverter
    {
        #region Public Methods

        UserReportBreakTable ConvertModelToTable(string userUniqueId, List<UserReportBreak> model);

        List<UserReportBreak> ConvertTableToModel(UserReportBreakTable table);

        Break ConvertTableToModel(BreakTable table);
        BreakDemographic ConvertTableToModel(BreakDemographicTable table);
        Commercial ConvertTableToModel(CommercialTable table);
        Demographic ConvertTableToModel(DemographicTable table);

        #endregion
    }

    public class TableModelConverter : ITableModelConverter
    {
        #region Public Methods

        public UserReportBreakTable ConvertModelToTable(string userUniqueId, List<UserReportBreak> model)
        {
            var json = JsonConvert.SerializeObject(model);

            var userReportBreakTable =
                new UserReportBreakTable
                {
                    UserUniqueId = userUniqueId,
                    Report = json
                };
            return userReportBreakTable;
        }

        public List<UserReportBreak> ConvertTableToModel(UserReportBreakTable table)
        {
            var userReportBreaks = JsonConvert.DeserializeObject<List<UserReportBreak>>(table.Report);
            return userReportBreaks;
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