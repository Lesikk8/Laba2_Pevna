using System;
using System.Collections.Generic;
using LabXML.Model;

namespace LabXML.XML;

public interface IXMLStrategy
{
    public bool Execute();
    public List<Sale> GetAllSales();
    public List<Sale> GetByInvoiceId(string invoiceId);
    public List<Sale> GetByMarketBranch(Branch marketBranch);
    public List<Sale> GetByCity(string city);
    public List<Sale> GetByCustomerType(CustomerType customerType);
    public List<Sale> GetByGender(Gender gender);
    public List<Sale> GetByProductLine(string productLine);
    public List<Sale> GetByProductUnitPrice(double min, double max);
    public List<Sale> GetByProductQuantity(int min, int max);
    public List<Sale> GetByProductCostWithoutTax(double min, double max);
    public List<Sale> GetByProductTax(double min, double max);
    public List<Sale> GetByProductTotal(double min, double max);
    public List<Sale> GetByDate(DateTime dateTimeMin, DateTime dateTimeMax);
    public List<Sale> GetByPayment(Payment payment);
    public List<Sale> GetByRating(double min, double max);

    
}