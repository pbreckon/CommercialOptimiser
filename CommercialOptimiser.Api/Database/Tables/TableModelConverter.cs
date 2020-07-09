using System;
using System.Linq;
using CommercialOptimiser.Data.Models;

namespace CommercialOptimiser.Api.Database.Tables
{
    public interface ITableModelConverter
    {
        Break ConvertTableToModel(BreakTable table);
        BreakDemographic ConvertTableToModel(BreakDemographicTable table);
        Commercial ConvertTableToModel(CommercialTable table);
        Demographic ConvertTableToModel(DemographicTable table);
    }

    public class TableModelConverter : ITableModelConverter
    {
        #region Public Methods

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