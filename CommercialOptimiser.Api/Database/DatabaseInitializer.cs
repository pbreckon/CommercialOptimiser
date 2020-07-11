using System;
using System.Collections.Generic;
using System.Linq;
using CommercialOptimiser.Api.Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CommercialOptimiser.Api.Database
{
    public interface IDatabaseInitializer
    {
        #region Public Methods

        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Adds some default values to the Db
        /// </summary>
        void SeedData();

        #endregion
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        #region Members

        private readonly IServiceScopeFactory _scopeFactory;

        #endregion

        #region Constructors

        public DatabaseInitializer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
            context.Database.EnsureCreated();
        }

        public void SeedData()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();

            if (context.Demographics.Any() ||
                context.Breaks.Any() ||
                context.BreakDemographics.Any() ||
                context.Commercials.Any()) return;

            var demographics =
                new List<DemographicTable>
                {
                    new DemographicTable {Title = "W25-30"},
                    new DemographicTable {Title = "M18-35"},
                    new DemographicTable {Title = "T18-40"}
                };
            context.Demographics.AddRange(demographics);
            context.SaveChanges();

            var breaks =
                new List<BreakTable>
                {
                    new BreakTable {Title = "Break1", Capacity = 3},
                    new BreakTable {Title = "Break2", Capacity = 3, InvalidCommercialTypes = "Finance"},
                    new BreakTable {Title = "Break3", Capacity = 3}
                };
            context.Breaks.AddRange(breaks);
            context.SaveChanges();

            var breakDemographics =
                new List<BreakDemographicTable>
                {
                    new BreakDemographicTable
                    {
                        Break = breaks[0], Demographic = demographics[0], Rating = 80
                    },
                    new BreakDemographicTable
                    {
                        Break = breaks[0], Demographic = demographics[1], Rating = 100
                    },
                    new BreakDemographicTable
                    {
                        Break = breaks[0], Demographic = demographics[2], Rating = 250
                    },
                    new BreakDemographicTable
                    {
                        Break = breaks[1], Demographic = demographics[0], Rating = 50
                    },
                    new BreakDemographicTable
                    {
                        Break = breaks[1], Demographic = demographics[1], Rating = 120
                    },
                    new BreakDemographicTable
                    {
                        Break = breaks[1], Demographic = demographics[2], Rating = 200
                    },
                    new BreakDemographicTable
                    {
                        Break = breaks[2], Demographic = demographics[0], Rating = 350
                    },
                    new BreakDemographicTable
                    {
                        Break = breaks[2], Demographic = demographics[1], Rating = 150
                    },
                    new BreakDemographicTable
                    {
                        Break = breaks[2], Demographic = demographics[2], Rating = 500
                    }
                };
            context.BreakDemographics.AddRange(breakDemographics);
            context.SaveChanges();

            var commercials =
                new List<CommercialTable>
                {
                    new CommercialTable
                    {
                        Title = "Commercial 1", CommercialType = "Automotive", Demographic = demographics[0]
                    },
                    new CommercialTable
                    {
                        Title = "Commercial 2", CommercialType = "Travel", Demographic = demographics[1]
                    },
                    new CommercialTable
                    {
                        Title = "Commercial 3", CommercialType = "Travel", Demographic = demographics[2]
                    },
                    new CommercialTable
                    {
                        Title = "Commercial 4", CommercialType = "Automotive", Demographic = demographics[1]
                    },
                    new CommercialTable
                    {
                        Title = "Commercial 5", CommercialType = "Automotive", Demographic = demographics[1]
                    },
                    new CommercialTable
                    {
                        Title = "Commercial 6", CommercialType = "Finance", Demographic = demographics[0]
                    },
                    new CommercialTable
                    {
                        Title = "Commercial 7", CommercialType = "Finance", Demographic = demographics[1]
                    },
                    new CommercialTable
                    {
                        Title = "Commercial 8", CommercialType = "Automotive", Demographic = demographics[2]
                    },
                    new CommercialTable
                    {
                        Title = "Commercial 9", CommercialType = "Travel", Demographic = demographics[0]
                    },
                    new CommercialTable
                    {
                        Title = "Commercial 10", CommercialType = "Finance", Demographic = demographics[2]
                    }
                };
            context.Commercials.AddRange(commercials);
            context.SaveChanges();
        }

        #endregion
    }
}