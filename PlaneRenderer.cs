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
    public class PlaneRenderer
    {
        public void RenderHSVPlane (HSVAxisDefinitions AxisLayout, int Z, Image OutImage, double SurfaceWidth, double SurfaceHeight)
        {
            List<ColorBlenderInterface.ColorBlock> ColorBlocks = new List<ColorBlenderInterface.ColorBlock>();
            ColorBlenderInterface cbi = new ColorBlenderInterface();
            int FinalStride = (int)SurfaceWidth * 4;
            byte[] PixelMap = new byte[(int)SurfaceHeight * FinalStride];
            int CellWidth = (int)SurfaceWidth / 256;
            int CellHeight = (int)SurfaceHeight / 256;
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    double H = (double)Z / (double)360.0;
                    double S = (double)y / (double)255.0;
                    double V = (double)x / (double)255.0;
                    Color CellColor = GetHSVColorAt(H, S, V, AxisLayout);
                    ColorBlenderInterface.ColorBlock CB = new ColorBlenderInterface.ColorBlock();
                    CB.Left = x * CellWidth;
                    CB.Top = y * CellHeight;
                    CB.Width = CellWidth;
                    CB.Height = CellHeight;
                    CB.BlockColor = CellColor.ToBGRA();
                    ColorBlocks.Add(CB);
                }
            }

            cbi.DrawColorBlocks(ref PixelMap, (int)SurfaceWidth, (int)SurfaceHeight, FinalStride, ColorBlocks, Colors.Transparent);
            if (!OutImage.Dispatcher.CheckAccess())
            {
                OutImage.Dispatcher.Invoke
                    (
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action
                    (
                        delegate ()
                        {
                            OutImage.Source = BitmapSource.Create((int)SurfaceWidth, (int)SurfaceHeight, 96.0, 96.0, PixelFormats.Bgra32,
                               null, PixelMap, FinalStride);
                        }
                )
                );
            }
            else
                OutImage.Source = BitmapSource.Create((int)SurfaceWidth, (int)SurfaceHeight, 96.0, 96.0, PixelFormats.Bgra32,
                   null, PixelMap, FinalStride);
        }

        public void RenderCMYPlane (CMYAxisDefinitions AxisLayout, int Z, Image OutImage, double SurfaceWidth, double SurfaceHeight)
        {
            List<ColorBlenderInterface.ColorBlock> ColorBlocks = new List<ColorBlenderInterface.ColorBlock>();
            ColorBlenderInterface cbi = new ColorBlenderInterface();
            int FinalStride = (int)SurfaceWidth * 4;
            byte[] PixelMap = new byte[(int)SurfaceHeight * FinalStride];
            int CellWidth = (int)SurfaceWidth / 256;
            int CellHeight = (int)SurfaceHeight / 256;
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    Color CellColor = GetCMYColorAt(x, y, Z, AxisLayout);
                    ColorBlenderInterface.ColorBlock CB = new ColorBlenderInterface.ColorBlock();
                    CB.Left = x * CellWidth;
                    CB.Top = y * CellHeight;
                    CB.Width = CellWidth;
                    CB.Height = CellHeight;
                    CB.BlockColor = CellColor.ToBGRA();
                    ColorBlocks.Add(CB);
                }
            }

            cbi.DrawColorBlocks(ref PixelMap, (int)SurfaceWidth, (int)SurfaceHeight, FinalStride, ColorBlocks, Colors.Transparent);
            if (!OutImage.Dispatcher.CheckAccess())
            {
                OutImage.Dispatcher.Invoke
                    (
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action
                    (
                        delegate ()
                        {
                            OutImage.Source = BitmapSource.Create((int)SurfaceWidth, (int)SurfaceHeight, 96.0, 96.0, PixelFormats.Bgra32,
                               null, PixelMap, FinalStride);
                        }
                )
                );
            }
            else
                OutImage.Source = BitmapSource.Create((int)SurfaceWidth, (int)SurfaceHeight, 96.0, 96.0, PixelFormats.Bgra32,
                      null, PixelMap, FinalStride);
        }

        public void RenderRGBPlane (RGBAxisDefinitions AxisLayout, int Z, Image OutImage, double SurfaceWidth, double SurfaceHeight)
        {
            List<ColorBlenderInterface.ColorBlock> ColorBlocks = new List<ColorBlenderInterface.ColorBlock>();
            ColorBlenderInterface cbi = new ColorBlenderInterface();
            int FinalStride = (int)SurfaceWidth * 4;
            byte[] PixelMap = new byte[(int)SurfaceHeight * FinalStride];
            int CellWidth = (int)SurfaceWidth / 256;
            int CellHeight = (int)SurfaceHeight / 256;
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    Color CellColor = GetColorAt(x, y, Z, AxisLayout);
                    ColorBlenderInterface.ColorBlock CB = new ColorBlenderInterface.ColorBlock();
                    CB.Left = x * CellWidth;
                    CB.Top = y * CellHeight;
                    CB.Width = CellWidth;
                    CB.Height = CellHeight;
                    CB.BlockColor = CellColor.ToBGRA();
                    ColorBlocks.Add(CB);
                }
            }

            cbi.DrawColorBlocks(ref PixelMap, (int)SurfaceWidth, (int)SurfaceHeight, FinalStride, ColorBlocks, Colors.Transparent);
            if (!OutImage.Dispatcher.CheckAccess())
            {
                OutImage.Dispatcher.Invoke
                    (
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action
                    (
                        delegate ()
                        {
                            OutImage.Source = BitmapSource.Create((int)SurfaceWidth, (int)SurfaceHeight, 96.0, 96.0, PixelFormats.Bgra32,
                               null, PixelMap, FinalStride);
                        }
                )
                );
            }
            else
                OutImage.Source = BitmapSource.Create((int)SurfaceWidth, (int)SurfaceHeight, 96.0, 96.0, PixelFormats.Bgra32,
                     null, PixelMap, FinalStride);
        }

        private Color GetColorAt (int X, int Y, int Z, RGBAxisDefinitions AxisLayout)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;
            switch (AxisLayout)
            {
                case RGBAxisDefinitions.RGB:
                    r = (byte)X;
                    g = (byte)Y;
                    b = (byte)Z;
                    break;

                case RGBAxisDefinitions.RBG:
                    r = (byte)X;
                    b = (byte)Y;
                    g = (byte)Z;
                    break;

                case RGBAxisDefinitions.GRB:
                    g = (byte)X;
                    r = (byte)Y;
                    b = (byte)Z;
                    break;

                case RGBAxisDefinitions.GBR:
                    g = (byte)X;
                    b = (byte)Y;
                    r = (byte)Z;
                    break;

                case RGBAxisDefinitions.BRG:
                    b = (byte)X;
                    r = (byte)Y;
                    g = (byte)Z;
                    break;

                case RGBAxisDefinitions.BGR:
                    b = (byte)X;
                    g = (byte)Y;
                    r = (byte)Z;
                    break;

                default:
                    throw new InvalidOperationException("Unexpected axis layout.");
            }
            return Color.FromRgb(r, g, b);
        }

        private Color GetCMYColorAt (int C, int M, int Y, CMYAxisDefinitions AxisLayout)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;
            switch (AxisLayout)
            {
                case CMYAxisDefinitions.CMY:
                    CMYtoRGB((byte)C, (byte)M, (byte)Y, out r, out g, out b);
                    break;

                case CMYAxisDefinitions.CYM:
                    CMYtoRGB((byte)C, (byte)Y, (byte)M, out r, out g, out b);
                    break;

                case CMYAxisDefinitions.MCY:
                    CMYtoRGB((byte)M, (byte)C, (byte)Y, out r, out g, out b);
                    break;

                case CMYAxisDefinitions.MYC:
                    CMYtoRGB((byte)M, (byte)Y, (byte)C, out r, out g, out b);
                    break;

                case CMYAxisDefinitions.YCM:
                    CMYtoRGB((byte)Y, (byte)C, (byte)M, out r, out g, out b);
                    break;

                case CMYAxisDefinitions.YMC:
                    CMYtoRGB((byte)Y, (byte)M, (byte)C, out r, out g, out b);
                    break;

                default:
                    throw new InvalidOperationException("Unexpected axis layout.");
            }
            return Color.FromRgb(r, g, b);
        }

        private void CMYtoRGB (byte C, byte M, byte Y, out byte R, out byte G, out byte B)
        {
            R = (byte)(255 - C);
            G = (byte)(255 - M);
            B = (byte)(255 - Y);
        }

        private Color GetHSVColorAt (double H, double S, double V, HSVAxisDefinitions AxisLayout)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;
            switch (AxisLayout)
            {
                case HSVAxisDefinitions.HSV:
                    HSVtoRGB(H, S, V, out r, out g, out b);
                    break;

                case HSVAxisDefinitions.HVS:
                    HSVtoRGB(H, V, S, out r, out g, out b);
                    break;

                case HSVAxisDefinitions.VHS:
                    HSVtoRGB(V, H, S, out r, out g, out b);
                    break;

                case HSVAxisDefinitions.VSH:
                    HSVtoRGB(V, S, H, out r, out g, out b);
                    break;

                case HSVAxisDefinitions.SHV:
                    HSVtoRGB(S, H, V, out r, out g, out b);
                    break;

                case HSVAxisDefinitions.SVH:
                    HSVtoRGB(S, V, H, out r, out g, out b);
                    break;

                default:
                    throw new InvalidOperationException("Unexpected axis layout.");
            }
            return Color.FromRgb(r, g, b);
        }

        private void HSVtoRGB (double H, double S, double V, out byte R, out byte G, out byte B)
        {
            R = 0;
            G = 0;
            B = 0;
            if (S == 0.0)
            {
                R = (byte)(255.0 * V);
                G = R;
                B = R;
                return;
            }
            int Sector = (int)Math.Floor(H);
            double Fact = H - Sector;
            double x = V * (1.0 - S);
            x *= 255.0;
            double y = V * (1.0 - (S * Fact));
            y *= 255.0;
            double z = V * (1.0 - (S * (1.0 - Fact)));
            z *= 255.0;
            V *= 255.0;
            switch (Sector)
            {
                case 0:
                    R = (byte)(V);
                    G = (byte)(z);
                    B = (byte)(x);
                    break;

                case 1:
                    R = (byte)(y);
                    G = (byte)(V);
                    B = (byte)(x);
                    break;

                case 2:
                    R = (byte)(x);
                    G = (byte)(V);
                    B = (byte)(z);
                    break;

                case 3:
                    R = (byte)(x);
                    G = (byte)(y);
                    B = (byte)(V);
                    break;

                case 4:
                    R = (byte)(z);
                    G = (byte)(y);
                    B = (byte)(V);
                    break;

                default:
                    R = (byte)(V);
                    G = (byte)(x);
                    B = (byte)(y);
                    break;
            }
        }
    }
}
