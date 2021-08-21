using System.ComponentModel.DataAnnotations;

namespace Server
{
    public class UserWallet 
    {
        [Key]
        public string UserId { get; set; }
        public double LastReceived { get; set; }
        public double LastSent { get; set; }
        public double BTC { get; set; }
        public double ETH { get; set; }
        public double BCH { get; set; }
        public double XRP { get; set; }
        public double BNB { get; set; }
        public double DOGE { get; set; }
        public double LINK { get; set; }
        public double CHZ { get; set; }
        public double DOT { get; set; }
        public double USDT { get; set; }
        public double USDD { get; set; }
        public string Currency { get; set; }

        public double GetCoin(string coin)
        {
            double ret = 0;
            if(coin == nameof(BTC))ret = BTC;
            else if(coin == nameof(ETH)) ret = ETH;
            else if(coin == nameof(BCH)) ret = BCH;
            else if(coin == nameof(XRP)) ret = XRP;
            else if(coin == nameof(BNB)) ret = BNB;
            else if(coin == nameof(DOGE)) ret = DOGE;
            else if(coin == nameof(LINK)) ret = LINK;
            else if(coin == nameof(CHZ)) ret = CHZ;
            else if(coin == nameof(DOT)) ret = DOT;
            else if(coin == nameof(USDT)) ret = USDT;
            else if(coin == nameof(USDD)) ret = USDD;
            return ret;
        }
    }
}