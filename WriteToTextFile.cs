using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesDataFromIMDB
{
    /// <summary>
    /// Write the Movis data into text file
    /// </summary>
    public static class WriteToTextFile
    {
        public static void writeText(string sb)
        {
            string path = @"c:\temp\MyMovie.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            // This text is added only once to the file.
            using (FileStream fs = File.Create(path))
            {
                byte[] Moviedata = new UTF8Encoding(true).GetBytes(sb);
                fs.Write(Moviedata, 0, Moviedata.Length);
                System.Windows.Forms.MessageBox.Show("The movie data was saved to the file ", "Message");
            }
        }
    }
}
