using System;
using System.Collections.Generic;
using System.Linq;
using System.IO; 
using System.Globalization;

class Program {
    public static void Main()
    {
        string filePath = "Parcels.txt";
        List<string> records = new List<string>(File.ReadAllLines(filePath).Skip(1)); 

        //Sorting the records by street name and then by street number
        var sortedRecords = records.Select(record => new Parcel(record))
                                     .OrderBy(parcel => parcel.StreetName)
                                     .ThenBy(parcel => parcel.StreetNumber)
                                     .ToList();

        /**Sorting by first name, the second element of the original 'Owner' string, as it's 
            last name first. Also ensuring the code doesn't break if the name isn't in the
            expected format. 
            Uncomment the below code, and comment the above sortedRecords to run and see the sorted results        
        **/
        // var sortedRecords = records.Select(record => new Parcel(record))
        //  .OrderBy(parcel => {
        //      var nameParts = parcel.Owner.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        //      return nameParts.Length > 1 ? nameParts[1] : parcel.Owner;
        //  })
        //  .ThenBy(parcel => parcel.StreetName)
        //  .ThenBy(parcel => parcel.StreetNumber)
        //  .ToList();

        //Printing the results out onto the Console
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

        //Ensuring SaleDate field isn't empty before attempting to parse as DateTime
        if (!string.IsNullOrEmpty(fields[4]))
        {
            SaleDate = DateTime.Parse(fields[4], CultureInfo.InvariantCulture, DateTimeStyles.None);
        }
        else
        {
            //Setting to default value (DateTime.MinValue) if the field is empty
            SaleDate = DateTime.MinValue; 
        }

      SalePrice = double.Parse(fields[5]);
      Link = fields[6];

      var addressParts = fields[1].Split(' ');
      StreetNumber = int.Parse(addressParts[0]);
      StreetName = string.Join(" ", addressParts.Skip(1).Take(addressParts.Length - 2));

    }

    //This is the method that creates the url link to Google Maps
    public string GetGoogleMapsLink()
    {
        string baseUrl = "https://www.google.com/maps/search/?api=1&query=";
        string fullAddress = $"{Address}, Mazama, WA"; // Concatenate the hardcoded city and state
        string query = Uri.EscapeDataString(fullAddress); // Properly encode the full address
        return baseUrl + query;
    }


    public override string ToString()
    {
        return $"{Pin}|{Address}|{Owner}|{MarketValue}|{SaleDate.ToShortDateString()}|{SalePrice}|{Link}";
        /** This will add an extra column that links to Google Maps.
            Uncomment the below line and run to see the results, commenting the above line that doesn't
            include the extra Google Maps column.
        **/ 
        //return $"{Pin}|{Address}|{Owner}|{MarketValue}|{SaleDate.ToShortDateString()}|{SalePrice}|{Link}|{GetGoogleMapsLink()}";
    }


}