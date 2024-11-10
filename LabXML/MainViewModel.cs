using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabXML.Model;
using LabXML.XML;

namespace LabXML;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] 
    public List<Sale> sales;

    [ObservableProperty]
    public int salesNumber;
    
    [ObservableProperty] public string invoiceId = "";
    [ObservableProperty] public Branch marketBranch;
    [ObservableProperty] public string city = "";
    [ObservableProperty] public CustomerType customerType;
    [ObservableProperty] public Gender gender;
    [ObservableProperty] public string productLine = "";
    [ObservableProperty] public double productUnitPriceMin;
    [ObservableProperty] public double productUnitPriceMax;
    [ObservableProperty] public int productQuantityMin;
    [ObservableProperty] public int productQuantityMax;
    [ObservableProperty] public double productCostWithoutTaxMin;
    [ObservableProperty] public double productCostWithoutTaxMax;
    [ObservableProperty] public double productTaxMin;
    [ObservableProperty] public double productTaxMax;
    [ObservableProperty] public double productTotalMin;
    [ObservableProperty] public double productTotalMax;
    [ObservableProperty] public DateTime dateTimeMin;
    [ObservableProperty] public DateTime dateTimeMax;
    [ObservableProperty] public Payment payment;
    [ObservableProperty] public double ratingMin;
    [ObservableProperty] public double ratingMax;
    [ObservableProperty] public string sourcePath;
    public IXMLStrategy CurrentStrategy { get; set; }
    public Filters CurrentFilter { get; set; }
    public Branch[] BranchOptions { get; set; }
    public CustomerType[] CustomerTypeOptions { get; set; }
    public Gender[] GenderOptions { get; set; }
    public Payment[] PaymentOptions { get; set; }

    [ObservableProperty]
    public XMLStrategies currentStrategyType;
    
    private IXMLStrategy _domStrategy;
    private IXMLStrategy _saxStrategy;
    private IXMLStrategy _linqStrategy;
    
    [RelayCommand]
    private async void WindowLoaded()
    {
        while (true)
        {
            await Task.Delay(1000);
            Debug.WriteLine(ProductTaxMax);
        }
    }

    [RelayCommand]
    private void SaveHtml()
    {
        var invoiceIds = Sales.Select(s => s.InvoiceId);
        XElement root = XElement.Load(sourcePath);
        var saleElements = root.Elements().Where(x => invoiceIds.Contains(x.Element("InvoiceId").Value));
        var xml = $"<Sales>{string.Concat(saleElements)}</Sales>";
        
        XslCompiledTransform transform = new XslCompiledTransform();
        transform.Load(@"C:\Users\Олеся Певна\Downloads\Projects2\OOP_Lab2\LabXML\Dataset\output.xsl");
        StringWriter results = new StringWriter();
        XmlReader xmlStringReader = XmlReader.Create(new StringReader(xml));
        transform.Transform(xmlStringReader, null, results);
        File.WriteAllText(@"C:\Users\Олеся Певна\Downloads\Projects2\OOP_Lab2\LabXML\Dataset\abc.html", results.ToString());
    }
    
    [RelayCommand]
    private void Filter()
    {
        switch (CurrentStrategyType)
        {
            case XMLStrategies.DOM:
                CurrentStrategy = _domStrategy;
                break;
            case XMLStrategies.SAX:
                CurrentStrategy = _saxStrategy;
                break;
            case XMLStrategies.LINQ:
                CurrentStrategy = _linqStrategy;
                break;
        }
        switch (CurrentFilter)
        {
            case Filters.None:
                Sales = CurrentStrategy.GetAllSales();
                break;
            case Filters.InvoiceId:
                 Sales = CurrentStrategy.GetByInvoiceId(InvoiceId);
                break;
            case Filters.MarketBranch:
                Sales = CurrentStrategy.GetByMarketBranch(MarketBranch);
                break;
            case Filters.City:
                Sales = CurrentStrategy.GetByCity(City);
                break;
            case Filters.CustomerType:
                Sales = CurrentStrategy.GetByCustomerType(CustomerType);
                break;
            case Filters.Gender:
                Sales = CurrentStrategy.GetByGender(Gender);
                break;
            case Filters.ProductLine:
                Sales = CurrentStrategy.GetByProductLine(ProductLine);
                break;
            case Filters.ProductUnitPrice:
                Sales = CurrentStrategy.GetByProductUnitPrice(ProductUnitPriceMin, ProductUnitPriceMax);
                break;
            case Filters.ProductQuantity:
                Sales = CurrentStrategy.GetByProductQuantity(ProductQuantityMin, ProductQuantityMax);
                break;
            case Filters.ProductCost:
                Sales = CurrentStrategy.GetByProductUnitPrice(ProductCostWithoutTaxMin, ProductCostWithoutTaxMax);
                break;
            case Filters.ProductTax:
                Sales = CurrentStrategy.GetByProductTax(ProductTaxMin, ProductTaxMax);
                break;
            case Filters.ProductTotal:
                Sales = CurrentStrategy.GetByProductTotal(ProductTotalMin, ProductTotalMax);
                break;
            case Filters.Date:
                Sales = CurrentStrategy.GetByDate(DateTimeMin, DateTimeMax);
                break;
            case Filters.Payment:
                Sales = CurrentStrategy.GetByPayment(Payment);
                break;
            case Filters.Rating:
                Sales = CurrentStrategy.GetByRating(RatingMin, RatingMax);
                break;
        }
        SalesNumber = Sales.Count;
    }
    public MainViewModel()
    {
        sourcePath = @"C:\Users\Олеся Певна\Downloads\Projects2\OOP_Lab2\LabXML\Dataset\supermarket_sales - Sheet1 (3).xml";
        _domStrategy = new DOMStrategy(sourcePath);
        _domStrategy.Execute();
        _saxStrategy = new SAXStrategy(sourcePath);
        _saxStrategy.Execute();
        _linqStrategy = new LINQStrategy(sourcePath);
        _linqStrategy.Execute();
        CurrentStrategy = _domStrategy;
        Sales = CurrentStrategy.GetAllSales();
        SalesNumber = Sales.Count;
        dateTimeMin = DateTime.Now;
        dateTimeMax = DateTime.Now;
        BranchOptions = Enum.GetValues<Branch>();
        CustomerTypeOptions = Enum.GetValues<CustomerType>();
        GenderOptions = Enum.GetValues<Gender>();
        PaymentOptions = Enum.GetValues<Payment>();
    }
}