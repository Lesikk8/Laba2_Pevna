using System;
using System.Globalization;

namespace LabXML.Model;

public class SaleFactory
{
    private static SaleFactory _instance = null;
    public static SaleFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaleFactory();
            }
            return _instance;
        }
    }

    public Sale Create(string invoiceIdStr, string branchStr, string cityStr, string customerTypeStr, string genderStr, string productLineStr, string unitPriceStr, string quantityStr, string taxStr, string totalStr, string dateStr, string timeStr, string paymentStr, string costOfGoodsStr, string ratingStr)
    {
        var sale = new Sale();
        bool isValid = true;
        
        sale.InvoiceId = invoiceIdStr;

        isValid &= Enum.TryParse(branchStr, out Branch branch);
        sale.MarketBranch = branch;

        sale.City = cityStr;

        isValid &= Enum.TryParse(customerTypeStr, out CustomerType customerType);
        sale.CustomerType = customerType;

        isValid &= Enum.TryParse(genderStr, out Gender gender);
        sale.Gender = gender;

        sale.ProductLine = productLineStr;

        isValid &= double.TryParse(unitPriceStr, NumberStyles.Number,
            CultureInfo.InvariantCulture, out double unitPrice);
        sale.ProductUnitPrice = unitPrice;

        isValid &= int.TryParse(quantityStr, out int quantity);
        sale.ProductQuantity = quantity;

        isValid &= double.TryParse(taxStr, NumberStyles.Number,
            CultureInfo.InvariantCulture, out double tax);
        sale.ProductTax = tax;

        isValid &= double.TryParse(totalStr, NumberStyles.Number,
            CultureInfo.InvariantCulture, out double total);
        sale.ProductTotal = total;

        isValid &= DateTime.TryParseExact(dateStr, "M/d/yyyy", null, DateTimeStyles.None, out var date);
        sale.Date = date;
        isValid &= TimeSpan.TryParseExact(timeStr, @"h\:mm", null, TimeSpanStyles.None, out var time);
        sale.Date += time;

        var paymentText = paymentStr;
        var val = paymentText == "Ewallet" ? "EWallet" : paymentText;
        val = paymentText == "Credit card" ? "CreditCard" : val;
        isValid &= Enum.TryParse(val, out Payment payment);
        sale.Payment = payment;

        isValid &= double.TryParse(costOfGoodsStr, NumberStyles.Number,
            CultureInfo.InvariantCulture, out double costOfGoods);
        sale.ProductCostWithoutTax = costOfGoods;

        isValid &= double.TryParse(ratingStr, NumberStyles.Number,
            CultureInfo.InvariantCulture, out double rating);
        sale.Rating = rating;

        return isValid ? sale : null;
    } 
}