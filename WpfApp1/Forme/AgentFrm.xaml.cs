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
    /// Interaction logic for AgentFrm.xaml
    /// </summary>
    public partial class AgentFrm : Window
    {
        Konekcija con = new Konekcija();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;

        public AgentFrm()
        {
            InitializeComponent();
            txtIme.Focus();
            connection = con.KreirajKonekciju();
        }

        public AgentFrm(bool update, DataRowView row)
        {
            InitializeComponent();
            txtIme.Focus();
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
                cmd.Parameters.Add("@Ime", SqlDbType.NVarChar).Value = txtIme.Text;
                cmd.Parameters.Add("@Prezime", SqlDbType.NVarChar).Value = txtPrezime.Text;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;
                cmd.Parameters.Add("@BrojTel", SqlDbType.NVarChar).Value = txtBrojTel.Text;
                if (update)
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update dbo.tblAgent set Ime = @Ime, Prezime = @Prezime, Email = @Email, BrojTel = @BrojTel where AgentID = @ID";
                    row = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.tblAgent(Ime, Prezime, Email, BrojTel) values (@Ime, @Prezime, @Email, @BrojTel)";
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
