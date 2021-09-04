using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ShineALight
{
    public partial class CurvePlot : UserControl
    {
        public CurvePlot()
        {
            InitializeComponent();
        }

        public Func<double, double> Function { get; set; } = (double x) => { return x; };

        private void CurvePlot_Load(object sender, EventArgs e)
        {
            Refresh();
        }

        public override void Refresh()
        {
            chart1.Series.Clear();
            var series1 = new Series
            {
                Name = "Function",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };

            chart1.Series.Add(series1);

            for (double i = 0; i <= 1; i += 0.01)
            {
                series1.Points.AddXY(i, Function(i));
            }
            chart1.Invalidate();
        }
    }
}
