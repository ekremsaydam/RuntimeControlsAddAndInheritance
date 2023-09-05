using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Windows.Forms.DataVisualization.Charting;
using static System.IO.Directory;
using static System.Windows.Forms.Application;
using static System.Windows.Forms.Screen;

namespace ControlsDialogsComponents
{
    public partial class VTSample : Form
    {
        private int _height;
        private DataTable _dataTable;
        private BindingNavigator _bindingNavigator;
        private BindingSource _bindingSource;
        private DataGridView _dataGridView;
        private ToolStripStatusLabel _toolStripStatusLabel, _toolStripLabel;
        private StatusStrip _statusStrip;
        private ToolStripComboBox _toolstripComboFilter, _toolStripComboBox;
        private NotifyIcon _notifyIcon;

        public VTSample() => InitializeComponent();

        private string _computerName;
        private void VTSample_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.Sizable;
            Left = Top = 0;
            Size = PrimaryScreen!.WorkingArea.Size;
            _height = Size.Height;
            ShowInTaskbar = false;
            AutoScroll = true;
            SqlConnectionStringBuilder builder = new();
            _computerName = Environment.MachineName;
            //builder.DataSource = computerName;
            builder.DataSource = @"192.168.10.100\BEDB";
            builder.InitialCatalog = "Northwind";
            builder.UserID = "sa";
            builder.Password = "Password1";

            using SqlConnection sqlConnection = new(builder.ConnectionString);
            using SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Products", sqlConnection);
            _dataTable = new();
            try
            {
                DataSet dataSet = new();
                _dataTable.TableName = "1.Tablom";
                dataSet.Tables.Add(_dataTable);
                sqlDataAdapter.Fill(_dataTable);
            }
            catch (Exception ex)
            {
                throw new Exception($"Server Adı: {builder["Server"]} veya" +
                                    $"Database Adı: {builder["Database"]} ya da bağlantı adaptörü" +
                                    $"{typeof(SqlDataAdapter)} hatalı veya olası diğer hatalar" +
                                    $"{ex.Message}");
            }

            SplitContainer splitContainer = new()
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
            };
            Controls.Add(splitContainer);

            _bindingSource = new BindingSource
            {
                DataSource = _dataTable
            };

            _bindingNavigator = new(true)
            {
                BindingSource = _bindingSource,
            };
            _bindingNavigator.DeleteItem.Enabled = false;
            _bindingNavigator.AddNewItem.Enabled = false;
            splitContainer.SplitterDistance = _bindingNavigator.Height;
            splitContainer.Panel1.Controls.Add(_bindingNavigator);

            _dataGridView = new();
            _dataGridView.AllowDrop = _dataGridView.AllowUserToAddRows = false;
            _dataGridView.AllowUserToDeleteRows = false;
            _dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //_dataGridView.AutoResizeColumn(1);
            _dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            _dataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
            _dataGridView.DataSource = _bindingSource;
            _dataGridView.Dock = DockStyle.Fill;
            _dataGridView.ColumnHeadersVisible = true;
            splitContainer.Panel2.Controls.Add(_dataGridView);
            DataGridViewRowColor(_dataGridView);
            _dataGridView.DataBindingComplete += _dataGridView_DataBindingComplete;

            _statusStrip = new();
            _toolStripStatusLabel = new();
            _toolStripLabel = new();
            ToolStripButton toolStripButton = new();
            ToolStripSplitButton toolStripSplitButton = new();

            _toolstripComboFilter = new();
            _toolStripComboBox = new();

            _statusStrip.Items.Add(_toolStripStatusLabel);
            ToolStripSeparator toolStripSeparator = new();
            _statusStrip.Items.Add(toolStripSeparator);
            _statusStrip.Items.Add(_toolStripLabel);
            _statusStrip.Items.Add(_toolStripComboBox);

            ToolStripComboBoxFill(_toolStripComboBox, out var gridWidth);
            _toolStripComboBox.SelectedIndexChanged += _toolStripComboBox_SelectedIndexChanged;

            _statusStrip.Items.Add(_toolstripComboFilter);
            _statusStrip.Items.Add(toolStripButton);
            toolStripButton.Click += ToolStripButton_Click;
            toolStripButton.Text = "Filtrele";

            string directoryInfo = GetParent(StartupPath)!.Parent!.Parent!.Parent!.FullName;

            toolStripSplitButton.Image = Image.FromFile(directoryInfo + "\\images\\chart.png");
            toolStripSplitButton.Click += ToolStripSplitButton_Click;

            _statusStrip.Items.Add(toolStripSplitButton);
            ToolStripButton splitButtonPrint = new()
            {
                Image = Image.FromFile(directoryInfo + "\\images\\printer.png")
            };
            splitButtonPrint.Click += SplitButtonPrint_Click;
            _statusStrip.Items.Add(splitButtonPrint);
            Controls.Add(_statusStrip);

            Width = gridWidth + 96;
            LabelWriter();

            Icon sysIcon = SystemIcons.Information;
            Icon icon = new(sysIcon, 50, 50);
            _notifyIcon = new NotifyIcon()
            {
                Icon = icon,
                BalloonTipIcon = ToolTipIcon.Info,
                BalloonTipTitle = (string)builder["Database"],
                Text = sqlDataAdapter.SelectCommand.CommandText,
                Visible = true
            };

            FormClosing += VTSample_FormClosing;
        }

        private void VTSample_FormClosing(object? sender, FormClosingEventArgs e) => _notifyIcon.Dispose();

        PrintDocument printDocument;
        private Bitmap bitmap;
        private void SplitButtonPrint_Click(object? sender, EventArgs e)
        {
            printDocument = new();
            printDocument.PrintPage += PrintDocument_PrintPage;
            PrintDialog printDialog = new()
            {
                Document = printDocument
            };

            printDialog.UseEXDialog = true;
            bitmap = new(_dataGridView.Width, _dataGridView.Height);
            _dataGridView.DrawToBitmap(bitmap, 
                new Rectangle(20, 20, _dataGridView.Width, _dataGridView.Height));

            DialogResult dialogResult = printDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                printDocument.DocumentName = _computerName;
                printDocument.DefaultPageSettings.Landscape = true;
                printDocument.Print();
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e) => e.Graphics!.DrawImage(bitmap, 0, 0);

        private void ToolStripSplitButton_Click(object? sender, EventArgs e)
        {
            using Form chartForm = new()
            {
                AutoSize = true,
                WindowState = FormWindowState.Normal,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                Left = Top = 0,
                Size = PrimaryScreen!.WorkingArea.Size,
                ShowInTaskbar = true,
                AutoScroll = true,
            };

            Chart chart = new()
            {
                Size = chartForm.Size
            };

            Title title = new()
            {
                Text = "Fiyat ve Stok durumu"
            };
            chart.Titles.Add(title);
            var axisX = _dataTable.Columns["ProductName"]!.Caption;
            var axisY = _dataTable.Columns["UnitPrice"]!.Caption;
            ChartArea chartArea = new();
            chartArea.AxisX.Title = axisX;
            chartArea.AxisY.Title = axisY;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisY.Interval = 10;

            Font font = new("Arial", 14, FontStyle.Bold);
            title.Font = chartArea.AxisX.TitleFont = chartArea.AxisY.TitleFont =
                chartArea.AxisX.LabelStyle.Font = chartArea.AxisY.LabelStyle.Font = font;
            chartArea.Area3DStyle.Enable3D = true;
            chartArea.Area3DStyle.LightStyle = LightStyle.Simplistic;
            chartArea.Area3DStyle.Rotation = 10;
            chartArea.Area3DStyle.WallWidth = 25;
            chartArea.Area3DStyle.Inclination = 40;
            chart.ChartAreas.Add(chartArea);
            Series series = new()
            {
                ChartType = SeriesChartType.Column
            };
            series.SetCustomProperty("DrawingStyle", "Cylinder");
            series.IsValueShownAsLabel = true;
            series.XValueMember = _dataGridView.Columns[axisX]?.DataPropertyName;
            series.YValueMembers = _dataGridView.Columns[axisY]?.DataPropertyName;

            chart.Series.Add(series);
            chart.SaveImage("Chart.bmp", ChartImageFormat.Bmp);
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.DataSource = _dataGridView.DataSource;
            chartForm.Controls.Add(chart);
            ChartColorFormat(chart, series, chartArea);
            chartForm.ShowDialog();
        }

        private static void ChartColorFormat(Chart chart, Series series, ChartArea chartArea)
        {
            chart.BackColor = chartArea.BackColor = Color.FloralWhite;
            chartArea.AxisX.MajorGrid.LineColor = chartArea.AxisY.MajorGrid.LineColor = Color.Black;
            chart.BackSecondaryColor =
                series.BackSecondaryColor = chartArea.BackSecondaryColor = Color.LightGoldenrodYellow;
            chart.BackGradientStyle =
                chartArea.BackGradientStyle = series.BackGradientStyle = GradientStyle.VerticalCenter;

            series.Palette = ChartColorPalette.Pastel;
        }

        private bool _clicked = false;
        private void ToolStripButton_Click(object? sender, EventArgs e)
        {
            if (_toolstripComboFilter.Items.Count == 0 ||
                _toolstripComboFilter.Text == string.Empty)
                return;

            _notifyIcon.Visible = true;
            _clicked = !_clicked;
            if (!_clicked)
            {
                ((ToolStripButton)sender!).Text = "Filtrele";
                _dataTable.DefaultView.RowFilter = null;
                DataGridViewRowColor(_dataGridView);
                Height = _height;
                return;
            }
            ((ToolStripButton)sender!).Text = @"Tümünü Göster";
            DataGridViewRowColor(_dataGridView);

            try
            {
                _dataTable.DefaultView.RowFilter = $"{_toolStripComboBox.Text} LIKE '%{_toolstripComboFilter.Text}'%";
            }
            catch (Exception)
            {
                if (_toolStripComboBox.Text == string.Empty) return;
                string aranan = _toolstripComboFilter.Text.Replace(",", ".");
                _dataTable.DefaultView.RowFilter = $"{_toolStripComboBox.Text} = {aranan}";
            }

            string m = $$"""
                         Toplam {{_row}} satır var.
                         Stok Adedi : {{_totalStockFilter}}
                         Bilgi ikonuna iki kez tıklayıp kaldırabilirsiniz.
                         """;
            _notifyIcon.BalloonTipText = m;
            _notifyIcon.ShowBalloonTip(3_000);
            _notifyIcon.DoubleClick += _notifyIcon_DoubleClick;
        }

        private void _notifyIcon_DoubleClick(object? sender, EventArgs e) => _notifyIcon.Visible = false;

        private void _toolStripComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var columnName = ((ToolStripComboBox)sender!).Text;
            DataColumn dataColumn = _dataTable.Columns[columnName]!;
            ControlsClear(_toolstripComboFilter);
            //_toolstripComboFilter.Shorted = true;

            foreach (DataRow item in _dataTable.Rows)
            {
                if (!_toolstripComboFilter.Items.Contains(item[dataColumn]))
                {
                    _toolstripComboFilter.Items.Add(item[dataColumn]);
                }
            }
        }

        private void ToolStripComboBoxFill(ToolStripComboBox toolStripComboBox, out int gridGenislik)
        {
            var _shape = string.Empty;
            var width = 0;
            for (int j = 0; j < _dataGridView.Columns.Count; j++)
            {
                var dataGridViewColumn = _dataGridView.Columns[j];
                dataGridViewColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                var dTblColumn = _dataTable.Columns[j];
                if (dTblColumn.DataType == typeof(short)
                    || dTblColumn.DataType == typeof(double)
                    || dTblColumn.DataType == typeof(decimal)
                   )
                {
                    _shape = "N2";
                }
                else if (dTblColumn.DataType == typeof(int))
                {
                    _shape = "N0";
                }
                else
                {
                    dataGridViewColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }

                dataGridViewColumn.DefaultCellStyle.Format = _shape;
                width += dataGridViewColumn.Width;
                toolStripComboBox.Items.Add(_dataTable.Columns[j].ColumnName);
            }

            gridGenislik = width;
        }

        private void _dataGridView_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e) =>
            LabelWriter();

        private int _row;
        private double _totalStockFilter;
        private void LabelWriter()
        {
            ControlsClear(_toolStripStatusLabel);
            var totalStock = _dataTable.Compute("SUM(UnitsInStock)", "true");
            _toolStripStatusLabel.Text = $"Toplam Stok: {totalStock:0,00} adet.";
            _row = _dataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible);
            _toolStripLabel.Text = $"Toplam Satır: {_row} =>";

            _totalStockFilter = _dataGridView.Rows.Cast<DataGridViewRow>()
                .Sum(x => Convert.ToDouble(x.Cells["UnitsInStock"].Value));
            _toolStripLabel.Text += $@" Filtreye göre toplam stok: {_totalStockFilter} =>";

            double totalOrder = _dataGridView.Rows.Cast<DataGridViewRow>()
                .Sum(x => Convert.ToDouble(x.Cells["UnitsOnOrder"].Value));
            _toolStripLabel.Text += $@" sipariş adedi: {totalOrder}";
        }

        private static void ControlsClear(params dynamic[]? controls)
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

        private static void DataGridViewRowColor(DataGridView dataGridView)
        {
            dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.GhostWhite;
        }
    }
}