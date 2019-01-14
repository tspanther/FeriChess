using System;
using System.Collections.Generic;
using System.Net;
using MySql.Data.MySqlClient;

namespace FeriChess.Repositories
{
    /// <summary>
    /// Razred za shranjevanje v in uporabo MySQL baze
    /// primer uporabe
    /// MoveRepository r = new MoveRepository("creativepowercell.asuscomm.com", "Uporabnik", "FeriChess");
    /// </summary>
    public class MoveRepository
    {
        private MySqlConnection Connection;
        /// <summary>
        /// Konstruktor za razred. Potreben za povezavo
        /// </summary>
        /// <param name="address">Naslov do streznika npr. Localhosz</param>
        /// <param name="user"> Uporabnisko ime od racuna za upravljanje s bazo</param>
        /// <param name="pass"> Geslo od racuna za upravljanje s bazo</param>
        public MoveRepository(string address, string user, string pass)
        {
            if (address != String.Empty && user != String.Empty && pass != String.Empty)
            {
                IPAddress[] addresslist = Dns.GetHostAddresses(address);
                address = addresslist[0].ToString();
                string PodatkiPovezave = String.Format("server={0};user={1};database=Sah;port=3306;password={2}", address, user, pass);
                Connection = new MySqlConnection(PodatkiPovezave);
            }
        }
        /// <summary>
        /// Funkcija za dodajanje uporabnika v bazo
        /// </summary>
        /// <param name="user"> Vzdevek, ki ga bo uporabljal uporabnik</param>
        /// <param name="pass"> Geslo, ki ga bo uporabljal uporabnik</param>
        /// <returns></returns>
        public bool DodajIgralca(string user, string pass)
        {
            try
            {
                Connection.Open();
                string Data = "INSERT INTO Igralec (Vzdevek, Geslo, DanRegistracije) VALUES(@Vzdevek, @Geslo, @DanRegistracije)";
                MySqlCommand Command = new MySqlCommand(Data, Connection);
                Command.CommandTimeout = 10;
                Command.Parameters.AddWithValue("@Vzdevek", user);
                Command.Parameters.AddWithValue("@Geslo", pass);
                string datum = DateTime.Today.ToString("yyyy-MM-dd").Replace(".", "-");
                Command.Parameters.AddWithValue("@DanRegistracije", datum);
                Command.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception)
            {
                Connection.Close();
                return false;
            }
        }


        public bool Exsists(string user, string pass)
        {
            bool exsists = false;
            Connection.Open();
            string Command = "SELECT * FROM Igralec WHERE Vzdevek=@Vzdevek AND Geslo=@Geslo";
            MySqlCommand Ukaz = new MySqlCommand(Command, Connection);
            Ukaz.Parameters.AddWithValue("@Vzdevek", user);
            Ukaz.Parameters.AddWithValue("@Geslo", pass);
            MySqlDataReader Branje = Ukaz.ExecuteReader();
            if (Branje.Read())
            {
                exsists = true;
            }
            Branje.Close();
            return exsists;
        }

        public string GetPuzzle(int id)
        {
            Connection.Open();
            string Data = "SELECT * FROM Puzzle WHERE ID=@ID";
            MySqlCommand Command = new MySqlCommand(Data, Connection);
            Command.Parameters.AddWithValue("@ID", id);
            MySqlDataReader Reader = Command.ExecuteReader();
            if (Reader.Read())
            {
                return Reader.GetString("data");
            }
            Reader.Close();
            return "";
        }


        /// <summary>
        /// Funkcija za dodajanje rojstnega datuma
        /// </summary>
        /// <param name="id"> ID od uporabnika aplikacije</param>
        /// <param name="date"> rojstni datum v formatu YYYY-MM-DD (leto, mesec, dan) </param>
        /// <returns>Vrne true ce uspesno spremeni datum rojstva, drugače vrne false</returns>
        public bool AddBirthday(int id, string date)
        {
            try
            {
                Connection.Open();
                string Data = " UPDATE Igralec SET RojstniDan = @rojstniDan WHERE IDigralec = @IDigralec";
                MySqlCommand Command = new MySqlCommand(Data, Connection);
                Command.CommandTimeout = 10;
                Command.Parameters.AddWithValue("@RojstniDan", date);
                Command.Parameters.AddWithValue("@IDigralec", id);
                Command.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception)
            {
                Connection.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za povecanje indeksa odigranih iger
        /// </summary>
        /// <param name="id">ID od uporabnika aplikacije</param>
        /// <returns>Vrne true ce uspesno poveca indeks odigranih iger. Drugače vrne false</returns>
        public bool PovecajSteviloIger(int id)
        {
            try
            {
                Connection.Open();
                string Data = " UPDATE Igralec SET SteviloKoncanihIger = SteviloKoncanihIger + 1 WHERE IDigralec = @IDigralec";
                MySqlCommand Command = new MySqlCommand(Data, Connection);
                Command.CommandTimeout = 10;
                Command.Parameters.AddWithValue("@IDigralec", id);
                Command.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception)
            {
                Connection.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za dodajanje igre v aplikacije
        /// </summary>
        /// <param name="id1"> ID igralca, ki bo igral s belimi figurami</param>
        /// <param name="id2"> ID igralca, ki bo igral s crnimi figurami</param>
        /// <param name="Inc"> Indeks za povecevanje casa potez</param>
        /// <param name="time"> Cas potez v formatu HH-MM-SS (ure, minute, sekunde)</param>
        /// <returns>Vrne true če uspešno doda igro. Drugače vrne false</returns>
        public bool Addgame(int id1, int id2, int Inc, string time)
        {
            try
            {
                Connection.Open();
                string Data = "INSERT INTO Igra (Igralec0, Igralec1, Inc, Cas) VALUES (@Igralec1ID , @Igralec2ID, @Inc, @Cas)";
                MySqlCommand Command = new MySqlCommand(Data, Connection);
                Command.CommandTimeout = 10;
                Command.Parameters.AddWithValue("@Igralec1ID", id1);
                Command.Parameters.AddWithValue("@Igralec2ID", id2);
                Command.Parameters.AddWithValue("@Inc", Inc);
                Command.Parameters.AddWithValue("@Cas", time);
                Command.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception)
            {
                Connection.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za dodajanje končnega stanja igre
        /// </summary>
        /// <param name="gameid"> ID od igre, ki jo želimo dokončati</param>
        /// <param name="state"> Končno stanje šahovnice</param>
        /// <returns></returns>
        public bool NastaviKoncnoStanjeIgre(int gameid, string state)
        {
            try
            {
                Connection.Open();
                string PodatkiUkaza = " UPDATE Igra SET KoncnoStanje = @KoncnoStanje WHERE IDigre = @IDigre";
                MySqlCommand Ukaz = new MySqlCommand(PodatkiUkaza, Connection);
                Ukaz.CommandTimeout = 10;
                Ukaz.Parameters.AddWithValue("@KoncnoStanje", state);
                Ukaz.Parameters.AddWithValue("@IDigre", gameid);
                Ukaz.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception)
            {
                Connection.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za dodajanje zmagovalca k igri
        /// </summary>
        /// <param name="id"> ID igre k kateri dodajamo zmagovalca</param>
        /// <param name="winner"> vrednost zmagovalca. 0 je beli igralec. 1 je črni</param>
        /// <returns>Vrne true če uspešno doda igralca, drugače vrne false</returns>
        public bool NastaviZmagovalcaIgre(int id, bool winner)
        {
            try
            {
                Connection.Open();
                string Data = " UPDATE Igra SET Zmagovalec = @Zmagovalec WHERE IDigre = @IDigre";
                MySqlCommand Command = new MySqlCommand(Data, Connection);
                Command.CommandTimeout = 10;
                Command.Parameters.AddWithValue("@Zmagovalec", winner);
                Command.Parameters.AddWithValue("@IDigre", id);
                Command.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception)
            {
                Connection.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za dodajanje  poteze k igri
        /// </summary>
        /// <param name="gameid">ID igre k kateri dodajamo</param>
        /// <param name="moves"> niz poteze, ki jo dodajamo</param>
        /// <param name="time"> čas kako dolgo je trajala poteza HH-MM-SS (ure, minute, sekunde)</param>
        /// <returns> vrne true, če uspešno doda potezo drugače vrne false</returns>
        public bool DodajPotezo(int gameid, string moves, string time)
        {
            try
            {
                Connection.Open();
                string data = "INSERT INTO Poteze (Igra_IDigre, Poteza, CasPoteze) VALUES (@IDigre, @Poteza, @Cas)";
                MySqlCommand Command = new MySqlCommand(data, Connection);
                Command.CommandTimeout = 10;
                Command.Parameters.AddWithValue("@IDigre", gameid);
                Command.Parameters.AddWithValue("@Poteza", moves);
                Command.Parameters.AddWithValue("@Cas", time);
                Command.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception)
            {
                Connection.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za dodajanje sporočila k igri
        /// </summary>
        /// <param name="id"> ID igralca, ki pošilja sporočilo</param>
        /// <param name="gameid"> ID igre k kateri dodajamo sporočilo</param>
        /// <param name="data"> Vsebina sporočila, ki ga dodajamo</param>
        /// <returns>Vrne true, če uspešno doda sporočilo. Drugače vrne false</returns>
        public bool DodajSporocilo(int id, int gameid, string data)
        {
            try
            {
                Connection.Open();
                string commandData = "INSERT INTO Sporocilo (Vsebina, Igralec_IDigralec, Igra_IDigre, Cas) VALUES (@Vsebina, @Igralec_IDigralec, @Igra_IDigre , NOW() )";
                MySqlCommand Command = new MySqlCommand(commandData, Connection);
                Command.CommandTimeout = 10;
                Command.Parameters.AddWithValue("@Vsebina", data);
                Command.Parameters.AddWithValue("@Igralec_IDigralec", id);
                Command.Parameters.AddWithValue("@Igra_IDigre", gameid);
                Command.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception)
            {
                Connection.Close();
                return false;
            }
        }

        /// <summary>
        /// Funkcija, ki vrne vzdevek igralca iz njegovega ID
        /// </summary>
        /// <param name="ID">ID igralca</param>
        /// <returns>vrne vzdevek igralca</returns>
        public string name2id(int ID)
        {
            string vzdevek = String.Empty;
            try
            {
                Connection.Open();
                string data = "SELECT Vzdevek FROM Igralec WHERE IDigralec = @ID ";
                MySqlCommand Command = new MySqlCommand(data, Connection);
                Command.Parameters.AddWithValue("@ID", ID);
                MySqlDataReader Reader = Command.ExecuteReader();
                Reader.Read();
                vzdevek = Reader["Vzdevek"].ToString();
                Reader.Close();
            }
            catch (Exception)
            {
                return vzdevek;
            }
            Connection.Close();
            return vzdevek;
        }
        /// <summary>
        /// Funkcija, ki vrne ID iz vzdevka igralca
        /// </summary>
        /// <param name="id">Vzdevek iz katerega zelimo dobiti ID</param>
        /// <returns>vrne ID od izbranega igralca</returns>
        public int id2nick(string id)
        {
            string nick = String.Empty;
            try
            {
                Connection.Open();
                string data = "SELECT IDigralec FROM Igralec WHERE Vzdevek = @Vzdevek ";
                MySqlCommand Command = new MySqlCommand(data, Connection);
                Command.Parameters.AddWithValue("@Vzdevek", id);
                MySqlDataReader Reader = Command.ExecuteReader();
                Reader.Read();
                nick = Reader["IDigralec"].ToString();
                Reader.Close();
            }
            catch (Exception)
            {
            }
            Connection.Close();
            return int.Parse(nick);
        }
    }
}