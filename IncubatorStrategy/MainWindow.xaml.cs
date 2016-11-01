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
        //add extra row with buttons

        Game marvellousGame;
        Timer timer = new Timer(3000);
        Dictionary<int, StackPanel> sectionStackPanelDictionary;
        List<StackPanel> itemStackPanelList;

        public MainWindow()
        {
            InitializeComponent();
            Button butt = new Button();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            marvellousGame = new Game(4);
            sectionStackPanelDictionary = new Dictionary<int, StackPanel>();
            itemStackPanelList = new List<StackPanel>();

            DisplayStackPanelList(itemWrapPanel, marvellousGame.ItemsList);
            DisplayStackPanelList(currencyWrapPanel, marvellousGame.CurrencyList);
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

        void AddNewSection(CreatureType type) //add section: stack panel (with grid and stack panel (with buttons))
        {
            int index = marvellousGame.Add(type); //add section
            if (index != -1)
            {
                //main stack panel
                StackPanel stPanForGridAndButtons = new StackPanel() { Orientation = Orientation.Vertical };
                
                //grid
                Grid currGrid = new Grid();
                //currGrid.Name = "_" + Convert.ToString(index) + "_sectionGrid";
                //currGrid.MouseDown += new MouseButtonEventHandler(currGrid_MouseDown);
                PlaceRowsAndColumns(currGrid, marvellousGame.SectionDictionary[index].Size);
                DrawBGOfTheField(currGrid, marvellousGame.SectionDictionary[index], index);

                //smaller stack panel for buttons
                StackPanel secPanForButtons = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };

                //buttons
                Button sellAllButton = new Button() { Content = "Sell all" };
                sellAllButton.Name = "_"+Convert.ToString(index)+ "_sellAllButton";
                sellAllButton.Click += new RoutedEventHandler(sellAllButton_Click);
                
                Button collectAllButton = new Button() { Content = "Collect All" };
                Button removeCorpsesButton = new Button() { Content = "Remove corpses" };
                Button moreInfoButton = new Button() { Content = ">" };

                //add all buttons to a smaller stack panel
                secPanForButtons.Children.Add(sellAllButton);
                secPanForButtons.Children.Add(collectAllButton);
                secPanForButtons.Children.Add(removeCorpsesButton);
                secPanForButtons.Children.Add(moreInfoButton);

                //add grid and smaller stack panel to bigger stack panel
                stPanForGridAndButtons.Children.Add(currGrid);
                stPanForGridAndButtons.Children.Add(secPanForButtons);

                //add bigger stack panel to the dictionary
                sectionStackPanelDictionary.Add(index, stPanForGridAndButtons);

                //add stack panel to the window
                sectionGrid.Children.Add(stPanForGridAndButtons);
            }
        }

        void sellAllButton_Click(object sender, RoutedEventArgs e)
        {
            int index = FunctionsAndConstants.GetNumberFromName(((Button)sender).Name);
            marvellousGame.SellAllAdultCreaturesInSection(index);
            UpdateAllItemQuantitiesAndDrawSections();
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

        //void AddNewCreatureToEverySection() 
        //{
        //    foreach (var c in marvellousGame.SectionDictionary) 
        //    {
        //        c.Value.Add(new Point(Section.rnd.Next(1, 9), Section.rnd.Next(1, 9)));
        //    }
        //}

        void DrawSections() 
        {
            foreach (var gr in sectionStackPanelDictionary) //for each stack panel in dictionary
            {
                DrawEntities((Grid)gr.Value.Children[0], marvellousGame.SectionDictionary[gr.Key], gr.Key);
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
            for (int x = 0; x < section.Size.X; x++)
            {
                for (int y = 0; y < section.Size.Y; y++)
                {
                    Image img = new Image();
                    img.Name = "_" + Convert.ToString(index) + "_entityImage";
                    img.MouseUp += new MouseButtonEventHandler(img_MouseUp);
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

            foreach (var p in section.PositionsForEquipment) 
            {
                ((Image)gridToPlaceOn.Children[(int)section.Size.Y * (int)p.X + (int)p.Y]).Source = new BitmapImage(section.WallPictureUri);
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
                    //img.Click += new RoutedEventHandler(btn_Click);

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

        void DrawEntities(Grid whereToDraw, Section section, int index)//draw equipment and creatures in section
        {
            //delete everything but BG
            int tmp = (int)(section.Size.X*section.Size.Y);
            whereToDraw.Children.RemoveRange(tmp, whereToDraw.Children.Count - tmp);

            foreach (var p in section.Entities)
            {
                Image img = new Image();
                //Alert!
                //U gotta solve this issue with clicking later
                //img.Click += new RoutedEventHandler(btn_Click);

                //img.Height = 32;
                //img.Width = 32;
                img.Source = new BitmapImage(((IUri)p.Value).GetUri());
                img.Stretch = Stretch.None;
                if (p.Value is Creature)
                    img.ToolTip = ((Creature)p.Value).Health + Environment.NewLine + ((Creature)p.Value).Age;
                if (p.Value is Equipment)
                    img.ToolTip = ((Equipment)p.Value).PercentageOfBreaking + Environment.NewLine + ((Equipment)p.Value).pubVarRandV;
                Grid.SetColumn(img, (int)p.Key.Y);
                Grid.SetRow(img, (int)p.Key.X);
                whereToDraw.Children.Add(img);
            }
        }

        void img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int index = FunctionsAndConstants.GetNumberFromName(((Image)sender).Name);
            marvellousGame.AddCreatureIntoSection(index, new Point(Grid.GetRow(((Image)sender)), Grid.GetColumn(((Image)sender))));
            UpdateAllItemQuantitiesAndDrawSections();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            AddNewSection(CreatureType.HellEyeCreature);
            timer.Start();
        }
    }
}
