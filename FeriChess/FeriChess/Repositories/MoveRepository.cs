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
        private MySqlConnection Povezava;
        /// <summary>
        /// Konstruktor za razred. Potreben za povezavo
        /// </summary>
        /// <param name="naslov">Naslov do streznika npr. Localhosz</param>
        /// <param name="uporabnisko"> Uporabnisko ime od racuna za upravljanje s bazo</param>
        /// <param name="geslo"> Geslo od racuna za upravljanje s bazo</param>
        public MoveRepository(string naslov, string uporabnisko, string geslo)
        {
            if (naslov != String.Empty && uporabnisko != String.Empty && geslo != String.Empty)
            {
                IPAddress[] addresslist = Dns.GetHostAddresses(naslov);
                naslov = addresslist[0].ToString();
                string PodatkiPovezave = String.Format("server={0};user={1};database=Sah;port=3306;password={2}", naslov, uporabnisko, geslo);
                Povezava = new MySqlConnection(PodatkiPovezave);
            }
        }
        /// <summary>
        /// Funkcija za dodajanje uporabnika v bazo
        /// </summary>
        /// <param name="vzdevek"> Vzdevek, ki ga bo uporabljal uporabnik</param>
        /// <param name="geslo"> Geslo, ki ga bo uporabljal uporabnik</param>
        /// <returns></returns>
        public bool DodajIgralca(string vzdevek, string geslo)
        {
            try
            {
                Povezava.Open();
                string PodatkiUkaza = "INSERT INTO Igralec (Vzdevek, Geslo, DanRegistracije) VALUES(@Vzdevek, @Geslo, @DanRegistracije)";
                MySqlCommand Ukaz = new MySqlCommand(PodatkiUkaza, Povezava);
                Ukaz.CommandTimeout = 10;
                Ukaz.Parameters.AddWithValue("@Vzdevek", vzdevek);
                Ukaz.Parameters.AddWithValue("@Geslo", geslo);
                string datum = DateTime.Today.ToString("yyyy-MM-dd").Replace(".", "-");
                Ukaz.Parameters.AddWithValue("@DanRegistracije", datum);
                Ukaz.ExecuteNonQuery();
                Povezava.Close();
                return true;
            }
            catch (Exception)
            {
                Povezava.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za dodajanje rojstnega datuma
        /// </summary>
        /// <param name="IDigralca"> ID od uporabnika aplikacije</param>
        /// <param name="rojstniDatum"> rojstni datum v formatu YYYY-MM-DD (leto, mesec, dan) </param>
        /// <returns>Vrne true ce uspesno spremeni datum rojstva, drugače vrne false</returns>
        public bool DodajRojstniDatum(int IDigralca, string rojstniDatum)
        {
            try
            {
                Povezava.Open();
                string PodatkiUkaza = " UPDATE Igralec SET RojstniDan = @rojstniDan WHERE IDigralec = @IDigralec";
                MySqlCommand Ukaz = new MySqlCommand(PodatkiUkaza, Povezava);
                Ukaz.CommandTimeout = 10;
                Ukaz.Parameters.AddWithValue("@RojstniDan", rojstniDatum);
                Ukaz.Parameters.AddWithValue("@IDigralec", IDigralca);
                Ukaz.ExecuteNonQuery();
                Povezava.Close();
                return true;
            }
            catch (Exception)
            {
                Povezava.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za povecanje indeksa odigranih iger
        /// </summary>
        /// <param name="IDigralca">ID od uporabnika aplikacije</param>
        /// <returns>Vrne true ce uspesno poveca indeks odigranih iger. Drugače vrne false</returns>
        public bool PovecajSteviloIger(int IDigralca)
        {
            try
            {
                Povezava.Open();
                string PodatkiUkaza = " UPDATE Igralec SET SteviloKoncanihIger = SteviloKoncanihIger + 1 WHERE IDigralec = @IDigralec";
                MySqlCommand Ukaz = new MySqlCommand(PodatkiUkaza, Povezava);
                Ukaz.CommandTimeout = 10;
                Ukaz.Parameters.AddWithValue("@IDigralec", IDigralca);
                Ukaz.ExecuteNonQuery();
                Povezava.Close();
                return true;
            }
            catch (Exception)
            {
                Povezava.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za dodajanje igre v aplikacije
        /// </summary>
        /// <param name="Igralec1ID"> ID igralca, ki bo igral s belimi figurami</param>
        /// <param name="Igralec2ID"> ID igralca, ki bo igral s crnimi figurami</param>
        /// <param name="Inc"> Indeks za povecevanje casa potez</param>
        /// <param name="Cas"> Cas potez v formatu HH-MM-SS (ure, minute, sekunde)</param>
        /// <returns>Vrne true če uspešno doda igro. Drugače vrne false</returns>
        public bool DodajIgro(int Igralec1ID, int Igralec2ID, int Inc, string Cas)
        {
            try
            {
                Povezava.Open();
                string PodatkiUkaza = "INSERT INTO Igra (Igralec0, Igralec1, Inc, Cas) VALUES (@Igralec1ID , @Igralec2ID, @Inc, @Cas)";
                MySqlCommand Ukaz = new MySqlCommand(PodatkiUkaza, Povezava);
                Ukaz.CommandTimeout = 10;
                Ukaz.Parameters.AddWithValue("@Igralec1ID", Igralec1ID);
                Ukaz.Parameters.AddWithValue("@Igralec2ID", Igralec2ID);
                Ukaz.Parameters.AddWithValue("@Inc", Inc);
                Ukaz.Parameters.AddWithValue("@Cas", Cas);
                Ukaz.ExecuteNonQuery();
                Povezava.Close();
                return true;
            }
            catch (Exception)
            {
                Povezava.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za dodajanje končnega stanja igre
        /// </summary>
        /// <param name="IDigre"> ID od igre, ki jo želimo dokončati</param>
        /// <param name="KoncnoStanje"> Končno stanje šahovnice</param>
        /// <returns></returns>
        public bool NastaviKoncnoStanjeIgre(int IDigre, string KoncnoStanje)
        {
            try
            {
                Povezava.Open();
                string PodatkiUkaza = " UPDATE Igra SET KoncnoStanje = @KoncnoStanje WHERE IDigre = @IDigre";
                MySqlCommand Ukaz = new MySqlCommand(PodatkiUkaza, Povezava);
                Ukaz.CommandTimeout = 10;
                Ukaz.Parameters.AddWithValue("@KoncnoStanje", KoncnoStanje);
                Ukaz.Parameters.AddWithValue("@IDigre", IDigre);
                Ukaz.ExecuteNonQuery();
                Povezava.Close();
                return true;
            }
            catch (Exception)
            {
                Povezava.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za dodajanje zmagovalca k igri
        /// </summary>
        /// <param name="IDigre"> ID igre k kateri dodajamo zmagovalca</param>
        /// <param name="Zmagovalec"> vrednost zmagovalca. 0 je beli igralec. 1 je črni</param>
        /// <returns>Vrne true če uspešno doda igralca, drugače vrne false</returns>
        public bool NastaviZmagovalcaIgre(int IDigre, bool Zmagovalec)
        {
            try
            {
                Povezava.Open();
                string PodatkiUkaza = " UPDATE Igra SET Zmagovalec = @Zmagovalec WHERE IDigre = @IDigre";
                MySqlCommand Ukaz = new MySqlCommand(PodatkiUkaza, Povezava);
                Ukaz.CommandTimeout = 10;
                Ukaz.Parameters.AddWithValue("@Zmagovalec", Zmagovalec);
                Ukaz.Parameters.AddWithValue("@IDigre", IDigre);
                Ukaz.ExecuteNonQuery();
                Povezava.Close();
                return true;
            }
            catch (Exception)
            {
                Povezava.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za dodajanje  poteze k igri
        /// </summary>
        /// <param name="IDigra">ID igre k kateri dodajamo</param>
        /// <param name="Poteza"> niz poteze, ki jo dodajamo</param>
        /// <param name="Cas"> čas kako dolgo je trajala poteza HH-MM-SS (ure, minute, sekunde)</param>
        /// <returns> vrne true, če uspešno doda potezo drugače vrne false</returns>
        public bool DodajPotezo(int IDigra, string Poteza, string Cas)
        {
            try
            {
                Povezava.Open();
                string PodatkiUkaza = "INSERT INTO Poteze (Igra_IDigre, Poteza, CasPoteze) VALUES (@IDigre, @Poteza, @Cas)";
                MySqlCommand Ukaz = new MySqlCommand(PodatkiUkaza, Povezava);
                Ukaz.CommandTimeout = 10;
                Ukaz.Parameters.AddWithValue("@IDigre", IDigra);
                Ukaz.Parameters.AddWithValue("@Poteza", Poteza);
                Ukaz.Parameters.AddWithValue("@Cas", Cas);
                Ukaz.ExecuteNonQuery();
                Povezava.Close();
                return true;
            }
            catch (Exception)
            {
                Povezava.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za dodajanje sporočila k igri
        /// </summary>
        /// <param name="Igralec_IDigralec"> ID igralca, ki pošilja sporočilo</param>
        /// <param name="Igra_IDigre"> ID igre k kateri dodajamo sporočilo</param>
        /// <param name="Vsebina"> Vsebina sporočila, ki ga dodajamo</param>
        /// <returns>Vrne true, če uspešno doda sporočilo. Drugače vrne false</returns>
        public bool DodajSporocilo(int Igralec_IDigralec, int Igra_IDigre, string Vsebina)
        {
            try
            {
                Povezava.Open();
                string PodatkiUkaza = "INSERT INTO Sporocilo (Vsebina, Igralec_IDigralec, Igra_IDigre, Cas) VALUES (@Vsebina, @Igralec_IDigralec, @Igra_IDigre , NOW() )";
                MySqlCommand Ukaz = new MySqlCommand(PodatkiUkaza, Povezava);
                Ukaz.CommandTimeout = 10;
                Ukaz.Parameters.AddWithValue("@Vsebina", Vsebina);
                Ukaz.Parameters.AddWithValue("@Igralec_IDigralec", Igralec_IDigralec);
                Ukaz.Parameters.AddWithValue("@Igra_IDigre", Igra_IDigre);
                Ukaz.ExecuteNonQuery();
                Povezava.Close();
                return true;
            }
            catch (Exception)
            {
                Povezava.Close();
                return false;
            }
        }
        /// <summary>
        /// Funkcija za vračanje vseh potez neke igre
        /// </summary>
        /// <param name="Igra_IDigre"> ID igre od katere poteze želimo vrniti</param>
        /// <returns>Vrne List<string> s vsemimi potezami izbrane igre v istem vrstnem redu kot so bile izvedene</returns>
        public List<string> VrniPoteze(int Igra_IDigre)
        {
            List<string> poteze = new List<string>();
            try
            {
                Povezava.Open();
                string PodatkiUkaza = "SELECT Poteza FROM Poteze WHERE Igra_IDigre=@Igra_IDigre";
                MySqlCommand Ukaz = new MySqlCommand(PodatkiUkaza, Povezava);
                Ukaz.Parameters.AddWithValue("@Igra_IDigre", Igra_IDigre);
                MySqlDataReader Branje = Ukaz.ExecuteReader();
                while (Branje.Read())
                {
                    poteze.Add(Branje[0].ToString());
                }
                Branje.Close();
            }
            catch (Exception)
            {
                //
            }
            Povezava.Close();
            return poteze;
        }
        /// <summary>
        /// Funkcija, ki vrne vzdevek igralca iz njegovega ID
        /// </summary>
        /// <param name="ID">ID igralca</param>
        /// <returns>vrne vzdevek igralca</returns>
        public string VzdevekIzID(int ID)
        {
            string vzdevek = String.Empty;
            try
            {
                Povezava.Open();
                string PodatkiUkaza = "SELECT Vzdevek FROM Igralec WHERE IDigralec = @ID ";
                MySqlCommand Ukaz = new MySqlCommand(PodatkiUkaza, Povezava);
                Ukaz.Parameters.AddWithValue("@ID", ID);
                MySqlDataReader Branje = Ukaz.ExecuteReader();
                Branje.Read();
                vzdevek = Branje["Vzdevek"].ToString();
                Branje.Close();
            }
            catch (Exception)
            {
                return vzdevek;
            }
            Povezava.Close();
            return vzdevek;
        }
        /// <summary>
        /// Funkcija, ki vrne ID iz vzdevka igralca
        /// </summary>
        /// <param name="Vzdevek">Vzdevek iz katerega zelimo dobiti ID</param>
        /// <returns>vrne ID od izbranega igralca</returns>
        public int IDizVzdevka(string Vzdevek)
        {
            string vzdevek = String.Empty;
            try
            {
                Povezava.Open();
                string PodatkiUkaza = "SELECT IDigralec FROM Igralec WHERE Vzdevek = @Vzdevek ";
                MySqlCommand Ukaz = new MySqlCommand(PodatkiUkaza, Povezava);
                Ukaz.Parameters.AddWithValue("@Vzdevek", Vzdevek);
                MySqlDataReader Branje = Ukaz.ExecuteReader();
                Branje.Read();
                vzdevek = Branje["IDigralec"].ToString();
                Branje.Close();
            }
            catch (Exception)
            {
            }
            Povezava.Close();
            return int.Parse(vzdevek);
        }
    }
}