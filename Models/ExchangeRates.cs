using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Server
{
    public static class ExchangeRates
    {
        public static double USD { get; set; }
        public static double GBP { get; set; }
        public static double EUR { get; set; }
        public static double CNY { get; set; }
        public static double CZK { get; set; }
        public static double BTC { get; set; }
        public static double ETH { get; set; }
        public static double BCH { get; set; }
        public static double XRP { get; set; }
        public static double USDT { get; set; }
        public static double USDD { get; set; }
        public static double BNB { get; set; }
        public static double DOGE { get; set; }
        public static double LINK { get; set; }
        public static double CHZ { get; set; }
        public static double DOT { get; set; }

        public static void UpdateRates(ExchangeRatesResponse res)
        {
            // usd
            USD = 1;
            USDT = res.rates.First(r => r.asset_id_quote == nameof(USDT)).rate;
            USDD = USDT;
            // GBP
            GBP = res.rates.First(r => r.asset_id_quote == nameof(GBP)).rate;
            // EUR
            EUR = res.rates.First(r => r.asset_id_quote == nameof(EUR)).rate;
            // CNY
            CNY = res.rates.First(r => r.asset_id_quote == nameof(CNY)).rate;
            // CZK
            CZK = res.rates.First(r => r.asset_id_quote == nameof(CZK)).rate;
            // BTC
            BTC = res.rates.First(r => r.asset_id_quote == nameof(BTC)).rate;
            // ETH
            ETH = res.rates.First(r => r.asset_id_quote == nameof(ETH)).rate;
            // BCH
            BCH = res.rates.First(r => r.asset_id_quote == nameof(BCH)).rate;
            // XRP
            XRP = res.rates.First(r => r.asset_id_quote == nameof(XRP)).rate;
            // BNB
            BNB = res.rates.First(r => r.asset_id_quote == nameof(BNB)).rate;
            // DOGE
            DOGE = res.rates.First(r => r.asset_id_quote == nameof(DOGE)).rate;
            // LINK
            LINK = res.rates.First(r => r.asset_id_quote == nameof(LINK)).rate;
            // CHZ
            CHZ = res.rates.First(r => r.asset_id_quote == nameof(CHZ)).rate;
            // DOT
            DOT = res.rates.First(r => r.asset_id_quote == nameof(DOT)).rate;
        }

    }

    public class RatesResponse
    {
        public double USD { get; set; }
        public double GBP { get; set; }
        public double EUR { get; set; }
        public double CNY { get; set; }
        public double CZK { get; set; }
        public double BTC { get; set; }
        public double ETH { get; set; }
        public double BCH { get; set; }
        public double XRP { get; set; }
        public double USDT { get; set; }
        public double USDD { get; set; }
        public double BNB { get; set; }
        public double DOGE { get; set; }
        public double LINK { get; set; }
        public double CHZ { get; set; }
        public double DOT { get; set; }

        public RatesResponse()
        {
            USD = ExchangeRates.USD;
            GBP = ExchangeRates.GBP;
            EUR = ExchangeRates.EUR;
            CNY = ExchangeRates.CNY;
            CZK = ExchangeRates.CZK;
            BTC = ExchangeRates.BTC;
            ETH = ExchangeRates.ETH;
            BCH = ExchangeRates.BCH;
            XRP = ExchangeRates.XRP;
            USDT = ExchangeRates.USDT;
            USDD = ExchangeRates.USDD;
            BNB = ExchangeRates.BNB;
            DOGE = ExchangeRates.DOGE;
            LINK = ExchangeRates.LINK;
            CHZ = ExchangeRates.CHZ;
            DOT = ExchangeRates.DOT;
        }
    }

    public class ExchangeRatesResponse
    {
        public string asset_id_base { get; set; }
        public List<Rates> rates { get; set; }
    }
    public class Rates
    {
        public DateTime time { get; set; }
        public string asset_id_quote { get; set; }
        public double rate { get; set; }
    }
}