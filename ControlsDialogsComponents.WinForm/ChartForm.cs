using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlsDialogsComponents.WinForm
{
    public partial class ChartForm : Form
    {
        public ChartForm()
        {
            InitializeComponent();
        }

        public DataView DataSource { get; set; }

        private void ChartForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'northwindDataSet.Products' table. You can move, or remove it, as needed.
            this.productsTableAdapter.Fill(this.northwindDataSet.Products);
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Top = this.Left = 0;

            chart.DataSource = DataSource;
            chart.ChartAreas[0].AxisX.Title = "Ürünler";
            chart.ChartAreas[0].AxisY.Title = "Stok Miktarı";
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisY.Interval = 10;

            chart.Series[0].XValueMember = "ProductName";
            chart.Series[0].YValueMembers = "UnitsInStock";


        }
    }
}
