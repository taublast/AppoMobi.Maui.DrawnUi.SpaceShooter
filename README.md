# _.NET MAUI_ Space Shooter Game Etude


https://github.com/taublast/AppoMobi.Maui.DrawnUi.SpaceShooter/assets/25801194/30523e94-12d5-4740-8af3-bebf11ef317f

## _What's New_

* MacCatalyst net8 crash fixed

## _The Challenge_

To create a simple yet heavily animated arcade cross-platform game, using .NET MAUI XAML with Skia drawn UI, showing all the advantages that [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui) and [SkiaSharp](https://github.com/mono/SkiaSharp) provide. 
The game runs on Android, Windows, iOS, and Mac (Catalyst), all from a single code base. Desktop versions support both mouse and keyboard.

## _Implementation_

Driven by [one](https://github.com/mooict/WPF-Space-shooter-game) of the awesome [ICT MOO tutorials](https://www.youtube.com/@mooict/videos), much content to play with, knowing we can do it all with .NET MAUI.  

Free [Lottie animations](https://lottiefiles.com/) quickly fulfilled the need for animated content.

[DrawnUI](https://github.com/taublast/AppoMobi.Maui.DrawnUi.Demo) pre-alpha nuget was used to draw virtual controls on a Skia canvas.

## _Of Note_

Android and iOS run hardware-accelerated, with iOS using Apple Metal and Android on GL. 

For Windows and Mac Catalyst, hardware acceleration is not yet implemented, they still run above 100 FPS. 

Desktop versions present non-resizable windows with platform-independent key events. They can be dragged among different displays adapting to new scale. Disabling the maximize button still needs to be implemented.

After updating to NET8 app window is not centering on Catalyst until fix is found (estimated soon).

## _Final Words_

Attained FPS looks okay in Release builds and even on Debug. 
With an optimized design, especially in regards to control caching, we could imagine more games and fancy animations built with [#dotnetmaui](https://twitter.com/search?q=%23dotnetmaui).

[Lottie animations](https://lottiefiles.com/) have proven themselves to be very useful to quickly implement animations.

Tried to make the garbage collector trigger as little as possible using [Unity's suggested techniques](https://docs.unity3d.com/Manual/performance-garbage-collection-best-practices.html). It still might have its small impact but we can hope for .NET MAUI someday to adopt a custom [incremental GC-collector](https://docs.unity3d.com/Manual/performance-incremental-garbage-collection.html) that Unity is using.

Would add examples of playing frames from a sprite sheet and separate image files if there would be enough interest from the community.

## _Licencing_

This code and the DrawnUI nuget are provided under the [MIT license](https://github.com/taublast/AppoMobi.Maui.DrawnUi.SpaceShooter?tab=MIT-1-ov-file#readme). ICT MOO space ships sprites come under the [Apache 2.0 license](https://github.com/mooict/WPF-Space-shooter-game?tab=Apache-2.0-1-ov-file#readme).
