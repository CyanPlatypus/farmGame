using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace IncubatorStrategy
{
    public delegate void TurnOnOff ();

    class Section
    {
        public static Random rnd = new Random();

        public Dictionary<Point, IAction> Entities { get; private set; }

        public Point Size { get; private set; }

        public List<Point> PositionsForEquipment { get; private set; }

        public CreatureType CreatureType { get; private set; }

        public Uri WallPictureUri { get; private set; }

        public Uri FloorPictureUri { get; private set; }

        protected event TurnOnOff TurnOn;

        protected event TurnOnOff TurnOff;

        //lamp Quantity and fan Quantity

        public Section(CreatureType type) 
        {
            Entities = new Dictionary<Point, IAction>();
            CreatureType = type;

            switch (CreatureType)
            {
                case CreatureType.HellEyeCreature:
                    {
                        CreateSection("hellFloor.png", "hellWall.png",
                            "hellLampOn.png", "hellLampOff.png", "hellLampOff.png", 1, 0,
                            "hellFanOn.png", "hellFanOff.png", "hellFanBroken.png", 2, 0,
                            6, 4, new Point (10, 10));
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

        public virtual bool Add(Point position)
        {
            if (!Entities.ContainsKey(position) &&
                PositionsForEquipment.IndexOf(position) == -1)//if the's nothing in this position and this position is not for equipment
            {
                Entities.Add(position, new Creature(CreatureType));
                if ((CheckAliveCreaturesQuantity() == 1) &&
                    (TurnOn != null))
                    TurnOn();
                return true;
            }
            return false;
        }

        public void RemoveAt(Point position) //only Creatures can be removed
        {
            if (Entities[position] is Creature)
            {
                Entities.Remove(position);
                if ((CheckAliveCreaturesQuantity() == 0) &&
                    (TurnOff != null))
                    TurnOff();
            }
        }

        public ItemType SellAllAdultCreatures(ref int money) //sell all adult creaturs, return type of item and pointa earned
        {
            int count = 0;
            foreach (var c in Entities.ToList()) //i have no idea why .ToList() helps to 
            {
                if (c.Value is Creature)
                {
                    if (((Creature)c.Value).Stage == StagesOfLife.adult)
                    {
                        count++;
                        RemoveAt(c.Key);
                    }
                }
            }
            money = count*FunctionsAndConstants.sellingCreaturePrise[CreatureType].Item1;
            return FunctionsAndConstants.sellingCreaturePrise[CreatureType].Item2;
        }

        protected int CheckAliveCreaturesQuantity() 
        {
            int crQ = 0;
            foreach (var i in Entities) 
            {
                if ( (i.Value is Creature ) && ( ((Creature)i.Value).Stage != StagesOfLife.dead) )
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

        public void Action() 
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
                if (element is Creature)
                {
                    ((Creature)element).CurrentDamage = damage;
                    element.Action();
                }
            }
        }
    }

    //class HellSection : Section 
    //{
    //    public Uri WallPictureUri { get; private set; }

    //    public Uri FloorPictureUri { get; private set; }

    //    public HellSection(Point size) : this( 6, 4, size)//lamp fan
    //    {
    //        creatureType = typeof(HellEyeCreature);
    //        WallPictureUri = new Uri("pack://application:,,,/Images/hellWall.png");
    //        FloorPictureUri = new Uri("pack://application:,,,/Images/hellFloor.png");
    //    }

    //    private HellSection(int lamp, int fan, Point size)
    //        : base( lamp, fan, size)
    //    {
    //    }

    //    public void Add(Point position)
    //    {
    //        base.Add(new HellEyeCreature(), position);
    //    }

    //    protected override void AddNewEquipmentEntities(int quantity, EquipmentType type, ref List<Point> positions)
    //    {
    //        for (int i = 0; i < quantity; i++)
    //        {
    //            int tmp = rnd.Next(0, positions.Count);

    //            switch (type) 
    //            {
    //                //case lamp
    //                case EquipmentType.lamp: 
    //                    {
    //                        Entities.Add(positions[tmp], new Equipment(type, "hellLampOn.png", "hellLampOff.png", 1));
    //                        break; 
    //                    }
    //                //case fan
    //                case EquipmentType.fan:
    //                    {
    //                        Entities.Add(positions[tmp], new Equipment(type, "hellFanOn.png", "hellFanOff.png", 2));
    //                        break;
    //                    }
    //            }
    //            positions.RemoveAt(tmp);
    //        }
    //    }
    //}
}
