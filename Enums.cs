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
    public partial class MainWindow
    {
        Dictionary<GeneralAxisDefintions, Tuple<RGBAxisDefinitions, HSVAxisDefinitions, CMYAxisDefinitions>> AxisMappings =
            new Dictionary<GeneralAxisDefintions, Tuple<RGBAxisDefinitions, HSVAxisDefinitions, CMYAxisDefinitions>>()
            {
                {GeneralAxisDefintions._123, new Tuple<RGBAxisDefinitions, HSVAxisDefinitions, CMYAxisDefinitions>(RGBAxisDefinitions.RGB,HSVAxisDefinitions.HSV,CMYAxisDefinitions.CMY) },
                {GeneralAxisDefintions._132, new Tuple<RGBAxisDefinitions, HSVAxisDefinitions, CMYAxisDefinitions>(RGBAxisDefinitions.RBG,HSVAxisDefinitions.HVS,CMYAxisDefinitions.CYM) },
                {GeneralAxisDefintions._213, new Tuple<RGBAxisDefinitions, HSVAxisDefinitions, CMYAxisDefinitions>(RGBAxisDefinitions.GRB,HSVAxisDefinitions.SHV,CMYAxisDefinitions.MCY) },
                {GeneralAxisDefintions._231, new Tuple<RGBAxisDefinitions, HSVAxisDefinitions, CMYAxisDefinitions>(RGBAxisDefinitions.GBR,HSVAxisDefinitions.SVH,CMYAxisDefinitions.MYC) },
                {GeneralAxisDefintions._312, new Tuple<RGBAxisDefinitions, HSVAxisDefinitions, CMYAxisDefinitions>(RGBAxisDefinitions.BRG,HSVAxisDefinitions.VHS,CMYAxisDefinitions.YCM) },
                {GeneralAxisDefintions._321, new Tuple<RGBAxisDefinitions, HSVAxisDefinitions, CMYAxisDefinitions>(RGBAxisDefinitions.BGR,HSVAxisDefinitions.VSH,CMYAxisDefinitions.YMC) },
            };
    }

    public enum RGBAxisDefinitions
    {
        RGB,
        RBG,
        GRB,
        GBR,
        BRG,
        BGR
    }

    public enum CMYAxisDefinitions
    {
        CMY,
        CYM,
        MCY,
        MYC,
        YCM,
        YMC
    }

    public enum HSVAxisDefinitions
    {
        HSV,
        HVS,
        SHV,
        SVH,
        VHS,
        VSH
    }

    public enum GeneralAxisDefintions
    {
        _123,
        _132,
        _213,
        _231,
        _312,
        _321
    }
}
