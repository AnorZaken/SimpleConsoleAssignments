using System;

namespace ConsoleAssignments
{
    public enum PromptUpdateCause
    {
        Backspace,
        BackspaceOnEmpty,
        Enter,
        Escape,
        InputAppended,
        InputHasModifier,
        InputNonPrintable, // not Backspace or Escape, but something similar, e.g. Home, F1, etc.
        InputRejectedByFilter,
        TextMaxLength,
    }

    public record PromptUpdate(string Text, string OldText, CursorPosition CursorPosition, ConsoleKeyInfo Input, PromptUpdateCause Cause)
    {
        public bool IsFinalUpdate => Cause
            is PromptUpdateCause.Enter
            or PromptUpdateCause.Escape;

        public bool IsInputIgnored => Cause
            is PromptUpdateCause.BackspaceOnEmpty
            or PromptUpdateCause.InputHasModifier
            or PromptUpdateCause.InputNonPrintable
            or PromptUpdateCause.InputRejectedByFilter
            or PromptUpdateCause.TextMaxLength;

        public bool IsInputValid => Cause
            is PromptUpdateCause.InputAppended
            or PromptUpdateCause.TextMaxLength;

        public PromptUpdate(string Text, CursorPosition CursorPosition, ConsoleKeyInfo Input, PromptUpdateCause Cause) : this(Text, Text, CursorPosition, Input, Cause) { }
    }
}
