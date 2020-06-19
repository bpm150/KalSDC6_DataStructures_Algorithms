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

            var host = "bril.ly";
            Console.WriteLine($"Creating a URLShortener using the short host \"{host}\".\n\n");
            var shortener = new URLShortener(host, 6);

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

        // Stick with storing these interally as strings?
        // How much of long string to store? (Incl. the scheme?)
        public class URLShortener
        {
            private readonly Dictionary<string, string> dict;

            private readonly Uri hUri;

            private readonly Random rand;

            // Design: A six-character path after the specified hostname
            private readonly int _keyLen;

            // Could make this a simple array instead of a list
            // Declare it to be of fixed size (62) by adding the ranges
            // Then fill it out at runtime in the constructor
            private readonly List<char> keyChars;

            public URLShortener(string hostName, int keyLen)
            {
                if (hostName == null)
                    throw new ArgumentNullException("param is null");

                _keyLen = keyLen;

                dict = new Dictionary<string, string>();

                // UriBuilder because it elegantly handles adding the http scheme
                // if no scheme was specified
                // A scheme is required for the call to IsWellFormedUriString below


                //// Also makes the OriginalString stored inside each URLShortener
                //// more consistent
                //var hnSlash = hostName;
                //if (hostName.Length > 0 && hostName[^1] != '/')
                //    hnSlash = $"{hostName}/";

                hUri = new UriBuilder(hostName).Uri;
                //{
                //    Host = host,
                //    // At some point, I was having issues with port numbers
                //    // showing up in strings and that causing an issue
                //    // This suppresses the port number being set, I think
                //    //Port = -1,
                //};

                //hUri = ub.Uri;
                //if (!Uri.IsWellFormedUriString(hostUri.ToString(), UriKind.Absolute) ||
                if (!hUri.IsWellFormedOriginalString() ||
                    //Uri.CheckHostName(hostUri.DnsSafeHost) == UriHostNameType.Unknown ||
                    (hUri.PathAndQuery.Length == 1 && hUri.PathAndQuery[0] != '/') ||
                    hUri.PathAndQuery.Length > 1 ||
                    hUri.Fragment.Length > 0)
                        throw new ArgumentException("host name invalid");

                // BUG:
                // Ticks is of type long
                // The Random ctor requires an int
                // Must explicit cast
                rand = new Random((int)DateTime.Now.Ticks);

                keyChars = new List<char>();
                for (var i = 'A'; i <= 'Z'; ++i)
                    keyChars.Add(i);
                for (var j = 'a'; j <= 'z'; ++j)
                    keyChars.Add(j);
                for (var k = '0'; k <= '9'; ++k)
                    keyChars.Add(k);

                // This doesn't help the way that I had hoped it would:
                // https://stackoverflow.com/questions/7578857/how-to-check-whether-a-string-is-a-valid-http-url
                //var hostType = Uri.CheckHostName(d);

                // MAYYBE COME BACK TO THIS LATER
                //// Contains '.'
                //var dotCount = 0;

                //// Better to construct a Uri here and access the Host property
                //foreach (var ch in d)
                //{
                // Hrm. ch is a char after all
                // Why did I think that an individual character read from
                // a string was of type int? Maybe it is sometimes?
                //int c = ((char)ch).ToLower();

                // BUG:
                // ToLower is on the Char class, but it is a static
                // method
                //var c = Char.ToLower(ch);
                //if ((c >= 'a' && c <= 'z') ||
                //    (c >= '0' && c <= '9') ||
                //    c == '-' || c == '.')
                //{
                //    if (c == '.')
                //    {
                //        // ALTERNATE:
                //        //if (++dotCount > 1)
                //        ++dotCount;
                //    }
                //}
                //else
                //    throw new ArgumentException("invalid domain/host syntax");

                //if (dotCount != 1)
                //    throw new ArgumentException("invalid domain/host syntax");
                //}
            }

            public string Shorten(string longUrl)
            {
                if (longUrl == null)
                    throw new ArgumentNullException("param is null");

                // MAYBE URIBUILDER WILL THROW IF IS NOT WELL-FORMED?

                // Clean up the use of UriBuilder, Uri and string types here
                // What would be better?

                // We need UriBuilder to put the default http scheme on the Uri
                // if one was not specified by the caller
                var lUri = new UriBuilder(longUrl).Uri;
                //{
                //    Host = longUrl,
                //    // Suppress the default port getting automatically set by
                //    // the ctor
                //    //Port = -1
                //};
                //var lUri = ubLong.Uri;
                //if (!Uri.IsWellFormedUriString(lUri.ToString(), UriKind.Absolute))
                if (!lUri.IsWellFormedOriginalString() )
                    //Uri.CheckHostName(longUri.DnsSafeHost) == UriHostNameType.Unknown
                    throw new ArgumentException("Cannot shorten. Not a well-formed URL.");

                // Use the lib facilties for this
                //var dStart = lu.IndexOf("//") + 2;
                //var toStore = lu.Substring(dStart);

                // Deciding to store the entire specified uri, including the scheme
                // var longPath = $"{uri.Host}{uri.PathAndQuery}{uri.Fragment}";
                string key;
                do
                    key = GenerateKey();
                while (dict.ContainsKey(key));

                // BUG: Returning the URL the caller specified (less the scheme)
                // instead of the shortened url that they asked for
                //return dict[key] = toStore;

                dict[key] = lUri.ToString();

                // BUG (while fixing bug mentioned above)
                // When creating an object in a return call that will be
                // immediately discarded, still need to call new as you would
                // to the right of the '=' operator

                //var ubShort = new UriBuilder(_host)
                //{
                //    Host = _host,
                //    Path = key,
                //    // At some point, I was having issues with port numbers
                //    // showing up in strings and that causing an issue
                //    // This suppresses the port number being set, I think
                //    //Port = -1,
                //};

                return new Uri(hUri, key).ToString();
            }

            private string GenerateKey()
            {
                var sb = new StringBuilder();
                // BUG:
                // Want six elements
                // for (var i = 0; i <= KEY_LEN; ++i)
                // The above gave seven
                for (var i = 0; i < _keyLen; ++i)
                {
                    // For List, the number of items is Count, not Length
                    //var randInd = rand.Next(keyChars.Length);
                    var randIdx = rand.Next(keyChars.Count);
                    sb.Append(keyChars[randIdx]);
                }
                return sb.ToString();
            }


            // TODO: Finish fixing up Lookup
            public string Lookup(string shortUrl)
            {
                if (shortUrl == null)
                    throw new ArgumentNullException("param is null");

                var sUri = new UriBuilder(shortUrl).Uri;

                // Don't need to explicilty check the hostname here
                // we have exactly what it must be for comparison
                //if (!Uri.IsWellFormedUriString(sUri.ToString(), UriKind.Absolute) ||
                if (!sUri.IsWellFormedOriginalString() ||
                    //sUri.PathAndQuery.Length != KEY_LEN + 1 || 
                    //sUri.PathAndQuery[0] != '/' ||
                    sUri.Fragment.Length > 0)
                        throw new ArgumentException("invalid format for shortened url");
                // We handle the path and the query being correct later on

                //// TODO: For a Uri object, the Host property is what we'd compare
                //var dStart = SU.IndexOf("//") + 2;
                //var dStartStr = SU.Substring(dStart); // Can rewrite with Index and Range
                //var h = dStartStr.Substring(0, _host.Length); // Also Index and Range

                if (sUri.Host != hUri.Host)
                    throw new ArgumentException($"hostname mismatch");

                // BUG:
                // The key string still has the '/' on it
                // (I even check for that!)
                // The if statement would need to check for KEY_LEN + 1
                // But better to check for (then remove) the '/' before
                // calling a substring "key"
                //var key = dStartStr.Substring(dom.Length);
                // ABOVE: THE +1 IS NEEDED TO GET PAST THE '/'

                //var key = dStartStr.Substring(h.Length + 1);
                //if (dStartStr[h.Length] != '/' || key.Length != KEY_LEN)
                //    throw new ArgumentException("bad URL format");

                //// Drop all '/' from path so that it is easy to measure
                //// and also ready to look up if it is the correct length
                //string key;
                //var path = sUri.PathAndQuery.Replace("/", null);
                //// How bad is it to look up a VERY long string in a dictionary
                //// when it is not a valid key?
                //if (path.Length == KEY_LEN && dict.ContainsKey(path))
                //{
                //    key = path;
                //    return dict[key];
                //}

                // Making it shorter for video recording
                //string key;
                var key = sUri.PathAndQuery.Replace("/", null);
                // How bad is it to look up a VERY long string in a dictionary
                // when it is not a valid key?
                if (key.Length == _keyLen && dict.ContainsKey(key))
                    return dict[key];


                // Change in design:
                // Now using scheme originally specified by the user when
                // the long url was shortened, instead of copying over from
                // the shortened url
                //var protoAW = SU.Substring(0, dStart);
                //return $"{protoAW}{dict[key]}";

                throw new ArgumentException("no match found");
            }

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
        }

    }
}
