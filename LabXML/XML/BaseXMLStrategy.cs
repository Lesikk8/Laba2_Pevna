using System;
using System.Collections.Generic;
using System.Linq;
using LabXML.Model;

namespace LabXML.XML;

public abstract class BaseXMLStrategy : IXMLStrategy 
{
    protected List<Sale> _sales;

    public abstract bool Execute();
    
    public List<Sale> GetAllSales()
    {
        return _sales;
    }

    public List<Sale> GetByInvoiceId(string invoiceId)
    {
        return _sales.Where(s => s.InvoiceId.Contains(invoiceId)).ToList();
    }

    public List<Sale> GetByMarketBranch(Branch marketBranch)
    {
        return _sales.Where(s => s.MarketBranch == marketBranch).ToList();
    }

    public List<Sale> GetByCity(string city)
    {
        return _sales.Where(s => s.City.Contains(city)).ToList();
    }

    public List<Sale> GetByCustomerType(CustomerType customerType)
    {
        return _sales.Where(s => s.CustomerType == customerType).ToList();
    }

    public List<Sale> GetByGender(Gender gender)
    {
        return _sales.Where(s => s.Gender == gender).ToList();
    }

    public List<Sale> GetByProductLine(string productLine)
    {
        return _sales.Where(s => s.ProductLine.Contains(productLine)).ToList();
    }

    public List<Sale> GetByProductUnitPrice(double min, double max)
    {
        return _sales.Where(s => s.ProductUnitPrice >= min && s.ProductUnitPrice <= max).ToList();
    }

    public List<Sale> GetByProductQuantity(int min, int max)
    {
        return _sales.Where(s => s.ProductQuantity >= min && s.ProductQuantity <= max).ToList();
    }

    public List<Sale> GetByProductCostWithoutTax(double min, double max)
    {
        return _sales.Where(s => s.ProductCostWithoutTax >= min && s.ProductCostWithoutTax <= max).ToList();
    }

    public List<Sale> GetByProductTax(double min, double max)
    {
        return _sales.Where(s => s.ProductTax >= min && s.ProductTax <= max).ToList();
    }

    public List<Sale> GetByProductTotal(double min, double max)
    {
        return _sales.Where(s => s.ProductTotal >= min && s.ProductTotal <= max).ToList();
    }

    public List<Sale> GetByDate(DateTime dateTimeMin, DateTime dateTimeMax)
    {
        return _sales.Where(s => s.Date >= dateTimeMin && s.Date <= dateTimeMax).ToList();
    }

    public List<Sale> GetByPayment(Payment payment)
    {
        return _sales.Where(s => s.Payment == payment).ToList();
    }

    public List<Sale> GetByRating(double min, double max)
    {
        return _sales.Where(s => s.Rating >= min && s.Rating <= max).ToList();
    }
}