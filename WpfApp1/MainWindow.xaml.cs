using IT73_2022.Forme;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IT73_2022
{
    public partial class MainWindow : Window
    {
        Konekcija con = new Konekcija();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;
        private string CurrentTable;

        #region Select queries
        private static string SelectAgent = @"select AgentID as ID, Ime, Prezime, Email, BrojTel from dbo.tblAgent";
        private static string SelectAranzman = @"select AranzmanID as ID, Drzava, Grad, dbo.tblHotel.Naziv, dbo.tblAgent.Ime, dbo.tblIzlet.Destinacija, dbo.tblAranzman.Cena, dbo.tblPrevoznik.Kapacitet
                                        from dbo.tblAranzman
                                        left join dbo.tblHotel on dbo.tblAranzman.HotelID = dbo.tblHotel.HotelID
                                        left join dbo.tblIzlet on dbo.tblAranzman.IzletID = dbo.tblIzlet.IzletID
                                        left join dbo.tblPrevoznik on dbo.tblAranzman.PrevoznikID = dbo.tblPrevoznik.PrevoznikID
                                        left join dbo.tblAgent on dbo.tblAranzman.AgentID = dbo.tblAgent.AgentID";
        private static string SelectHotel = @"select HotelID as ID, Naziv, Adresa, BrojTel, Email, Kapacitet from dbo.tblHotel";
        private static string SelectIzlet = @"select IzletID as ID, tblAgent.Ime, Destinacija, dbo.tblIzlet.Cena, DatumObilaska
                                        from dbo.tblIzlet
                                        left join dbo.tblAgent on dbo.tblIzlet.AgentID = dbo.tblAgent.AgentID";
        private static string SelectKlijent = @"select KlijentID as ID, Ime, Prezime, BrojTel, BrojPas, Email, Osiguranje, dbo.tblAranzman.Drzava
                                        from dbo.tblKlijent
                                        left join dbo.tblAranzman on dbo.tblKlijent.AranzmanID = dbo.tblAranzman.AranzmanID";
        private static string SelectPrevoznik = @"select PrevoznikID as ID, Kapacitet, DatumPocetka, DatumKraja, Cena from dbo.tblPrevoznik";
        private static string SelectSoba = @"Select SobaID as ID, dbo.tblSoba.Kapacitet, dbo.tblHotel.Naziv
                                        from dbo.tblSoba
                                        left join dbo.tblHotel on dbo.tblSoba.HotelID = dbo.tblHotel.HotelID";
        #endregion

        #region Select with statements
        private static string SelectStatementAgent = @"select * from dbo.tblAgent where AgentID=";
        private static string SelectStatementAranzman = @"select * from dbo.tblAranzman where AranzmanID=";
        private static string SelectStatementHotel = @"select * from dbo.tblHotel where HotelID=";
        private static string SelectStatementIzlet = @"select * from dbo.tblIzlet where IzletID=";
        private static string SelectStatementKlijent = @"select * from dbo.tblKlijent where KlijentID=";
        private static string SelectStatementPrevoznik = @"select * from dbo.tblPrevoznik where PrevoznikID=";
        private static string SelectStatementSoba = @"select * from dbo.tblSoba where SobaID=";
        #endregion

        #region Delete queries
        private static string DeleteAgent = @"delete from dbo.tblAgent where AgentID=";
        private static string DeleteAranzman = @"delete from dbo.tblAranzman where AranzmanID=";
        private static string DeletetHotel = @"delete from dbo.tblHotel where HotelID=";
        private static string DeleteIzlet = @"delete from dbo.tblIzlet where IzletID=";
        private static string DeleteKlijent = @"delete from dbo.tblKlijent where KlijentID=";
        private static string DeletePrevoznik = @"delete from dbo.tblPrevoznik where PrevoznikID=";
        private static string DeleteSoba = @"delete from dbo.tblSoba where SobaID=";
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            connection = con.KreirajKonekciju();
            loadData(SelectAgent);
        }

        private void loadData(string SelectString)
        {
            try
            {
                connection.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(SelectString, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                if (dataGridCentralni != null)
                    dataGridCentralni.ItemsSource = dataTable.DefaultView;
                CurrentTable = SelectString;
                dataAdapter.Dispose();
                dataTable.Dispose();
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

        private void btnAgent_Click(object sender, RoutedEventArgs e)
        {
            loadData(SelectAgent);
        }

        private void btnAranzman_Click(object sender, RoutedEventArgs e)
        {
            loadData(SelectAranzman);
        }

        private void btnHotel_Click(object sender, RoutedEventArgs e)
        {
            loadData(SelectHotel);
        }

        private void btnIzlet_Click(object sender, RoutedEventArgs e)
        {
            loadData(SelectIzlet);
        }

        private void btnKlijent_Click(object sender, RoutedEventArgs e)
        {
            loadData(SelectKlijent);
        }

        private void btnPrevoznik_Click(object sender, RoutedEventArgs e)
        {
            loadData(SelectPrevoznik);
        }

        private void btnSoba_Click(object sender, RoutedEventArgs e)
        {
            loadData(SelectSoba);
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window form;
            if (CurrentTable.Equals(SelectAgent))
            {
                form = new AgentFrm();
                form.ShowDialog();
                loadData(SelectAgent);
            }
            else if (CurrentTable.Equals(SelectAranzman))
            {
                form = new AranzmanFrm();
                form.ShowDialog();
                loadData(SelectAranzman);
            }
            else if (CurrentTable.Equals(SelectHotel))
            {
                form = new HotelFrm();
                form.ShowDialog();
                loadData(SelectHotel);
            }
            else if (CurrentTable.Equals(SelectIzlet))
            {
                form = new IzletFrm();
                form.ShowDialog();
                loadData(SelectIzlet);
            }
            else if (CurrentTable.Equals(SelectKlijent))
            {
                form = new KlijentFrm();
                form.ShowDialog();
                loadData(SelectKlijent);
            }
            else if (CurrentTable.Equals(SelectPrevoznik))
            {
                form = new PrevoznikFrm();
                form.ShowDialog();
                loadData(SelectPrevoznik);
            }
            else if (CurrentTable.Equals(SelectSoba))
            {
                form = new SobaFrm();
                form.ShowDialog();
                loadData(SelectSoba);
            }
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTable.Equals(SelectAgent))
            {
                FillForm(SelectStatementAgent);
                loadData(SelectAgent);
            }
            else if (CurrentTable.Equals(SelectAranzman))
            {
                FillForm(SelectStatementAranzman);
                loadData(SelectAranzman);
            }
            else if (CurrentTable.Equals(SelectHotel))
            {
                FillForm(SelectStatementHotel);
                loadData(SelectHotel);
            }
            else if (CurrentTable.Equals(SelectIzlet))
            {
                FillForm(SelectStatementIzlet);
                loadData(SelectIzlet);
            }
            else if (CurrentTable.Equals(SelectKlijent))
            {
                FillForm(SelectStatementKlijent);
                loadData(SelectKlijent);
            }
            else if (CurrentTable.Equals(SelectPrevoznik))
            {
                FillForm(SelectStatementPrevoznik);
                loadData(SelectPrevoznik);
            }
            else if (CurrentTable.Equals(SelectSoba))
            {
                FillForm(SelectStatementSoba);
                loadData(SelectSoba);
            }
        }

        private void FillForm(string selectStatement)
        {
            try
            {
                connection.Open();
                update = true;
                row = (DataRowView)dataGridCentralni.SelectedItems[0];
                SqlCommand cmd = new SqlCommand { Connection = connection };
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = row["ID"];
                cmd.CommandText = selectStatement + "@ID";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (CurrentTable.Equals(SelectAgent))
                    {
                        AgentFrm FormAgent = new AgentFrm(update, row);
                        FormAgent.txtIme.Text = reader["Ime"].ToString();
                        FormAgent.txtPrezime.Text = reader["Prezime"].ToString();
                        FormAgent.txtEmail.Text = reader["Email"].ToString();
                        FormAgent.txtBrojTel.Text = reader["BrojTel"].ToString();
                        FormAgent.ShowDialog();                 
                    }
                    else if (CurrentTable.Equals(SelectAranzman))
                    {
                        AranzmanFrm FormAranzman = new AranzmanFrm(update, row);
                        FormAranzman.txtDrzava.Text = reader["Drzava"].ToString();
                        FormAranzman.txtGrad.Text = reader["Grad"].ToString();
                        FormAranzman.txtCena.Text = reader["Cena"].ToString();
                        FormAranzman.cbHotel.SelectedValue = reader["HotelID"].ToString();
                        FormAranzman.cbAgent.SelectedValue = reader["AgentID"].ToString();
                        FormAranzman.cbIzlet.SelectedValue = reader["IzletID"].ToString();
                        FormAranzman.cbPrevoznik.SelectedValue = reader["PrevoznikID"].ToString();
                        FormAranzman.ShowDialog();
                    }
                    else if (CurrentTable.Equals(SelectHotel))
                    {
                        HotelFrm FormHotel = new HotelFrm(update, row);
                        FormHotel.txtNaziv.Text = reader["Naziv"].ToString();
                        FormHotel.txtAdresa.Text = reader["Adresa"].ToString();
                        FormHotel.txtBrojTel.Text = reader["BrojTel"].ToString();
                        FormHotel.txtEmail.Text = reader["Email"].ToString();
                        FormHotel.txtKapacitet.Text = reader["Kapacitet"].ToString();
                        FormHotel.ShowDialog();
                    }
                    else if (CurrentTable.Equals(SelectIzlet))
                    {
                        IzletFrm FormIzlet = new IzletFrm(update, row);
                        FormIzlet.txtDestinacija.Text = reader["Destinacija"].ToString();
                        FormIzlet.txtCena.Text = reader["Cena"].ToString();
                        FormIzlet.date.SelectedDate = (DateTime)reader["DatumObilaska"];
                        FormIzlet.cbAgent.SelectedValue = reader["AgentID"].ToString();
                        FormIzlet.ShowDialog();
                    }
                    else if (CurrentTable.Equals(SelectKlijent))
                    {
                        KlijentFrm FormKlijent = new KlijentFrm(update, row);
                        FormKlijent.txtIme.Text = reader["Ime"].ToString();
                        FormKlijent.txtPrezime.Text = reader["Prezime"].ToString();
                        FormKlijent.txtBrojTel.Text = reader["BrojTel"].ToString();
                        FormKlijent.txtBrojPas.Text = reader["BrojPas"].ToString();
                        FormKlijent.txtEmail.Text = reader["Email"].ToString();
                        FormKlijent.txtOsiguranje.Text = reader["Osiguranje"].ToString();
                        FormKlijent.cbAranzman.SelectedValue = reader["AranzmanID"].ToString();
                        FormKlijent.ShowDialog();
                    }
                    else if (CurrentTable.Equals(SelectPrevoznik))
                    {
                        PrevoznikFrm FormPrevoznik = new PrevoznikFrm(update, row);
                        FormPrevoznik.txtKapacitet.Text = reader["Kapacitet"].ToString();
                        FormPrevoznik.date1.SelectedDate = (DateTime)reader["DatumPocetka"];
                        FormPrevoznik.date2.SelectedDate = (DateTime)reader["DatumKraja"];
                        FormPrevoznik.txtCena.Text = reader["Cena"].ToString();
                        FormPrevoznik.ShowDialog();
                    }
                    else if (CurrentTable.Equals(SelectSoba))
                    {
                        SobaFrm FormSoba = new SobaFrm(update, row);
                        FormSoba.txtKapacitet.Text = reader["Kapacitet"].ToString();
                        FormSoba.cbHotel.SelectedValue = reader["HotelID"].ToString();
                        FormSoba.ShowDialog();
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Nije selektovan ni jedan red!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        private void btnObrisi_Click_1(object sender, RoutedEventArgs e)
        {
            if (CurrentTable.Equals(SelectAgent))
            {
                DeleteData(DeleteAgent);
                loadData(SelectAgent);
            }
            else if (CurrentTable.Equals(SelectAranzman))
            {
                DeleteData(DeleteAranzman);
                loadData(SelectAranzman);
            }
            else if (CurrentTable.Equals(SelectHotel))
            {
                DeleteData(DeletetHotel);
                loadData(SelectHotel);
            }
            else if (CurrentTable.Equals(SelectIzlet))
            {
                DeleteData(DeleteIzlet);
                loadData(SelectIzlet);
            }
            else if (CurrentTable.Equals(SelectKlijent))
            {
                DeleteData(DeleteKlijent);
                loadData(SelectKlijent);
            }
            else if (CurrentTable.Equals(SelectPrevoznik))
            {
                DeleteData(DeletePrevoznik);
                loadData(SelectPrevoznik);
            }
            else if (CurrentTable.Equals(SelectSoba))
            {
                DeleteData(DeleteSoba);
                loadData(SelectSoba);
            }
        }

        private void DeleteData(string deleteQuery)
        {
            try
            {
                connection.Open();
                row = (DataRowView)dataGridCentralni.SelectedItems[0];
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da obrisete?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = connection
                    };
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = deleteQuery + "@ID";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Nije selektovan ni jedan red!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException)
            {
                MessageBox.Show("Ima povezanih podataka sa drugim tabelama!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }
    }
}
