namespace BinanceAlgorithm.Resourses
{
    public class History
    {
        public string Sumbol { get; set; }
        public int long_bet { get; set; }
        public int short_bet { get; set; }
        public int long_win { get; set; }
        public int short_win { get; set; }
        public int long_loss { get; set; }
        public int short_loss { get; set; }
        public History(string Sumbol, int long_bet, int short_bet, int long_win, int short_win, int long_loss, int short_loss)
        {
            this.Sumbol = Sumbol;
            this.long_bet = long_bet;
            this.short_bet = short_bet;
            this.long_win = long_win;
            this.short_win = short_win;
            this.long_loss = long_loss;
            this.short_loss = short_loss;
        }
    }
}
