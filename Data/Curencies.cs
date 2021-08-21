using System.Collections.Generic;

namespace Server
{
    public static class Currencies
    {
        public const string USD = "USD";
        public const string EUR = "EUR";
        public const string GBP = "GBP";
        public const string CNY = "CNY";
        public const string CZK = "CZK";

        public static List<string> List = new List<string>
        {
            USD,EUR,GBP,CNY,CZK
        };
    }
    public static class Coins
    {
        public const string BTC = "BTC";
        public const string  ETH = "ETH";
        public const string BCH = "BCH";
        public const string XRP = "XRP";
        public const string USDT = "USDT";
        public const string USDD = "USDD";
        public const string BNB = "BNB";
        public const string DOGE = "DOGE";
        public const string LINK = "LINK";
        public const string CHZ = "CHZ";
        public const string DOT = "DOT";

        public static List<string> List = new List<string>
        {
            BTC,ETH,BCH,XRP,USDT,USDD,BNB,DOGE,LINK,CHZ,DOT
        };
    }
}