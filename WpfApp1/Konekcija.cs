using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace IT73_2022
{
    class Konekcija
    {
        public SqlConnection KreirajKonekciju()
        {
            SqlConnectionStringBuilder ccnSb = new SqlConnectionStringBuilder
            {
                DataSource = @"3CATS",
                InitialCatalog = "TuristickaAgencija",
                IntegratedSecurity = true
            };
            string con = ccnSb.ToString();
            SqlConnection konekcija = new SqlConnection(con);
            return konekcija;
        }
    }
}
