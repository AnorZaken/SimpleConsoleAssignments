namespace ConsoleAssignments
{
    public static class Modulus
    {
        public static int Mod(this int number, int modulus)
        {
            int r = number % modulus;
            return r + (r >> 31 & modulus); // equivalent to " return r < 0 ? r + modulus : r; " but without branching.
        }
    }
}
