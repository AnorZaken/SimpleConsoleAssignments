namespace ConsoleAssignments
{
    public static class Modulus
    {
        // This function is the proper mathematical modulus.
        // (The %-operator is a remainder, incorrectly called modulus by progremmer convention.)
        public static int Mod(this int number, int modulus)
        {
            int r = number % modulus;
            return r + (r >> 31 & modulus); // functionally equivalent to " return r < 0 ? r + modulus : r; " but without the branching.
        }
    }
}
