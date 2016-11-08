using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IncubatorStrategy
{

    public enum StagesOfLife{ newborn = 0, child, adult, dead};
    public enum PlantCreatureType { HellFlowerPlant, CarrotPlant };
    public enum AnimalCreatureType { HellEyeAnimal, Rabbit };


    abstract class Creature : IAction, IUri
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
            protected set { pictureUri = ImageUri[0]; }
        }

        //public int CurrentDamage { get; set; }

        //public PlantCreatureType Type { get; set; }

        public int Health
        {
            get { return health; }
            protected set
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
            protected set
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
            protected set
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

        public Creature() {}

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

        public abstract void Action();
    }



    class PlantCreature : Creature//, IAction, IUri
    {
        //protected Dictionary<StagesOfLife, int> stagesDuration;

        //protected int health;
        //protected int age;
        //protected Uri pictureUri;
        //protected StagesOfLife stage; 

        //protected List<Uri> ImageUri { get; set; }

        //public Uri PictureUri 
        //{
        //    get 
        //    {
        //        //switch (Stage)
        //        //{
        //        //    case (StagesOfLife)0: { pictureUri = ImageUri[0]; return pictureUri; }
        //        //    case (StagesOfLife)1: { pictureUri = ImageUri[1]; return pictureUri; }
        //        //    case (StagesOfLife)2: { pictureUri = ImageUri[2]; return pictureUri; }
        //        //    case (StagesOfLife)3: { pictureUri = ImageUri[3]; return pictureUri; }
        //        //    //default: { pictureUri = ImageUri[0]; return; }
        //        //}
        //        return pictureUri; 
        //    }
        //    internal set { pictureUri = ImageUri[0]; }
        //}

        public int CurrentDamage { get; set; }

        public PlantCreatureType Type { get; set; }

        //public int Health 
        //{
        //    get { return health; }
        //    internal set 
        //    {
        //        if (Stage != StagesOfLife.adult)
        //        {
        //            health = value;
        //            if (health <= 0)
        //            {
        //                Stage = StagesOfLife.dead;
        //                health = 0;
        //            }
        //        }
        //    } 
        //}

        //public StagesOfLife Stage 
        //{
        //    get { return stage; }
        //    internal set 
        //    {
        //        stage = value;

        //        switch (Stage)
        //        {
        //            case (StagesOfLife)0: { pictureUri = ImageUri[0]; return; }
        //            case (StagesOfLife)1: { pictureUri = ImageUri[1]; return; }
        //            case (StagesOfLife)2: { pictureUri = ImageUri[2]; return; }
        //            case (StagesOfLife)3: { pictureUri = ImageUri[3]; return; }
        //            //default: { pictureUri = ImageUri[0]; return; }
        //        }
        //    }
        //}

        //public int Age 
        //{
        //    get { return age; } 
        //    private set 
        //    {
        //        if (Stage != StagesOfLife.dead)
        //        {
        //            age = value;
        //            foreach (var stage in stagesDuration)
        //            {
        //                if (age == stage.Value)
        //                {
        //                    Stage = stage.Key;
        //                    break;
        //                }
        //            }
        //        }
        //    } 
        //}

        public PlantCreature(PlantCreatureType type)
        {
            Type = type;
            switch (Type)
            {
                case PlantCreatureType.HellFlowerPlant:
                    {
                        CreateCreature("hellPlantSmall.png", "hellPlantMedium.png", "hellPlantBig.png", "eyeDead.png", 40, 4, 7);
                        break;
                    }
                case PlantCreatureType.CarrotPlant:
                    {
                        CreateCreature("carrotPlantSmall.png", "carrotPlantMedium.png", "carrotPlantBig.png", "carrotPlantDead.png", 50, 6, 9);
                        break;
                    }
            }

            Age = 0;
            CurrentDamage = 0;
        }

        //protected void CreateCreature(string pictureNewborn, string pictureChild, string pictureAdult, string pictureDead, 
        //    int health = 30,
        //    int childStartsWith = 6, int adultStartsWith = 9) 
        //{
        //    stagesDuration = new Dictionary<StagesOfLife, int>(); //filling dictionary with stages duration
        //    stagesDuration.Add(StagesOfLife.child, childStartsWith);
        //    stagesDuration.Add(StagesOfLife.adult, adultStartsWith);

        //    ImageUri = new List<Uri>(); // filling Uri list with stages images
        //    ImageUri.Add(new Uri("pack://application:,,,/Images/" + pictureNewborn));
        //    ImageUri.Add(new Uri("pack://application:,,,/Images/" + pictureChild));
        //    ImageUri.Add(new Uri("pack://application:,,,/Images/" + pictureAdult));
        //    ImageUri.Add(new Uri("pack://application:,,,/Images/" + pictureDead));

        //    PictureUri = ImageUri[0];
        //    Stage = StagesOfLife.newborn;
        //    Health = health;
        //}

        //public Uri GetUri() 
        //{
        //    return PictureUri;
        //}

        public override void Action()
        {
            Health -= CurrentDamage;
            Age++;
        }
    }


    public delegate bool GetFood(object sender, FoodEventArgs e);

    class AnimalCreature : Creature//, IAction, IUri
    {
        //protected Dictionary<StagesOfLife, int> stagesDuration;

        //protected int health;
        //protected int age;
        //protected Uri pictureUri;
        //protected StagesOfLife stage; 

        //protected List<Uri> ImageUri { get; set; }

        //public Uri PictureUri 
        //{
        //    get 
        //    {
        //        //switch (Stage)
        //        //{
        //        //    case (StagesOfLife)0: { pictureUri = ImageUri[0]; return pictureUri; }
        //        //    case (StagesOfLife)1: { pictureUri = ImageUri[1]; return pictureUri; }
        //        //    case (StagesOfLife)2: { pictureUri = ImageUri[2]; return pictureUri; }
        //        //    case (StagesOfLife)3: { pictureUri = ImageUri[3]; return pictureUri; }
        //        //    //default: { pictureUri = ImageUri[0]; return; }
        //        //}
        //        return pictureUri; 
        //    }
        //    internal set { pictureUri = ImageUri[0]; }
        //}

        public int CurrentDamage { get; set; }

        public AnimalCreatureType Type { get; set; }

        //public int Health 
        //{
        //    get { return health; }
        //    internal set 
        //    {
        //        if (Stage != StagesOfLife.adult)
        //        {
        //            health = value;
        //            if (health <= 0)
        //            {
        //                Stage = StagesOfLife.dead;
        //                health = 0;
        //            }
        //        }
        //    } 
        //}

        //public StagesOfLife Stage 
        //{
        //    get { return stage; }
        //    internal set 
        //    {
        //        stage = value;

        //        switch (Stage)
        //        {
        //            case (StagesOfLife)0: { pictureUri = ImageUri[0]; return; }
        //            case (StagesOfLife)1: { pictureUri = ImageUri[1]; return; }
        //            case (StagesOfLife)2: { pictureUri = ImageUri[2]; return; }
        //            case (StagesOfLife)3: { pictureUri = ImageUri[3]; return; }
        //            //default: { pictureUri = ImageUri[0]; return; }
        //        }
        //    }
        //}

        //public int Age 
        //{
        //    get { return age; } 
        //    private set 
        //    {
        //        if (Stage != StagesOfLife.dead)
        //        {
        //            age = value;
        //            foreach (var stage in stagesDuration)
        //            {
        //                if (age == stage.Value)
        //                {
        //                    Stage = stage.Key;
        //                    break;
        //                }
        //            }
        //        }
        //    } 
        //}

        public ItemType FoodType { get; private set; }

        public event GetFood AskForFood;

        public AnimalCreature(AnimalCreatureType type)
        {
            Type = type;
            switch (Type)
            {
                case AnimalCreatureType.HellEyeAnimal:
                    {
                        CreateCreature(ItemType.hellFlower,"eyeSmall.png", "eyeMedium.png", "eyeBig.png", "eyeDead.png", 40, 4, 7);
                        break;
                    }
                case AnimalCreatureType.Rabbit:
                    {
                        CreateCreature(ItemType.carrot, "rabbitSmall.png", "rabbitMedium.png", "rabbitBig.png", "rabbitDead.png", 20, 3, 6);
                        break;
                    }
            }

            Age = 0;
            CurrentDamage = 0;
        }

        void CreateCreature(ItemType food, string pictureNewborn, string pictureChild, string pictureAdult, string pictureDead,
            int health = 30,
            int childStartsWith = 6, int adultStartsWith = 9)
        {
            base.CreateCreature(pictureNewborn, pictureChild, pictureAdult, pictureDead,
            health,
            childStartsWith, adultStartsWith );
            FoodType = food;
        }

        //protected void CreateCreature(string pictureNewborn, string pictureChild, string pictureAdult, string pictureDead, 
        //    int health = 30,
        //    int childStartsWith = 6, int adultStartsWith = 9) 
        //{
        //    stagesDuration = new Dictionary<StagesOfLife, int>(); //filling dictionary with stages duration
        //    stagesDuration.Add(StagesOfLife.child, childStartsWith);
        //    stagesDuration.Add(StagesOfLife.adult, adultStartsWith);

        //    ImageUri = new List<Uri>(); // filling Uri list with stages images
        //    ImageUri.Add(new Uri("pack://application:,,,/Images/" + pictureNewborn));
        //    ImageUri.Add(new Uri("pack://application:,,,/Images/" + pictureChild));
        //    ImageUri.Add(new Uri("pack://application:,,,/Images/" + pictureAdult));
        //    ImageUri.Add(new Uri("pack://application:,,,/Images/" + pictureDead));

        //    PictureUri = ImageUri[0];
        //    Stage = StagesOfLife.newborn;
        //    Health = health;
        //}

        //public Uri GetUri() 
        //{
        //    return PictureUri;
        //}

        public override void Action()
        {
            if ((AskForFood != null) && (stage!=StagesOfLife.adult) )
            {
                if (AskForFood(this, new FoodEventArgs(FoodType, 1))) //change quantity (How much food units this animal needs in it's stage of life?)
                    Age++;
                else
                    Health -= 1;
            }
        }
    }
    
    public class FoodEventArgs : EventArgs
    {
        public ItemType FoodType {get; private set;}

        public int Quantity {get; private set;}

        public FoodEventArgs(ItemType foodType, int quantity)
        {
            FoodType = foodType;
            Quantity = quantity;
        }
    }
}
