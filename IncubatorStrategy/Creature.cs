using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IncubatorStrategy
{

    public enum StagesOfLife{ newborn = 0, child, adult, dead};
    public enum CreatureType { HellEyeCreature };

    class Creature : IAction, IUri
    {
        protected Dictionary<StagesOfLife, int> stagesDuration;

        protected int health;
        protected int age;
        protected Uri pictureUri;
        protected StagesOfLife stage; 

        protected List<Uri> ImageUri { get; set; }

        public Uri PictureUri 
        {
            get 
            {
                //switch (Stage)
                //{
                //    case (StagesOfLife)0: { pictureUri = ImageUri[0]; return pictureUri; }
                //    case (StagesOfLife)1: { pictureUri = ImageUri[1]; return pictureUri; }
                //    case (StagesOfLife)2: { pictureUri = ImageUri[2]; return pictureUri; }
                //    case (StagesOfLife)3: { pictureUri = ImageUri[3]; return pictureUri; }
                //    //default: { pictureUri = ImageUri[0]; return; }
                //}
                return pictureUri; 
            }
            internal set { pictureUri = ImageUri[0]; }
        }

        public int CurrentDamage { get; set; }

        public CreatureType Type { get; set; }

        public int Health 
        {
            get { return health; }
            internal set 
            {
                if (Stage != StagesOfLife.adult)
                {
                    health = value;
                    if (health <= 0)
                    {
                        Stage = StagesOfLife.dead;
                        health = 0;
                    }
                }
            } 
        }

        public StagesOfLife Stage 
        {
            get { return stage; }
            internal set 
            {
                stage = value;

                switch (Stage)
                {
                    case (StagesOfLife)0: { pictureUri = ImageUri[0]; return; }
                    case (StagesOfLife)1: { pictureUri = ImageUri[1]; return; }
                    case (StagesOfLife)2: { pictureUri = ImageUri[2]; return; }
                    case (StagesOfLife)3: { pictureUri = ImageUri[3]; return; }
                    //default: { pictureUri = ImageUri[0]; return; }
                }
            }
        }

        public int Age 
        {
            get { return age; } 
            private set 
            {
                if (Stage != StagesOfLife.dead)
                {
                    age = value;
                    foreach (var stage in stagesDuration)
                    {
                        if (age == stage.Value)
                        {
                            Stage = stage.Key;
                            break;
                        }
                    }
                }
            } 
        }

        public Creature(CreatureType type)
        {
            Type = type;
            switch (Type)
            {
                case CreatureType.HellEyeCreature:
                    {
                        CreateCreature("eyeSmall.png", "eyeMedium.png", "eyeBig.png", "eyeDead.png", 40, 4, 7);
                        break;
                    }
            }

            Age = 0;
            CurrentDamage = 0;
        }

        protected void CreateCreature(string pictureNewborn, string pictureChild, string pictureAdult, string pictureDead, 
            int health = 30,
            int childStartsWith = 6, int adultStartsWith = 9) 
        {
            stagesDuration = new Dictionary<StagesOfLife, int>(); //filling dictionary with stages duration
            stagesDuration.Add(StagesOfLife.child, childStartsWith);
            stagesDuration.Add(StagesOfLife.adult, adultStartsWith);

            ImageUri = new List<Uri>(); // filling Uri list with stages images
            ImageUri.Add(new Uri("pack://application:,,,/Images/" + pictureNewborn));
            ImageUri.Add(new Uri("pack://application:,,,/Images/" + pictureChild));
            ImageUri.Add(new Uri("pack://application:,,,/Images/" + pictureAdult));
            ImageUri.Add(new Uri("pack://application:,,,/Images/" + pictureDead));

            PictureUri = ImageUri[0];
            Stage = StagesOfLife.newborn;
            Health = health;
        }

        public Uri GetUri() 
        {
            return PictureUri;
        }

        public void Action() 
        {
            Health -= CurrentDamage;
            Age++;
        }
    }

    

    //class HellEyeCreature : Creature 
    //{
    //    public HellEyeCreature()
    //        : this("eyeSmall.png", "eyeMedium.png", "eyeBig.png", "eyeDead.png", 40, 4, 7)
    //    {
    //    }

    //    private HellEyeCreature(string pictureNewborn, string pictureChild, string pictureAdult, string pictureDead, int health = 30, int childStartsWith = 6, int adultStartsWith = 9)
    //    :base (pictureNewborn, pictureChild, pictureAdult, pictureDead, health, childStartsWith, adultStartsWith)
    //    { }
    //}
}
