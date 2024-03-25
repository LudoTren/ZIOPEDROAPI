namespace ZIOPEDROAPI
{
    public class Bet
    {
        public int IdTrain { get; set; }

        public DateTime TimeStamp;
        public int CodiceTreno;

    }
    public class Fermata
    {
        public string Id { get; set; }
        private string stazione;
        public string Stazione { get { return stazione; } set { if (value == null) stazione = string.Empty; else stazione = value; } }
        private string provvedimenti;
        public string Provvedimenti { get { return provvedimenti; } set { if (value == null) provvedimenti = string.Empty; else provvedimenti = value; } }
        public long Programmata { get; set; }
        public int Ritardo { get; set; }
        public object Partenza_Teorica { get; set; }
        public object Arrivo_Teorico { get; set; }
        public object PartenzaReale { get; set; }
        public object ArrivoReale { get; set; }
        public int RitardoPartenza { get; set; }
        public int RitardoArrivo { get; set; }
    }

    internal class Treno
    {
        public string NumeroTreno { get; set; }
        public string OrigineZero { get; set; }
        public string DestinazioneZero { get; set; }
        public List<Fermata> Fermate { get; set; }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        public string DisplayTrainData()
        {
            string s = ($"\nNumero Treno: {NumeroTreno} \nParte: {OrigineZero}\nArriva: {DestinazioneZero}\n");
            try
            {
                s += $"Orario: {UnixTimeStampToDateTime(Fermate[0].Programmata / 1000)}";
            }
            catch { }

            return s;
            /*foreach (Fermata f in Fermate)
            {
                try
                {
                    Console.WriteLine($"Stazione:{f.Stazione}");
                    Console.WriteLine($"Ritardo Arrivo: {f.RitardoArrivo}");
                    Console.WriteLine($"Ritardo Partenza: {f.RitardoPartenza}");
                    System.DateTime dat_Time = UnixTimeStampToDateTime((long)f.Arrivo_Teorico / 1000);
                    string print_the_Date = dat_Time.ToShortDateString() + " " + dat_Time.ToShortTimeString();
                    Console.WriteLine($"ArrivoTeorico: {print_the_Date}");

                    dat_Time = UnixTimeStampToDateTime((long)f.ArrivoReale / 1000);
                    print_the_Date = dat_Time.ToShortDateString() + " " + dat_Time.ToShortTimeString();
                    Console.WriteLine($"ArrivoReale: {print_the_Date}");



                    dat_Time = UnixTimeStampToDateTime((long)f.Partenza_Teorica / 1000);
                    print_the_Date = dat_Time.ToShortDateString() + " " + dat_Time.ToShortTimeString();
                    Console.WriteLine($"Partenza Teoria: {print_the_Date}");

                    dat_Time = UnixTimeStampToDateTime((long)f.PartenzaReale / 1000);
                    print_the_Date = dat_Time.ToShortDateString() + " " + dat_Time.ToShortTimeString();
                    Console.WriteLine($"PartenzaReale: {print_the_Date}");

                    
                    

                }
                catch (Exception ex) { Debug.WriteLine(ex); }
                Console.WriteLine("\n");
            }
            */
            // Aggiungi qui altri campi che vuoi visualizzare
        }
    }
}
