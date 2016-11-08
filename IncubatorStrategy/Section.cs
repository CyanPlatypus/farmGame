using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace IncubatorStrategy
{
    public delegate void TurnOnOff ();

    abstract class Section
    {
        public static Random rnd = new Random();

        public Dictionary<Point, IAction> Entities { get; protected set; }

        public Point Size { get; protected set; }

        //public PlantCreatureType CreatureType { get; protected set; }

        //public Uri WallPictureUri { get; protected set; }

        public Uri FloorPictureUri { get; protected set; }

        public Section() { }

        //protected abstract void CreateSection();

        public abstract bool Add(Point position);

        public abstract void RemoveAt(Point position);

        public void RemoveAllDeadCreatures() 
        {
            foreach(var e in Entities.ToList())
            {
                if (e.Value is Creature) 
                {
                    if (((Creature)(e.Value)).Stage == StagesOfLife.dead)
                        Entities.Remove(e.Key);
                }
            }
        }

        public abstract ItemType SellAllAdultCreatures(ref int money);

        public abstract void Action();
    }



    class PlantSection: Section
    {
        public List<Point> PositionsForEquipment { get; private set; }

        public PlantCreatureType CreatureType { get; private set; }

        protected event TurnOnOff TurnOn;

        protected event TurnOnOff TurnOff;

        public Uri WallPictureUri { get; protected set; }

        public PlantSection(PlantCreatureType type) 
        {
            Entities = new Dictionary<Point, IAction>();
            CreatureType = type;

            switch (CreatureType)
            {
                case PlantCreatureType.HellFlowerPlant:
                    {
                        CreateSection("hellFloor.png", "hellWall.png",
                            "hellLampOn.png", "hellLampOff.png", "hellLampOff.png", 1, 0,
                            "hellFanOn.png", "hellFanOff.png", "hellFanBroken.png", 2, 0,
                            6, 4, new Point (10, 10));
                        break;
                    }
                case PlantCreatureType.CarrotPlant:
                    {
                        CreateSection("gardenFloor.png", "gardenWall.png",
                            "gardenLampOn.png", "gardenLampOff.png", "gardenLampOff.png", 1, 0,
                            "gardenFanOn.png", "hellFanOff.png", "hellFanBroken.png", 1, 0,
                            6, 2, new Point(10, 10));
                        break;
                    }
            }
        }

        protected void CreateSection(string floorPicture, string wallPicture,
            string lampOn, string lampOff, string lampBroken, int lampDamage, int lampPersantage,
            string fanOn, string fanOff, string fanBroken, int fanDamage, int fanPersantage,
            int lampQuantity, int fanQuantity, Point size)
        {
            Size = size;

            PositionsForEquipment = FindPossiblePositionsForEquipment();

            List<Point> positionsForEquipment = FindPossiblePositionsForEquipment();
            AddNewEquipmentEntities(lampQuantity, ref positionsForEquipment, EquipmentType.lamp, lampOn, lampOff, lampBroken, lampDamage, lampPersantage);
            AddNewEquipmentEntities(fanQuantity, ref positionsForEquipment, EquipmentType.fan, fanOn, fanOff, fanBroken, fanDamage, fanPersantage);

            WallPictureUri = new Uri("pack://application:,,,/Images/" + wallPicture);
            FloorPictureUri = new Uri("pack://application:,,,/Images/" + floorPicture);
        }

        protected virtual void AddNewEquipmentEntities(int quantity, ref List<Point> positions,
            EquipmentType type, 
            string on, string off, string broken,
            int dmg, int pers) 
        {
            for (int i = 0; i < quantity; i++) 
            {
                int tmp = rnd.Next(0, positions.Count);

                Equipment e = new Equipment(type, on, off, broken, dmg, pers);
                TurnOff += e.TurnOff;
                TurnOn += e.TurnOn;
         
                Entities.Add(positions[tmp], e);
                positions.RemoveAt(tmp);
            }
        }

        public override bool Add(Point position)
        {
            if (!Entities.ContainsKey(position) &&
                PositionsForEquipment.IndexOf(position) == -1)//if the's nothing in this position and this position is not for equipment
            {
                Entities.Add(position, new PlantCreature(CreatureType));
                if ((CheckAliveCreaturesQuantity() == 1) &&
                    (TurnOn != null))
                    TurnOn();
                return true;
            }
            return false;
        }

        public override void RemoveAt(Point position) //only Creatures can be removed
        {
            if (Entities[position] is PlantCreature)
            {
                Entities.Remove(position);
                if ((CheckAliveCreaturesQuantity() == 0) &&
                    (TurnOff != null))
                    TurnOff();
            }
        }

        public override ItemType SellAllAdultCreatures(ref int money) //sell all adult creaturs, return type of item and pointa earned
        {
            int count = RemoveAndCountAdultCreatures();

            money = count*FunctionsAndConstants.sellingPlantCreaturePrise[CreatureType].Item1;
            return FunctionsAndConstants.sellingPlantCreaturePrise[CreatureType].Item2;
        }

        public ItemType CollectAllAdultCreatures(ref int money) //collect all adult creaturs, return type of item and points earned
        {
            int count = RemoveAndCountAdultCreatures();

            money = count * FunctionsAndConstants.collectingPlantCreaturePrise[CreatureType].Item1;
            return FunctionsAndConstants.collectingPlantCreaturePrise[CreatureType].Item2;
        }

        int RemoveAndCountAdultCreatures() 
        {
            int count = 0;
            foreach (var c in Entities.ToList())
            {
                if (c.Value is PlantCreature)
                {
                    if (((PlantCreature)c.Value).Stage == StagesOfLife.adult)
                    {
                        count++;
                        RemoveAt(c.Key);
                    }
                }
            }
            return count;
        }

        protected int CheckAliveCreaturesQuantity() 
        {
            int crQ = 0;
            foreach (var i in Entities) 
            {
                if ( (i.Value is PlantCreature ) && ( ((PlantCreature)i.Value).Stage != StagesOfLife.dead) )
                    crQ++;
            }
            return crQ;
        }

        protected List<Point> FindPossiblePositionsForEquipment()
        {
            List<Point> listWeReturn = new List<Point>();

            //choosing for possible positions for equipments cells around the perimeter
            for (int i = 0; i < Size.Y; i++) //upper, lower
            {
                listWeReturn.Add(new Point(0,i));
                listWeReturn.Add(new Point(Size.X -1, i));
            }
            for (int i = 1; i < Size.X-1; i++) // to the left, to the right
            {
                listWeReturn.Add(new Point(i, 0));
                listWeReturn.Add(new Point(i, Size.Y-1));
            }
            
            return listWeReturn;
        }

        public bool FixEquipment(Point point) 
        {
            if (PositionsForEquipment.Contains(point) && !((Equipment)Entities[point]).IsNotBroken)
            {
                ((Equipment)Entities[point]).Status = WorkingStatus.on;
                return true;
            }
            return false;
        }

        public EquipmentType? ReturnEquipmentType(Point point) 
        {
            if (PositionsForEquipment.Contains(point))
                return ((Equipment)Entities[point]).Type;
            else return null;
        }

        public override void Action() 
        {
            int damage = 0;

            foreach (var element in Entities.Values)
            {
                if (element is Equipment)
                {
                    element.Action();
                    if (!((Equipment)element).IsNotBroken)
                        damage += ((Equipment)element).Damage;
                }
            }
            //the same thing for Creatures
            foreach (var element in Entities.Values)
            {
                if (element is PlantCreature)
                {
                    ((PlantCreature)element).CurrentDamage = damage;
                    element.Action();
                }
            }
        }
    }


    class AnimalSection : Section
    {
        public AnimalCreatureType CreatureType { get; private set; }

        public event GetFood CreatureNeedsFood; 

        public AnimalSection(AnimalCreatureType type)
        {
            Entities = new Dictionary<Point, IAction>();
            CreatureType = type;

            switch (CreatureType)
            {
                case AnimalCreatureType.HellEyeAnimal:
                    {
                        CreateSection("hellFloor.png", new Point(5, 5));
                        break;
                    }
                case AnimalCreatureType.Rabbit://!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    {
                        CreateSection("gardenFloor.png", new Point(5, 5));
                        break;
                    }
            }
        }

        protected void CreateSection(string floorPicture, Point size)
        {
            Size = size;

            FloorPictureUri = new Uri("pack://application:,,,/Images/" + floorPicture);
        }

        public override bool Add(Point position)
        {
            if (!Entities.ContainsKey(position) )//if the's nothing in this position
            {
                AnimalCreature aC = new AnimalCreature(CreatureType);
                aC.AskForFood += AskForFoodForCreatures;
                Entities.Add(position, aC);
                return true;
            }
            return false;
        }

        public override void RemoveAt(Point position) //only Creatures can be removed
        {
            if (Entities[position] is AnimalCreature)
            {
                Entities.Remove(position);
            }
        }

        bool AskForFoodForCreatures(object sender, FoodEventArgs e) 
        {
            if ( CreatureNeedsFood != null)
            {
                if (CreatureNeedsFood(this, e))
                    return true;
                else 
                    return false;
            }
            return false;
        }

        public override ItemType SellAllAdultCreatures(ref int money) //sell all adult creaturs, return type of item and pointa earned
        {
            int count = 0;
            foreach (var c in Entities.ToList()) //i have no idea why .ToList() helps to 
            {
                if (c.Value is AnimalCreature)
                {
                    if (((AnimalCreature)c.Value).Stage == StagesOfLife.adult)
                    {
                        count++;
                        RemoveAt(c.Key);
                    }
                }
            }
            money = count * FunctionsAndConstants.sellingAnimalCreaturePrise[CreatureType].Item1;
            return FunctionsAndConstants.sellingAnimalCreaturePrise[CreatureType].Item2;
        }

        public override void Action()
        {
            //the same thing for Creatures
            foreach (var element in Entities.Values)
            {
               element.Action();
            }
        }
    }
}
