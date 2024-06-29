namespace UmsatzKategorisierung.Data
{
    public static class TransactionConverter
    {
        public static Transaction Convert(string text)
        {
            var columns = text.Split(';');
            var dataPoint = new Transaction()
            {
                OwnerAccountType = columns[0],
                OwnerIban = columns[1],
                OwnerBic = columns[2],
                OwnerBankName = columns[3],
                BookingDay = DateTime.Parse(columns[4]),
                ValutaDate = DateTime.Parse(columns[5]),
                ReceiverAccountName = columns[6],
                ReceiverIban = columns[7],
                ReceiverBic = columns[8],
                BookingType = columns[9],
                BookingText = columns[10],
                Amount = double.Parse(columns[11]),
                Currency = columns[12],
                BalanceAfter = double.Parse(columns[13]),
                Remark = columns[14],
                InitialCategory = columns[15],
                TexRelevant = columns[16],
                CreditorId = columns[17],
                MandateReference = columns[18],
            };

            dataPoint.Category = Categorizer.FindCategoryForTransaktion(dataPoint);

            return dataPoint;
        }

    }
}
