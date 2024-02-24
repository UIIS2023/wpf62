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
    /// Interaction logic for AranzmanFrm.xaml
    /// </summary>
    public partial class AranzmanFrm : Window
    {
        Konekcija con = new Konekcija();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;

        public AranzmanFrm()
        {
            InitializeComponent();
            txtDrzava.Focus();
            connection = con.KreirajKonekciju();
            fillComboBoxHotel();
            fillComboBoxAgent();
            fillComboBoxIzlet();
            fillComboBoxPrevoznik();
        }

        public AranzmanFrm(bool update, DataRowView row)
        {
            InitializeComponent();
            txtDrzava.Focus();
            connection = con.KreirajKonekciju();
            fillComboBoxHotel();
            fillComboBoxAgent();
            fillComboBoxIzlet();
            fillComboBoxPrevoznik();
            this.update = update;
            this.row = row;
        }

        private void fillComboBoxHotel()
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

        private void fillComboBoxAgent()
        {
            try
            {
                connection.Open();
                string IzaberiAgenta = @"select AgentID, dbo.tblAgent.Ime from dbo.tblAgent";
                SqlDataAdapter daAgent = new SqlDataAdapter(IzaberiAgenta, connection);
                DataTable dtAgent = new DataTable();
                daAgent.Fill(dtAgent);
                cbAgent.ItemsSource = dtAgent.DefaultView;
                cbAgent.DisplayMemberPath = "Ime";
                cbAgent.SelectedValuePath = "AgentID";
                daAgent.Dispose();
                dtAgent.Dispose();
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

        private void fillComboBoxIzlet()
        {
            try
            {
                connection.Open();
                string IzaberiIzlet = @"select IzletID, dbo.tblIzlet.Destinacija from dbo.tblIzlet";
                SqlDataAdapter daIzlet = new SqlDataAdapter(IzaberiIzlet, connection);
                DataTable dtIzlet = new DataTable();
                daIzlet.Fill(dtIzlet);
                cbIzlet.ItemsSource = dtIzlet.DefaultView;
                cbIzlet.DisplayMemberPath = "Destinacija";
                cbIzlet.SelectedValuePath = "IzletID";
                daIzlet.Dispose();
                dtIzlet.Dispose();
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

        private void fillComboBoxPrevoznik()
        {
            try
            {
                connection.Open();
                string IzaberiPrevoznika = @"select PrevoznikID, dbo.tblPrevoznik.Kapacitet from dbo.tblPrevoznik";
                SqlDataAdapter daPrevoznik = new SqlDataAdapter(IzaberiPrevoznika, connection);
                DataTable dtPrevoznik = new DataTable();
                daPrevoznik.Fill(dtPrevoznik);
                cbPrevoznik.ItemsSource = dtPrevoznik.DefaultView;
                cbPrevoznik.DisplayMemberPath = "Kapacitet";
                cbPrevoznik.SelectedValuePath = "PrevoznikID";
                daPrevoznik.Dispose();
                dtPrevoznik.Dispose();
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
                cmd.Parameters.Add("@Drzava", SqlDbType.NVarChar).Value = txtDrzava.Text;
                cmd.Parameters.Add("@Grad", SqlDbType.NVarChar).Value = txtGrad.Text;
                cmd.Parameters.Add("@Cena", SqlDbType.Decimal).Value = Convert.ToDecimal(txtCena.Text);
                cmd.Parameters.Add("@HotelID", SqlDbType.Int).Value = cbHotel.SelectedValue;
                cmd.Parameters.Add("@AgentID", SqlDbType.Int).Value = cbAgent.SelectedValue;
                cmd.Parameters.Add("@IzletID", SqlDbType.Int).Value = cbIzlet.SelectedValue;
                cmd.Parameters.Add("@PrevoznikID", SqlDbType.Int).Value = cbPrevoznik.SelectedValue;
                if (update)
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update dbo.tblAranzman set Drzava = @Drzava, Grad = @Grad, Cena = @Cena, HotelID = @HotelID, AgentID = @AgentID, IzletID = @IzletID, PrevoznikID = @PrevoznikID where AranzmanID = @ID";
                    row = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.tblAranzman(Drzava, Grad, Cena, HotelID, AgentID, IzletID, PrevoznikID) values (@Drzava, @Grad, @Cena, @HotelID, @AgentID, @IzletID, @PrevoznikID)";
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
