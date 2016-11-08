using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IncubatorStrategy
{
    enum EquipmentType { lamp = 0, fan = 1 }

    enum WorkingStatus { off, on, broken };

    class Equipment : IAction, IUri
    {
        public static Random rnd = new Random();

        protected Uri picture;

        protected bool isNotBroken;

        protected WorkingStatus status;

        public WorkingStatus Status
        {
            get { return status; }
            set 
            {
                status = value;
                switch (status) 
                {
                    case WorkingStatus.on: { picture = pictureUriOn; isNotBroken = true; return; }
                    case WorkingStatus.off: { picture = pictureUriOff; isNotBroken = true; return; }
                    case WorkingStatus.broken: { picture = pictureUriBroken; isNotBroken = false; return; }
                }
            }
        }

        public bool IsNotBroken 
        {
            get { return isNotBroken; }
            protected set 
            {
                isNotBroken = value;
                //if (IsNotBroken)
                //    picture = pictureUriOn;
                //else picture = pictureUriOff;
            } 
        }

        public int PercentageOfBreaking { get; set; }

        public EquipmentType Type { get; private set; }

        public Uri PictureUri
        {
            get { return picture; }
            protected set { picture = pictureUriOn; }
        }

        public int Damage { get; set; } 

        Uri pictureUriOn { get; set; }
        Uri pictureUriOff { get; set; }
        Uri pictureUriBroken { get; set; }

        public Equipment(EquipmentType type, 
            string picOn, string picOff, string picBroken, 
            int dmg = 1, int percentage = 0)
        {
            isNotBroken = true;
            this.Type = type;
            Damage = dmg;
            pictureUriOn = new Uri("pack://application:,,,/Images/" + picOn);
            pictureUriOff = new Uri("pack://application:,,,/Images/" + picOff);
            pictureUriBroken = new Uri("pack://application:,,,/Images/" + picBroken);
            if ((percentage <= 0) || (percentage >= 100))
                percentage = rnd.Next(2, 5); //not such a bad chance
            PercentageOfBreaking = percentage;
            picture = new Uri("pack://application:,,,/Images/" + picOff);
            status = WorkingStatus.off;
        }

        //public Equipment(EquipmentType type, int percentage = 0, int dmg = 1)
        //{
        //    IsOn = true;
        //    Type = type;
        //    Damage = dmg;

        //    if ((percentage <= 0) || (percentage >= 100))
        //        percentage = rnd.Next(20, 30); //not such a bad chance
        //    PercentageOfBreaking = percentage;
        //}

        public Uri GetUri()
        {
            return PictureUri;
        }

        //public int pubVarRandV = 0;

        public void Action()
        {
            if (status == WorkingStatus.on)
            {
                int pubVarRandV = rnd.Next(1, 101);
                if (pubVarRandV <= PercentageOfBreaking)
                {
                    IsNotBroken = false;
                    Status = WorkingStatus.broken;
                }
            }
        }

        public void TurnOff()
        {
            if (isNotBroken)
                Status = WorkingStatus.off;
        }

        public void TurnOn()
        {
            if (isNotBroken)
                Status = WorkingStatus.on;
        }
    }
}
