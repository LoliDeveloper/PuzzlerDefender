namespace PuzzlerDefender
{
    class PersonData
    {
        public PersonData(int adsShow, int adsInterval) { AdsShow = adsShow; AdsInterval = adsInterval; }
        public int HPDino { get; set; }
        public int FullHPDino { get; set; }
        public int EasySecRecord { get; set; }
        public int MediumSecRecord { get; set; }
        public int HardSecRecord { get; set; }
        public string StatusData { get; set; }
        public int Coins { get; set; }
        int AdsShow { get; set; }
        int AdsInterval { get; set; }
    }
}