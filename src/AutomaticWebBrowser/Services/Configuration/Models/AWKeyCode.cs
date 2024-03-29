﻿using AutomaticWebBrowser.Services.Configuration.Attributes;

namespace AutomaticWebBrowser.Services.Configuration.Models
{
    /// <summary>
    /// 键盘按键键值
    /// </summary>
    enum AWKeyCode
    {
        [Key ("Escape", KeyCode = 27)]
        Escape = 0x0001,
        [Key ("0", ")", KeyCode = 48)]
        Digit0 = 0x0002,
        [Key ("1", "!", KeyCode = 49)]
        Digit1 = 0x0003,
        [Key ("2", "@", KeyCode = 50)]
        Digit2 = 0x0004,
        [Key ("3", "#", KeyCode = 51)]
        Digit3 = 0x0005,
        [Key ("4", "$", KeyCode = 52)]
        Digit4 = 0x0006,
        [Key ("5", "%", KeyCode = 53)]
        Digit5 = 0x0007,
        [Key ("6", "^", KeyCode = 54)]
        Digit6 = 0x0008,
        [Key ("7", "&", KeyCode = 55)]
        Digit7 = 0x0009,
        [Key ("8", "*", KeyCode = 56)]
        Digit8 = 0x000A,
        [Key ("9", "(", KeyCode = 57)]
        Digit9 = 0x000B,
        [Key ("-", "_", KeyCode = 189)]
        Minus = 0x000C,
        [Key ("=", "+", KeyCode = 187)]
        Equal = 0x000D,
        [Key ("Backspace", KeyCode = 8)]
        Backspace = 0x000E,
        [Key ("Tab", KeyCode = 9)]
        Tab = 0x000F,
        [Key ("q", "Q", KeyCode = 81)]
        KeyQ = 0x0010,
        [Key ("w", "W", KeyCode = 87)]
        KeyW = 0x0011,
        [Key ("e", "E", KeyCode = 69)]
        KeyE = 0x0012,
        [Key ("r", "R", KeyCode = 82)]
        KeyR = 0x0013,
        [Key ("t", "T", KeyCode = 84)]
        KeyT = 0x0014,
        [Key ("y", "Y", KeyCode = 89)]
        KeyY = 0x0015,
        [Key ("u", "U", KeyCode = 85)]
        KeyU = 0x0016,
        [Key ("i", "I", KeyCode = 73)]
        KeyI = 0x0017,
        [Key ("o", "O", KeyCode = 79)]
        KeyO = 0x0018,
        [Key ("p", "P", KeyCode = 80)]
        KeyP = 0x0019,
        [Key ("[", "{", KeyCode = 219)]
        BracketLeft = 0x001A,
        [Key ("]", "}", KeyCode = 221)]
        BracketRight = 0x001B,
        [Key ("Enter", KeyCode = 13)]
        Enter = 0x001C,
        [Key ("Control", KeyCode = 17)]
        ControlLeft = 0x001D,
        [Key ("a", "A", KeyCode = 65)]
        KeyA = 0x001E,
        [Key ("s", "S", KeyCode = 83)]
        KeyS = 0x001F,
        [Key ("d", "D", KeyCode = 68)]
        KeyD = 0x0020,
        [Key ("f", "F", KeyCode = 70)]
        KeyF = 0x0021,
        [Key ("g", "G", KeyCode = 71)]
        KeyG = 0x0022,
        [Key ("h", "H", KeyCode = 72)]
        KeyH = 0x0023,
        [Key ("j", "J", KeyCode = 74)]
        KeyJ = 0x0024,
        [Key ("k", "K", KeyCode = 75)]
        KeyK = 0x0025,
        [Key ("l", "L", KeyCode = 76)]
        KeyL = 0x0026,
        [Key (";", ":", KeyCode = 186)]
        Semicolon = 0x0027,
        [Key ("'", "\"", KeyCode = 222)]
        Quote = 0x0028,
        [Key ("`", "~", KeyCode = 192)]
        Backquote = 0x0029,
        [Key ("Shift", KeyCode = 16)]
        ShiftLeft = 0x002A,
        [Key ("\\", "|", KeyCode = 220)]
        Backslash = 0x002B,
        [Key ("z", "Z", KeyCode = 90)]
        KeyZ = 0x002C,
        [Key ("x", "X", KeyCode = 88)]
        KeyX = 0x002D,
        [Key ("c", "C", KeyCode = 67)]
        KeyC = 0x002E,
        [Key ("v", "V", KeyCode = 86)]
        KeyV = 0x002F,
        [Key ("b", "B", KeyCode = 66)]
        KeyB = 0x0030,
        [Key ("n", "N", KeyCode = 78)]
        KeyN = 0x0031,
        [Key ("m", "M", KeyCode = 77)]
        KeyM = 0x0032,
        [Key (",", "<", KeyCode = 188)]
        Comma = 0x0033,
        [Key (".", ">", KeyCode = 190)]
        Period = 0x0034,
        [Key ("/", "?", KeyCode = 191)]
        Slash = 0x0035,
        [Key ("Shift", KeyCode = 16)]
        ShiftRight = 0x0036,
        [Key ("*", KeyCode = 106)]
        NumpadMultiply = 0x0037,
        [Key ("Alt", KeyCode = 18)]
        AltLeft = 0x0038,
        [Key (" ", KeyCode = 32)]
        Space = 0x0039,
        [Key ("CapsLock", KeyCode = 20)]
        CapsLock = 0x003A,
        [Key ("F1", KeyCode = 112)]
        F1 = 0x003B,
        [Key ("F2", KeyCode = 113)]
        F2 = 0x003C,
        [Key ("F3", KeyCode = 114)]
        F3 = 0x003D,
        [Key ("F4", KeyCode = 115)]
        F4 = 0x003E,
        [Key ("F5", KeyCode = 116)]
        F5 = 0x003F,
        [Key ("F6", KeyCode = 117)]
        F6 = 0x0040,
        [Key ("F7", KeyCode = 118)]
        F7 = 0x0041,
        [Key ("F8", KeyCode = 119)]
        F8 = 0x0042,
        [Key ("F9", KeyCode = 120)]
        F9 = 0x0043,
        [Key ("F10", KeyCode = 121)]
        F10 = 0x0044,
        [Key ("7", KeyCode = 103)]
        Numpad7 = 0x0047,
        [Key ("8", KeyCode = 104)]
        Numpad8 = 0x0048,
        [Key ("9", KeyCode = 105)]
        Numpad9 = 0x0049,
        [Key ("-", KeyCode = 109)]
        NumpadSubtract = 0x004A,
        [Key ("4", KeyCode = 100)]
        Numpad4 = 0x004B,
        [Key ("5", KeyCode = 101)]
        Numpad5 = 0x004C,
        [Key ("6", KeyCode = 102)]
        Numpad6 = 0x004D,
        [Key ("+", KeyCode = 107)]
        NumpadAdd = 0x004E,
        [Key ("1", KeyCode = 97)]
        Numpad1 = 0x004F,
        [Key ("2", KeyCode = 98)]
        Numpad2 = 0x0050,
        [Key ("3", KeyCode = 99)]
        Numpad3 = 0x0051,
        [Key ("0", KeyCode = 96)]
        Numpad0 = 0x0052,
        [Key (".", KeyCode = 110)]
        NumpadDecimal = 0x0053,
        [Key ("F11", KeyCode = 12)]
        F11 = 0x0057,
        [Key ("F12", KeyCode = 122)]
        F12 = 0x0058,
        [Key ("F13", KeyCode = 123)]
        F13 = 0x005B,
        [Key ("F14", KeyCode = 124)]
        F14 = 0x005C,
        [Key ("F15", KeyCode = 125)]
        F15 = 0x005D,
        [Key ("F16", KeyCode = 126)]
        F16 = 0x0063,
        [Key ("F17", KeyCode = 127)]
        F17 = 0x0064,
        [Key ("F18", KeyCode = 128)]
        F18 = 0x0065,
        [Key ("F19", KeyCode = 129)]
        F19 = 0x0066,
        [Key ("F20", KeyCode = 130)]
        F20 = 0x0067,
        [Key ("F21", KeyCode = 131)]
        F21 = 0x0068,
        [Key ("F22", KeyCode = 132)]
        F22 = 0x0069,
        [Key ("F23", KeyCode = 133)]
        F23 = 0x006A,
        [Key ("F24", KeyCode = 134)]
        F24 = 0x006B,
        [Key ("Enter", KeyCode = 13)]
        NumpadEnter = 0xE01C,
        [Key ("Control", KeyCode = 17)]
        ControlRight = 0xE01D,
        [Key ("/", KeyCode = 111)]
        NumpadDivide = 0xE035,
        [Key ("AltGraph", KeyCode = 18)]
        AltRight = 0xE038,
        [Key ("NumLock", KeyCode = 144)]
        NumLock = 0xE045,
        [Key ("Home", KeyCode = 36)]
        Home = 0xE047,
        [Key ("ArrowUp", KeyCode = 38)]
        ArrowUp = 0xE048,
        [Key ("PageUp", KeyCode = 33)]
        PageUp = 0xE049,
        [Key ("ArrowLeft", KeyCode = 37)]
        ArrowLeft = 0xE04B,
        [Key ("ArrowRight", KeyCode = 39)]
        ArrowRight = 0xE04D,
        [Key ("End", KeyCode = 35)]
        End = 0xE04F,
        [Key ("ArrowDown", KeyCode = 40)]
        ArrowDown = 0xE050,
        [Key ("PageDown", KeyCode = 34)]
        PageDown = 0xE051,
        [Key ("Insert", KeyCode = 45)]
        Insert = 0xE052,
        [Key ("Delete", KeyCode = 46)]
        Delete = 0xE053,
        [Key ("ContextMenu", KeyCode = 93)]
        ContextMenu = 0xE05D
    }
}
