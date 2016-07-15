﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Localization.SqlLocalizer.DbStringLocalizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace ImportExportLocalization.Controllers
{
    [Route("api/ImportExport")]
    public class ImportExportController : Controller
    {
        private IStringExtendedLocalizerFactory _stringExtendedLocalizerFactory;

        public ImportExportController(IStringExtendedLocalizerFactory stringExtendedLocalizerFactory)
        {
            _stringExtendedLocalizerFactory = stringExtendedLocalizerFactory;
        }

        // http://localhost:6062/api/ImportExport/localizedData.csv
        [HttpGet]
        [Route("localizedData.csv")]
        [Produces("text/csv")]
        public IActionResult GetDataAsCsv()
        {
            return Ok(_stringExtendedLocalizerFactory.GetLocalizationData());
        }


        [Route("files")]
        [HttpPost]
        [ServiceFilter(typeof(ValidateMimeMultipartContentFilter))]
        public IActionResult ImportCsvFile(CsvImportDescription csvImportDescription)
        {
            // TODO validate that data is a csv file.
            var names = new List<string>();
            var contentTypes = new List<string>();

            if (ModelState.IsValid)
            {
                foreach (var file in csvImportDescription.File)
                {
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        contentTypes.Add(file.ContentType);

                        names.Add(fileName);

                        var inputStream = file.OpenReadStream();
                        var serializer = new JsonSerializer();

                        using (var sr = new StreamReader(inputStream))
                        using (var jsonTextReader = new JsonTextReader(sr))
                        {
                            var data =  serializer.Deserialize(jsonTextReader) as IList<LocalizationRecord>;
                            // TODO save the data to the database
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
