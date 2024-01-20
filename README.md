# _.NET MAUI_ Space Shooter Arcade Game

## _The Challenge_

To create a simple yet heavily animated arcade cross-platform game, using .NET MAUI XAML with Skia drawn UI, showing all the advantages that [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui) and [SkiaSharp](https://github.com/mono/SkiaSharp) provide. 
The game runs on on Android, Windows, iOS, and Mac (Catalyst), all from a single code base. Desktop versions support both mouse and keyboard!

## _The Game_

Driven by one of the awesome [ICT MOO tutorials](https://www.youtube.com/@mooict/videos). There is so much content to play with now that we could do all that in .NET MAUI!

Free [Lottie animations](https://lottiefiles.com/) quickly fulfill the need for animated content.

This code here is intended as an etude and you are fully encouraged to reuse it for creating something exceptional of your own!

## _Of Note_

Android and iOS run fully hardware-accelerated, with iOS using Apple Metal and Android running on GL. 
For Windows and Mac Catalyst, hardware acceleration is not yet implemented, but they still run above 100 FPS. 
For desktop we implemented non-resizable windows and they receive platform-independent key events. Destop windows can be dragged among different displays dinamically adapting to their scale. 
[DrawnUi](https://github.com/taublast/AppoMobi.Maui.DrawnUi.Demo) pre-alpha nuget was used, the rendering engine for .Net MAUI designed to draw your custom UI on the Skia canvas.

## _Final Words_

Attained FPS looks okay in Release builds and even on Debug. 
With an optimized design, especially in regards to control caching, we could imagine more games and fancy animations built with [#dotnetmaui](https://twitter.com/search?q=%23dotnetmaui).

[Lottie animations](https://lottiefiles.com/) have proven themselves to be very easy and useful, quickly added during game creation.

Windows version is actually behind other platforms due to the lack of HW-acceleration but this might [change soon](https://github.com/mono/SkiaSharp/issues/1893).


The main enemy was the garbage collector, which is feared for its unpredictable lag spikes during long animations.
We tried to make it trigger as little as possible using [Unity's suggested techniques](https://docs.unity3d.com/Manual/performance-garbage-collection-best-practices.html). It still might have its small impact but we can hope for .NET MAUI some day to adopt a custom [incremental GC-collector](https://docs.unity3d.com/Manual/performance-incremental-garbage-collection.html) that Unity is using.
