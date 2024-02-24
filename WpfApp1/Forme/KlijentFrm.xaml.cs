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
    /// Interaction logic for KlijentFrm.xaml
    /// </summary>
    public partial class KlijentFrm : Window
    {
        Konekcija con = new Konekcija();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;

        public KlijentFrm()
        {
            InitializeComponent();
            txtIme.Focus();
            connection = con.KreirajKonekciju();
            fillComboBox();
        }

        public KlijentFrm(bool update, DataRowView row)
        {
            InitializeComponent();
            txtIme.Focus();
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
                string IzaberiAranzman = @"select AranzmanID, dbo.tblAranzman.Drzava from dbo.tblAranzman";
                SqlDataAdapter daAranzman = new SqlDataAdapter(IzaberiAranzman, connection);
                DataTable dtAranzman = new DataTable();
                daAranzman.Fill(dtAranzman);
                cbAranzman.ItemsSource = dtAranzman.DefaultView;
                cbAranzman.DisplayMemberPath = "Drzava";
                cbAranzman.SelectedValuePath = "AranzmanID";
                daAranzman.Dispose();
                dtAranzman.Dispose();
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
                cmd.Parameters.Add("@Ime", SqlDbType.NVarChar).Value = txtIme.Text;
                cmd.Parameters.Add("@Prezime", SqlDbType.NVarChar).Value = txtPrezime.Text;
                cmd.Parameters.Add("@BrojTel", SqlDbType.NVarChar).Value = txtBrojTel.Text;
                cmd.Parameters.Add("@BrojPas", SqlDbType.NVarChar).Value = txtBrojPas.Text;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;
                cmd.Parameters.Add("@Osiguranje", SqlDbType.NVarChar).Value = txtOsiguranje.Text;
                cmd.Parameters.Add("@AranzmanID", SqlDbType.Int).Value = cbAranzman.SelectedValue;
                if (update)
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update dbo.tblKlijent set Ime = @Ime, Prezime = @Prezime, BrojTel = @BrojTel, BrojPas = @BrojPas, Email = @Email, Osiguranje = @Osiguranje, AranzmanID = @AranzmanID where KlijentID = @ID";
                    row = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.tblKlijent(Ime, Prezime, BrojTel, BrojPas, Email, Osiguranje, AranzmanID) values (@Ime, @Prezime, @BrojTel, @BrojPas, @Email, @Osiguranje, @AranzmanID)";
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
