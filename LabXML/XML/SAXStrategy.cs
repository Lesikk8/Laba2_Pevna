using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using LabXML.Model;

namespace LabXML.XML;

public class SAXStrategy : BaseXMLStrategy
{
    private readonly XmlReader _reader;
    private Scopes _currentScope;
    private readonly Dictionary<string, Scopes> _stringToScope;
    private Dictionary<Scopes, Action<Sale, string>> _scopeToValue;

    public SAXStrategy(string xmlPath)
    {
        _reader = XmlReader.Create(xmlPath, new XmlReaderSettings());
        _sales = new List<Sale>();
        _stringToScope = new Dictionary<string, Scopes>
        {
            {"InvoiceId", Scopes.InvoiceId},
            {"Branch", Scopes.Branch},
            {"City", Scopes.City},
            {"CustomerType", Scopes.CustomerType},
            {"Gender", Scopes.Gender},
            {"ProductLine", Scopes.ProductLine},
            {"UnitPrice", Scopes.UnitPrice},
            {"Quantity", Scopes.Quantity},
            {"Tax", Scopes.Tax},
            {"Total", Scopes.Total},
            {"Date", Scopes.Date},
            {"Time", Scopes.Time},
            {"Payment", Scopes.Payment},
            {"CostOfGoods", Scopes.CostOfGoods},
            {"Rating", Scopes.Rating}
        };
    }

    public override bool Execute()
    {
        var currentSale = new Sale();
        while (_reader.Read())
            if (_reader.NodeType == XmlNodeType.Element)
            {
                if (_reader.Name == "Sale") currentSale = new Sale();
                else if (_stringToScope.Keys.Contains(_reader.Name))
                    _currentScope = _stringToScope[_reader.Name];
                else if (_reader.Name != "Sales") return false;
            }
            else if (_reader.NodeType == XmlNodeType.Text)
            {
                switch (_currentScope)
                {
                    case Scopes.None:
                        break;
                    case Scopes.InvoiceId:
                        currentSale.InvoiceId = _reader.Value;
                        break;
                    case Scopes.Branch:
                        if (!Enum.TryParse(_reader.Value, out Branch marketBranch)) return false;
                        currentSale.MarketBranch = marketBranch;
                        break;
                    case Scopes.City:
                        currentSale.City = _reader.Value;
                        break;
                    case Scopes.CustomerType:
                        if (!Enum.TryParse(_reader.Value, out CustomerType customerType)) return false;
                        currentSale.CustomerType = customerType;
                        break;
                    case Scopes.Gender:
                        if (!Enum.TryParse(_reader.Value, out Gender gender)) return false;
                        currentSale.Gender = gender;
                        break;
                    case Scopes.ProductLine:
                        currentSale.ProductLine = _reader.Value;
                        break;
                    case Scopes.UnitPrice:
                        if (!double.TryParse(_reader.Value, NumberStyles.Number,
                                CultureInfo.InvariantCulture, out var unitPrice)) return false;
                        currentSale.ProductUnitPrice = unitPrice;
                        break;
                    case Scopes.Quantity:
                        if (!int.TryParse(_reader.Value, out var quantity)) return false;
                        currentSale.ProductQuantity = quantity;
                        break;
                    case Scopes.Tax:
                        if (!double.TryParse(_reader.Value, NumberStyles.Number,
                                CultureInfo.InvariantCulture, out var tax)) return false;
                        currentSale.ProductTax = tax;
                        break;
                    case Scopes.Total:
                        if (!double.TryParse(_reader.Value, NumberStyles.Number,
                                CultureInfo.InvariantCulture, out var total)) return false;
                        currentSale.ProductTotal = total;
                        break;
                    case Scopes.Date:
                        if (!DateTime.TryParseExact(_reader.Value, "M/d/yyyy", null, DateTimeStyles.None, out var date))
                            return false;
                        currentSale.Date = date;
                        break;
                    case Scopes.Time:
                        if (!TimeSpan.TryParseExact(_reader.Value, @"h\:mm", null, TimeSpanStyles.None, out var time))
                            return false;
                        currentSale.Date += time;
                        break;
                    case Scopes.Payment:
                        var val = _reader.Value == "Ewallet" ? "EWallet" : _reader.Value;
                        val = _reader.Value == "Credit card" ? "CreditCard" : val;
                        if (!Enum.TryParse(val, out Payment payment)) return false;
                        currentSale.Payment = payment;
                        break;
                    case Scopes.CostOfGoods:
                        if (!double.TryParse(_reader.Value, NumberStyles.Number,
                                CultureInfo.InvariantCulture, out var costOfGoods)) return false;
                        currentSale.ProductCostWithoutTax = costOfGoods;
                        break;
                    case Scopes.Rating:
                        if (!double.TryParse(_reader.Value, NumberStyles.Number,
                                CultureInfo.InvariantCulture, out var rating)) return false;
                        currentSale.Rating = rating;
                        break;
                }
            }
            else if (_reader.NodeType == XmlNodeType.EndElement)
            {
                switch (_reader.Name)
                {
                    case "Sale":
                        _sales.Add(currentSale);
                        break;
                    default:
                        _currentScope = Scopes.None;
                        break;
                }
            }

        return true;
    }

    private enum Scopes
    {
        None,
        InvoiceId,
        Branch,
        City,
        CustomerType,
        Gender,
        ProductLine,
        UnitPrice,
        Quantity,
        Tax,
        Total,
        Date,
        Time,
        Payment,
        CostOfGoods,
        Rating
    }
}