# Space Shooter Arcade Game Etude    
### Build with DrawnUi for .Net Maui

_Destined to evaluate .Net Maui performance and 2-D game potential._

## The Challenge

To create a simple yet heavily animated arcade cross-platform game, running on Android, Windows, iOS, and Mac (Catalyst) with a single code base, using all the advantages that .Net Maui and SkiaSharp would provide.

## The Game

Inspired by one of the awesome ITC MOO tutorials. There is much more content to be played with, as you can see now we can do it all in .Net Maui!

Free Lottie animations were used to fulfill the need for animated content quickly.

This code remains to be an etude and you are fully encouraged to reuse it for  creating something exceptional of your own!

## To Notice

For Windows and Mac desktops, we implemented non-resizable windows.

Android and iOS run fully hardware-accelerated, with iOS using Apple Metal and Android on GL. For Windows and Mac Catalyst, this is yet not implemented, they still run above 100 fps though.

## Post Words

Attained FPS looks okay in Release builds and even on Debug. With an optimized design, especially in regards to control caching, we could imagine more games built with #dotnetmaui coming.

Lottie animations also have proven themselves to be very effective and useful to be quickly consumed during game creation.

Our main enemy was the GC collector which is feared for its unpredictable lag spikes during long animations. We tried to make it trigger as little as possible using Unity's suggested techniques. It still has its impact so we can hope some day .Net Maui could adopt that ... custom GC-collector that Unity is already using.


