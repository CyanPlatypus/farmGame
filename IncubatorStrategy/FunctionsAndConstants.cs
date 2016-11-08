using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IncubatorStrategy
{
    static class FunctionsAndConstants
    {
        //sell
        public static Dictionary<PlantCreatureType, Tuple<int, ItemType>> sellingPlantCreaturePrise = new Dictionary<PlantCreatureType, Tuple<int, ItemType>>() 
        {
            {PlantCreatureType.HellFlowerPlant, new Tuple<int, ItemType>(10, ItemType.cyanDiamond)},
            {PlantCreatureType.CarrotPlant, new Tuple<int, ItemType>(6, ItemType.cyanDiamond)}
        };

        public static Dictionary<AnimalCreatureType, Tuple<int, ItemType>> sellingAnimalCreaturePrise = new Dictionary<AnimalCreatureType, Tuple<int, ItemType>>() 
        {
            {AnimalCreatureType.HellEyeAnimal, new Tuple<int, ItemType>(1, ItemType.redDiamond)},
            {AnimalCreatureType.Rabbit, new Tuple<int, ItemType>(25, ItemType.cyanDiamond)}
        };

        //collect
        public static Dictionary<PlantCreatureType, Tuple<int, ItemType>> collectingPlantCreaturePrise = new Dictionary<PlantCreatureType, Tuple<int, ItemType>>() 
        {
            {PlantCreatureType.HellFlowerPlant, new Tuple<int, ItemType>(2, ItemType.hellFlower)},
            {PlantCreatureType.CarrotPlant, new Tuple<int, ItemType>(3, ItemType.carrot)}
        };

        //buy
        public static Dictionary<PlantCreatureType, Tuple<int, ItemType>> buyingPlantCreaturePrise = new Dictionary<PlantCreatureType, Tuple<int, ItemType>>() 
        {
            {PlantCreatureType.HellFlowerPlant, new Tuple<int, ItemType>(5, ItemType.cyanDiamond)},
            {PlantCreatureType.CarrotPlant, new Tuple<int, ItemType>(2, ItemType.cyanDiamond)}
        };

        public static Dictionary<AnimalCreatureType, Tuple<int, ItemType>> buyingAnimalCreaturePrise = new Dictionary<AnimalCreatureType, Tuple<int, ItemType>>() 
        {
            {AnimalCreatureType.HellEyeAnimal, new Tuple<int, ItemType>(15, ItemType.cyanDiamond)},
            {AnimalCreatureType.Rabbit, new Tuple<int, ItemType>(10, ItemType.cyanDiamond)}
        };

        public static Dictionary<PlantCreatureType, Tuple<int, ItemType>> buyingPlantSectionPrise = new Dictionary<PlantCreatureType, Tuple<int, ItemType>>() 
        {
            {PlantCreatureType.HellFlowerPlant, new Tuple<int, ItemType>(10, ItemType.cyanDiamond)},
            {PlantCreatureType.CarrotPlant, new Tuple<int, ItemType>(8, ItemType.cyanDiamond)}
        };

        public static Dictionary<AnimalCreatureType, Tuple<int, ItemType>> buyingAnimalSectionPrise = new Dictionary<AnimalCreatureType, Tuple<int, ItemType>>() 
        {
            {AnimalCreatureType.HellEyeAnimal, new Tuple<int, ItemType>(15, ItemType.cyanDiamond)},
            {AnimalCreatureType.Rabbit, new Tuple<int, ItemType>(10, ItemType.cyanDiamond)}
        };

        //fix equipment
        public static Dictionary<PlantCreatureType, Tuple<int, ItemType>> fixinEquipmentPrise = new Dictionary<PlantCreatureType, Tuple<int, ItemType>>() 
        {
            {PlantCreatureType.HellFlowerPlant, new Tuple<int, ItemType>(6, ItemType.cyanDiamond)},
            {PlantCreatureType.CarrotPlant, new Tuple<int, ItemType>(4, ItemType.cyanDiamond)}
        };

        public static int GetNumberFromName(string name) 
        {
            //bad
            return int.Parse(Convert.ToString(name[1]));
        }
        public static string GetSectionTypeFromName(string name)
        {
            //bad
            return Convert.ToString(name[3]);
        }
    }
}
