using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IncubatorStrategy
{
    public enum ItemType { redDiamond = 0, cyanDiamond = 1, carrot, hellFlower };

    class Item: IUri
    {
        protected ItemType type;

        public int Quantity { get; private set; }
        public Uri PictureUri { get; private set; }
        public ItemType Type
        { get { return type;}
            private set 
            {
                type = value;
                switch (type) 
                {
                    case ItemType.redDiamond: { PictureUri = new Uri("pack://application:,,,/Images/redDiamond.png"); IsCurrency = true; Quantity = 0; return; }
                    case ItemType.cyanDiamond: { PictureUri = new Uri("pack://application:,,,/Images/cyanDiamond.png"); IsCurrency = true; return; }
                    case ItemType.carrot: { PictureUri = new Uri("pack://application:,,,/Images/carrot.png"); IsCurrency = false; Quantity = 0; return; }
                    case ItemType.hellFlower: { PictureUri = new Uri("pack://application:,,,/Images/hellFlower.png"); IsCurrency = false; Quantity = 0; return; }
                    //case ItemType.pollen: { PictureUri = new Uri("pack://application:,,,/Images/magicLiquid.png"); IsCurrency = false; Quantity = 0; return; }
                }
            }
        }
        public bool IsCurrency { get; private set; }

        public Item(ItemType type, int num ) 
        {
            Quantity = num;
            Type = type;;
        }

        public Uri GetUri() 
        {
            return PictureUri;
        }

        public void IncreaseQuantity(int number) 
        {
            if (number < 0)
                return;
            Quantity += number;
        }

        public bool CanAffordBuying(int price) 
        {
            if ((Quantity - price) >= 0)
            {
                return true;
            }
            else
                return false;
        }

        public void ReduceQuantity(int number) 
        {
            if (CanAffordBuying(number))
            {
                Quantity -= number;
            }
        }
    }
}
