using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ControlsDialogsComponents.WinForm
{
    public partial class VTSample : Form
    {
        public VTSample()
        {
            InitializeComponent();
        }

        private void ProductsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.productsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.northwindDataSet);
        }

        private void VTSample_Load(object sender, EventArgs e)
        {
            this.Top = this.Left = 0;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            // TODO: This line of code loads data into the 'northwindDataSet.Products' table. You can move, or remove it, as needed.
            this.productsTableAdapter.Fill(this.northwindDataSet.Products);
            

            for (int i = 0; i < this.productsDataGridView.Columns.Count; i++)
            {
                this.toolStripComboBox.Items.Add(this.productsDataGridView.Columns[i].DataPropertyName);
            }

            notifyIcon.Icon = SystemIcons.Information;
            notifyIcon.BalloonTipTitle = this.northwindDataSet.Products.TableName;
            notifyIcon.Text = this.northwindDataSet.Products.TableName;
        }

        private void ToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var columnName = ((ToolStripComboBox)sender).Text;
            toolstripComboFilter.Text = string.Empty;
            toolstripComboFilter.Items.Clear();
            DataColumn dataColumn = this.northwindDataSet.Products.Columns[columnName];

            foreach (DataRow item in this.northwindDataSet.Products.Rows)
            {
                if (!toolstripComboFilter.Items.Contains(item[dataColumn]))
                {
                    toolstripComboFilter.Items.Add(item[dataColumn]);
                }
            }
        }

        private bool _clicked = false;
        private int _row;
        private double _totalStockFilter;
        private void ToolStripButton_Click(object sender, EventArgs e)
        {
            ControlsClear(toolStripLabel);

            if (toolstripComboFilter.Items.Count == 0 ||
                toolstripComboFilter.Text == string.Empty)
                return;

            notifyIcon.Visible = true;
            _clicked = !_clicked;
            if (!_clicked)
            {
                ((ToolStripButton)sender).Text = "Filtrele";
                this.northwindDataSet.Products.DefaultView.RowFilter = null;
                return;
            }

            ((ToolStripButton)sender).Text = @"Tümünü Göster";

            try
            {
                this.northwindDataSet.Products.DefaultView.RowFilter = $"{toolStripComboBox.Text} LIKE '%{toolstripComboFilter.Text}%'";
            }
            catch (Exception)
            {
                if (toolStripComboBox.Text == string.Empty) return;
                string aranan = toolstripComboFilter.Text.Replace(",", ".");
                this.northwindDataSet.Products.DefaultView.RowFilter = $"{toolStripComboBox.Text} = {aranan}";
            }
            finally
            {
                productsDataGridView.DataSource = this.northwindDataSet.Products.DefaultView;
            }
            _totalStockFilter = productsDataGridView.Rows.Cast<DataGridViewRow>()
                .Sum(x => Convert.ToDouble(x.Cells[6].Value));

            _row = productsDataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible);

            string m = $"Toplam {_row} satır var.\nStok Adedi : {_totalStockFilter}\nBilgi ikonuna iki kez tıklayıp kaldırabilirsiniz.";
            notifyIcon.BalloonTipText = m;
            notifyIcon.ShowBalloonTip(3_000);
        }

        private void LabelWriter()
        {
            ControlsClear(toolStripStatusLabel);

            var totalStock = this.northwindDataSet.Products.Compute("SUM(UnitsInStock)", "true");
            toolStripStatusLabel.Text = $"Toplam Stok: {totalStock:0,00} adet.";
            _row = productsDataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible);
            toolStripLabel.Text = $"Toplam Satır: {_row} =>";

            double totalStockFilter = productsDataGridView.Rows.Cast<DataGridViewRow>()
                .Sum(x => Convert.ToDouble(x.Cells[6].Value));
            toolStripLabel.Text += $@" Filtreye göre toplam stok: {totalStockFilter} =>";

            double totalOrder = productsDataGridView.Rows.Cast<DataGridViewRow>()
                .Sum(x => Convert.ToDouble(x.Cells[7].Value));
            toolStripLabel.Text += $@" sipariş adedi: {totalOrder}";
        }

        private static void ControlsClear(params dynamic[] controls)
        {
            if (controls is null) return;
            try
            {
                foreach (dynamic control in controls)
                {
                    control.Text = string.Empty;
                    control.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void productsDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            LabelWriter();
        }

        private void splitButtonPrint_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = printDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                printDocument.DocumentName = Environment.MachineName;
                printDocument.DefaultPageSettings.Landscape = true;
                printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4", 827, 1170);
                printDocument.Print();
            }
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bitmap = new Bitmap(productsDataGridView.Width, productsDataGridView.Height);
            productsDataGridView.DrawToBitmap(bitmap, new Rectangle(0, 0, productsDataGridView.Width, productsDataGridView.Height));
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        private void toolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            ChartForm chartForm = new ChartForm();
            chartForm.DataSource = this.northwindDataSet.Products.DefaultView;
            chartForm.ShowDialog();
        }
    }
}
