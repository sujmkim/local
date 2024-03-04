using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        string filePath = "path_to_your_file.txt";
        List<string> records = new List<string>(File.ReadAllLines(filePath)); 

        var sortedRecords = records.Select(record => new Parcel(record))
                                   .OrderBy(parcel => parcel.StreetName)
                                   .ThenBy(parcel => parcel.StreetNumber)
                                   .ToList();

        foreach (var parcel in sortedRecords)
        {
            Console.WriteLine(parcel);
        }
    }
}

public class Parcel
{
    public string Pin { get; set; }
    public string Address { get; set; }
    public string Owner { get; set; }
    public double MarketValue { get; set; }
    public DateTime SaleDate { get; set; }
    public double SalePrice { get; set; }
    public string Link { get; set; }
    public string StreetName { get; set; }
    public int StreetNumber { get; set; }

    public Parcel(string record)
    {
        var fields = record.Split('|');
        Pin = fields[0];
        Address = fields[1];
        Owner = fields[2];
        MarketValue = double.Parse(fields[3]);
        SaleDate = DateTime.Parse(fields[4]);
        SalePrice = double.Parse(fields[5]);
        Link = fields[6];

        var addressParts = fields[1].Split(' ');
        StreetNumber = int.Parse(addressParts[0]);
        StreetName = string.Join(" ", addressParts.Skip(1).Take(addressParts.Length - 2));
    }

    public override string ToString()
    {
        return $"{Pin}|{Address}|{Owner}|{MarketValue}|{SaleDate.ToShortDateString()}|{SalePrice}|{Link}";
    }
}