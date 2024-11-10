using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using LabXML.Model;

namespace LabXML.XML;

public class DOMStrategy : BaseXMLStrategy
{
    private readonly XmlDocument _document;
    private XmlNode _root;
    private List<XmlNode> _saleNodes;
    public DOMStrategy(string xmlFilePath)
    {
        try
        {
            _document = new XmlDocument();
            _document.Load(xmlFilePath);
            _saleNodes = new List<XmlNode>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public override bool Execute()
    {
        _root = _document.SelectSingleNode("descendant::Sales");
        _saleNodes = _root.ChildNodes.Cast<XmlNode>().ToList();
        _sales = _saleNodes.Select(CreateSaleFromNode).ToList();
        return !_sales.Any(x => x == null);
    }
    private Sale CreateSaleFromNode(XmlNode saleNode)
    {
        var sale = new Sale();
        bool isValid = true;

        var invoiceId = saleNode.SelectSingleNode("descendant::InvoiceId")?.InnerText;
        var branch = saleNode.SelectSingleNode("descendant::Branch")?.InnerText;
        var city = saleNode.SelectSingleNode("descendant::City")?.InnerText;
        var customerType = saleNode.SelectSingleNode("descendant::CustomerType")?.InnerText;
        var gender = saleNode.SelectSingleNode("descendant::Gender")?.InnerText;
        var productLine = saleNode.SelectSingleNode("descendant::ProductLine")?.InnerText;
        var unitPrice = saleNode.SelectSingleNode("descendant::UnitPrice")?.InnerText;
        var quantity = saleNode.SelectSingleNode("descendant::Quantity")?.InnerText;
        var tax = saleNode.SelectSingleNode("descendant::Tax")?.InnerText;
        var total = saleNode.SelectSingleNode("descendant::Total")?.InnerText;
        var date = saleNode.SelectSingleNode("descendant::Date")?.InnerText;
        var time = saleNode.SelectSingleNode("descendant::Time")?.InnerText;
        var payment = saleNode.SelectSingleNode("descendant::Payment")?.InnerText;
        var costOfGoods = saleNode.SelectSingleNode("descendant::CostOfGoods")?.InnerText;
        var rating = saleNode.SelectSingleNode("descendant::Rating")?.InnerText;

        return SaleFactory.Instance.Create(invoiceId, branch, city, customerType, gender, productLine, unitPrice,
            quantity, tax, total, date, time, payment, costOfGoods, rating);
    }

    
}