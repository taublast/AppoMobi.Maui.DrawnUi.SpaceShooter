using Microsoft.UI.Xaml;
using SpaceShooter.Game;
using System.Diagnostics;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpaceShooter.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        var appWindow = this.Application.Windows.First() as Microsoft.Maui.Controls.Window;
        var window = appWindow.Handler.PlatformView as Microsoft.Maui.MauiWinUIWindow;

        //hook keys
        var native = window.Content as Microsoft.UI.Xaml.Controls.Panel;
        native.PreviewKeyUp += (sender, args) =>
        {
            var mapped = WindowsKeyMapper.MapToMaui(args.Key);
            MauiGame.KeyboardReleased(mapped);

            Trace.WriteLine($"[KEY] {args.Key} => {mapped}");
        };
        native.PreviewKeyDown += (sender, args) =>
        {
            var mapped = WindowsKeyMapper.MapToMaui(args.Key);
            MauiGame.KeyboardPressed(mapped);
        };
    }
}


public static class WindowsKeyMapper
{

    /// <summary>
    /// Same as map to Java tbh
    /// </summary>
    /// <param name="virtualKey"></param>
    /// <returns></returns>
    public static MauiKey MapToMaui(VirtualKey virtualKey)
    {
        switch (virtualKey)
        {
        case VirtualKey.Space: return MauiKey.Space;
        case VirtualKey.Left: return MauiKey.ArrowLeft;
        case VirtualKey.Up: return MauiKey.ArrowUp;
        case VirtualKey.Right: return MauiKey.ArrowRight;
        case VirtualKey.Down: return MauiKey.ArrowDown;
        case VirtualKey.Number0: return MauiKey.Digit0;
        case VirtualKey.Number1: return MauiKey.Digit1;
        case VirtualKey.Number2: return MauiKey.Digit2;
        case VirtualKey.Number3: return MauiKey.Digit3;
        case VirtualKey.Number4: return MauiKey.Digit4;
        case VirtualKey.Number5: return MauiKey.Digit5;
        case VirtualKey.Number6: return MauiKey.Digit6;
        case VirtualKey.Number7: return MauiKey.Digit7;
        case VirtualKey.Number8: return MauiKey.Digit8;
        case VirtualKey.Number9: return MauiKey.Digit9;
        case VirtualKey.A: return MauiKey.KeyA;
        case VirtualKey.B: return MauiKey.KeyB;
        case VirtualKey.C: return MauiKey.KeyC;
        case VirtualKey.D: return MauiKey.KeyD;
        case VirtualKey.E: return MauiKey.KeyE;
        case VirtualKey.F: return MauiKey.KeyF;
        case VirtualKey.G: return MauiKey.KeyG;
        case VirtualKey.H: return MauiKey.KeyH;
        case VirtualKey.I: return MauiKey.KeyI;
        case VirtualKey.J: return MauiKey.KeyJ;
        case VirtualKey.K: return MauiKey.KeyK;
        case VirtualKey.L: return MauiKey.KeyL;
        case VirtualKey.M: return MauiKey.KeyM;
        case VirtualKey.N: return MauiKey.KeyN;
        case VirtualKey.O: return MauiKey.KeyO;
        case VirtualKey.P: return MauiKey.KeyP;
        case VirtualKey.Q: return MauiKey.KeyQ;
        case VirtualKey.R: return MauiKey.KeyR;
        case VirtualKey.S: return MauiKey.KeyS;
        case VirtualKey.T: return MauiKey.KeyT;
        case VirtualKey.U: return MauiKey.KeyU;
        case VirtualKey.V: return MauiKey.KeyV;
        case VirtualKey.W: return MauiKey.KeyW;
        case VirtualKey.X: return MauiKey.KeyX;
        case VirtualKey.Y: return MauiKey.KeyY;
        case VirtualKey.Z: return MauiKey.KeyZ;
        case VirtualKey.CapitalLock: return MauiKey.CapsLock;
        case VirtualKey.Insert: return MauiKey.Insert;
        case VirtualKey.Delete: return MauiKey.Delete;
        case VirtualKey.Snapshot: return MauiKey.PrintScreen;
        case VirtualKey.Home: return MauiKey.Home;
        case VirtualKey.End: return MauiKey.End;
        case VirtualKey.PageDown: return MauiKey.PageDown;
        case VirtualKey.PageUp: return MauiKey.PageUp;
        case VirtualKey.Escape: return MauiKey.Escape;
        case VirtualKey.Pause: return MauiKey.Pause;
        case VirtualKey.Menu: return MauiKey.AltLeft;
        case VirtualKey.LeftMenu: return MauiKey.AltLeft;
        case VirtualKey.RightMenu: return MauiKey.AltRight;
        case VirtualKey.Shift: return MauiKey.ShiftLeft;
        case VirtualKey.LeftShift: return MauiKey.ShiftLeft;
        case VirtualKey.RightShift: return MauiKey.ShiftRight;
        case VirtualKey.LeftControl: return MauiKey.ControlLeft;
        case VirtualKey.RightControl: return MauiKey.ControlRight;
        case VirtualKey.Control: return MauiKey.ControlLeft;
        case VirtualKey.Enter: return MauiKey.Enter;
        case VirtualKey.Tab: return MauiKey.Tab;
        case VirtualKey.Back: return MauiKey.Backspace;
        case VirtualKey.F1: return MauiKey.F1;
        case VirtualKey.F2: return MauiKey.F2;
        case VirtualKey.F3: return MauiKey.F3;
        case VirtualKey.F4: return MauiKey.F4;
        case VirtualKey.F5: return MauiKey.F5;
        case VirtualKey.F6: return MauiKey.F6;
        case VirtualKey.F7: return MauiKey.F7;
        case VirtualKey.F8: return MauiKey.F8;
        case VirtualKey.F9: return MauiKey.F9;
        case VirtualKey.F10: return MauiKey.F10;
        case VirtualKey.F11: return MauiKey.F11;
        case VirtualKey.F12: return MauiKey.F12;
        case VirtualKey.NumberKeyLock: return MauiKey.NumLock;
        case VirtualKey.Scroll: return MauiKey.ScrollLock;
        case VirtualKey.NumberPad0: return MauiKey.Numpad0;
        case VirtualKey.NumberPad1: return MauiKey.Numpad1;
        case VirtualKey.NumberPad2: return MauiKey.Numpad2;
        case VirtualKey.NumberPad3: return MauiKey.Numpad3;
        case VirtualKey.NumberPad4: return MauiKey.Numpad4;
        case VirtualKey.NumberPad5: return MauiKey.Numpad5;
        case VirtualKey.NumberPad6: return MauiKey.Numpad6;
        case VirtualKey.NumberPad7: return MauiKey.Numpad7;
        case VirtualKey.NumberPad8: return MauiKey.Numpad8;
        case VirtualKey.NumberPad9: return MauiKey.Numpad9;
        case VirtualKey.LeftWindows: return MauiKey.MetaLeft;
        case VirtualKey.RightWindows: return MauiKey.MetaRight;
        case VirtualKey.Divide: return MauiKey.NumpadDivide;
        case VirtualKey.Multiply: return MauiKey.NumpadMultiply;
        case VirtualKey.Subtract: return MauiKey.NumpadSubtract;
        case VirtualKey.Add: return MauiKey.NumpadAdd;
        }

        if ((int)virtualKey == 187) return MauiKey.Equal;
        if ((int)virtualKey == 189) return MauiKey.Minus;
        if ((int)virtualKey == 192) return MauiKey.Backquote;


        return MauiKey.Unknown;

    }
}

