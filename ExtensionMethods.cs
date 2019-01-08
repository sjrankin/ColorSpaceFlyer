using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorSpaceFlyer
{
    public static class ExtensionMethods
    {
        public static UInt32 ToBGRA (this Color Source)
        {
            UInt32 BGRA = (UInt32)(Source.B << 24);
            BGRA |= (UInt32)(Source.G << 16);
            BGRA |= (UInt32)(Source.R << 8);
            BGRA |= (UInt32)(Source.A);
            return BGRA;
        }
    }
}
