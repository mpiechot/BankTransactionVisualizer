using System;

namespace UmsatzKategorisierung.Data
{
    /// <summary>
    ///     A class that represents a single transaction for bank transfers.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Gets or sets the account type of the owner.
        /// </summary>
        public string OwnerAccountType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the IBAN of the owner.
        /// </summary>
        public string OwnerIban { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the BIC of the owner.
        /// </summary>
        public string OwnerBic { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the bank name of the owner.
        /// </summary>
        public string OwnerBankName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the booking day.
        /// </summary>
        public DateTime BookingDay { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the valutadate.
        /// </summary>
        public DateTime ValutaDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the account type of the receiver.
        /// </summary>
        public string ReceiverAccountName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the IBAN of the receiver.
        /// </summary>
        public string ReceiverIban { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the BIC of the receiver.
        /// </summary>
        public string ReceiverBic { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the booking type.
        /// </summary>
        public string BookingType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the booking text.
        /// </summary>
        public string BookingText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the amount for the transaction.
        /// </summary>
        public double Amount { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the balance after the transaction.
        /// </summary>
        public double BalanceAfter { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets the remark.
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the initial category.
        /// This Value is set inside the CSV file and is used to determine the category.
        /// </summary>
        public string InitialCategory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Category.
        /// </summary>
        public Category Category { get; set; } = Category.Sonstiges;

        /// <summary>
        /// Gets or sets the tax relevancy.
        /// </summary>
        public string TexRelevant { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the creditor id.
        /// </summary>
        public string CreditorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the mandate reference.
        /// </summary>
        public string MandateReference { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Transaction {{'{BookingDay}' - '{BookingText}' - '{ReceiverAccountName}' - '{Amount}€' - '{Category}'}}";
        }
    }
}

