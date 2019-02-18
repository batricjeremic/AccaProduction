using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AccaProduction.Models;
using AccaProduction.Repository;
using AccaProduction.Utils;
using AccaProduction.WriteModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace AccaProduction.Controllers
{
    public class ExcelExportController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IPolaganjaRepository _polaganja;
        private readonly IKandidatRepository _kandidati;

        public ExcelExportController(IHostingEnvironment hostingEnvironment, IPolaganjaRepository polaganja, IKandidatRepository kandidat)
        {
            _hostingEnvironment = hostingEnvironment;
            _polaganja = polaganja;
            _kandidati = kandidat;
        }

        public async Task<IActionResult> ExportNonProcessed()
        {
            var polaganja = User.IsInRole(SD.AdminEndUser)
                ? await _polaganja.GetNonProcessedRequests()
                : await _polaganja.GetNonProcessedRequests(await _kandidati.GetIdByEmail(User.Identity.Name));

            return await WritePolaganja(polaganja);
        }

        public async Task<IActionResult> ExportProcessed()
        {
            var polaganjas = User.IsInRole(SD.AdminEndUser)
                ? await _polaganja.GetProcessedRequests()
                : await _polaganja.GetProcessedRequests(await _kandidati.GetIdByEmail(User.Identity.Name));

            return await WritePolaganja(polaganjas);
        }

        [Authorize(Roles = SD.AdminEndUser)]
        public async Task<IActionResult> ExportCandidates(string option = null, string search = null)
        {
            var candidates = await _kandidati.GetFilteredKandidats(option, search);

            string[,] data = new string[candidates.Count() + 1, (new KandidatWriteModel()).GetType().GetProperties().Length];

            WriteHeader(new KandidatWriteModel(), data);

            #region writebody
            int r = 1;
            foreach (var item in candidates)
            {
                //prepare model data
                var writeModel = new KandidatWriteModel()
                {
                    Id = item.IdAccaNumber,
                    Department = item.Odeljenje,
                    FullName = $"{item.Ime} {item.Prezime}",
                    Email = item.Email,
                    PassedExamCount = (await _polaganja.GetCompletedExams(item.IdAccaNumber)).Count()
                };

                WriteRow(writeModel, data, r);

                r++;
            }
            #endregion

            return await Export(data, $"Students {DateTime.Today.Day}-{DateTime.Today.Month}.xlsx");
        }

        private async Task<IActionResult> Export(string[,] inputData, string filename)
        {
            string rootFolder = _hostingEnvironment.WebRootPath;
            string fileName = filename;
            string url = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, fileName);
            FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));

            var memoryStream = new MemoryStream();
            
            using (var fs = new FileStream(Path.Combine(rootFolder, fileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("export");

                for (int r = 0; r < inputData.GetLength(0); r++)
                {
                    IRow row = excelSheet.CreateRow(r);
                    for (int c = 0; c < inputData.GetLength(1); c++)
                    {
                        row.CreateCell(c).SetCellValue(inputData[r, c]);
                    }
                }

                workbook.Write(fs);
            }

            using (var stream = new FileStream(Path.Combine(rootFolder, fileName), FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }
            file.Delete();
            memoryStream.Position = 0;
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);


        }

        private void WriteRow(object input, string[,] data, int rowIndex)
        {
            Type itemType = input.GetType();
            PropertyInfo[] itemProperties = itemType.GetProperties();

            for (int i = 0; i < itemProperties.Length; i++)
            {
                data[rowIndex, i] = itemProperties[i].GetValue(input).ToString();
            }
        }

        private void WriteHeader(object input, string[,] data)
        {

            Type modelType = (input).GetType();
            PropertyInfo[] properties = modelType.GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                data[0, i] = properties[i].Name;
            }
        }

        private async Task<IActionResult> WritePolaganja(IEnumerable<Polaganja> polaganja)
        {

            string[,] data = new string[polaganja.Count() + 1, (new PolaganjaWriteModel()).GetType().GetProperties().Length];

            WriteHeader(new PolaganjaWriteModel(), data);

            #region writebody
            int r = 1;
            foreach (var item in polaganja)
            {
                //prepare model data
                var writeModel = new PolaganjaWriteModel()
                {
                    Id = item.Id,
                    Rok = item.Rok.NazivRoka,
                    IspitId = item.IspitId,
                    IspitName = item.Ispit.Name,
                    NewCode = item.Ispit.NewCode,
                    OldCode = item.Ispit.OldCode,
                    KandidatId = item.KandidatId,
                    KandidatFullName = $"{item.Kandidat.Ime} {item.Kandidat.Prezime}",
                    Status = item.Status.StatusName
                    
                };

                WriteRow(writeModel, data, r);

                r++;
            }
            #endregion

            return await Export(data, $"Requests {DateTime.Today.Day}-{DateTime.Today.Month}.xlsx");
        }
    }
}