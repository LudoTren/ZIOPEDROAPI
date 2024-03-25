using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Security;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Net.Http;
using MySqlX.XDevAPI;
namespace ZIOPEDROAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BetInfoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BetInfoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet(Name = "BetsController")]
        /*public string Get()
        {
            MySqlConnection sql = new MySqlConnection();
            sql.ConnectionString = "Server=localhost;Database=LudoTrain;Uid=user;Pwd=user123;";
            sql.Open();
            MySqlCommand cmd = sql.CreateCommand();
            cmd.CommandText = "SELECT * FROM Treno";
            List<Bet> bets= new List<Bet>();
            MySqlDataReader reader=cmd.ExecuteReader();
            while (reader.Read())
            {
                bets.Add(new Bet()
                {
                    IdTrain = reader.GetFieldValue<int>("idTreno"),
                    TimeStamp = reader.GetFieldValue<DateTime>("TimeStamp"),
                    CodiceTreno=reader.GetFieldValue<int>("codiceTreno")
                }) ;
            }
            if (bets.Count > 0)
            {
                return JsonConvert.SerializeObject(bets);
            }
            else
            {
                Response.StatusCode = 100;
                return "";
            }


        }*/
        public string Get(int id)
        {
            string s;
            
            return JsonConvert.SerializeObject(RicercaTreno(id.ToString()));
        }
        static Treno PrintTrain(string trainCode, string stationCode, long timestamp)
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = string.Format("http://www.viaggiatreno.it/infomobilita/resteasy/viaggiatreno/andamentoTreno/{1}/{0}/{2}", stationCode, trainCode, timestamp.ToString());

                var task = Task.Run(() => client.GetAsync(url));
                task.Wait();
                HttpResponseMessage response =task.Result;
                if (response.IsSuccessStatusCode)
                {
                    var task2=Task.Run(() => response.Content.ReadAsStringAsync());
                    task2.Wait();
                    string json =task2.Result;
                    Treno trainData;
                    // Deserializza il JSON in un oggetto TrainData
                    trainData = Newtonsoft.Json.JsonConvert.DeserializeObject<Treno>(json);
                    Newtonsoft.Json.JsonSerializer writer = new Newtonsoft.Json.JsonSerializer();
                    // Visualizza i dati del treno
                    return trainData;


                }
                else
                {
                    Console.WriteLine("Errore nella richiesta: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Si è verificato un errore: " + ex.Message);
            }
            return null;
        }

        static Treno RicercaTreno(string codiceTreno)
        {
            string url = string.Format("http://www.viaggiatreno.it/infomobilita/resteasy/viaggiatreno/cercaNumeroTrenoTrenoAutocomplete/{0}", codiceTreno);
            using (HttpClient client1 = new HttpClient())
            {
                var task = Task.Run(() => client1.GetAsync(url));
                task.Wait();
                HttpResponseMessage response = task.Result;

                if (response.IsSuccessStatusCode)
                {
                    var task2 = Task.Run(() => response.Content.ReadAsStringAsync());
                    task2.Wait();
                    string trainData = task2.Result;

                    try
                    {
                        string traindInfo = trainData.Split('|')[1];
                        return PrintTrain(traindInfo.Split('-')[1].Trim(), traindInfo.Split('-')[0].Trim(), long.Parse(traindInfo.Split('-')[2].Trim()));
                    }
                    catch { }

                }
                else
                {
                    Console.WriteLine("Errore nella richiesta: " + response.StatusCode);
                }
            }
            return null;
        }
    }
}
