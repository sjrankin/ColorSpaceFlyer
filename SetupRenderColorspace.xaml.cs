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
using System.Windows.Shapes;
using System.ComponentModel;
using System.IO;

namespace ColorSpaceFlyer
{
    /// <summary>
    /// Interaction logic for SetupRenderColorspace.xaml
    /// </summary>
    public partial class SetupRenderColorspace : Window
    {
        public SetupRenderColorspace ()
        {
            InitializeComponent();
            SetStatusText("");
            DurationBlock.Text = "";
        }

        private void SetStatusText (string StatusText)
        {
            if (!RenderStatus.Dispatcher.CheckAccess())
            {
                RenderStatus.Dispatcher.Invoke
                    (
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action
                    (
                        delegate ()
                        {
                            RenderStatus.Text = StatusText;
                        }
                        )
                    );
            }
            else
                RenderStatus.Text = StatusText;
        }

        private Nullable<int> IntFromTextBox (TextBox TB)
        {
            if (TB == null)
                throw new ArgumentNullException("TB");
            string Raw = TB.Text;
            int IValue = 0;
            if (!int.TryParse(Raw, out IValue))
                return null;
            return IValue;
        }

        private void PopulateRenderData (ref RenderData RD)
        {
            if (RD == null)
                throw new ArgumentNullException("RD");
            ComboBoxItem CSI = ColorSpaceCombo.SelectedItem as ComboBoxItem;
            if (CSI == null)
            {
                RD = null;
                return;
            }
            RD.ColorSpaceName = CSI.Content as string;
            RD.AxisOrder = GeneralAxisDefintions._123;  //for now
            RD.Overwrite = OverwriteOldFiles.IsChecked.Value;
            Nullable<int> IValue = IntFromTextBox(FrameWidth);
            if (!IValue.HasValue)
                IValue = 512;
            RD.FrameWidth = IValue.Value;
            if (RD.FrameWidth < 1)
                throw new InvalidOperationException("Frame width too small.");
            IValue = IntFromTextBox(FrameHeight);
            if (!IValue.HasValue)
                IValue = 512;
            RD.FrameHeight = IValue.Value;
            if (RD.FrameHeight < 1)
                throw new InvalidOperationException("Frame height too small.");
            IValue = IntFromTextBox(FrameCount);
            if (!IValue.HasValue)
                IValue = 256;
            if (RD.ColorSpaceName == "HSV")
                RD.FrameCount = (int)(Math.Min(IValue.Value, 360));
            else
                RD.FrameCount = (int)(Math.Min(IValue.Value, 256));
            RD.FrameCount = (int)(Math.Max(0, RD.FrameCount));
        }

        private RenderData RD;

        private void RenderFramesHandler (object Sender, RoutedEventArgs e)
        {
            RD = new RenderData();
            PopulateRenderData(ref RD);
            if (RD == null)
            {
                SetStatusText("Error gathering render data.");
                return;
            }

            RenderStart = DateTime.Now;
            DurationBlock.Text = "";
            BackgroundWorker BW = new BackgroundWorker();
            BW.WorkerReportsProgress = true;
            BW.DoWork += FrameRendering;
            BW.RunWorkerCompleted += FrameRenderingCompleted;
            BW.ProgressChanged += HandleRenderProgressChanged;
            RenderButton.IsEnabled = false;
            RD.RenderSurface = new Image();
            RD.RenderSurface.Width = RD.FrameWidth;
            RD.RenderSurface.Height = RD.FrameHeight;
            BW.RunWorkerAsync(RD);
        }

        private DateTime RenderStart;

        private void HandleRenderProgressChanged (object Sender, ProgressChangedEventArgs e)
        {
            Image R = e.UserState as Image;
            int Count = e.ProgressPercentage;
            string FileName = Count.ToString("D3") + "_" + RD.ColorSpaceName + RD.AxisOrder.ToString() + ".png";
            var PngEncoder = new PngBitmapEncoder();
            PngEncoder.Frames.Add(BitmapFrame.Create((BitmapSource)RD.RenderSurface.Source));
            using (FileStream stream = new FileStream(FileName, FileMode.Create))
                PngEncoder.Save(stream);
            TimeSpan RenderDuration = DateTime.Now - RenderStart;
            DurationBlock.Text = RenderDuration.ToString(@"hh\:mm\:ss");
        }

        private void FrameRenderingCompleted (object Sender, RunWorkerCompletedEventArgs e)
        {
            SetStatusText("Rendering completed.");
            RenderButton.IsEnabled = true;
        }

        private void FrameRendering (object Sender, DoWorkEventArgs e)
        {
            BackgroundWorker BW = Sender as BackgroundWorker;
            RenderData RD = e.Argument as RenderData;
            if (RD == null)
            {
                return;
            }

            PlaneRenderer PR = new PlaneRenderer();
            for (int i = 0; i < RD.FrameCount; i++)
            {
                SetStatusText("Rendering frame " + (i + 1).ToString() + " of " + RD.FrameCount.ToString());
                switch (RD.ColorSpaceName)
                {
                    case "RGB":
                        PR.RenderRGBPlane(RGBAxisDefinitions.RGB, i, RD.RenderSurface, RD.FrameWidth, RD.FrameHeight);
                        break;

                    case "CMY":
                        PR.RenderCMYPlane(CMYAxisDefinitions.CMY, i, RD.RenderSurface, RD.FrameWidth, RD.FrameHeight);
                        break;

                    case "HSV":
                        PR.RenderHSVPlane(HSVAxisDefinitions.HSV, i, RD.RenderSurface, RD.FrameWidth, RD.FrameHeight);
                        break;
                }
                BW.ReportProgress(i, RD.RenderSurface);
#if false
                string FileName = i.ToString("D3") + "_" + RD.ColorSpaceName + RD.AxisOrder.ToString();
                var PngEncoder = new PngBitmapEncoder();
                PngEncoder.Frames.Add(BitmapFrame.Create((BitmapSource)RD.RenderSurface.Source));
                using (FileStream stream = new FileStream(FileName, FileMode.Create))
                    PngEncoder.Save(stream);
#endif
            }
        }

        private void CloseHandler (object Sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HandleColorSpaceChanged (object Sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = Sender as ComboBox;
            if (CB == null)
                return;
            ComboBoxItem CBI = CB.SelectedItem as ComboBoxItem;
            if (CBI == null)
                return;
            string ColorSpaceName = (string)CBI.Tag;
            UpdateAxisOrders(ColorSpaceName);
        }

        private void UpdateAxisOrders (string ColorSpaceName)
        {
            if (AxisOrderCombo == null)
                return;
            List<string> OrderNames = new List<string>();
            switch (ColorSpaceName)
            {
                case "RGB":
                    OrderNames.Add("RGB");
                    OrderNames.Add("RBG");
                    OrderNames.Add("GRB");
                    OrderNames.Add("GBR");
                    OrderNames.Add("BRG");
                    OrderNames.Add("BGR");
                    break;

                case "HSV":
                    OrderNames.Add("HSV");
                    OrderNames.Add("HVS");
                    OrderNames.Add("SHV");
                    OrderNames.Add("SVH");
                    OrderNames.Add("VHS");
                    OrderNames.Add("VSH");
                    break;

                case "CMY":
                    OrderNames.Add("CMY");
                    OrderNames.Add("CYM");
                    OrderNames.Add("MCY");
                    OrderNames.Add("MYC");
                    OrderNames.Add("YCM");
                    OrderNames.Add("YMC");
                    break;
            }

            for (int i = 0; i < 6; i++)
            {
                ((ComboBoxItem)AxisOrderCombo.Items[i]).Content = OrderNames[i];
            }
        }

        private void HandleAllFramesCheckChanged (object Sender, RoutedEventArgs e)
        {
            CheckBox CB = Sender as CheckBox;
            if (CB == null)
                return;
            if (CB.IsChecked.Value)
            {
                FrameCount.Text = "256";
            }
        }
    }

    public class RenderData
    {
        public RenderData ()
        {
            FrameCount = 256;
            FrameWidth = 512;
            FrameHeight = 512;
            Overwrite = true;
            ColorSpaceName = "RGB";
            RenderSurface = null;
            AxisOrder = GeneralAxisDefintions._123;
        }

        public int FrameCount { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public bool Overwrite { get; set; }
        public string ColorSpaceName { get; set; }
        public GeneralAxisDefintions AxisOrder { get; set; }
        public Image RenderSurface { get; set; }
    }
}
