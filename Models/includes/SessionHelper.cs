using System.Globalization;
using System.Text;
using btp.Models.Data;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace btp.Models.includes;
public static class SessionHelper
{
    public static string GetNom(HttpContext httpContext)
    {
        return httpContext.Session.GetString("nom");
    }
    public static void ResetDataBase(BtpContext context){
        string sql="DO $$ DECLARE table_record RECORD; ";
        sql+=" BEGIN FOR table_record IN ";
        sql+=" SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE' LOOP";
        sql+=" EXECUTE format('TRUNCATE TABLE %I RESTART IDENTITY CASCADE', table_record.table_name); ";
        sql+=" END LOOP;END $$; ";
        context.Database.ExecuteSqlRaw(sql);
        context.SaveChanges();
        string request = " insert into btp (email,mdp) values('a@gmail.com','androany') ";
        context.Database.ExecuteSqlRaw(request);
        context.SaveChanges();
    }
    public static string SavePhoto(IFormFile photo, string fileName)
    {
        if (photo == null || photo.Length == 0)
            throw new ArgumentException("Fichier non spécifié.");
        fileName=fileName.Replace(" ","");
        var fileExtension = Path.GetExtension(Path.GetFileName(photo.FileName));
        var uploadsFolder = "wwwroot/Upload/";
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);
        fileName = fileName+ fileExtension;
        var filePath = Path.Combine(uploadsFolder, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            photo.CopyTo(stream);
        }
        return fileName;
    }
     public static int GetTotalPages<T>(DbContext dbContext, int pageSize) where T : class
    {
        int totalElements = dbContext.Set<T>().Count();
        int totalPages = (int)Math.Ceiling((double)totalElements / pageSize);
        return totalPages;
    }
    public static List<string[]> GetDataFromCsvFile(string filePath, string delimiter)
        {
            try
            {
                List<string[]> allData = new List<string[]>();

                // Configuration de CsvHelper pour lire les fichiers CSV avec les virgules comme séparateurs
                var csvConfig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentCulture)
                {
                    Delimiter = delimiter,
                    HasHeaderRecord = true, // Si la première ligne contient les en-têtes, mettez cela à true
                    Encoding = Encoding.UTF8
                };

                using (var reader = new StreamReader(filePath,Encoding.UTF8))
                using (var csv = new CsvReader(reader, csvConfig))
                {
                    csv.Read();
                    csv.ReadHeader();
                    // Lecture des données ligne par ligne
                    while (csv.Read())
                    {
                        // Récupération des champs de la ligne actuelle
                        var record = csv.Parser.Record;
                        allData.Add(record);
                    }
                }
                return allData;
            }
            catch (Exception ex)
            {
                // Gérer les exceptions ici
                throw new Exception(ex.Message);
            }
        }
    // sady manao enregistrements,generique fa ovaina arakaraka
    public static byte[] ExportDataToExcel<T>(List<T> data, string filePath)where T:class
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Data");

            var properties = typeof(T).GetProperties();

            // Ajouter les en-têtes
            int column = 1;
            foreach (var property in properties)
            {
                worksheet.Cells[1, column].Value = property.Name;
                column++;
            }

            // Ajouter les données
            int row = 2;
            foreach (var item in data)
            {
                column = 1;
                foreach (var property in properties)
                {
                    Console.WriteLine("ATOOOO = "+property.GetValue(item));
                    worksheet.Cells[row, column].Value = property.GetValue(item);
                    column++;
                }
                row++;
            }

            // Sauvegarder le fichier Excel
            File.WriteAllBytes("Upload/"+filePath, package.GetAsByteArray());
            return package.GetAsByteArray();
        }
    }
    public static string GetType(){
        return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        
    }
    //  public static byte[] ExportDataToExcel(List<Produit> data, string filePath)
    // {
    //     using (var package = new ExcelPackage())
    //     {
    //         var worksheet = package.Workbook.Worksheets.Add("Data");



    //         // Ajouter les en-têtes
    //         int column = 1;
           
    //             worksheet.Cells[1, 1].Value = "id";
    //             worksheet.Cells[1, 2].Value = "nom";
    //             ;

    //         // Ajouter les données
    //         int row = 2;
    //         foreach (var item in data)
    //         {
    //          worksheet.Cells[row, 1].Value = item.Idproduit;
    //          Console.WriteLine("LE NOM   ="+item.Nom);
    //             worksheet.Cells[row, 2].Value = item.Nom;
    //             row++;   
    //         }

    //         // Sauvegarder le fichier Excel
    //         File.WriteAllBytes("Upload/"+filePath, package.GetAsByteArray());
    //         return package.GetAsByteArray();
    //     }
    // }

    public static byte[] ExportDataToExcel<T>(this List<T> data) where T : class
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Data");

            // Chargez les données à partir de la liste directement dans la feuille de calcul
            worksheet.Cells.LoadFromCollection(data, true);

            return package.GetAsByteArray();
        }
    }
    public static string GetFormatted(double d){
        return d.ToString("#,##0.00",CultureInfo.CurrentCulture);
    }
}
