using System;

namespace ConsoleAssignments.Assignments
{
    record A03_ColorChanger() : Assignment(3, "Color Changer")
    {
        private CursorColors colorsDefault = ConsoleX.Cursor.Colors;
        private CursorColors colorsAlternative = new CursorColors(ConsoleColor.White, ConsoleColor.DarkMagenta);
        private bool isToggled = false;

        protected override void WriteHeader()
        {
            (isToggled ? colorsDefault : colorsAlternative).Apply();
            isToggled = !isToggled;
            base.WriteHeader();
        }

        protected override void Implementation()
        {
            Console.WriteLine(isToggled
                ? "Switched to alternative colors (run again to switch back)."
                : "Switched back to default colors.");
        }
    }
}
