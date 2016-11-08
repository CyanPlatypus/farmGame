using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace IncubatorStrategy
{
    public interface IAction
    {
        void Action();
    }

    public interface IUri 
    {
        Uri GetUri();
    }

    class Game
    {
        public Dictionary<int, PlantSection> PlantSectionDictionary { get; protected set; }

        public Dictionary<int, AnimalSection> AnimalSectionDictionary { get; protected set; }

        public List<Item> ItemsList { get; protected set; }

        public List<Item> CurrencyList { get; protected set; }

        public int PlantSectionQuantity { get; protected set; }

        public int AnimalSectionQuantity { get; protected set; }

        public Game(int plantSectionQuantity, int animalSectionQuantity)
        {
            PlantSectionDictionary = new Dictionary<int,PlantSection>();
            AnimalSectionDictionary = new Dictionary<int, AnimalSection>();

            CurrencyList = new List<Item>();
            ItemsList = new List<Item>();

            PlantSectionQuantity = plantSectionQuantity;
            AnimalSectionQuantity = animalSectionQuantity;

            SetItemsAndCurrencyLists();
        }

        protected void SetItemsAndCurrencyLists()
        {
            foreach (ItemType s in Enum.GetValues(typeof(ItemType))) 
            {
                Item item = new Item(s, 100);
                if (item.IsCurrency)
                    CurrencyList.Add(item);
                else
                    ItemsList.Add(item);
            }
        }

        public int AddPlantSection(PlantCreatureType type) 
        {
            for (int i = 0; i < PlantSectionQuantity; i++) 
            {
                if (!PlantSectionDictionary.ContainsKey(i)) 
                {
                    foreach (var c in CurrencyList)
                    {
                        if (c.Type == FunctionsAndConstants.buyingPlantSectionPrise[type].Item2 &&
                            c.CanAffordBuying(FunctionsAndConstants.buyingPlantSectionPrise[type].Item1))
                        {
                            c.ReduceQuantity(FunctionsAndConstants.buyingPlantSectionPrise[type].Item1);

                            PlantSectionDictionary.Add(i, new PlantSection(type));
                            return i;
                        }
                    }
                    
                }
            }
            return -1;
        }

        public int AddAnimalSection(AnimalCreatureType type)
        {
            for (int i = 0; i < AnimalSectionQuantity; i++)
            {
                if (!AnimalSectionDictionary.ContainsKey(i))
                {
                    foreach (var c in CurrencyList)
                    {
                        if (c.Type == FunctionsAndConstants.buyingAnimalSectionPrise[type].Item2 &&
                            c.CanAffordBuying(FunctionsAndConstants.buyingAnimalSectionPrise[type].Item1))
                        {
                            c.ReduceQuantity(FunctionsAndConstants.buyingAnimalSectionPrise[type].Item1);

                            AnimalSection aS = new AnimalSection(type);
                            aS.CreatureNeedsFood += GiveFoodToAnimalCreature;
                            AnimalSectionDictionary.Add(i, aS);
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        bool GiveFoodToAnimalCreature(object sender, FoodEventArgs e)
        {
            foreach (var i in ItemsList)
            {
                if (i.Type == e.FoodType && i.CanAffordBuying(e.Quantity))
                {
                    i.ReduceQuantity(e.Quantity);
                    return true;
                }
            }
            return false;
        }

        public void AddCreatureIntoSection(string sectionType, int sectionIndex, Point point) 
        {
            switch (sectionType) 
            {
                case "a": 
                    {
                        if (AnimalSectionDictionary.ContainsKey(sectionIndex)) //if section with this number exists
                        {
                            foreach (var c in CurrencyList) //for all currency check
                            {
                                //if it's type equals the type of currency we're spending on bying this creature
                                if (c.Type == FunctionsAndConstants.buyingAnimalCreaturePrise[AnimalSectionDictionary[sectionIndex].CreatureType].Item2)
                                {
                                    //if we can afford to buy this creature and..
                                    if (c.CanAffordBuying(FunctionsAndConstants.buyingAnimalCreaturePrise[AnimalSectionDictionary[sectionIndex].CreatureType].Item1) &&
                                        AnimalSectionDictionary[sectionIndex].Add(point)) //..we found a place for it (and placed it)
                                        c.ReduceQuantity(FunctionsAndConstants.buyingAnimalCreaturePrise[AnimalSectionDictionary[sectionIndex].CreatureType].Item1); //reduse the quantity of this currency
                                    return;
                                }
                            }
                        }
                        break;
                    }
                case "p":
                    {
                        if (PlantSectionDictionary.ContainsKey(sectionIndex)) //if section with this number exists
                        {
                            foreach (var c in CurrencyList) //for all currency check
                            {
                                //if it's type equals the type of currency we're spending on bying this creature
                                if (c.Type == FunctionsAndConstants.buyingPlantCreaturePrise[PlantSectionDictionary[sectionIndex].CreatureType].Item2)
                                {
                                    //if we can afford to buy this creature and..
                                    if (c.CanAffordBuying(FunctionsAndConstants.buyingPlantCreaturePrise[PlantSectionDictionary[sectionIndex].CreatureType].Item1) &&
                                        PlantSectionDictionary[sectionIndex].Add(point)) //..we found a place for it (and placed it)
                                        c.ReduceQuantity(FunctionsAndConstants.buyingPlantCreaturePrise[PlantSectionDictionary[sectionIndex].CreatureType].Item1); //reduse the quantity of this currency
                                    return;
                                }
                            }
                        }
                        break;
                    }
            }
            //if (PlantSectionDictionary.ContainsKey(sectionIndex)) //if section with this number exists
            //{
            //    foreach (var c in CurrencyList) //for all currency check
            //    {
            //        //if it's type equals the type of currency we're spending on bying this creature
            //        if (c.Type == FunctionsAndConstants.buyingPlantCreaturePrise[PlantSectionDictionary[sectionIndex].CreatureType].Item2)
            //        {
            //            //if we can afford to buy this creature and..
            //            if (c.CanAffordBuying(FunctionsAndConstants.buyingPlantCreaturePrise[PlantSectionDictionary[sectionIndex].CreatureType].Item1) &&
            //                PlantSectionDictionary[sectionIndex].Add(point)) //..we found a place for it (and placed it)
            //                c.ReduceQuantity(FunctionsAndConstants.buyingPlantCreaturePrise[PlantSectionDictionary[sectionIndex].CreatureType].Item1); //reduse the quantity of this currency
            //            return;
            //        }
            //    }
            //}
        }

        public void Delete(int sectionNumber) 
        {
            if (PlantSectionDictionary.ContainsKey(sectionNumber))
                PlantSectionDictionary.Remove(sectionNumber);
        }

        public void Action()
        {
            foreach (var section in PlantSectionDictionary.Values)
            {
                section.Action();
            }
            foreach (var section in AnimalSectionDictionary.Values)
            {
                section.Action();
            }
        }

        public void FixEquipment(int index, Point point) 
        {
            foreach (var c in CurrencyList) 
            {
                if ((c.Type == FunctionsAndConstants.fixinEquipmentPrise[PlantSectionDictionary[index].CreatureType].Item2) &&
                    (c.CanAffordBuying(FunctionsAndConstants.fixinEquipmentPrise[PlantSectionDictionary[index].CreatureType].Item1)) &&
                    PlantSectionDictionary[index].FixEquipment(point))
                {
                    c.ReduceQuantity(FunctionsAndConstants.fixinEquipmentPrise[PlantSectionDictionary[index].CreatureType].Item1);
                }
                    
            }
        }

        public void SellAllAdultCreaturesInSection(string sectionType, int sectionIndex) 
        {
            int pointsEarned = 0;
            ItemType t = PlantSectionDictionary[sectionIndex].SellAllAdultCreatures(ref pointsEarned);
            if (sectionType == "a")
                t = AnimalSectionDictionary[sectionIndex].SellAllAdultCreatures(ref pointsEarned);

            foreach (var i in CurrencyList)
            {
                if (i.Type == t)
                    i.IncreaseQuantity(pointsEarned);
            }
        }

        public void CollectAllAdultCreaturesInSection(int sectionIndex)
        {
            int pointsEarned = 0;
            ItemType t = PlantSectionDictionary[sectionIndex].CollectAllAdultCreatures(ref pointsEarned);

            foreach (var i in ItemsList)
            {
                if (i.Type == t)
                    i.IncreaseQuantity(pointsEarned);
            }
        }

        public void RemoveAllCorpses(string sectionType, int sectionIndex) 
        {
            if (sectionType == "p")
                PlantSectionDictionary[sectionIndex].RemoveAllDeadCreatures();
            if (sectionType == "a")
                AnimalSectionDictionary[sectionIndex].RemoveAllDeadCreatures();
        }
    }
}
