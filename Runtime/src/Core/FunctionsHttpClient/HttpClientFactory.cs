using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;

namespace RGN.Impl.Firebase.Core.FunctionsHttpClient
{
    public class HttpClientFactory
    {
        private const int handlerExpirySeconds = 120;
        private static readonly object factoryLock = new object();

        private static readonly Dictionary<string, HttpClientFactory> factories =
                new Dictionary<string, HttpClientFactory>()
                {
                { string.Empty, new HttpClientFactory() }
                };

        public static HttpClient Get() => factories[string.Empty].GetNewHttpClient();

        // Each unique HttpClientHandler gets a new Connection limit per origin, so create
        // a new "named" client factory to get a new handler (used by each HttpClient from
        // that factory), and thus new set of connections.
        //
        // For example, if you have a few long-running requests, you might choose to put
        // them on their own handler/connections so you don't block other faster requests
        // to the same host.
        public static HttpClient Get(string name)
        {
            HttpClientFactory factory;
            lock (factoryLock)
            {
                if (!factories.TryGetValue(name, out factory))
                {
                    factory = new HttpClientFactory();
                    factories.Add(name, factory);
                }
            }
            return factory.GetNewHttpClient();
        }

        private HttpClientHandler _currentHandler = new HttpClientHandler();
        private readonly Stopwatch _handlerTimer = new Stopwatch();
        private readonly object _handlerLock = new object();

        private HttpClientFactory() { }

        private HttpClient GetNewHttpClient() =>
                new HttpClient(GetHandler(), disposeHandler: false);

        private HttpClientHandler GetHandler()
        {
            lock (_handlerLock)
            {
                if (_handlerTimer.Elapsed.TotalSeconds > handlerExpirySeconds)
                {
                    // Leave the old HttpClientHandler for the GC. DON'T Dispose() it!
                    _currentHandler = new HttpClientHandler();
                    _handlerTimer.Restart();
                }
                return _currentHandler;
            }
        }
    }
}
