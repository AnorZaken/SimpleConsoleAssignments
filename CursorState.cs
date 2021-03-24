using System;

namespace ConsoleAssignments
{
    public record CursorState
    {
        public CursorState()
            : this(new(), new(), TryGetIsVisible()) { }

        public CursorState(CursorPosition Position, CursorColors Colors)
            : this(Position, Colors, TryGetIsVisible()) { }

        public CursorState(CursorPosition Position, CursorColors Colors, bool IsVisible)
            => (this.Position, this.Colors, this.IsVisible) = (Position, Colors, IsVisible);

        public CursorPosition Position { get; init; }
        public CursorColors Colors { get; init; }

        public bool IsVisible { get; init; } // ignored on non-windows platforms

        public int Left => Position.Left;
        public int Top => Position.Top;
        public ConsoleColor ForeColor => Colors.Foreground;
        public ConsoleColor BackColor => Colors.Background;

        public CursorState Apply()
        {
            Position.Apply();
            Colors.Apply();
            if (OperatingSystem.IsWindows())
                Console.CursorVisible = IsVisible;
            return this;
        }

        private static bool TryGetIsVisible() => !OperatingSystem.IsWindows() || Console.CursorVisible;
    }
}
