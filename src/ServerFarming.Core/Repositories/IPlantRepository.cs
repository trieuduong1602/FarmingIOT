﻿using FarmingDatabase.Model;
using ServerFarming.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFarming.Core.Repositories
{
    public interface IPlantRepository
    {
        void AddNewPlant(PlantType plant);
        List<PlantDetail> GetPlantDetail(long farmComponentId);
        List<PlantKB> GetAllPlant();
    }
}
