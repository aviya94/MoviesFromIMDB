using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MoviesDataFromIMDB
{
    public abstract class GetDataFromIMDB
    {
        protected string FileContent;
        protected const string SiteTitle = "http://www.imdb.com/title/";
        protected const string urlLeft = "https://www.imdb.com/find?q=";
        protected string urlRight;
        protected abstract void IMDBScraper();
        protected string IMDBGetData(string IMDBStringToSerch)
        {
            //download the whole page, to be able to search it by regex
            string URL = urlLeft + IMDBStringToSerch + urlRight;
            StreamReader sr = new StreamReader(new WebClient().OpenRead(URL));
            string txtFromIMDB = sr.ReadToEnd();
            return txtFromIMDB;
        }

    }
}
