using System;

namespace ConsoleAssignments
{
    static class ConsoleKeyInfoEx
    {
        // for filtering out non-text like Ctrl+C, Alt+S, etc from Console.ReadKey-inputs.
        public static bool HasModifiersOtherThanShift(this ConsoleKeyInfo keyInfo) => (keyInfo.Modifiers & ~ConsoleModifiers.Shift) != 0;

        // for filtering out non-printable characters like Esc, Sleep, Home, etc from Console.ReadKey-inputs.
        // (NOTE: a few control characters ARE printable, in a sense, for example Enter)
        public static bool IsAControlChar(this ConsoleKeyInfo keyInfo) => char.IsControl(keyInfo.KeyChar);
    }
}
