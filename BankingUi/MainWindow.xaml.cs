using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Windows;

namespace BankingUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LiveCharts.WinForms.PieChart pieChart = new LiveCharts.WinForms.PieChart();
            Random rnd = new Random();
            SeriesCollection series = new SeriesCollection();
            for (int i = 0; i < 5; i++)
            {
                series.Add(new PieSeries
                {
                    Title = $"Data {i}",
                    Values = new ChartValues<double> { rnd.Next(1, 10) },
                    DataLabels = true
                });
            }
            pieChart.Series = series;

        }
    }
}
