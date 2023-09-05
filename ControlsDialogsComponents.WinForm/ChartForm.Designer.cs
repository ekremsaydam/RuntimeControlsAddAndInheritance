using System.Windows.Forms;

namespace ControlsDialogsComponents.WinForm
{
    partial class ChartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.productsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.northwindDataSet = new ControlsDialogsComponents.WinForm.NorthwindDataSet();
            this.productsTableAdapter = new ControlsDialogsComponents.WinForm.NorthwindDataSetTableAdapters.ProductsTableAdapter();
            this.northwindDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.northwindDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.northwindDataSetBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.Color.FloralWhite;
            this.chart.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.VerticalCenter;
            this.chart.BackSecondaryColor = System.Drawing.Color.LightGoldenrodYellow;
            chartArea1.Area3DStyle.Enable3D = true;
            chartArea1.Area3DStyle.Inclination = 40;
            chartArea1.Area3DStyle.Rotation = 10;
            chartArea1.Area3DStyle.WallWidth = 25;
            chartArea1.BackColor = System.Drawing.Color.FloralWhite;
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.VerticalCenter;
            chartArea1.BackSecondaryColor = System.Drawing.Color.LightGoldenrodYellow;
            chartArea1.Name = "ChartArea";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.DataSource = this.productsBindingSource;
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Name = "chart";
            this.chart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.VerticalCenter;
            series1.BackSecondaryColor = System.Drawing.Color.LightGoldenrodYellow;
            series1.ChartArea = "ChartArea";
            series1.CustomProperties = "DrawingStyle=Cylinder";
            series1.Legend = "Legend1";
            series1.Name = "Series";
            series1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(800, 450);
            this.chart.TabIndex = 0;
            this.chart.Text = "Fiyat ve Stok durumu";
            title1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.Name = "title";
            this.chart.Titles.Add(title1);
            // 
            // productsBindingSource
            // 
            this.productsBindingSource.DataMember = "Products";
            this.productsBindingSource.DataSource = this.northwindDataSet;
            // 
            // northwindDataSet
            // 
            this.northwindDataSet.DataSetName = "NorthwindDataSet";
            this.northwindDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // productsTableAdapter
            // 
            this.productsTableAdapter.ClearBeforeFill = true;
            // 
            // northwindDataSetBindingSource
            // 
            this.northwindDataSetBindingSource.DataSource = this.northwindDataSet;
            this.northwindDataSetBindingSource.Position = 0;
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ChartForm";
            this.Text = "ChartForm";
            this.Load += new System.EventHandler(this.ChartForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.northwindDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.northwindDataSetBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private NorthwindDataSet northwindDataSet;
        private System.Windows.Forms.BindingSource productsBindingSource;
        private NorthwindDataSetTableAdapters.ProductsTableAdapter productsTableAdapter;
        private BindingSource northwindDataSetBindingSource;
    }
}