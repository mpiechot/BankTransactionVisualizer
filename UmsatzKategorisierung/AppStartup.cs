using UmsatzKategorisierung.Data;

namespace UmsatzKategorisierung
{
    public class AppStartup
    {
        private List<Transaction> transactions;

        public AppStartup(string filePath)
        {
            transactions = File.ReadAllLines(filePath)
                .Skip(1)                                // Skip the header
                .Select(TransactionConverter.Convert)   // Convert each line to a transaction
                .ToList();
        }

        public void Run()
        {
            var transactionsGroupedByCategory = transactions.GroupBy(data => (data.BookingDay.Month, data.BookingDay.Year));

            foreach (var group in transactionsGroupedByCategory)
            {
                Console.WriteLine($"{group.Key.Month}/{group.Key.Year}: {group.Sum(data => data.Amount)}");
                Console.WriteLine();
                var categoryTransactions = group.GroupBy(transaction => transaction.Category).OrderByDescending(categoryGroup => categoryGroup.Sum(data => data.Amount));
                foreach (var category in categoryTransactions)
                {
                    Console.WriteLine($"- {category.Key}: {category.Sum(data => data.Amount)}Euro");
                }
                Console.WriteLine();
            }
        }
    }
}
