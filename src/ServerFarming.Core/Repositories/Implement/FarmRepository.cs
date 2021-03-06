﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmingDatabase.Model;
using FarmingDatabase.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using ServerFarming.Core.Model;
using System.Data.Common;

namespace ServerFarming.Core.Repositories.Implement
{
    public class FarmRepository : IFarmRepository
    {
        private readonly FarmingDbContext _farmingContext;
        public FarmRepository(FarmingDbContext farmingContext)
        {
            this._farmingContext = farmingContext;
        }
        public void AddNewFarm(Farm farm)
        {
            _farmingContext.Farms.Add(farm);
            _farmingContext.SaveChanges();
        }

        public void AddNewFarmComponent(Farm_Component farmComponent)
        {
            _farmingContext.FarmComponents.Add(farmComponent);
            _farmingContext.SaveChanges();
        }

        public List<Sensor_Record> GetEnvInfoToday(long farmComponentId)
        {
            string dateFormat = "yyyy-MM-dd";
            var result = _farmingContext.SensorRecords.Where(sensorData => 
                sensorData.Timestamp.ToString(dateFormat) == DateTime.Now.ToString(dateFormat)
                && sensorData.Farm_ComponentId == farmComponentId
                ).ToList();
            return result;
        }

        public List<Farm> GetFarmByUserID(long userID)
        {
            return _farmingContext.Farms.Where(farm => farm.UserId == userID).ToList();
        }

        public List<Farm_Component> GetFarmComponents(long farmID)
        {
            return _farmingContext.FarmComponents.Where(farmComponent => farmComponent.FarmId == farmID).ToList();
        }

        public OverallMonthEnvironment GetOverallEnvironmentInfo(long farmComponentId)
        {
            OverallMonthEnvironment result = new OverallMonthEnvironment
            {
                Temperature = 0,
                Luminosity = 0,
                Soil_Humidity = 0,
            };
            using (var connection = _farmingContext.Database.GetDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    string query = @"
declare @CurDate datetime2(7)
Select @CurDate = GETDATE()
SELECT 
	AVG(Temperature) as Temperature,
	AVG(Luminosity) as Luminosity, 
	AVG(Soil_Humidity) as Soil_Humidity
FROM [FarmingDatabase].[dbo].[SensorRecords]
where 
	month(cast(Timestamp as date)) = month(cast(@CurDate as date))
	and year(cast(Timestamp as date)) = year(cast(@CurDate as date))
	and Farm_ComponentId=4
";
                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = new OverallMonthEnvironment
                            {
                                Temperature = reader.GetDouble(0),
                                Luminosity = reader.GetDouble(1),
                                Soil_Humidity = reader.GetDouble(2),
                            };
                        }
                    }
                }
            }
            return result;
        }
    }
}
