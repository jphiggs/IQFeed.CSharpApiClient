using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using IQFeed.CSharpApiClient.Examples.Common;
using IQFeed.CSharpApiClient.Lookup;

namespace IQFeed.CSharpApiClient.Examples.Examples.FuturesChain
{
    public class FuturesChainExample : IExampleAsync
    {
        public bool Enable => false; // *** SET TO TRUE TO RUN THIS EXAMPLE ***
        public string Name => typeof(FuturesChainExample).Name;
        #region symbols
        private static readonly List<string> Symbols = new List<string>()
        {
            "@ES",
            "@NQ",
            "@BTC",
            "@RTY",
            "@YM",
            "@AD",
            "@BP",
            "@BR",
            "@CD",
            "@EU",
            "@JY",
            "@PX",
            "@SF" 
        };
        #endregion

        public async Task RunAsync()
        {
            // Step 1 - !!! Configure your credentials for IQConnect in user environment variable or app.config !!!
            //              Check the documentation for more information.               

            // Step 2 - Run IQConnect launcher
            IQFeedLauncher.Start();

            // Step 3 - Use the appropriate factory to create the client
            var lookupClient = LookupClientFactory.CreateNew();

            // Step 4 - Connect it
            lookupClient.Connect();

            // Step 5 - Make any requests you need or want!

            foreach (var symbol in Symbols)
            {
                var futureChain1 = await lookupClient.Chains.ReqChainFutureAsync(symbol, "FGHJKMNQUVXZ", "89012334567");
                Console.WriteLine($"Fetched {futureChain1.Count(),3:d} contract months for {symbol}:");
                foreach (var msg in futureChain1)
                {
                    Console.WriteLine($"{msg.Expiration:MMM-yyyy} {msg.FutureRoot} {msg.Symbol} ");
                }
            }
        }

        void IExample.Run()
        {
            throw new System.NotImplementedException();
        }
    }
}

