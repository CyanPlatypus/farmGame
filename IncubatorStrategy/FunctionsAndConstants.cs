using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IncubatorStrategy
{
    static class FunctionsAndConstants
    {
        public static Dictionary<CreatureType, Tuple<int, ItemType>> sellingCreaturePrise = new Dictionary<CreatureType, Tuple<int, ItemType>>() 
        {
            {CreatureType.HellEyeCreature, new Tuple<int, ItemType>(10, ItemType.cyanDiamond)}
        };

        public static Dictionary<CreatureType, Tuple<int, ItemType>> buyingCreaturePrise = new Dictionary<CreatureType, Tuple<int, ItemType>>() 
        {
            {CreatureType.HellEyeCreature, new Tuple<int, ItemType>(5, ItemType.cyanDiamond)}
        };

        public static int GetNumberFromName(string name) 
        {
            //bad
            return int.Parse(Convert.ToString(name[1]));
        }
    }
}
