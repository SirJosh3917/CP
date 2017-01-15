using System;
using System.Collections.Generic;
using System.Text;

namespace CP {
	/// <summary>
	/// A render pane is used as a bigger CDraw area, and you can render it correctly too.
	/// </summary>
	public class RenderPane {
		private CP.KernalHandle.CharInfo[] buf;

		/// <summary>
		/// The width of the render pane.
		/// </summary>
		public uint Width { get; private set; }

		/// <summary>
		/// The height of the render pane.
		/// </summary>
		public uint Height { get; private set; }

		/// <summary>
		/// The x position on the frame on the Render Pane to render from.
		/// </summary>
		public uint RenderX {
			get {
				return _rx;
			}
			set {
				if( value > Width - 80 )
					return;

				if( value < 0 )
					return;

				_rx = value;
			}
		}
		private uint _rx = 0;

		/// <summary>
		/// The y position on the frame on the Render Pane to render from.
		/// </summary>
		public uint RenderY {
			get {
				return _ry;
			}
			set {
				if( value > Height - 25 )
					return;

				if( value < 0 )
					return;

				_ry = value;
			}
		}
		private uint _ry = 0;

		/// <summary>
		/// Create a new render pane.
		/// </summary>
		/// <param name="width">The width of the render pane.</param>
		/// <param name="height">The height of the render pane.</param>
		public RenderPane( uint width, uint height ) {
			Width = width;
			Height = height;
			buf = new CP.KernalHandle.CharInfo[width * height];
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
					//if ((_y - y) * bmp.Height + (_x - x) > (bmp.Width * bmp.Height) - 1) return;

					buf[_y * Width + _x] = bmp._chs[( _y - y ) * bmp.Width + ( _x - x )];
				}

			if( RefreshScreen ) {
				Refresh();
			}
		}

		/// <summary>
		/// Clears the render pane
		/// </summary>
		public void Clear() {
			buf = new CP.KernalHandle.CharInfo[Width * Height];
			Refresh();
		}

		/// <summary>
		/// Refreshes the render pane and then the screen to apply any changes made to the console screen.
		/// </summary>
		/// <returns>True if it went well, false if not.</returns>
		public void Refresh() {
			CDraw.GeneratedCDraw.DrawBitmap( 0, 0, GetRenderBitmap(), true );
		}

		/// <summary>
		/// Get a 80 by 25 bitmap of the RenderPane rendered based on the RenderX and RenderY
		/// </summary>
		/// <returns></returns>
		public Bitmap GetRenderBitmap() {
			Bitmap _r = new Bitmap( 80, 25 );

			for( uint _y = 0; _y < 25; _y++ ) {
				uint y = _y + RenderY;

				for( uint _x = 0; _x < 80; _x++ ) {
					uint x = _x + RenderX;
					if( _y * 80 + _x < 2000 )
						_r._chs[_y * 80 + _x] = buf[y * Width + ( _x + RenderX )];
				}
			}

			return _r;
		}
	}
}
