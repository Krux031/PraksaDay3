using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;

namespace PraksaDay2.Controllers
{
    public class HelloController : ApiController
    {
        public static List<string> popis = new List<string>() { "kruh", "mlijeko" , "jaja", "riza", "banane", "sok"};

        SqlCommand dohvati = null;
        SqlTransaction transaction;
        public static SqlConnection konekcija = new SqlConnection(@"Server=tcp:kruninserver.database.windows.net,1433;Initial Catalog=kruninabaza;Persist Security Info=False;User ID=krux031;Password=sifra;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        public HttpResponseMessage Get()
        {
            dohvati = new SqlCommand("select * from stanovnici", konekcija);
            string tekst = "";

            try
            {
                if (konekcija.State == ConnectionState.Closed) 
                {
                    konekcija.Open();
                }
                //konekcija.BeginTransaction();
                SqlDataReader reader = dohvati.ExecuteReader();

                while (reader.Read())
                {
                    tekst = tekst + "<br />" + reader.GetString(1)+ " " + reader.GetString(2) + " " + reader.GetString(4);
                }

                //transaction.Commit();
                return Request.CreateResponse(HttpStatusCode.OK, tekst);
            }
            catch (SqlException Ex)
            {
                transaction.Rollback();
                return Request.CreateResponse(HttpStatusCode.NoContent, "No Content");
            } 
            finally
            {
                if (konekcija.State == ConnectionState.Open)
                    konekcija.Close();
            }

            //return popis;
        }
        public HttpResponseMessage Post([FromBody]string item)
        {
            popis.Add(item);
            return Request.CreateResponse(HttpStatusCode.OK, "OK");
        }
        public HttpResponseMessage Put ([FromBody] string item)
        {
            if(popis.Contains(item) == false)
            {
                popis.Add(item);
                return Request.CreateResponse(HttpStatusCode.Created, "Created");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No Content");
            }

        }
        public HttpResponseMessage Delete([FromBody] string item)
        {
            if (popis.Contains(item) == true)
            {
                popis.Remove(item);
                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found");
            }
        }

    }
}
