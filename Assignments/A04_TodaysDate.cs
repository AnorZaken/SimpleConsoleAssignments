using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;

namespace ConsoleAssignments.Assignments
{
    using static Console;

    record A04_TodaysDate() : Assignment(4, "Todays Date")
    {
        protected override void Implementation()
        {
            WriteLine("Today's date is " + DateTime.Now.ToLongDateString() + ".");
            WriteBonusFact();
        }


        private void WriteBonusFact()
        {
            const string subject = "aviation"; // there is 'death' , 'birth' , 'event' , 'aviation'
            
            WriteLine();
            Write($" ... Requesting {subject} fact");
            bool success = TryGetFactHelper(out string fact); // <-- synchronous API request (slow, code waits here)
            ConsoleX.ClearRow();
            ConsoleX.Cursor.Position = (0, CursorTop);
            if (success)
                ConsoleX.WriteWords(fact, padRight:1, appendNewLine:false);
            else
                WriteLine($" ... Request for a random {subject} fact failed :(");

            bool TryGetFactHelper(out string fact)
            {
                fact = string.Empty;
                if (!TryRequestEvent(DateTime.Now, out string date, out string text, subject))
                    return false;

                int i = date.IndexOf('-');
                if (i == -1 || !int.TryParse(date.Substring(0, i), out int year))
                    return false;

                StringBuilder result = new();
                int yearsAgo = DateTime.Now.Year - year;
                var header = $"Random {subject} fact about a similar day, from {(yearsAgo == 0 ? "this year" : $"{yearsAgo} years ago")} :";
                result.AppendLine(header);
                result.AppendLine(new string('-', header.Length)); // divider line
                result.AppendLine($"{date}:");
                result.AppendLine(text);
                fact = result.ToString();
                return true;
            }
        }

        private static readonly HttpClient client;

        static A04_TodaysDate()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("http://api.hiztory.org/date/"),
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));
        }

        // example url: http://api.hiztory.org/date/death/03/25/api.xml
        // there is 'death' , 'birth' , 'event' , 'aviation'
        // aviation is the primary data in this API
        /* example response:
            <aviation>
                <status code="200" message="OK"/>
                <event date="1943-03-05" content="Twelve German Heinkel He 111 bombers attack Convoy RA-53 during its voyage from Murmansk in the Soviet Union to Loch Ewe, Scotland, but cause no damage."/>
            </aviation>
        */
        private static bool TryRequestEvent(DateTime date, out string eventDate, out string eventText, string subject = "aviation")
        {
            eventDate = eventText = string.Empty;
            try
            {
                XDocument xDoc;
                HttpResponseMessage response = client.GetAsync(RequestUrlFromDate(date, subject)).Result;

                if (!response.IsSuccessStatusCode)
                    return false;

                xDoc = XDocument.Load(response.Content.ReadAsStream());
                var xElem = xDoc.Element(subject)?.Element("event");
                var xDate = xElem?.Attribute("date");
                var xText = xElem?.Attribute("content");
                if (xDate == null || xText == null)
                    return false;

                eventDate = xDate.Value;
                eventText = xText.Value;
                return true;
            }
            catch
            {
                // don't really care what failed because this is completely optional to the assignment.
                // if we want to debug it we can use a breakpoint.
                return false;
            }

            static string RequestUrlFromDate(DateTime date, string subject)
            {
                return $"{subject}/{date.Month:d2}/{date.Day:d2}/api.xml";
            }
        }
    }
}
