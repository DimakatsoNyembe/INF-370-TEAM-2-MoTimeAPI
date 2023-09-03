using MoTimeAPI.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

public static class ExcelHelper
{
    public static List<Client> ParseClientsFromStream(Stream stream)
    {
        var clients = new List<Client>();
        using (var package = new ExcelPackage(stream))
        {
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++) // Assuming row 1 contains headers
            {
                string account = (worksheet.Cells[row, 1].Value?.ToString() ?? "");
                string accountManager = worksheet.Cells[row, 2].Value?.ToString();
                string department = worksheet.Cells[row, 3].Value?.ToString();
                string siteCode = worksheet.Cells[row, 4].Value?.ToString();

                if (account.Length > 50)
                {
                    // If it exceeds the limit, truncate it to the maximum length
                    account = account.Substring(0, 50);
                }

                int projectCode;
                string projectCodeStr = worksheet.Cells[row, 5].Value?.ToString();
                if (int.TryParse(projectCodeStr, out projectCode))
                {
                    // Successfully parsed the projectCode as an integer
                    clients.Add(new Client
                    {
                        Account = account,
                        AccountManager = accountManager,
                        Department = department,
                        SiteCode = siteCode,
                        ProjectCode = projectCode
                    });
                }
                else
                {
                    // Handle the case where the value cannot be parsed as an integer
                    // You can set a default value or raise an error, depending on your needs
                    // For example, setting it to -1 as a default:
                    clients.Add(new Client
                    {
                        Account = account,
                        AccountManager = accountManager,
                        Department = department,
                        SiteCode = siteCode,
                        ProjectCode = -1
                    });
                }
            }

        }
        return clients;
    }

    public static byte[] GenerateExcelBytes(List<Client> clients)
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Clients");
            worksheet.Cells.LoadFromCollection(clients, true);

            var excelBytes = package.GetAsByteArray();
            return excelBytes;
        }
    }
}
