using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CP {
	public class Bitmap {
		internal KernalHandle.CharInfo[] _chs;

		/// <summary>
		/// The width of the height
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		/// The height of the bitmap
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		/// Create a new console bitmap
		/// </summary>
		/// <param name="width">Width of the bitmap</param>
		/// <param name="height">Height of the bitmap</param>
		public Bitmap( uint width, uint height ) {
			Width = ( (int)( width ) );
			Height = ( (int)( height ) );

			_chs = new KernalHandle.CharInfo[width * height];
		}

		public byte[ ] Save() {
			//Create a new byte[] array for the file
			byte[ ] fileByte = new byte[ _chs.Length * 2 + 9 ];

			//Header Byte
			fileByte[ 0 ] = 0xCB;

			//Convert the width and the height to 4 bytes
			byte[ ] res = BitConverter.GetBytes( Width );
			if (BitConverter.IsLittleEndian)
				Array.Reverse( res );

			for (int i = 0 ; i < 4 ; i++)
				fileByte[ i + 1 ] = res[ i ];

			res = BitConverter.GetBytes( Height );
			if (BitConverter.IsLittleEndian)
				Array.Reverse( res );

			for (int i = 0 ; i < 4 ; i++)
				fileByte[ i + 5 ] = res[ i ];

			//Loop through each charecter on the bitmap
			for (int i = 0 ; i < _chs.Length ; i++) {

				//Store it as "Charecter, Hex for color"

				fileByte[ ( i * 2 ) + 9 ] = _chs[ i ].Char.AsciiChar;

				var bytes = ( byte )( ( ( int )_chs[ i ].Attributes % 256 ) );

				fileByte[ ( i * 2 ) + 10 ] = bytes;
			}

			return fileByte;
		}

		/// <summary>
		/// Save the Bitmap to a file
		/// </summary>
		/// <param name="file"></param>
		public void Save(string file) {
			//Try to write it to the file
			try {
				System.IO.File.WriteAllBytes( file, Save() );
			} catch {
				throw;
			}
			
		}

		public void Load(byte[ ] fileByte) {
			//Check the header byte
			if (fileByte[ 0 ] != 0xCB)
				return;

			//Store the integer width and heights
			byte[ ] wid = { fileByte[ 1 ], fileByte[ 2 ], fileByte[ 3 ], fileByte[ 4 ] };
			byte[ ] hei = { fileByte[ 5 ], fileByte[ 6 ], fileByte[ 7 ], fileByte[ 8 ] };

			//Convert them
			if (BitConverter.IsLittleEndian) {
				Array.Reverse( wid );
				Array.Reverse( hei );
			}

			//Set the width and height
			Width = BitConverter.ToInt32( wid, 0 );
			Height = BitConverter.ToInt32( hei, 0 );

			//Set all the pixels of the bitmap
			_chs = new KernalHandle.CharInfo[ ( fileByte.Length - 9 ) / 2 ];
			for (int i = 0 ; i < ( fileByte.Length - 9 ) / 2 ; i++) {
				if (fileByte.Length >= ( i * 2 + 1 )) {
					_chs[ i ].Char.AsciiChar = fileByte[ ( i * 2 ) + 9 ];
					_chs[ i ].Attributes = ( short )fileByte[ ( i * 2 ) + 10 ];
				}
			}
		}

		/// <summary>
		/// Load the Bitmap from a file
		/// </summary>
		/// <param name="file"></param>
		public void Load(string file) {
			//Read the file
			byte[ ] fileByte = System.IO.File.ReadAllBytes( file );

			//Load the bitmap
			Load( fileByte );
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
		public void Fill( short Color ) {
			for( uint i = 0; i < Width * Height; i++ ) {
				_chs[i].Attributes = Color;
			}
		}

		/// <summary>
		/// Fill up the entire bitmap with a piece of text
		/// </summary>
		/// <param name="Text">The text to fill in with</param>
		public void Fill( char Text ) {
			for( uint i = 0; i < Width * Height; i++ ) {
				_chs[i].Char.AsciiChar = ( (byte)( Text ) );
			}
		}

		/// <summary>
		/// Fill the bitmap with a certain color and text
		/// </summary>
		/// <param name="Color">The color ( e.g. 0x0a )</param>
		/// <param name="Text">The charecter to fill with</param>
		public void Fill( short Color, char Text ) {
			Fill( Color );
			Fill( Text );
		}

		/// <summary>
		/// Set a pixel at a certain X and Y
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		/// <param name="Color">The color ( e.g. 0x0a )</param>
		/// <param name="Charecter">The charecter to set it with</param>
		public void SetPixel( uint x, uint y, short Color, char Charecter ) {
			SetPixel( x, y, Color );
			SetPixel( x, y, Charecter );
		}

		/// <summary>
		/// Draw a border around the bitmap
		/// </summary>
		/// <param name="Color">The color ( e.g. 0x0a )</param>
		/// <param name="Charecter">The charecter to use</param>
		public void Border( short Color, char Charecter ) {
			for( uint x = 0; x < Width; x++ )
				for( uint y = 0; y < Height; y++ )
					if( x == 0 || x == Width - 1 || y == 0 || y == Height - 1 ) {
						_chs[y * Width + x].Attributes = Color;
						_chs[y * Width + x].Char.AsciiChar = ( (byte)( Charecter ) );
					}
		}

		/// <summary>
		/// Draw a border around the Bitmap.
		/// </summary>
		/// <param name="Color">The color to use</param>
		/// <param name="Text">The text to describe the border. Leave blank if you don't want text.</param>
		public void Border(short Color, string Text) {
			Border( Color, ' ' );

			DrawHorizontalText( 0, 0, Color, _getStr( Width, "-", "+" ) );
			for (uint i = 1 ; i < ( uint )Height - 1 ; i++) {
				SetPixel( 0, i, '|' );
				SetPixel( ( uint )Width - 1, i, '|' );
			}
			DrawHorizontalText( 0, ( uint )Height - 1, Color, _getStr( Width, "-", "+" ) );

			DrawHorizontalText( 1, 0, Color, Text );
		}

		private string _getStr( int len, string rep, string end, string othend = "" ) {
			System.Text.StringBuilder strb = new StringBuilder( len );
			strb.Append( end );
			for( int i = 0; i < len - 2; i++ ) {
				strb.Append( rep );
			}
			if( othend == "" )
				strb.Append( end );
			else
				strb.Append( othend );
			string ret = strb.ToString();
			strb = null;
			return ret;
		}

		/// <summary>
		/// Set the color of a pixel
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		/// <param name="Color">The color ( e.g. 0x0a )</param>
		public void SetPixel( uint x, uint y, short Color ) {
			if( y * Width + x < ( Width * Height ) ) {
				_chs[y * Width + x].Attributes = Color;
			}
		}

		/// <summary>
		/// Set the text of a pixel
		/// </summary>
		/// <param name="x">The x position</param>
		/// <param name="y">The y position</param>
		/// <param name="Charecter">The charecter to set it with</param>
		public void SetPixel( uint x, uint y, char Text ) {
			if( y * Width + x < ( Width * Height ) ) {
				_chs[y * Width + x].Char.AsciiChar = ( (byte)( Text ) );
			}
		}

		/// <summary>
		/// Draw vertical text at a certain place
		/// </summary>
		/// <param name="_x">The x position of the text</param>
		/// <param name="_y">The y position of the text</param>
		/// <param name="Color">The color of the text ( e.g. 0x0a )</param>
		/// <param name="Text">The text</param>
		public void DrawVerticalText( uint _x, uint _y, short Color, string Text ) {
			if( Text.Length < 1 )
				return;

			uint x = 0;
			x = _x;
			uint y = 0;
			y = _y;

			int _index = 0;
			while( _index < Text.Length ) {
				if( y > Height ) {
					y = _y + 1;
					x++;
				}
				if( x > Width ) {
					SetPixel( x, y, Color, Convert.ToChar( ( Text ).Substring( _index, 1 ) ) );
					return;
				}

				SetPixel( x, y, Color, Convert.ToChar( ( Text ).Substring( _index, 1 ) ) );
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
		public void DrawHorizontalText( uint _x, uint _y, short Color, string Text ) {
			if( Text.Length < 1 )
				return;

			uint x = 0;
			x = _x;
			uint y = 0;
			y = _y;

			int _index = 0;
			while( _index < Text.Length ) {
				if( x > Width ) {
					x = _x + 1;
					y++;
				}
				if( y > Height ) {
					SetPixel( x, y, Color, Convert.ToChar( ( Text ).Substring( _index, 1 ) ) );
					return;
				}

				SetPixel( x, y, Color, Convert.ToChar( ( Text ).Substring( _index, 1 ) ) );
				x++;
				_index++;
			}
		}
	}
}
