using System;

namespace ConsoleAssignments.Assignments
{
    // Funktion där användaren ska ange namnet på sin karaktär och namnet på en motståndare.
    // Funktionen skall sedan själv lägga till slumpmässiga värden för Hälsa, Styrka och Tur, som sparas i en instans av en klass.
    record A16_RngDnD() : Assignment(16, "Rng DnD")
    {
        private static readonly Random rng = new();

        protected override void Implementation()
        {
            string? namnJag = ConsoleX.PromptLine("Mata in ditt egna karaktärsnamn: ");
            string? namnMot = ConsoleX.PromptLine("Samt din motståndares karaktärsnamn: ");

            if (string.IsNullOrWhiteSpace(namnJag))
                Console.WriteLine("Eftersom ditt namn är tomt så fick du namnet " + (namnJag = "Emelia Glensdotter"));
            if (string.IsNullOrWhiteSpace(namnMot))
                Console.WriteLine("Eftersom din motståndares namn är tomt så fick hen namnet " + (namnMot = "Jeeves Jr Cottburg"));

            var karJag = InstansieraKaraktär(namnJag);
            var karMot = InstansieraKaraktär(namnMot);

            Console.WriteLine();
            Console.WriteLine("I slumpens namn genererades följande karaktärsdata:");
            Console.WriteLine();
            Console.WriteLine(karJag);
            Console.WriteLine(karMot);
        }

        // (jag vet att koneventionen är att inte använda specialtecken i namn,
        // jag valde att ignorera det för denna uppgiften för att följa uppgiften ordagrant)
        private static Karaktär InstansieraKaraktär(string namn)
        {
            int hpMax = rng.Next(30, 51);
            return new Karaktär(namn)
            {
                MaxHälsa = hpMax,
                Hälsa = rng.Next(5, hpMax + 1),
                Styrka = rng.Next(2, 13),
                Tur = rng.Next(2, 13),
            };
        }

        public class Karaktär
        {
            public Karaktär(string namn) => Namn = namn;

            private int _hälsa;
            
            public int Hälsa { get => _hälsa; init => _hälsa = value; }
            public int MaxHälsa { get; init; }
            public int Styrka { get; init; }
            public int Tur { get; init; }
            public string Namn { get; init; }

            public override string ToString()
            {
                return $"Karaktär:{{Namn: {Namn}, Hälsa: {Hälsa}/{MaxHälsa}, Styrka: {Styrka}, Tur: {Tur}}}";
            }
        }
    }
}
