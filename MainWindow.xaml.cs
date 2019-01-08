using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColorSpaceFlyer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow ()
        {
            InitializeComponent();
            WriteStatusLine("");
        }

        private void StartFlying ()
        {
        }

        private void StopFlying ()
        {
        }

        private void MenuProgramExit (object Sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HandleAxisClick (object Sender, RoutedEventArgs e)
        {
            MenuItem MI = Sender as MenuItem;
            if (MI == null)
                return;
            RGB.IsChecked = false;
            RBG.IsChecked = false;
            GRB.IsChecked = false;
            GBR.IsChecked = false;
            BRG.IsChecked = false;
            BGR.IsChecked = false;
            MI.IsChecked = true;
        }

        private void HandleAxisIncreaseCheck (object Sender, RoutedEventArgs e)
        {
            MenuItem MI = Sender as MenuItem;
            if (MI == null)
                return;
            MI.IsChecked = !MI.IsChecked;
        }

        private void HandleToggleFlyState (object Sender, RoutedEventArgs e)
        {
            MenuItem MI = Sender as MenuItem;
            if (MI == null)
                return;
            ToggleCheck(MI);
            if (MenuItemIsChecked(MI))
                StartFlying();
            else
                StopFlying();
        }

        private void ToggleCheck (MenuItem Item)
        {
            if (Item == null)
                throw new ArgumentNullException("Item");
            Item.IsChecked = !Item.IsChecked;
        }

        private bool MenuItemIsChecked (MenuItem Item)
        {
            if (Item == null)
                throw new ArgumentNullException("Item");
            return Item.IsChecked;
        }

        private void HandleTestRender (object Sender, RoutedEventArgs e)
        {
            PlaneRenderer PR = new PlaneRenderer();
            //PR.RenderRGBPlane(RGBAxisDefinitions.RGB, 200, OutImage, ImageContainer.ActualWidth, ImageContainer.ActualHeight);
            //            PR.RenderCMYPlane(CMYAxisDefinitions.CMY, 200);
            PR.RenderHSVPlane(HSVAxisDefinitions.HSV, 255,OutImage, ImageContainer.ActualWidth, ImageContainer.ActualHeight);
        }

        private void WriteStatusLine (string StatusString)
        {
            StatusLine.Text = StatusString;
        }

        private void HandleColorSpaceCheck (object Sender, RoutedEventArgs e)
        {
            MenuItem MI = Sender as MenuItem;
            if (MI == null)
                return;
            ToggleCheck(MI);
        }

        private void BatchRenderColorspace (object Sender, RoutedEventArgs e)
        {
            SetupRenderColorspace SRC = new SetupRenderColorspace();
            SRC.ShowDialog();
        }
    }
}
