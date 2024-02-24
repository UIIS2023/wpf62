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
    /// Interaction logic for PrevoznikFrm.xaml
    /// </summary>
    public partial class PrevoznikFrm : Window
    {
        Konekcija con = new Konekcija();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;

        public PrevoznikFrm()
        {
            InitializeComponent();
            txtKapacitet.Focus();
            connection = con.KreirajKonekciju();
        }

        public PrevoznikFrm(bool update, DataRowView row)
        {
            InitializeComponent();
            txtKapacitet.Focus();
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
                cmd.Parameters.Add("@Kapacitet", SqlDbType.Int).Value = txtKapacitet.Text;
                cmd.Parameters.Add("@DatumPocetka", SqlDbType.Date).Value = date1.SelectedDate;
                cmd.Parameters.Add("@DatumKraja", SqlDbType.Date).Value = date2.SelectedDate;
                cmd.Parameters.Add("@Cena", SqlDbType.Decimal).Value = txtCena.Text;
                if (update)
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update dbo.tblPrevoznik set Kapacitet = @Kapacitet, DatumPocetka = @DatumPocetka, DatumKraja = @DatumKraja, Cena = @Cena where PrevoznikID = @ID";
                    row = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.tblPrevoznik(Kapacitet, DatumPocetka, DatumKraja, Cena) values (@Kapacitet, @DatumPocetka, @DatumKraja, @Cena)";
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
