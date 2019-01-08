using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Runtime.InteropServices;

namespace ColorSpaceFlyer
{
    public class ColorBlenderInterface
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ColorBlock
        {
            public Int32 Left;
            public Int32 Top;
            public Int32 Width;
            public Int32 Height;
            public Int32 Right;
            public Int32 Bottom;
            public UInt32 BlockColor;
            public byte A;
            public byte R;
            public byte G;
            public byte B;
        };

        [DllImport("ColorBlender.dll", EntryPoint = "_DrawBlocks@28", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        static extern unsafe bool DrawBlocks (void* Target, Int32 TargetWidth, Int32 TargetHeight, Int32 TargetStride,
        void* ColorBlockList, Int32 ColorBlockCount, UInt32 DefaultColor);

        /// <summary>
        /// Draw a list of color blocks.
        /// </summary>
        /// <param name="Target">Where the drawing will occur.</param>
        /// <param name="TargetWidth">Width of <paramref name="Target"/>.</param>
        /// <param name="TargetHeight">Height of <paramref name="Target"/>.</param>
        /// <param name="TargetStride">Stride of <paramref name="Target"/>.</param>
        /// <param name="ColorBlocks">List of color blocks to draw.</param>
        /// <param name="DefaultColor">The color to use when no blocks are at a given point.</param>
        /// <returns>True on success, false on failure.</returns>
        public bool DrawColorBlocks (ref byte[] Target, int TargetWidth, int TargetHeight, int TargetStride,
           List<ColorBlock> ColorBlocks, Color DefaultColor)
        {
            bool DrawnOK = true;
            UInt32 BGColor = (UInt32)((DefaultColor.B << 24) + (DefaultColor.G << 16) + (DefaultColor.R << 8) + (DefaultColor.A));
            unsafe
            {
                ColorBlock[] Blocks = new ColorBlock[ColorBlocks.Count];
                for (int i = 0; i < ColorBlocks.Count; i++)
                    Blocks[i] = ColorBlocks[i];
                fixed (void* Buffer = Target)
                {
                    fixed (void* BlockArray = Blocks)
                    {
                        DrawnOK = DrawBlocks(Buffer, TargetWidth, TargetHeight, TargetStride,
                            BlockArray, Blocks.Length, BGColor);
                    }
                }
            }
            return DrawnOK;
        }
    }
}
