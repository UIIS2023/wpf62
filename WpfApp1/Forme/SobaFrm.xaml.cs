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
    /// Interaction logic for SobaFrm.xaml
    /// </summary>
    public partial class SobaFrm : Window
    {
        Konekcija con = new Konekcija();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;

        public SobaFrm()
        {
            InitializeComponent();
            txtKapacitet.Focus();
            connection = con.KreirajKonekciju();
            fillComboBox();
        }

        public SobaFrm(bool update, DataRowView row)
        {
            InitializeComponent();
            txtKapacitet.Focus();
            connection = con.KreirajKonekciju();
            fillComboBox();
            this.update = update;
            this.row = row;
        }

        private void fillComboBox()
        {
            try
            {
                connection.Open();
                string IzaberiHotel = @"select HotelID, dbo.tblHotel.Naziv from dbo.tblHotel";
                SqlDataAdapter daHotel = new SqlDataAdapter(IzaberiHotel, connection);
                DataTable dtHotel = new DataTable();
                daHotel.Fill(dtHotel);
                cbHotel.ItemsSource = dtHotel.DefaultView;
                cbHotel.DisplayMemberPath = "Naziv";
                cbHotel.SelectedValuePath = "HotelID";
                daHotel.Dispose();
                dtHotel.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
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
                cmd.Parameters.Add("@Kapacitet", SqlDbType.Int).Value = txtKapacitet.Text;
                cmd.Parameters.Add("@HotelID", SqlDbType.Int).Value = cbHotel.SelectedValue;
                if (update)
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update dbo.tblSoba set Kapacitet = @Kapacitet, HotelID = @HotelID where SobaID = @ID";
                    row = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.tblSoba(Kapacitet, HotelID) values (@Kapacitet, @HotelID)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
