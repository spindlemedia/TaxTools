using System.Data;
using System.Text.Json;
using ExcelDataReader;
using SB12Calculator.Core;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

var config = new ExcelDataSetConfiguration
{
    UseColumnDataType = false,
    ConfigureDataTable = _ => new ExcelDataTableConfiguration
    {
        UseHeaderRow = true
    }
};

var spreadsheetDetails = new List<SpreadsheetDetail>();
using (var stream = File.Open("tea_rates.xlsx", FileMode.Open, FileAccess.Read))
{
    using (var reader = ExcelReaderFactory.CreateReader(stream))
    {
        var result = reader.AsDataSet(config);


        foreach (DataTable table in result.Tables)
        {
            Console.WriteLine($"Processing {table.TableName}");
            spreadsheetDetails.AddRange(ProcessTable(table));
        }
    }
}

var details = new List<DistrictDetail>();
foreach (var districtGroup in spreadsheetDetails.GroupBy(d => d.DistrictId))
{
    var first = districtGroup.FirstOrDefault();
    var detail = new DistrictDetail
    {
        DistrictId = first.DistrictId,
        DistrictName = first.DistrictName,
        Rates = new List<DistrictRateDetail>()
    };
    foreach (var yearDetail in districtGroup)
    {
        detail.Rates.Add(new DistrictRateDetail
        {
            Year = yearDetail.Year,
            MaximumCompressedRate = yearDetail.MaximumCompressedRate,
            ActualMORate = yearDetail.ActualMORate,
        });
    }
    details.Add(detail);
}

File.WriteAllText("rates.json", JsonSerializer.Serialize(details, new JsonSerializerOptions { WriteIndented = true }));

static List<SpreadsheetDetail> ProcessTable(DataTable table)
{
    if (!table.TableName.StartsWith("TY"))
        throw new Exception($"Unexpected table name {table.TableName}");

    var details = new List<SpreadsheetDetail>();
    var year = int.Parse(table.TableName.Substring(2));
    foreach (DataRow row in table.Rows)
    {
        var compressedColumn = year == 2018 ? "COMPRESSED M&O TAX RATE" : "MAXIMUM COMPRESSED M&O TAX RATE";
        var compressedRate = (decimal)Math.Round((double)row[compressedColumn], 4);
        var moRate = (decimal)Math.Round((double)row["M&O TAX RATE"], 4);
        details.Add(new SpreadsheetDetail(year, (string)row["DISTRICT_ID"], (string)row["DISTNAME"], compressedRate, moRate));
    }
    return details;
}

public record SpreadsheetDetail(int Year, string DistrictId, string DistrictName, decimal MaximumCompressedRate, decimal ActualMORate);