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
		public Bitmap(int width, int height)
		{
			Width = ((short)(width));
			Height = ((short)(height));

			_chs = new KernalHandle.CharInfo[width * height];
		}

		/// <summary>
		/// Fill the bitmap with a color
		/// </summary>
		/// <param name="Color">The color to use ( e.g. 0x0a )</param>
		public void Fill(short Color)
		{
			for (int i = 0; i < Width * Height; i++)
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
			for (int i = 0; i < Width * Height; i++)
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
		public void SetPixel(int x, int y, short Color, char Charecter)
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
			for(int x = 0; x < Width; x++)
			for(int y = 0; y < Height; y++)
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
		public void SetColor(int x, int y, short Color)
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
		public void SetText(int x, int y, char Text)
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
		public void DrawVerticalText(int _x, int _y, short Color, string Text)
		{
			int x = 0; x = _x;
			int y = 0; y = _y;

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
		public void DrawHorizontalText(int _x, int _y, short Color, string Text)
		{
			int x = 0; x = _x;
			int y = 0; y = _y;

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
