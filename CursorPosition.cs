using System;

namespace ConsoleAssignments
{
    public record CursorPosition(int Left, int Top)
    {
        public CursorPosition() : this(Console.CursorLeft, Console.CursorTop) { }

        public CursorPosition Apply()
        {
            (Console.CursorLeft, Console.CursorTop) = (Left, Top);
            return this;
        }

        public CursorPosition GetRelativeMove(int relativeMove, int leftPadding = 0, int rightPadding = 0)
            => CalculateRelativeMove(relativeMove, out _, leftPadding, rightPadding);

        // if it's not possible to move the full requested distance, actualMove will diff from relativeMove.
        public CursorPosition CalculateRelativeMove(int relativeMove, out int actualMove, int leftPadding = 0, int rightPadding = 0)
        {
            int maxX = Console.BufferWidth - rightPadding;
            int oldX = Left - leftPadding;
            if (oldX < 0 || Left > maxX)
                throw new InvalidOperationException("Current cursor position is not within the text area (according to padding).");

            int width = maxX - leftPadding;
            if (width <= 0)
                throw new ArgumentException("Width of the text area is zero. (Too much padding!)");

            int newX = leftPadding + (oldX + relativeMove).Mod(width);
            int newY = Top + (oldX + relativeMove) / width;
            if (newY < 0)
            {
                actualMove = -Top * width + newX - oldX;
                return new CursorPosition(leftPadding, 0);
            }
            else
            {
                actualMove = relativeMove;
                return new CursorPosition(newX, newY);
            }
        }

        public static implicit operator CursorPosition((int Left, int Top) tuple) => new(tuple.Left, tuple.Top);
    }
}
