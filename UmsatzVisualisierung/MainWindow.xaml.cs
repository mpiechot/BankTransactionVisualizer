using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UmsatzKategorisierung.Data;
using File = System.IO.File;

namespace UmsatzVisualisierung
{
    public partial class MainWindow : Window
    {
        private List<IGrouping<(int Month, int Year), Transaction>> transactions;

        private List<Category> selectedCategories = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();

        public SeriesCollection LineCollection { get; set; }
        public SeriesCollection PieChartData { get; set; }
        public List<int> Years { get; set; }
        public List<int> Months { get; set; }
        public int SelectedYear { get; set; }
        public int SelectedMonth { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            transactions = File.ReadAllLines(@"C:\MARPIE\C#-Projekte\UmsatzKategorisierung\UmsatzVisualisierung\Data\Umsaetze_Gesamt.csv")
                .Skip(1)                                // Skip the header
                .Select(TransactionConverter.Convert)   // Convert each line to a transaction
                .GroupBy(data => (data.BookingDay.Month, data.BookingDay.Year))
                .ToList();

            Years = transactions.Select(group => group.Key.Year).Distinct().ToList();
            Months = Enumerable.Range(1, 12).ToList();

            SelectedYear = Years.Min();
            SelectedMonth = Months.Min();

            PieChartData = new SeriesCollection();
            LineCollection = new SeriesCollection();
            CategoryListBox.ItemsSource = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();

            var categories = Enum.GetValues(typeof(Category)).Cast<Category>();
            cbCategory.ItemsSource = categories.ToList();
            // Set the first item as selected
            if (categories.Any())
            {
                cbCategory.SelectedItem = categories.First();
            }

            SetupLineChart();
            UpdatePieChart();
            UpdateCategoryTable();

            DataContext = this;
        }

        private void UpdateCategoryTable()
        {
            // Assuming the name of your DataGrid is dataGridTransactions
            // Assuming the name of your ComboBox is comboBoxCategory
            var selectedCategory = (Category)cbCategory.SelectedItem;

            var selectedTransactions = transactions
                .Where(group => group.Key.Year == SelectedYear && group.Key.Month == SelectedMonth)
                .SelectMany(group => group)
                .Where(transaction => transaction.Category == selectedCategory)
                .Select(transaction => new { Name = transaction.BookingText, Date = transaction.BookingDay, Amount = transaction.Amount })
                .ToList();

            dgTransactions.ItemsSource = selectedTransactions;
        }

        private void SetupLineChart()
        {
            //lcUmsaetze.Zoom = ZoomingOptions.X;

            //< wpf:LineSeries
            //            Title = "Umsatz"
            //            Values = "{Binding LineChartData}"
            //            DataLabels = "True"
            //            Fill = "Transparent"
            //            StrokeThickness = "1"
            //            PointGeometry = "{x:Static wpf:DefaultGeometries.Circle}" />
            var maxValue = transactions
                .Max(transactionMonth => transactionMonth
                    .Where(transaction => transaction.Amount < 0 && transaction.Category != Category.Gehalt)
                    .GroupBy(transaction => transaction.Category)
                    .Select(transactionGroup => transactionGroup.Sum(transaction => Math.Abs(transaction.Amount)))
                    .Max());
            var stepSize = 300;

            lcUmsaetze.AxisY.Add(new Axis
            {
                Title = "Amount in €",
                LabelFormatter = value => value.ToString("C"), // This will format the value as currency
                MinValue = 0, // This will ensure that the Y-Axis only draws positive values
                MaxValue = maxValue, // This will set the maximum value of the Y-Axis
                Separator = new LiveCharts.Wpf.Separator { Step = stepSize } // This will set the step size of the Y-Axis

            });

            lcUmsaetze.AxisX.Add(new Axis
            {
                Title = "Date",
                LabelFormatter = value => new DateTime(Years.Min() + ((int)value - 1) / 12, (int)value % 12 == 0 ? 12 : (int)value % 12, 1).ToString("MM/yyyy"), // This will format the value as MM/YYYY
                MinValue = 1, // This will ensure that the X-Axis only draws positive values
                Separator = new LiveCharts.Wpf.Separator { Step = 1 } // This will set the step size of the X-Axis
            });

            // Add a line for each category
            AddLines();
        }

        private void AddLines()
        {
            foreach (var category in selectedCategories)
            {
                if (category == Category.Gehalt)
                {
                    // Skip Gehalt, because it is not an expense
                    continue;
                }

                var dataPoints = new List<double>() { 0 };
                for (var year = Years.Min(); year <= Years.Max(); year++)
                {
                    for (var month = 1; month <= 12; month++)
                    {
                        var dataPoint = transactions
                            .Where(transactionMonth => transactionMonth.Key.Year == year && transactionMonth.Key.Month == month)
                            .Select(transactionMonth => transactionMonth
                                                           .Where(transaction => transaction.Category == category && transaction.Amount < 0)
                                                                                          .Sum(transaction => Math.Abs(transaction.Amount)))
                            .FirstOrDefault();
                        dataPoints.Add(dataPoint);
                    }
                }

                var lineSeries = new LineSeries
                {
                    Title = category.ToString(),
                    Values = new ChartValues<double>(dataPoints), // Populate these values with your data
                    Stroke = new SolidColorBrush(GetColorForCategory(category)),
                    StrokeThickness = 1,
                    PointGeometry = DefaultGeometries.Circle,
                    PointForeground = new SolidColorBrush(GetColorForCategory(category))
                };
                LineCollection.Add(lineSeries);
            }
        }

        private void UpdatePieChart()
        {
            if (PieChartData.Chart == null)
            {
                // PieChart is not yet initialized
                return;
            }

            PieChartData.Clear();
            var selectedGroup = transactions.FirstOrDefault(group => group.Key.Year == SelectedYear && group.Key.Month == SelectedMonth);

            if (selectedGroup == null)
            {
                PieChartData.Add(new PieSeries
                {
                    Title = "Keine Daten vorhanden",
                    Values = new ChartValues<double> { 100 },
                    DataLabels = true,
                    Fill = new SolidColorBrush(Colors.LightGray)
                });
                return;
            }

            var categoryTransactions = selectedGroup
                .Where(transaction => transaction.Amount < 0)
                .GroupBy(transaction => transaction.Category)
                .Where(category => !category.Key.ToString().Equals("Gehalt"))
                .OrderByDescending(categoryGroup => categoryGroup.Sum(data => data.Amount))
                .ToList();

            var moneySpent = 0.0;
            foreach (var category in categoryTransactions.OrderBy(group => group.Key.ToString()))
            {
                var spent = category.Sum(data => Math.Abs(data.Amount));
                var pieSeries = new PieSeries
                {
                    Title = category.Key.ToString(),
                    Values = new ChartValues<double> { spent },
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{category.Key.ToString()} ({Math.Round(chartPoint.Y)}€)",
                    Fill = new SolidColorBrush(GetColorForCategory(category.Key))
                };
                pieSeries.MouseDown += (s, e) =>
                {
                    cbCategory.SelectedItem = category.Key;
                };
                PieChartData.Add(pieSeries);
                moneySpent += spent;
            }
            tbMaxSpent.Text = $"Max: {moneySpent}€";
        }

        private Color GetColorForCategory(Category key)
        {
            switch (key)
            {
                case Category.Einkaufen:
                    return Colors.Red; // Rot für Einkaufen
                case Category.Restaurant:
                    return Colors.Blue; // Blau für Restaurant
                case Category.Bahn_Bus:
                    return Colors.Green; // Grün für Bahn/Bus
                case Category.Health:
                    return Colors.Yellow; // Gelb für Gesundheit
                case Category.Bildung:
                    return Colors.Purple; // Lila für Bildung
                case Category.Abos:
                    return Colors.Orange; // Orange für Abonnements
                case Category.Wohnung:
                    return Colors.Brown; // Braun für Wohnung
                case Category.Versicherung:
                    return Colors.Pink; // Pink für Versicherung
                case Category.Auto:
                    return Colors.Gray; // Grau für Auto
                case Category.Event:
                    return Colors.Cyan; // Cyan für Veranstaltungen
                case Category.Heimwerken:
                    return Colors.Maroon; // Kastanienbraun für Heimwerken
                case Category.Hobby:
                    return Colors.Magenta; // Magenta für Hobby
                case Category.Banking:
                    return Colors.Blue; // Blau für Banking
                case Category.Kredite:
                    return Colors.Indigo; // Blau für Banking
                case Category.Gehalt:
                    return Colors.DarkGreen; // Dunkelgrün für Gehalt
                default:
                    return Colors.Black; // Schwarz für andere Kategorien
            }
        }

        private void OnSelectedMonthYearChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePieChart();
            UpdateCategoryTable();
        }

        private void OnSelectedCategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCategoryTable();
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
        }

        private void OnCategoryManager(object sender, RoutedEventArgs e)
        {
        }

        private void OnLineCategoriesChanged(object sender, SelectionChangedEventArgs e)
        {
            lcUmsaetze.Series.Clear();

            selectedCategories = CategoryListBox.SelectedItems.Cast<Category>().ToList();

            AddLines();
        }
    }
}

