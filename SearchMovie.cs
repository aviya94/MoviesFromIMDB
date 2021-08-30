using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoviesDataFromIMDB
{
    public partial class MovieSearch : Form
    {
        GetDataMovieFromIMDB getDataMovieFromIMDB;
        public MovieSearch()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Use Task -   to keep ui not freeze
            var t = Task.Factory.StartNew(() =>
            {
                getDataMovieFromIMDB = new GetDataMovieFromIMDB(txtMovieToSerch.Text);
            });
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMovieToSerch.Text = "";
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
