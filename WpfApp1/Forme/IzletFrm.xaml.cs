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
    /// Interaction logic for IzletFrm.xaml
    /// </summary>
    public partial class IzletFrm : Window
    {
        Konekcija con = new Konekcija();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;

        public IzletFrm()
        {
            InitializeComponent();
            txtDestinacija.Focus();
            connection = con.KreirajKonekciju();
            fillComboBox();
        }

        public IzletFrm(bool update, DataRowView row)
        {
            InitializeComponent();
            txtDestinacija.Focus();
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

        private void btnSacuvaj_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = connection
                };

                cmd.Parameters.Add("@Destinacija", SqlDbType.NVarChar).Value = txtDestinacija.Text;
                cmd.Parameters.Add("@Cena", SqlDbType.Decimal).Value = Convert.ToDecimal(txtCena.Text);
                cmd.Parameters.Add("@DatumObilaska", SqlDbType.Date).Value = date.SelectedDate;
                cmd.Parameters.Add("@AgentID", SqlDbType.Int).Value = cbAgent.SelectedValue;
                if (update)
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update dbo.tblIzlet set Destinacija = @Destinacija, Cena = @Cena, DatumObilaska = @DatumObilaska, AgentID = @AgentID where IzletID = @ID";
                    row = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.tblIzlet(Destinacija, Cena, DatumObilaska, AgentID) values (@Destinacija, @Cena, @DatumObilaska, @AgentID)";
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
