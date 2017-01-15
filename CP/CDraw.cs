using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CP
{
	public class CDraw
	{
		private SafeFileHandle h;
		private CP.KernalHandle.CharInfo[] buf = new CP.KernalHandle.CharInfo[80 * 25];
		private KernalHandle.SmallRect rect = new KernalHandle.SmallRect() { Left = 0, Top = 0, Bottom = 25, Right = 80 };

		public static CDraw GeneratedCDraw { get; private set; }
		private static bool _gen = false;

		public CDraw() {
			if(!_gen)
			{
				GeneratedCDraw = this;
				_gen = true;
			}
			else
			{
				this.h = GeneratedCDraw.h;
				this.buf = GeneratedCDraw.buf;
				this.rect = GeneratedCDraw.rect;
			}

			Console.SetBufferSize(80, 25);
			Console.SetWindowSize(80, 25);
			h = KernalHandle.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

			if (h.IsInvalid)
			{
				Console.WriteLine("h.IsInvalid is invalid. Please restart the application.");
				throw new Exception("h.IsInvalid");
			}
		}

		/// <summary>
		/// Draw a bitmap on the screen
		/// </summary>
		/// <param name="x">The x position to draw it at</param>
		/// <param name="y">The y position to draw it at</param>
		/// <param name="bmp">The bitmap to use</param>
		/// <param name="RefreshScreen">If you want to refresh the screen. Refreshing the screen applies any changes you made so far to the screen.</param>
		public void DrawBitmap( uint x, uint y, Bitmap bmp, bool RefreshScreen = false ) {
			var _drawto = bmp._chs;

			for( uint _x = x; _x < x + bmp.Width; _x++ )
				for( uint _y = y; _y < y + bmp.Height; _y++ ) {
					if( buf.Length > ( _y * 80 + _x ) && ( _y * 80 + _x ) > -1 )
						buf[_y * 80 + _x] = bmp._chs[( _y - y ) * bmp.Width + ( _x - x )];
				}

			if( RefreshScreen ) {
				Refresh();
			}
		}

		/// <summary>
		/// Clears the screen
		/// </summary>
		public void Clear()
		{
			buf = new CP.KernalHandle.CharInfo[80 * 25];
			Refresh();
		}

		/// <summary>
		/// Refreshes the screen to apply any changes made.
		/// </summary>
		/// <returns>True if it went well, false if not.</returns>
		public bool Refresh()
		{
			return KernalHandle.WriteConsoleOutput(h, buf,
			 new CP.KernalHandle.Coord() { X = 80, Y = 25 },
			 new CP.KernalHandle.Coord() { X = 0, Y = 0 },
			 ref rect);
		}
	}
}
