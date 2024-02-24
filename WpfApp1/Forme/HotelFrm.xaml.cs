using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IT73_2022.Forme
{
    /// <summary>
    /// Interaction logic for HotelFrm.xaml
    /// </summary>
    public partial class HotelFrm : Window
    {
        Konekcija con = new Konekcija();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;

        public HotelFrm()
        {
            InitializeComponent();
            txtNaziv.Focus();
            connection = con.KreirajKonekciju();
        }

        public HotelFrm(bool update, DataRowView row)
        {
            InitializeComponent();
            txtNaziv.Focus();
            connection = con.KreirajKonekciju();
            this.update = update;
            this.row = row;
        }

        private void btnSacuvaj_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = connection
                };
                cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar).Value = txtNaziv.Text;
                cmd.Parameters.Add("@Adresa", SqlDbType.NVarChar).Value = txtAdresa.Text;
                cmd.Parameters.Add("@BrojTel", SqlDbType.NVarChar).Value = txtBrojTel.Text;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;
                cmd.Parameters.Add("@Kapacitet", SqlDbType.Int).Value = txtKapacitet.Text;
                if (update)
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update dbo.tblHotel set Naziv = @Naziv, Adresa = @Adresa, BrojTel = @BrojTel, Email = @Email, Kapacitet = @Kapacitet where HotelID = @ID";
                    row = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.tblHotel(Naziv, Adresa, BrojTel, Email, Kapacitet) values (@Ime, @Prezime, @BrojTel, @Email, @Kapacitet)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Pogresno uneti podaci!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        private void btnOtkazi_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
