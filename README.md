# Space Shooter Arcade Game Etude    
### Build with DrawnUi for .Net Maui

_Destined to evaluate MAUI performace and eventually games potential._

## The Challenge

To create a simple yet heavily animated arcade cross-platform game, runnig on Android, Windows, iOS and Mac (Catalyst) with a single code base, using all advantages that .Net Maui and SkiaSharp would provide.

## The Game

Inspired by one of the awesome ITC MOO tutorials. There is much more content to be played with, as you would see now we can do it all in .Net Maui!

Free Lottie animations where used to quickly fulfill the need for animated content.

This code remains to be an etude and your are fully encouraged to reuse it for  creating something exceptionnal of your own!

## To Notice

For Windows and Mac desktops we actually implemented non-resizable windows.

Android and iOS run fully hardware-accelerated, with iOS using Apple Metal and Android using GL. For Windows and Mac Catalyst this is yet not implemented though they still run above 100 fps.

## Post Words

Attained FPS looks okay in Release builds and even on Debug. With an optmimized design, especially in regards of controls caching, we could imagine more games built with #dotnetmaui coming.

Lottie animations also have proven themselves to be effective and very useful in order to be quickly consumed during game creation.

Our main enemy was the GC collector that is feared for its unpredictable lag spikes during long animations. We tried to make it trigger as less as possible using Unity suggested techniques. It still has its impact so we can just hope some day .Net Maui could adopt that ... custom GC-collector that Unity is already using.


