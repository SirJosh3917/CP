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

			//Random render pane examlpe
			/*
			var e = new RenderPane(100, 100);
			var b = new Bitmap(100, 100);
			b.Fill(0xa0);
			b.Border(0x1a, '*');
			b.DrawHorizontalText(0, 0, 0x1a, "Hello world I'm cool!");
			b.DrawVerticalText(0, 1, 0x1a, "Hello world I'm cool!");
			e.DrawBitmap(0, 0, b, true);
			

			while(true)
			{
				e.RenderX++;
				e.Refresh();
				System.Threading.Thread.Sleep(1000);
			}*/
			
			//Somethine random
			bool loading = true;

			new Thread(delegate()
			{
				Bitmap load = new Bitmap(40, 7);

				string lt = "Loading...";

				load.Fill(0x1a);

				load.DrawHorizontalText(15, 3, 0x9a, lt);
				load.DrawHorizontalText(0, 0, 0x00, "----------------------------------------");
				load.DrawHorizontalText(0, 6, 0x00, "----------------------------------------");

				uint y = 0;
				bool go = true;

				while (loading)
				{
					window.DrawBitmap(20, y, load, true);

					if (go)
						y++;
					else y--;

					if (y == 0 || y == 18)
						go = !go;

					Thread.Sleep(100);
				}
			}).Start();

			Bitmap pistol = new Bitmap(3, 2);
			pistol.Fill(0x80);
			pistol.DrawHorizontalText(1, 1, 0x00, "  ");

			Bitmap player_down = new Bitmap(5, 5);
			player_down.Fill(0xe0);
			player_down.DrawHorizontalText(1, 1, 0xe0, "***");
			player_down.DrawHorizontalText(1, 2, 0xe0, "._. ");
			player_down.DrawHorizontalText(1, 3, 0xe0, "\\./ ");
			player_down.SetPixel(3, 3, 0xe0, '/');
			player_down.Border(0x00, '-');

			Bitmap player_up = new Bitmap(5, 5);
			player_up.Fill(0xe0);
			player_up.DrawHorizontalText(1, 1, 0xe0, "***");
			player_up.DrawHorizontalText(1, 2, 0xe0, "'-'");
			player_up.DrawHorizontalText(1, 3, 0xe0, "\\-/");
			player_up.SetPixel(3, 3, 0xe0, '/');
			player_up.Border(0x00, '-');

			Bitmap player_right = new Bitmap(5, 5);
			player_right.Fill(0xe0);
			player_right.DrawHorizontalText(1, 1, 0xe0, "***");
			player_right.DrawHorizontalText(1, 2, 0xe0, " ._");
			player_right.DrawHorizontalText(1, 3, 0xe0, " \\.");
			player_right.Border(0x00, '-');

			Bitmap player_left = new Bitmap(5, 5);
			player_left.Fill(0xe0);
			player_left.DrawHorizontalText(1, 1, 0xe0, "***");
			player_left.DrawHorizontalText(1, 2, 0xe0, "_. ");
			player_left.DrawHorizontalText(1, 3, 0xe0, "./ ");
			player_left.Border(0x00, '-');

			Bitmap bullet = new Bitmap(4, 1);
			bullet.Fill(0x84);
			bullet.DrawHorizontalText(0, 0, 0x00, " ");
			bullet.DrawHorizontalText(1, 0, 0x84, " ");

			Bitmap dir = player_down;

			uint px = 0;
			uint py = 0;
			ConsoleKey k;

			while (((k = Console.ReadKey(true).Key) != ConsoleKey.Escape))
			{
				switch(k)
				{
					case ConsoleKey.W:
						dir = player_up;
						py--;
						break;
					case ConsoleKey.A:
						dir = player_left;
						px--;
						break;
					case ConsoleKey.S:
						dir = player_down;
						py++;
						break;
					case ConsoleKey.D:
						dir = player_right;
						px++;
						break;
					case ConsoleKey.Spacebar:
						if(dir == player_right && px == 5 && py == 8)
						{
							new Thread(delegate()
							{
								uint bulpos = 9;
								while(bulpos < 70)
								{
									bulpos++;
									window.DrawBitmap(bulpos, 8, bullet, true);
								}
							}).Start();
						}
						break;
				}
				Bitmap pos = new Bitmap(Convert.ToInt32(px + 1 + py), 1);
				pos.DrawHorizontalText(0, 0, 0x0a, px.ToString() + ":" + py.ToString());
				window.DrawBitmap(px, py, dir);
				window.DrawBitmap(0, 0, pos);
				window.DrawBitmap(10, 10, pistol, true);
			}

			Console.ReadKey();
		}
	}
}