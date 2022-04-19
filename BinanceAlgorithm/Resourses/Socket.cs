using Binance.Net.Clients;
using Binance.Net.Interfaces.Clients.UsdFuturesApi;
using CryptoExchange.Net.Authentication;
using System;

namespace BinanceAlgorithm
{
    public static class Socket
    {
        public static string ApiKey { get; private set; }
        public static string SecretKey { get; private set; }
        public static BinanceClient client = new BinanceClient();
        public static BinanceSocketClient socketClient = new BinanceSocketClient();
        public static IBinanceClientUsdFuturesApi futures { get; set; }
        public static IBinanceSocketClientUsdFuturesStreams futuresSocket { get; set; }
        public static void Connect(string api, string secret)
        {
            try
            {
                ApiKey = api;
                SecretKey = secret;
                client.SetApiCredentials(new ApiCredentials(ApiKey, SecretKey));
                socketClient = new BinanceSocketClient();
                socketClient.SetApiCredentials(new ApiCredentials(ApiKey, SecretKey));
                futures = client.UsdFuturesApi;
                futuresSocket = socketClient.UsdFuturesStreams;
            }
            catch (Exception e)
            {
                ErrorText.Add($"Connect {e.Message}");
            }
        }
    }
}
