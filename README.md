# CP
CP allows you to use low-end functions located in the kernal to do high-end functions for making extremely fast console games.

# Quick Guide

1) Create a CDraw ( You can only do this once! )

`CDraw window = new CDraw();`

2) Use the availible drawing resources!

There is Bitmap and RenderPane. The Bitmap is a picture that is whatever width and whatever height long, of which you use the various drawing functions to dreaw to. This is the main drawing class, so you should get familiar with it!

Next, you also need to learn about `window.DrawBitmap(x, y, bitmap);`. This draws the bitmap to the screen - however you may notice that nothing was drawn. This is because you need to call `window.Refresh();`, or do `window.DrawBitmap(x, y, bitmap, true);`