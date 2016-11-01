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
        //кол-во Секций расчитывается из размера поля, которое передаётся в конструкторе

        public Dictionary<int, Section> SectionDictionary { get; protected set; }

        public List<Item> ItemsList { get; protected set; }

        public List<Item> CurrencyList { get; protected set; }

        public int SectionQuantity { get; protected set; }

        public Game(int sectionQuantity)
        {
            SectionDictionary = new Dictionary<int,Section>();

            CurrencyList = new List<Item>();
            ItemsList = new List<Item>();

            SectionQuantity = sectionQuantity;

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

        public int Add(CreatureType type) 
        {
            for (int i = 0; i < SectionQuantity; i++) 
            {
                if (!SectionDictionary.ContainsKey(i)) 
                {
                    SectionDictionary.Add(i, new Section(type));
                    return i;
                }
            }
            return -1;
        }

        public void AddCreatureIntoSection(int sectionIndex, Point point) 
        {
            if (SectionDictionary.ContainsKey(sectionIndex)) //if section with this number exists
            {
                foreach(var c in CurrencyList) //for all currency check
                {
                    //if it's type equals the type of currency we're spending on bying this creature
                    if (c.Type == FunctionsAndConstants.buyingCreaturePrise[SectionDictionary[sectionIndex].CreatureType].Item2)
                    {
                        //if we can afford to buy this creature and..
                        if (c.CanAffordBuying(FunctionsAndConstants.buyingCreaturePrise[SectionDictionary[sectionIndex].CreatureType].Item1) &&
                            SectionDictionary[sectionIndex].Add(point)) //..we found a place for it (and placed it)
                            c.ReduceQuantity(FunctionsAndConstants.buyingCreaturePrise[SectionDictionary[sectionIndex].CreatureType].Item1); //reduse the quantity of this currency
                        return;
                    }
                }
            }
        }

        public void Delete(int sectionNumber) 
        {
            if (SectionDictionary.ContainsKey(sectionNumber))
                SectionDictionary.Remove(sectionNumber);
        }

        public void Action()
        {
            foreach (var section in SectionDictionary.Values)
            {
                section.Action();
            }
        }

        public void SellAllAdultCreaturesInSection(int sectionIndex) 
        {
            int pointsEarned = 0;
            ItemType t = SectionDictionary[sectionIndex].SellAllAdultCreatures(ref pointsEarned);

            foreach (var i in CurrencyList)
            {
                if (i.Type == t)
                    i.IncreaseQuantity(pointsEarned);
            }
        }
    }
}
