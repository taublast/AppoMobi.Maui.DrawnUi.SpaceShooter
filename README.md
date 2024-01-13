# Space Shooter Arcade Game Etude    
### Built with DrawnUi for .NET MAUI

## The Challenge

To create a simple yet heavily animated arcade cross-platform game, using .NET MAUI XAML with Skia drawn UI, showing all the advantages that .NET MAUI and SkiaSharp provide. The game runs on on Android, Windows, iOS, and Mac (Catalyst), all from a single code base.

## The Game

Driven by one of the awesome [ICT MOO tutorials](https://www.youtube.com/@mooict/videos). There is so much content to play with now that we could do all that in .NET MAUI!

Free Lottie animations quickly fulfill the need for animated content.

This code here is intended as an etude and you are fully encouraged to reuse it for creating something exceptional of your own!

## Of Note

For Windows and Mac desktops, we implemented non-resizable windows.

Android and iOS run fully hardware-accelerated, with iOS using Apple Metal and Android on GL. For Windows and Mac Catalyst, hardware acceleration is not yet implemented, but they still run above 100 FPS.

## Final Words

Attained FPS looks okay in Release builds and even on Debug. With an optimized design, especially in regards to control caching, we could imagine more games and fancy animations built with #dotnetmaui.

Lottie animations have proven themselves to be very easy and useful, quickly added during game creation.

The main enemy was the garbage collector, which is feared for its unpredictable lag spikes during long animations.
We tried to make it trigger as little as possible using [Unity's suggested techniques](https://docs.unity3d.com/Manual/performance-garbage-collection-best-practices.html). It still has its small impact but we can hope for .NET MAUI some day to adopt a custom [incremental GC-collector](https://docs.unity3d.com/Manual/performance-garbage-collector.html) that Unity is using.
