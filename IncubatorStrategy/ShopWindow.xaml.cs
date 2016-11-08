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
using System.Windows.Shapes;

namespace IncubatorStrategy
{
    /// <summary>
    /// Логика взаимодействия для ShopWindow.xaml
    /// </summary>
    public partial class ShopWindow : Window
    {
        Game game;
        Dictionary<PlantCreatureType,StackPanel> stackPanelDictionaryForSections;

        internal ShopWindow(ref Game game)
        {
            InitializeComponent();
            this.game = game;
            stackPanelDictionaryForSections = new Dictionary<PlantCreatureType, StackPanel>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        //void FillStackPanelDictionaryForSections() 
        //{
        //    foreach (CreatureType c in Enum.GetValues(typeof(CreatureType))) 
        //    {
        //        StackPanel sP = new StackPanel(){
        //    }
        //}
    }
}
