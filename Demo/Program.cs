using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Threading;
using CP;

namespace ConsolePlus
{
	class Program
	{
		static CDraw window;

		[STAThread]
		static void Main(string[] args)
		{
			window = new CDraw();

			Bitmap screen = new Bitmap( 80, 25 );

			screen.Fill( 0xA0, 'x' );

			//screen.Save( "example.bmp" );
			screen.Fill( 'y' );
			screen.Load( "example.bmp" );

			window.DrawBitmap( 0, 0, screen, true );

			Console.ReadKey();
		}
	}
}