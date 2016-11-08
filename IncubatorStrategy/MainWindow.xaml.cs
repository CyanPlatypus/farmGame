using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

namespace IncubatorStrategy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //create dictionary of grids
        //event add to add grid ;
        //event delete to delete particular grid ;
        //how to remove row and col definitions from grid? this way:
        //myGrid.RowDefinitions.RemoveRange(0, myGrid.RowDefinitions.Count - 1);
        //myGrid.ColumnDefinitions.RemoveRange(0, myGrid.ColumnDefinitions.Count -1);
        //
        //delete button fix all
        //write nice info (fixing cost and so on)
        //red diamonds to cyan
        //shop

        Game marvellousGame;
        Timer timer = new Timer(3000);
        Dictionary<int, StackPanel> plantSectionStackPanelDictionary;
        Dictionary<int, StackPanel> animalSectionStackPanelDictionary;
        List<StackPanel> itemStackPanelList;

        public MainWindow()
        {
            InitializeComponent();
            Button butt = new Button();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            marvellousGame = new Game(4, 3);
            plantSectionStackPanelDictionary = new Dictionary<int, StackPanel>();
            animalSectionStackPanelDictionary = new Dictionary<int, StackPanel>();
            itemStackPanelList = new List<StackPanel>();

            DisplayStackPanelList(itemWrapPanel, marvellousGame.ItemsList);
            DisplayStackPanelList(currencyWrapPanel, marvellousGame.CurrencyList);

            PutEverythingIntoShop();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action
                (() =>
                {
                    //AddNewCreatureToEverySection();
                    marvellousGame.Action();
                    UpdateAllItemQuantitiesAndDrawSections();
                }));
        }

        void UpdateAllItemQuantitiesAndDrawSections() 
        {
            UpdateAllItemQuantities();
            DrawSections();
        }

        void DisplayStackPanelList(WrapPanel wPanel, List<Item> iList) //place items and currency on their wrap panels
        {
            StackPanel sP = new StackPanel();
            foreach (Item i in iList)
            {
                sP = new StackPanel() { Orientation = Orientation.Horizontal };
                sP.Orientation = Orientation.Horizontal;

                Image im = new Image() { Source = new BitmapImage(i.GetUri()), Stretch = Stretch.None, ToolTip = Convert.ToString(i.Type) + Environment.NewLine + Convert.ToString(i.Quantity) };
                Label lbl = new Label() { Content = i.Quantity };

                sP.Children.Add(im);
                sP.Children.Add(lbl);

                wPanel.Children.Add(sP);
            }
        }

        void UpdateAllItemQuantities() 
        {
            UpdateItemQuantitiesInList(itemWrapPanel, marvellousGame.ItemsList);
            UpdateItemQuantitiesInList(currencyWrapPanel, marvellousGame.CurrencyList);
        }

        void UpdateItemQuantitiesInList(WrapPanel wPanel, List<Item> iList) 
        {
            for (int i = 0; i < iList.Count; i++)
            {
                ((Label)((StackPanel)wPanel.Children[i]).Children[1]).Content = iList[i].Quantity;
            }
        }

        //void AddSection(int index, string sectionType, Dictionary<int, StackPanel> stPanDict) 
        //{
        //    //main stack panel
        //    StackPanel stPanForGridAndButtons = new StackPanel() { Orientation = Orientation.Vertical };

        //    //grid
        //    Grid currGrid = new Grid();
        //    //currGrid.Name = "_" + Convert.ToString(index) + "_sectionGrid";
        //    //currGrid.MouseDown += new MouseButtonEventHandler(currGrid_MouseDown);

        //    switch (sectionType)
        //    {
        //        case "a":
        //            {
        //                PlaceRowsAndColumns(currGrid, marvellousGame.AnimalSectionDictionary[index].Size);
        //                DrawBGOfTheField(currGrid, marvellousGame.AnimalSectionDictionary[index], index);
        //                break;
        //            }
        //        case "p":
        //            {
        //                PlaceRowsAndColumns(currGrid, marvellousGame.PlantSectionDictionary[index].Size);
        //                DrawBGOfTheField(currGrid, marvellousGame.PlantSectionDictionary[index], index);
        //                break;
        //            }
        //    }

        //    //smaller stack panel for buttons
        //    StackPanel stPanForButtons = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };

        //    //buttons
        //    Button sellAllButton = new Button() { Content = "Sell all", Background = Brushes.Black, Foreground = Brushes.White};
        //    sellAllButton.Name = "_" + Convert.ToString(index) + "_" + sectionType + "_sellAllButton"; //_1_p_sellAllButton
        //    sellAllButton.Click += new RoutedEventHandler(sellAllButton_Click);

        //    Button removeCorpsesButton = new Button() { Content = "Remove corpses", Background = Brushes.Black, Foreground = Brushes.White };

        //    if (sectionType == "p")
        //    {
        //        Button collectAllButton = new Button() { Content = "Collect All", Background = Brushes.Black, Foreground = Brushes.White };
        //        collectAllButton.Name = "_" + Convert.ToString(index) + "_collectAllButton";
        //        collectAllButton.Click += new RoutedEventHandler(collectAllButton_Click);
        //        stPanForButtons.Children.Add(collectAllButton);

        //        Button fixBrokenEquipmentButton = new Button() { Content = "Fix Equipment", Background = Brushes.Black, Foreground = Brushes.White };
        //        fixBrokenEquipmentButton.Name = "_" + Convert.ToString(index) + "_fixBrokenEquipmentButton";
        //        //fixBrokenEquipmentButton.Click += 
        //        stPanForButtons.Children.Add(fixBrokenEquipmentButton);
        //    }
        //    Button moreInfoButton = new Button() { Content = ">", Background = Brushes.Black, Foreground = Brushes.White };

        //    //add all buttons to a smaller stack panel
        //    stPanForButtons.Children.Add(sellAllButton);
        //    //stPanForButtons.Children.Add(collectAllButton);
        //    stPanForButtons.Children.Add(removeCorpsesButton);
        //    //stPanForButtons.Children.Add(fixBrokenEquipmentButton);
        //    stPanForButtons.Children.Add(moreInfoButton);

        //    //add grid and smaller stack panel to bigger stack panel
        //    stPanForGridAndButtons.Children.Add(currGrid);
        //    stPanForGridAndButtons.Children.Add(stPanForButtons);

        //    //add bigger stack panel to the dictionary
        //    stPanDict.Add(index, stPanForGridAndButtons);

        //    //add stack panel to the window
        //    switch (sectionType) 
        //    {
        //        case "a": 
        //            {
        //                creatureWrapPanel.Children.Add(stPanForGridAndButtons);
        //                break;
        //            }
        //        case "p":
        //            {
        //                sectionGrid.Children.Add(stPanForGridAndButtons);
        //                break;
        //            }
        //    }
        //}

        void AddNewPlantSection(PlantCreatureType type) //add section: stack panel (with grid and stack panel (with buttons))
        {
            int index = marvellousGame.AddPlantSection(type); //add section
            if (index != -1)
            {
                //main stack panel
                StackPanel stPanForGridAndButtons = new StackPanel() { Orientation = Orientation.Vertical };

                //grid
                Grid currGrid = new Grid();
                //currGrid.Name = "_" + Convert.ToString(index) + "_sectionGrid";
                //currGrid.MouseDown += new MouseButtonEventHandler(currGrid_MouseDown);
                PlaceRowsAndColumns(currGrid, marvellousGame.PlantSectionDictionary[index].Size);
                DrawBGOfTheField(currGrid, marvellousGame.PlantSectionDictionary[index], index);

                //smaller stack panel for buttons
                StackPanel secPanForButtons = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };

                //buttons
                Button sellAllButton = new Button() { Content = "Sell all", Background = Brushes.Black, Foreground = Brushes.White  };
                sellAllButton.Name = "_" + Convert.ToString(index) + "_p_sellAllButton"; //_1_p_sellAllButton
                sellAllButton.Click += new RoutedEventHandler(sellAllButton_Click);

                Button collectAllButton = new Button() { Content = "Collect All", Background = Brushes.Black, Foreground = Brushes.White };
                collectAllButton.Name = "_" + Convert.ToString(index) + "_collectAllButton";
                collectAllButton.Click += new RoutedEventHandler(collectAllButton_Click);

                //Button fixBrokenEquipmentButton = new Button() { Content = "Fix Equipment", Background = Brushes.Black, Foreground = Brushes.White };
                //fixBrokenEquipmentButton.Name = "_" + Convert.ToString(index) + "_fixBrokenEquipmentButton";
                ////fixBrokenEquipmentButton.Click += 

                Button removeCorpsesButton = new Button() { Content = "Remove corpses", Background = Brushes.Black, Foreground = Brushes.White };
                removeCorpsesButton.Name = "_" + Convert.ToString(index) + "_p_removeCorpsesButton";
                removeCorpsesButton.Click += new RoutedEventHandler(removeCorpsesButton_Click);

                //Button moreInfoButton = new Button() { Content = ">", Background = Brushes.Black, Foreground = Brushes.White };

                //add all buttons to a smaller stack panel
                secPanForButtons.Children.Add(sellAllButton);
                secPanForButtons.Children.Add(collectAllButton);
                secPanForButtons.Children.Add(removeCorpsesButton);
                //secPanForButtons.Children.Add(fixBrokenEquipmentButton);
                //secPanForButtons.Children.Add(moreInfoButton);

                //add grid and smaller stack panel to bigger stack panel
                stPanForGridAndButtons.Children.Add(currGrid);
                stPanForGridAndButtons.Children.Add(secPanForButtons);

                //add bigger stack panel to the dictionary
                plantSectionStackPanelDictionary.Add(index, stPanForGridAndButtons);

                //add stack panel to the window
                sectionGrid.Children.Add(stPanForGridAndButtons);
            }
        }

        void removeCorpsesButton_Click(object sender, RoutedEventArgs e)
        {
            int index = FunctionsAndConstants.GetNumberFromName(((Button)sender).Name);
            string sectionType = FunctionsAndConstants.GetSectionTypeFromName(((Button)sender).Name);
            marvellousGame.RemoveAllCorpses (sectionType, index);
            UpdateAllItemQuantitiesAndDrawSections();
        }

        void collectAllButton_Click(object sender, RoutedEventArgs e)
        {
            int index = FunctionsAndConstants.GetNumberFromName(((Button)sender).Name);
            marvellousGame.CollectAllAdultCreaturesInSection(index);
            UpdateAllItemQuantitiesAndDrawSections();
        }

        void sellAllButton_Click(object sender, RoutedEventArgs e)
        {
            int index = FunctionsAndConstants.GetNumberFromName(((Button)sender).Name);
            string sectionType = FunctionsAndConstants.GetSectionTypeFromName(((Button)sender).Name);
            marvellousGame.SellAllAdultCreaturesInSection(sectionType, index);
            UpdateAllItemQuantitiesAndDrawSections();
        }

        void AddNewAnimalSection(AnimalCreatureType type) //add section: stack panel (with grid and stack panel (with buttons))
        {
            int index = marvellousGame.AddAnimalSection(type); //add section

            if (index != -1)
            {
                //main stack panel
                StackPanel stPanForGridAndButtons = new StackPanel() { Orientation = Orientation.Vertical };

                //grid
                Grid currGrid = new Grid();
                //currGrid.Name = "_" + Convert.ToString(index) + "_sectionGrid";
                //currGrid.MouseDown += new MouseButtonEventHandler(currGrid_MouseDown);
                PlaceRowsAndColumns(currGrid, marvellousGame.AnimalSectionDictionary[index].Size);
                DrawBGOfTheField(currGrid, marvellousGame.AnimalSectionDictionary[index], index);

                //smaller stack panel for buttons
                StackPanel stPanForButtons = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };

                //buttons
                Button sellAllButton = new Button() { Content = "Sell all", Background = Brushes.Black, Foreground = Brushes.White };
                sellAllButton.Name = "_" + Convert.ToString(index) + "_a_sellAllButton"; //_1_p_sellAllButton
                sellAllButton.Click += new RoutedEventHandler(sellAllButton_Click);

                Button removeCorpsesButton = new Button() { Content = "Remove corpses", Background = Brushes.Black, Foreground = Brushes.White };
                removeCorpsesButton.Name = "_" + Convert.ToString(index) + "_a_removeCorpsesButton";
                removeCorpsesButton.Click += new RoutedEventHandler(removeCorpsesButton_Click);

                //Button moreInfoButton = new Button() { Content = ">", Background = Brushes.Black, Foreground = Brushes.White };

                //add all buttons to a smaller stack panel
                stPanForButtons.Children.Add(sellAllButton);
                //stPanForButtons.Children.Add(collectAllButton);
                stPanForButtons.Children.Add(removeCorpsesButton);
                //stPanForButtons.Children.Add(moreInfoButton);

                //add grid and smaller stack panel to bigger stack panel
                stPanForGridAndButtons.Children.Add(currGrid);
                stPanForGridAndButtons.Children.Add(stPanForButtons);

                //add bigger stack panel to the dictionary
                animalSectionStackPanelDictionary.Add(index, stPanForGridAndButtons);

                //add stack panel to the window
                creatureWrapPanel.Children.Add(stPanForGridAndButtons);
            }
        }

        //void currGrid_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    int index = FunctionsAndConstants.GetNumberFromName(((Grid)sender).Name);
        //    marvellousGame.SectionList[index].Add();
        //    //var myKey = gridDictionary.FirstOrDefault(x => ((Grid)sender).Equals(x.Value)).Key;
        //    //if (gridDictionary.ContainsKey(myKey)) 
        //    //{
        //    //    marvellousGame.SectionList.Remove(myKey);
        //    //    gridDictionary[myKey].Children.RemoveRange(0, gridDictionary[myKey].Children.Count);
        //    //    gridDictionary.Remove(myKey);
        //    //}
        //    AddNewCreatureToEverySection();
        //    DrawSections();
        //}

        void DeleteSection() 
        {
        }

        void DrawSections() 
        {
            foreach (var gr in plantSectionStackPanelDictionary) //for each stack panel in dictionary
            {
                DrawEntities((Grid)gr.Value.Children[0], marvellousGame.PlantSectionDictionary[gr.Key], gr.Key);
            }
            foreach (var gr in animalSectionStackPanelDictionary) //for each stack panel in dictionary
            {
                DrawEntities((Grid)gr.Value.Children[0], marvellousGame.AnimalSectionDictionary[gr.Key], gr.Key);
            }
        }

        void PlaceRowsAndColumns(Grid currentGrid, Point size)
        {
            for (int i = 0; i < size.X; i++)
            {
                currentGrid.RowDefinitions.Add(new RowDefinition());
                //currentGrid.RowDefinitions[i].Height = new System.Windows.GridLength(16);
            }
            for (int j = 0; j < size.Y; j++)
            {
                currentGrid.ColumnDefinitions.Add(new ColumnDefinition());
                //currentGrid.ColumnDefinitions[j].Width = new System.Windows.GridLength(16);
            }

        }

        void DrawBGOfTheField(Grid gridToPlaceOn, Section section, int index) 
        {
            string sectionType = (section is AnimalSection)? "a":"p";

            for (int x = 0; x < section.Size.X; x++)
            {
                for (int y = 0; y < section.Size.Y; y++)
                {
                    Image img = new Image();
                    img.Name = "_" + Convert.ToString(index) + "_" + sectionType +"_entityImage"; //_1_a_entityImage
                    img.MouseUp += new MouseButtonEventHandler(bgImg_MouseUp);
                    //Alert!
                    //U gotta solve this issue with clicking later
                    //img.Click += new RoutedEventHandler(btn_Click);

                    //this.SizeToContent = SizeToContent.WidthAndHeight;

                    //img.Height = 32;
                    //img.Width = 32;
                    img.Source = new BitmapImage(section.FloorPictureUri);
                    img.Stretch = Stretch.None;

                    Grid.SetColumn(img, y);
                    Grid.SetRow(img, x);
                    gridToPlaceOn.Children.Add(img);
                }
            }

            if (section is PlantSection)
            {

                foreach (var p in ((PlantSection)section).PositionsForEquipment)
                {
                    ((Image)gridToPlaceOn.Children[(int)section.Size.Y * (int)p.X + (int)p.Y]).Source = new BitmapImage(((PlantSection)section).WallPictureUri);
                }

                foreach (var p in section.Entities)
                {
                    //gridToPlaceOn.Children[(int)marvellousGame.hS.Size.Y * (int)p.X + (int)p.Y] 
                    //gridToPlaceOn.ColumnDefinitions.

                    if (p.Value is Equipment)
                    {
                        Image img = new Image();
                        //Alert!
                        //U gotta solve this issue with clicking later
                        //img.MouseUp += new MouseButtonEventHandler(equipmentImg_MouseUp);

                        //this.SizeToContent = SizeToContent.WidthAndHeight;

                        //img.Height = 32;
                        //img.Width = 32;
                        img.Source = new BitmapImage(((Equipment)p.Value).PictureUri);
                        img.Stretch = Stretch.None;

                        Grid.SetColumn(img, (int)p.Key.Y);
                        Grid.SetRow(img, (int)p.Key.X);
                        gridToPlaceOn.Children.Add(img);
                    }
                }
            }
        }

        void DrawEntities(Grid whereToDraw, Section section, int index)//draw equipment and creatures in section
        {
            //delete everything but BG
            int tmp = (int)(section.Size.X*section.Size.Y);
            whereToDraw.Children.RemoveRange(tmp, whereToDraw.Children.Count - tmp);

            foreach (var p in section.Entities)
            {
                Image img = new Image();
                img.Name = "_" + index;

                //img.Height = 32;
                //img.Width = 32;
                img.Source = new BitmapImage(((IUri)p.Value).GetUri());
                img.Stretch = Stretch.None;
                if (p.Value is Creature)
                    img.ToolTip = Convert.ToString(((Creature)p.Value).Stage) + Environment.NewLine + "hp " + ((Creature)p.Value).Health + Environment.NewLine + ((Creature)p.Value).Age + " y.o.";
                if (p.Value is Equipment)
                {
                    img.ToolTip = ((Equipment)p.Value).Type + Environment.NewLine + ((Equipment)p.Value).PercentageOfBreaking + "%" + Environment.NewLine + ((Equipment)p.Value).Status;
                    img.MouseUp += new MouseButtonEventHandler(equipmentImg_MouseUp);
                }
                Grid.SetColumn(img, (int)p.Key.Y);
                Grid.SetRow(img, (int)p.Key.X);
                whereToDraw.Children.Add(img);
            }
        }

        void bgImg_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int index = FunctionsAndConstants.GetNumberFromName(((Image)sender).Name);
            string sectionType = FunctionsAndConstants.GetSectionTypeFromName(((Image)sender).Name);
            marvellousGame.AddCreatureIntoSection(sectionType, index, new Point(Grid.GetRow(((Image)sender)), Grid.GetColumn(((Image)sender))));
            UpdateAllItemQuantitiesAndDrawSections();
        }

        void equipmentImg_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int index = FunctionsAndConstants.GetNumberFromName(((Image)sender).Name);
            marvellousGame.FixEquipment(index, new Point(Grid.GetRow(((Image)sender)), Grid.GetColumn(((Image)sender))));
            UpdateAllItemQuantitiesAndDrawSections();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Up)
            //{
            //    AddNewPlantSection(PlantCreatureType.CarrotPlant);
            //    AddNewAnimalSection(AnimalCreatureType.Rabbit);
            //}
            //if (e.Key == Key.Down)
            //{
            //    AddNewPlantSection(PlantCreatureType.HellFlowerPlant);
            //    AddNewAnimalSection(AnimalCreatureType.HellEyeAnimal);
            //}
            //UpdateAllItemQuantitiesAndDrawSections();
            //timer.Start();
        }

        void PutEverythingIntoShop() 
        {
            PutItIn(plantSectionsStackPanelTabItem, new Uri("pack://application:,,,/Images/hellPlantBig.png"),
                FunctionsAndConstants.buyingPlantSectionPrise[PlantCreatureType.HellFlowerPlant].Item1, new Uri("pack://application:,,,/Images/cyanDiamond.png"),
                FunctionsAndConstants.buyingPlantCreaturePrise[PlantCreatureType.HellFlowerPlant].Item1, new Uri("pack://application:,,,/Images/cyanDiamond.png"), buyHellPlantSection);
            PutItIn(plantSectionsStackPanelTabItem, new Uri("pack://application:,,,/Images/carrotPlantBig.png"),
                FunctionsAndConstants.buyingPlantSectionPrise[PlantCreatureType.CarrotPlant].Item1, new Uri("pack://application:,,,/Images/cyanDiamond.png"),
                FunctionsAndConstants.buyingPlantCreaturePrise[PlantCreatureType.CarrotPlant].Item1, new Uri("pack://application:,,,/Images/cyanDiamond.png"), buyCarrotPlantSection);
            PutItIn(animalSectionsStackPanelTabItem, new Uri("pack://application:,,,/Images/eyeBig.png"),
                FunctionsAndConstants.buyingAnimalSectionPrise[AnimalCreatureType.HellEyeAnimal].Item1, new Uri("pack://application:,,,/Images/cyanDiamond.png"),
                FunctionsAndConstants.buyingAnimalCreaturePrise[AnimalCreatureType.HellEyeAnimal].Item1, new Uri("pack://application:,,,/Images/cyanDiamond.png"), buyHellAnimalSection);
            PutItIn(animalSectionsStackPanelTabItem, new Uri("pack://application:,,,/Images/rabbitBig.png"),
                FunctionsAndConstants.buyingAnimalSectionPrise[AnimalCreatureType.Rabbit].Item1, new Uri("pack://application:,,,/Images/cyanDiamond.png"),
                FunctionsAndConstants.buyingAnimalCreaturePrise[AnimalCreatureType.Rabbit].Item1, new Uri("pack://application:,,,/Images/cyanDiamond.png"), buyRabbitAnimalSection);
            
        }

        void PutItIn(StackPanel sP, Uri sectionUri, int sectionPrice, Uri sectionPriceURI, int creaturePrice, Uri creaturePriceURI, RoutedEventHandler rEH) 
        {
            StackPanel bigSP = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };
            Image sectionImg = new Image() { Source = new BitmapImage(sectionUri), Height = 26, Width = 26, Margin = new Thickness(5) };

            TextBlock tBSectioPrice = new TextBlock() { Text = "Price of the section " + sectionPrice };
            Image sectionPriceImg = new Image() { Source = new BitmapImage(sectionPriceURI), Height = 18, Width = 18};
            StackPanel sectionPriceSP = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(2) };
            sectionPriceSP.Children.Add(tBSectioPrice);
            sectionPriceSP.Children.Add(sectionPriceImg);

            TextBlock tBCreaturePrice = new TextBlock() { Text = "Price of the creature " + creaturePrice };
            Image creaturePriceImg = new Image() { Source = new BitmapImage(creaturePriceURI), Height = 18, Width = 18 };
            StackPanel creaturePriceSP = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(2)};
            creaturePriceSP.Children.Add(tBCreaturePrice);
            creaturePriceSP.Children.Add(creaturePriceImg);

            Button bt = new Button {Content = "Buy" };
            bt.Click += rEH;

            StackPanel smallSP = new StackPanel() { Orientation = Orientation.Vertical };
            smallSP.Children.Add(sectionPriceSP);
            smallSP.Children.Add(creaturePriceSP);
            smallSP.Children.Add(bt);

            bigSP.Children.Add(sectionImg);
            bigSP.Children.Add(smallSP);

            sP.Children.Add(bigSP);

        }

        void buyHellPlantSection(object sender, RoutedEventArgs e)
        {
            AddNewPlantSection(PlantCreatureType.HellFlowerPlant);
            UpdateAllItemQuantitiesAndDrawSections();
            if (!timer.Enabled)
                timer.Start();
        }

        void buyCarrotPlantSection(object sender, RoutedEventArgs e)
        {
            AddNewPlantSection(PlantCreatureType.CarrotPlant);
            UpdateAllItemQuantitiesAndDrawSections();
            if (!timer.Enabled)
                timer.Start();
        }

        void buyHellAnimalSection(object sender, RoutedEventArgs e)
        {
            AddNewAnimalSection(AnimalCreatureType.HellEyeAnimal);
            UpdateAllItemQuantitiesAndDrawSections();
            if (!timer.Enabled)
                timer.Start();
        }
        void buyRabbitAnimalSection(object sender, RoutedEventArgs e)
        {
            AddNewAnimalSection(AnimalCreatureType.Rabbit);
            UpdateAllItemQuantitiesAndDrawSections();
            if (!timer.Enabled)
                timer.Start();
        }
    }
}
