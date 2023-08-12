using System.Data;
using System.Text.Json;
using ExcelDataReader;
using TaxTools.Core.TaxLimitation;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

var config = new ExcelDataSetConfiguration
{
    UseColumnDataType = false,
    ConfigureDataTable = _ => new ExcelDataTableConfiguration
    {
        UseHeaderRow = true
    }
};

var taxRates = new Dictionary<(string DistrictId, int Year), decimal>();

using (var stream = File.Open("school-district-adopted-tax-rates.xlsx", FileMode.Open, FileAccess.Read))
{
    using (var reader = ExcelReaderFactory.CreateReader(stream))
    {
        var result = reader.AsDataSet(config);
        var table = result.Tables[0];
        foreach (DataRow row in table.Rows)
        {
            var districtId = (string)row[0];
            for (var i = 2; i < table.Columns.Count; i += 2)
            {
                if (string.IsNullOrEmpty(row[i].ToString()))
                    continue;

                var year = int.Parse(table.Columns[i].ColumnName.Trim().Substring(0, 4));
                var moRate = (decimal)Math.Round((double)row[i], 4);

                var isRateRaw = row[i + 1].ToString();
                decimal isRate = 0;
                if (!string.IsNullOrEmpty(isRateRaw))
                    isRate = (decimal)Math.Round((double)row[i + 1], 4);

                taxRates.Add((districtId, year), moRate + isRate);
            }
        }
    }
}

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
            TotalTaxRate = yearDetail.TotalTaxRate
        });
    }
    details.Add(detail);
}

File.WriteAllText("rates.json", JsonSerializer.Serialize(details, new JsonSerializerOptions { WriteIndented = true }));

List<SpreadsheetDetail> ProcessTable(DataTable table)
{
    if (!table.TableName.StartsWith("TY"))
        throw new Exception($"Unexpected table name {table.TableName}");

    var details = new List<SpreadsheetDetail>();
    var year = int.Parse(table.TableName.Substring(2));
    foreach (DataRow row in table.Rows)
    {
        var districtId = (string)row["DISTRICT_ID"];
        var compressedColumn = year == 2018 ? "COMPRESSED M&O TAX RATE" : "MAXIMUM COMPRESSED M&O TAX RATE";
        var compressedRate = (decimal)Math.Round((double)row[compressedColumn], 4);
        var rawMORate = row["M&O TAX RATE"].ToString();
        decimal? moRate = null;
        if (!string.IsNullOrEmpty(rawMORate))
            moRate = (decimal)Math.Round((double)row["M&O TAX RATE"], 4);
        decimal? totalRate = null;
        if (taxRates.TryGetValue((districtId, year), out var rate))
            totalRate = rate;
        details.Add(new SpreadsheetDetail(year, districtId, (string)row["DISTNAME"], compressedRate, moRate, totalRate));
    }
    return details;
}


public record SpreadsheetDetail(int Year, string DistrictId, string DistrictName, decimal MaximumCompressedRate, decimal? ActualMORate, decimal? TotalTaxRate);
