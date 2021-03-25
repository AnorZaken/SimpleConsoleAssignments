using System;

namespace ConsoleAssignments
{
    static class ConsoleKeyInfoEx
    {
        public static bool HasModifiersOtherThanShift(this ConsoleKeyInfo keyInfo) => (keyInfo.Modifiers & ~ConsoleModifiers.Shift) != 0;

        // i.e. the key is not printable.
        public static bool IsAControlChar(this ConsoleKeyInfo keyInfo) => char.IsControl(keyInfo.KeyChar);
    }
}
