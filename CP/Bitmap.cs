using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CP
{
	public class Bitmap
	{
		internal KernalHandle.CharInfo[] _chs;

		/// <summary>
		/// The width of the height
		/// </summary>
		public short Width { get; private set; }

		/// <summary>
		/// The height of the bitmap
		/// </summary>
		public short Height { get; private set; }

		/// <summary>
		/// Create a new console bitmap
		/// </summary>
		/// <param name="width">Width of the bitmap</param>
		/// <param name="height">Height of the bitmap</param>
		public Bitmap(uint width, uint height)
		{
			Width = ((short)(width));
			Height = ((short)(height));

			_chs = new KernalHandle.CharInfo[width * height];
		}

		/// <summary>
		/// Draw a bitmap on the screen
		/// </summary>
		/// <param name="x">The x position to draw it at</param>
		/// <param name="y">The y position to draw it at</param>
		/// <param name="bmp">The bitmap to use</param>
		public void DrawBitmap( uint x, uint y, Bitmap bmp ) {
			var _drawto = bmp._chs;

			for( uint _x = x; _x < x + bmp.Width; _x++ )
				for( uint _y = y; _y < y + bmp.Height; _y++ ) {
					if( _chs.Length > ( _y * Width + _x ) && ( _y * Width + _x ) > -1 )
						_chs[_y * Width + _x] = bmp._chs[( _y - y ) * bmp.Width + ( _x - x )];
				}
		}

		/// <summary>
		/// Fill the bitmap with a color
		/// </summary>
		/// <param name="Color">The color to use ( e.g. 0x0a )</param>
		public void Fill(short Color)
		{
			for (uint i = 0; i < Width * Height; i++)
			{
				_chs[i].Attributes = Color;
			}
		}

		/// <summary>
		/// Fill up the entire bitmap with a piece of text
		/// </summary>
		/// <param name="Text">The text to fill in with</param>
		public void Fill(char Text)
		{
			for (uint i = 0; i < Width * Height; i++)
			{
				_chs[i].Char.AsciiChar = ((byte)(Text));
			}
		}

		/// <summary>
		/// Fill the bitmap with a certain color and text
		/// </summary>
		/// <param name="Color">The color ( e.g. 0x0a )</param>
		/// <param name="Text">The charecter to fill with</param>
		public void Fill(short Color, char Text)
		{
			Fill(Color);
			Fill(Text);
		}

		/// <summary>
		/// Set a pixel at a certain X and Y
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		/// <param name="Color">The color ( e.g. 0x0a )</param>
		/// <param name="Charecter">The charecter to set it with</param>
		public void SetPixel(uint x, uint y, short Color, char Charecter)
		{
			if (y * Width + x < (Width * Height))
			{
				_chs[y * Width + x].Attributes = Color;
				_chs[y * Width + x].Char.AsciiChar = ((byte)(Charecter));
			}
		}

		/// <summary>
		/// Draw a border around the bitmap
		/// </summary>
		/// <param name="Color">The color ( e.g. 0x0a )</param>
		/// <param name="Charecter">The charecter to use</param>
		public void Border(short Color, char Charecter)
		{
			for(uint x = 0; x < Width; x++)
			for(uint y = 0; y < Height; y++)
				if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
				{
					_chs[y * Width + x].Attributes = Color;
					_chs[y * Width + x].Char.AsciiChar = ((byte)(Charecter));
				}
		}

		/// <summary>
		/// Set the color of a pixel
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		/// <param name="Color">The color ( e.g. 0x0a )</param>
		public void SetColor(uint x, uint y, short Color)
		{
			if (y * Width + x < (Width * Height))
			{
				_chs[y * Width + x].Attributes = Color;
			}
		}

		/// <summary>
		/// Set the text of a pixel
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		/// <param name="Charecter">The charecter to set it with</param>
		public void SetText(uint x, uint y, char Text)
		{
			if (y * Width + x < (Width * Height))
			{
				_chs[y * Width + x].Char.AsciiChar = ((byte)(Text));
			}
		}

		
		/// <summary>
		/// Draw vertical text at a certain place
		/// </summary>
		/// <param name="_x">The x position of the text</param>
		/// <param name="_y">The y position of the text</param>
		/// <param name="Color">The color of the text ( e.g. 0x0a )</param>
		/// <param name="Text">The text</param>
		public void DrawVerticalText(uint _x, uint _y, short Color, string Text)
		{
			uint x = 0; x = _x;
			uint y = 0; y = _y;

			int _index = 0;
			while (_index < Text.Length)
			{
				if (y > Height)
				{
					y = _y + 1;
					x++;
				}
				if (x > Width)
				{
					SetPixel(x, y, Color, Convert.ToChar((Text).Substring(_index, 1))); return;
				}

				SetPixel(x, y, Color, Convert.ToChar((Text).Substring(_index, 1)));
				y++;
				_index++;
			}
		}

		/// <summary>
		/// Draw horizontal text at a certain place
		/// </summary>
		/// <param name="_x">The x position of the text</param>
		/// <param name="_y">The y position of the text</param>
		/// <param name="Color">The color of the text ( e.g. 0x0a )</param>
		/// <param name="Text">The text</param>
		public void DrawHorizontalText(uint _x, uint _y, short Color, string Text)
		{
			uint x = 0; x = _x;
			uint y = 0; y = _y;

			int _index = 0;
			while (_index < Text.Length)
			{
				if (x > Width)
				{
					x = _x + 1;
					y++;
				}
				if (y > Height)
				{
					SetPixel(x, y, Color, Convert.ToChar((Text).Substring(_index, 1))); return;
				}

				SetPixel(x, y, Color, Convert.ToChar((Text).Substring(_index, 1)));
				x++;
				_index++;
			}
		}
	}
}
