using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment6
{
    class Problem10
    {

        public static void RunInteractiveTesting()
        {
            string intro =
                "==============\n" +
                "= Problem #10 =\n" +
                "==============\n" +
                "\n" +
                "Design a URL shortener like bit.ly.\n";

            Console.WriteLine(intro);

            var domain = "bril.ly";
            Console.WriteLine($"Creating a URLShortener using the short domain \"{domain}\".");
            var shortener = new URLShortener(domain);

            string input = String.Empty;
            while (input != "done")
            {
                Console.WriteLine(shortener.Dict_Debug_View);

                Console.WriteLine("\nEnter URLShortener command or \"done\"\n");
                input = Console.ReadLine();
                string[] commands = input.Split(' ');

                if (commands[0] == "shorten")
                {
                    var item = commands[1];
                    var shortened = shortener.Shorten(item);
                    Console.WriteLine($"Shortened to: {shortened}\n");
                }
                else if (commands[0] == "lookup")
                {
                    var item = commands[1];
                    var lookedup = shortener.Lookup(item);
                    Console.WriteLine($"Looked up: {lookedup}\n");
                }
            }
        }

        public class URLShortener
        {
            private const int KEY_LEN = 6;

            public URLShortener(string d)
            {
                dict = new Dictionary<string, string>();
                // BUG:
                // Ticks is of type long
                // The Random ctor requires an int
                // Must explicit cast
                rand = new Random((int)DateTime.Now.Ticks);

                var dotCount = 0;

                // Better to construct a Uri here and access the Host property
                foreach (var ch in d)
                {
                    // Hrm. ch is a char after all
                    // Why did I think that an individual character read from
                    // a string was of type int? Maybe it is sometimes?
                    //int c = ((char)ch).ToLower();

                    // BUG:
                    // ToLower is on the Char class, but it is a static
                    // method
                    var c = Char.ToLower(ch);
                    if ((c >= 'a' && c <= 'z') ||
                        (c >= '0' && c <= '9') ||
                        c == '-' || c == '.')
                    {
                        if (c == '.')
                        {
                            // ALTERNATE:
                            //if (++dotCount > 1)
                            ++dotCount;
                            if (dotCount > 1)
                                throw new ArgumentException("invalid domain/host syntax");
                        }
                    }
                    else
                        throw new ArgumentException("invalid domain/host syntax");
                }

                Domain = d;

                keyChars = new List<int>();
                for (var i = 'A'; i <= 'Z'; ++i)
                    keyChars.Add(i);
                for (var j = 'a'; j <= 'z'; ++j)
                    keyChars.Add(j);
                for (var k = '0'; k <= '9'; ++k)
                    keyChars.Add(k);
            }

            public string Shorten(string lu)
            {
                if (lu == null)
                    throw new ArgumentNullException("string lu is null");

                // Clean up the use of UriBuilder, Uri and string types here
                // What would be better?
                var ub1 = new UriBuilder(lu)
                {
                    // Suppress the default port getting automatically set by
                    // the ctro
                    Port = -1
                };
                var uri = ub1.Uri;
                if (!Uri.IsWellFormedUriString(uri.ToString(), UriKind.Absolute))
                    throw new ArgumentException($"string lu = \"{lu}\" " +
                        $"is not a well-formed URL");

                // Use the lib facilties for this
                //var dStart = lu.IndexOf("//") + 2;
                //var toStore = lu.Substring(dStart);

                var longPath = $"{uri.Host}{uri.PathAndQuery}{uri.Fragment}";
                string key;
                do
                    key = GenerateKey();
                while (dict.ContainsKey(key));

                // BUG: Returning the URL the caller specified (less the scheme)
                // instead of the shortened url that they asked for
                //return dict[key] = toStore;

                dict[key] = longPath;

                // BUG (while fixing bug mentioned above)
                // When creating an object in a return call that will be
                // immediately discarded, still need to call new as you would
                // to the right of the '=' operator

                var ub2 = new UriBuilder(Domain)
                {
                    Path = key
                };

                return ub2.Uri.ToString();
            }

            private string GenerateKey()
            {
                var sb = new StringBuilder();
                // BUG:
                // Want six elements
                // for (var i = 0; i <= KEY_LEN; ++i)
                // The above gave seven
                for (var i = 0; i < KEY_LEN; ++i)
                {
                    // For List, the number of items is Count, not Length
                    //var randInd = rand.Next(keyChars.Length);
                    var randInd = rand.Next(keyChars.Count);
                    sb.Append((char)keyChars[randInd]);
                }
                return sb.ToString();
            }


            // Uri.CheckHostName
            // validates hostname

            public string Lookup(string su)
            {
                if (su == null)
                    throw new ArgumentNullException("string su is null");

                // BUG:
                // The built-in class that knows about Urls is "Uri" not "Url"
                // The static (non-constructing) method is
                // "IsWellFormedUriString", not "IsValid"
                if (!Uri.IsWellFormedUriString(su, UriKind.Absolute))
                    throw new ArgumentException($"string su = \"{su}\"" +
                        $"is not a well-formed URL.");

                // For a Uri object, the Host property is what we'd compare

                var dStart = su.IndexOf("//") + 2;
                var dStartStr = su.Substring(dStart); // Can rewrite with Index and Range
                var dom = dStartStr.Substring(0, Domain.Length); // Also Index and Range
                if (dom != Domain)
                    throw new ArgumentException($"not the domain for this" +
                        $"URL shortener.");

                // BUG:
                // The key string still has the '/' on it
                // (I even check for that!)
                // The if statement would need to check for KEY_LEN + 1
                // But better to check for (then remove) the '/' before
                // calling a substring "key"
                //var key = dStartStr.Substring(dom.Length);
                // ABOVE: THE +1 IS NEEDED TO GET PAST THE '/'


                var key = dStartStr.Substring(dom.Length + 1);
                if (dStartStr[dom.Length] != '/' || key.Length != KEY_LEN)
                    throw new ArgumentException("bad URL format");

                if (dict.ContainsKey(key))
                {
                    var protoAW = su.Substring(0, dStart);
                    return $"{protoAW}{dict[key]}";
                }

                throw new ArgumentException("URL not found");
            }

            private Dictionary<string, string> dict;

            public string Dict_Debug_View
            {
                get
                {
                    var sb = new StringBuilder();

                    // Remember that Append puts all of this into one long string
                    // Newlines aren't added after the Append calls
                    // Got to put them there if you want them there

                    sb.Append(
                        "Dictionary<string, string> dict:\n" +
                        "Key\tValue");
                    foreach (var kvp in dict)
                        sb.Append($"\n{kvp.Key}\t{kvp.Value}");

                    return sb.ToString();
                }
            }


            // readonly??
            public string Domain { get; private set; }

            private Random rand;

            // Could make this a simple array instead of a list
            // Declare it to be of fixed size (62) by adding the ranges
            // Then fill it out at runtime in the constructor
            private List<int> keyChars;
        }

    }
}
