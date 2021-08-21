using System.Collections.Generic;

namespace Server
{
    public class CryptoWallet
    {
        public string Coin { get; set; }
        public string Address { get; set; }
    }
    
    public static class CryptoWallets
    {
        public static List<CryptoWallet> List = new List<CryptoWallet>
        {
            new CryptoWallet { Coin = Coins.BTC, Address = "bc1q0yzm3z46usqnwutgr7jjrq87pupgpywa3967ex"},
            new CryptoWallet{ Coin = Coins.BCH, Address = "qp2xlvu26jjvpg95hwv77hg8jr0vyfnnqu6u36mtg6"},
            new CryptoWallet { Coin = Coins.BNB, Address = "bnb1c6sxur8apucr7mqa94r3n4tmglnwrnqz53hg2a"},
            new CryptoWallet { Coin = Coins.CHZ, Address = "0x07Ab690D9bE6De151F079897b30D61D3e930B121"},
            new CryptoWallet { Coin = Coins.DOGE, Address = "DAjvzianc68ZUurDo5NXjBPh9n6G7w93yt"},
            new CryptoWallet { Coin = Coins.DOT, Address = "1vK9tmrXWFTf8uWhtKzEaPc58MEeSwKfq8KqeqtHurGku1x"},
            new CryptoWallet { Coin = Coins.ETH, Address = "0x07Ab690D9bE6De151F079897b30D61D3e930B121"},
            new CryptoWallet { Coin = Coins.LINK, Address = "0x07Ab690D9bE6De151F079897b30D61D3e930B121"},
            new CryptoWallet { Coin = Coins.USDD, Address = "0x03751e04cd2ea11a95f94d3d840ae7608296cad8"},
            new CryptoWallet { Coin = Coins.USDT, Address = "0x07Ab690D9bE6De151F079897b30D61D3e930B121"},
            new CryptoWallet { Coin = Coins.XRP, Address = "rG8LemVxWzQjBRWpSyCBdxaeirnnYLqBjZ"}
        };
    }

}