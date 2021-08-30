using MoviesDataFromIMDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace MoviesDataFromIMDB
{
    public class GetDataMovieFromIMDB : GetDataFromIMDB
    {
        List<string> movieIds = new List<string>();
        private string MovieName;
        StringBuilder sb = new StringBuilder();
        const string inDevelopment = "{\"id\":\"in_development";

        public GetDataMovieFromIMDB(string movieName)// constractor
        {
            urlRight = "&s=tt&ttype=ft&ref_=fn_ft";
            MovieName = movieName;
            string txtFromIMDB = IMDBGetData(MovieName);
            FindIdMovie(txtFromIMDB);
            IMDBScraper();

        }


        public void FindIdMovie(string txtFromIMDB)
        {
            string findBeginingID = "<a href=\"/title/";
            string endString = "/?ref_=fn_ft_tt";
            while (txtFromIMDB.IndexOf(findBeginingID) > -1)
            {
                int startstring = txtFromIMDB.IndexOf(findBeginingID) + findBeginingID.Length;
                string movieId = txtFromIMDB.Substring(startstring, txtFromIMDB.IndexOf(endString) - startstring);
                movieIds.Add(movieId);
                for (int i = 0; i < 2; i++)
                {
                    txtFromIMDB = txtFromIMDB.Substring(txtFromIMDB.IndexOf(endString) + endString.Length);
                }
            }
        }

        protected override void IMDBScraper()
        {

            //download the whole page, to be able to search it by regex
            StreamReader sr;
            for (int i = 0; i < movieIds.Count; i++)
            {
                string URL = SiteTitle + movieIds[i] + "/?ref_=fn_tt_tt_1";
                sr = new StreamReader(new WebClient().OpenRead(URL));
                FileContent = sr.ReadToEnd();

                string MovieStr = GetMovieTitel();

                if (MovieStr.ToLower().IndexOf(MovieName.ToLower()) > -1 && !FileContent.Contains(inDevelopment))
                {
                    string GenreStr = GetMovieGenre();
                    string DirectorStr = GetDirector();
                    string DurationStr = GetDuration();
                    string StarsStr = GetStars();
                    string MPAARatingPGStr = GetMPAARatingPG();
                    //add Movie row to String Builder
                    if (MovieStr != "")
                    {
                        sb.Append(MovieStr + " | " + GenreStr + " | " + MPAARatingPGStr + " | " + DurationStr + " | " + DirectorStr + " | " + StarsStr + Environment.NewLine);
                    }
                }
            }
            WriteToTextFile.writeText(sb.ToString());
        }


        /// <summary>
        /// Get the name off the movie
        /// </summary>
        /// <returns></returns>
        public string GetMovieTitel()
        {


            string _MovieTitle = "";
            try
            {
                //scrape the movie title               
                string titlePattern = "name\":\".*\",\"";

                Regex R1 = new Regex(titlePattern,
                    RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                if (R1.Matches(FileContent).Count > 0)
                {
                    _MovieTitle = R1.Matches(FileContent)[0].Value;
                    _MovieTitle = _MovieTitle.Substring(_MovieTitle.IndexOf(":") + 2, _MovieTitle.Length - _MovieTitle.IndexOf(":") - 10);
                    if (_MovieTitle.IndexOf(",") > 0)
                        _MovieTitle = _MovieTitle.Substring(0, _MovieTitle.IndexOf(",") - 1);
                }
            }
            catch (Exception ex)
            {
                //TODO ADD TO Err Log
                // MessageBox.Show(ex.Message);
            }
            return _MovieTitle;
        }
        /// <summary>
        /// Get Movie Director
        /// </summary>
        /// <returns></returns>
        public string GetDirector()
        {
            string _Director = "";
            try
            {
                string directorPattern = @":.Directed.by.*\..With";
                Regex R1 = new Regex(directorPattern,
                    RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                if (R1.Matches(FileContent).Count > 0)
                {
                    _Director = R1.Matches(FileContent)[0].Value;

                }
                _Director = _Director.Substring(12, _Director.IndexOf(" With") - 13);
            }
            catch (Exception ex)
            {
                //TODO ADD TO Err Log
                //MessageBox.Show(ex.Message);
            }
            return _Director;
        }
        /// <summary>
        /// get Movie Rating
        /// </summary>
        /// <returns></returns>
        public string GetRating()
        {
            string _Rating = "";
            try
            {
                string RatingPattern = @"ratingValue.:[0-9]*(\.)*[0-9]*},";
                Regex R1 = new Regex(RatingPattern,
                    RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                if (R1.Matches(FileContent).Count > 0)
                {
                    _Rating = R1.Matches(FileContent)[0].Value;
                    _Rating = (_Rating.Split('}')[0]).Trim();
                    _Rating = _Rating.Substring(13, _Rating.Length - 13);
                }
            }
            catch (Exception ex)
            {
                //TODO ADD TO Err Log
                //MessageBox.Show(ex.Message);
            }
            return _Rating;
        }
        /// <summary>
        /// Get Movie Duration
        /// </summary>
        /// <returns></returns>
        public string GetDuration()
        {
            string duration = "";
            try
            {
                string DurationPattern = @"PT[0-9]*H[0-9]*M";
                Regex R1 = new Regex(DurationPattern,
                    RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                if (R1.Matches(FileContent).Count > 0)
                {
                    duration = R1.Matches(FileContent)[0].Value;
                    duration = (duration.Split('}')[0]).Trim();
                    duration = duration.Substring(2, duration.Length - 2);
                    duration = duration.Replace("H", "H ").Replace("M", "Min");
                }
            }
            catch (Exception ex)
            {
                //TODO ADD TO Err Log
                //MessageBox.Show(ex.Message);
            }
            return duration;
        }
        /// <summary>
        /// Get mpvie stars 
        /// </summary>
        /// <returns></returns>
        public string GetStars()
        {
            string starsString = "";
            try
            {

                string starsPattern = @":.Directed.by.*\..With";//.*\..After";
                Regex R1 = new Regex(starsPattern,
                    RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                if (R1.Matches(FileContent).Count > 0)
                {
                    starsString = R1.Matches(FileContent)[0].Value;
                    starsString = starsString.Substring(starsString.IndexOf("With") + 5);
                    starsString = starsString.Substring(0, starsString.IndexOf(". "));
                    if (starsString.Contains("data-id"))
                        starsString = starsString.Substring(0, starsString.IndexOf("data-id") - 3);
                }
            }
            catch (Exception ex)
            {
                //TODO ADD TO Err Log
                //MessageBox.Show(ex.Message);
            }
            return starsString;
        }

        public string GetMPAARatingPG()
        {
            string starsString = "";
            try
            {
                string starsPattern = "contentRating\":\"PG-.*\",\"genre";
                Regex R1 = new Regex(starsPattern,
                    RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                if (R1.Matches(FileContent).Count > 0)
                {
                    starsString = R1.Matches(FileContent)[0].Value;
                    starsString = starsString.Substring(16);
                    starsString = (starsString.Split('\"')[0]).Trim();
                    starsString = starsString.Replace("-", " - ");
                }
                else
                    starsString = " R ";
            }
            catch (Exception ex)
            {
                //TODO ADD TO Err Log
                //MessageBox.Show(ex.Message);
            }
            return starsString;
        }
        public string GetMovieGenre()
        {
            //scrape movie genre
            string genreString = "";
            try
            {
                string genrePattern = "genre\":\\[.*\\],";
                Regex R1 = new Regex(genrePattern,
                        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                string genre;

                if (R1.Matches(FileContent).Count > 0)
                {
                    genre = R1.Matches(FileContent)[0].Value;
                    MatchCollection genreResults = R1.Matches(FileContent);
                    genreString = genreResults[0].ToString();
                    genreString = genreString.Substring(8, genreString.IndexOf("]") - 8);
                    genreString = genreString.Replace("\"", "");
                    genreString = genreString.Replace(",", ", ");

                }
            }
            catch (Exception ex)
            {
                //TODO ADD TO Err Log
                //MessageBox.Show(ex.Message);
            }
            return genreString;
        }
    }
}
