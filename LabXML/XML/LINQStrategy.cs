using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using LabXML.Model;

namespace LabXML.XML;

public class LINQStrategy : IXMLStrategy
{
    private readonly XElement _root;

    public LINQStrategy(string xmlPath)
    {
        _root = XElement.Load(xmlPath);
    }
    public bool Execute()
    {
        // var a = _root.Elements("Sale");
        return true;
    }

    public List<Sale> GetAllSales()
    {
        return _root.Elements().Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByInvoiceId(string invoiceId)
    {
        return _root.Elements().Where(e => e.Element("InvoiceId").Value.Contains(invoiceId)).Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByMarketBranch(Branch marketBranch)
    {
        return _root.Elements().Where(e =>
            {
                Enum.TryParse(e.Element("Branch").Value, out Branch branch);
                return branch == marketBranch;
            }).Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByCity(string city)
    {
        return _root.Elements().Where(e => e.Element("City").Value.Contains(city))
            .Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByCustomerType(CustomerType customerType)
    {
        return _root.Elements().Where(e =>
        {
            Enum.TryParse(e.Element("CustomerType").Value, out CustomerType type);
            return type == customerType;
        }).Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByGender(Gender gender)
    {
        return _root.Elements().Where(e =>
        {
            Enum.TryParse(e.Element("Gender").Value, out Gender g);
            return g == gender;
        }).Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByProductLine(string productLine)
    {
        return _root.Elements().Where(e => e.Element("ProductLine").Value.Contains(productLine))
            .Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByProductUnitPrice(double min, double max)
    {
        return _root.Elements().Where(e =>
        {
            double.TryParse(e.Element("UnitPrice").Value, NumberStyles.Number,
                CultureInfo.InvariantCulture, out double unitPrice);
            return min <= unitPrice && unitPrice <= max;
        }).Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByProductQuantity(int min, int max)
    {
        return _root.Elements().Where(e =>
        {
            int.TryParse(e.Element("Quantity").Value, out int quantity);
            return min <= quantity && quantity <= max;
        }).Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByProductCostWithoutTax(double min, double max)
    {
        return _root.Elements().Where(e =>
        {
            double.TryParse(e.Element("CostOfGoods").Value, NumberStyles.Number,
                CultureInfo.InvariantCulture, out double cost);
            return min <= cost && cost <= max;
        }).Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByProductTax(double min, double max)
    {
        return _root.Elements().Where(e =>
        {
            double.TryParse(e.Element("Tax").Value, NumberStyles.Number,
                CultureInfo.InvariantCulture, out double tax);
            return min <= tax && tax <= max;
        }).Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByProductTotal(double min, double max)
    {
        return _root.Elements().Where(e =>
        {
            double.TryParse(e.Element("Total").Value, NumberStyles.Number,
                CultureInfo.InvariantCulture, out double total);
            return min <= total && total <= max;
        }).Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByDate(DateTime dateTimeMin, DateTime dateTimeMax)
    {
        return _root.Elements().Where(e =>
        {
            DateTime.TryParseExact(e.Element("Date").Value, "M/d/yyyy", null, DateTimeStyles.None, out var date);
            var dateTime = date;
            TimeSpan.TryParseExact(e.Element("Time").Value, @"h\:mm", null, TimeSpanStyles.None, out var time);
            dateTime += time;
            return dateTimeMin <= dateTime && dateTime <= dateTimeMax;
        }).Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByPayment(Payment payment)
    {
        return _root.Elements().Where(e =>
        {
            var paymentText = e.Element("Payment").Value;
            var val = paymentText == "Ewallet" ? "EWallet" : paymentText;
            val = paymentText == "Credit card" ? "CreditCard" : val;
            Enum.TryParse(val, out Payment p);
            return p == payment;
        }).Select(CreateSaleFromXElement).ToList();
    }

    public List<Sale> GetByRating(double min, double max)
    {
        return _root.Elements().Where(e =>
        {
            double.TryParse(e.Element("Rating").Value, NumberStyles.Number,
                CultureInfo.InvariantCulture, out double rating);
            return min <= rating && rating <= max;
        }).Select(CreateSaleFromXElement).ToList();
    }

    private Sale CreateSaleFromXElement(XElement saleElement)
    {
        var sale = new Sale();
        var invoiceIdElement = saleElement.Element("InvoiceId").Value;
        var branchElement = saleElement.Element("Branch").Value;
        var cityElement = saleElement.Element("City").Value;
        var customerTypeElement = saleElement.Element("CustomerType").Value;
        var genderElement = saleElement.Element("Gender").Value;
        var productLineElement = saleElement.Element("ProductLine").Value;
        var unitPriceElement = saleElement.Element("UnitPrice").Value;
        var quantityElement = saleElement.Element("Quantity").Value;
        var taxElement= saleElement.Element("Tax").Value;
        var totalElement = saleElement.Element("Total").Value;
        var dateElement = saleElement.Element("Date").Value;
        var timeElement = saleElement.Element("Time").Value;
        var paymentElement = saleElement.Element("Payment").Value;
        var costOfGoodsElement = saleElement.Element("CostOfGoods").Value;
        var ratingElement = saleElement.Element("Rating").Value;

        return SaleFactory.Instance.Create(invoiceIdElement, branchElement, cityElement, customerTypeElement,
            genderElement, productLineElement, unitPriceElement, quantityElement, taxElement, totalElement, dateElement,
            timeElement, paymentElement, costOfGoodsElement, ratingElement);
    }
}