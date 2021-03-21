using System;

namespace ConsoleAssignments.Assignments
{
    record A03_ColorChanger() : Assignment(3, "Color Changer")
    {
        private ConsoleColorSet colorsDefault = ConsoleX.Colors.Current;
        private ConsoleColorSet colorsAlternative = new ConsoleColorSet(ConsoleColor.DarkMagenta, ConsoleColor.White);
        private bool isToggled = false;

        protected override void WriteHeader()
        {
            ConsoleX.Colors.Current = isToggled ? colorsDefault : colorsAlternative;
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
