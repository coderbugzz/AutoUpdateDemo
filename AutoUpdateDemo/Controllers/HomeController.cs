using AutoUpdateDemo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutoUpdateDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly AppDBContext _appContext;

        public HomeController(ILogger<HomeController> logger,
                              IWebHostEnvironment hostingEnvironment,
                               AppDBContext appContext)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _appContext = appContext;
        }

        public IActionResult Index()
        {
            InstallerViewModel model = new InstallerViewModel();
            model.installers = _appContext.installers.Select(m => m).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(InstallerViewModel model)
        {
            if (model.attachment != null)
            {
                //write file to a physical path
                var uniqueFileName = model.attachment.FileName;
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "installer");
                var filePath = Path.Combine(uploads, uniqueFileName);
                model.attachment.CopyTo(new FileStream(filePath, FileMode.Create));

                //save the attachment to the database
                Models.Installer_Info attachment = new Models.Installer_Info();
                attachment.System = uniqueFileName;
                attachment.cur_version = model.cur_version;
                attachment.location = filePath;

                _appContext.installers.Add(attachment);
                _appContext.SaveChanges();
            }
            return RedirectToAction("index");
        }

        [HttpGet]
        public FileStreamResult GetFileStreamResultDemo(string filename) //download file
        {
            string path = "wwwroot/installer/" + filename;
            var stream = new MemoryStream(System.IO.File.ReadAllBytes(path));
            string contentType = GetContenttype(filename);
            return new FileStreamResult(stream, new MediaTypeHeaderValue(contentType))
            {
                FileDownloadName = filename
            };
        }

        public IActionResult Updates(int systemID)
        {
            //ViewBag.installerID = systemID;

            UpdateViewModel model = new UpdateViewModel();
            model.updates = _appContext.updates.Select(m => m).Where(m => m.installer_ID == systemID).ToList();
            model.installer_ID = systemID;

            return View(model);
        }
        [HttpPost]
        public IActionResult Updates(UpdateViewModel model)
        {
            if (model.attachment != null)
            {
                //write file to a physical path
                var uniqueFileName = model.attachment.FileName;
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "updates");
                string subpath = uploads + "\\" + uniqueFileName.Split('.')[0] + "_" + model.version;
                System.IO.Directory.CreateDirectory(subpath);

                var filePath = Path.Combine(subpath, uniqueFileName);
                model.attachment.CopyTo(new FileStream(filePath, FileMode.Create));

                //save the attachment to the database
                Models.Installer_Update attachment = new Models.Installer_Update();
                attachment.installer_ID = model.installer_ID;
                attachment.version = model.version;
                attachment.update_location = "wwwroot\\updates" + "\\" + uniqueFileName.Split('.')[0] + "_" + model.version + "\\" + uniqueFileName;

                _appContext.updates.Add(attachment);


                //retrieve system record
                Models.Installer_Info installer = new Models.Installer_Info();
                installer = _appContext.installers.Select(m => m).Where(m => m.ID == model.installer_ID).FirstOrDefault();
                installer.cur_version = model.version;

                _appContext.installers.Update(installer);

                _appContext.SaveChanges();


            }
            return RedirectToAction("index");
        }
        [HttpGet]
        public Response<Models.Installer_Info> checkupdate(string version, int systemID)
        {
            Response<Models.Installer_Info> response = new Response<Models.Installer_Info>();
            Models.Installer_Info installer = new Models.Installer_Info();
            installer = _appContext.installers.Select(m => m).Where(m => m.ID == systemID).FirstOrDefault();
            if (installer != null)
            {
                if (installer.cur_version == version)
                {
                    response.Code = 300;
                    response.message = "No updates found";
                }
                else
                {
                    response.Code = 200;
                    response.message = "New update available";
                    response.Data = installer;
                }
            }
            return response;
        }

        [HttpGet]
        public FileStreamResult GetUpdateFile(int systemID) //download file
        {
            Models.Installer_Info installer = new Models.Installer_Info();
            Installer_Update update = new Installer_Update();
            installer = _appContext.installers.Select(m => m).Where(m => m.ID == systemID).FirstOrDefault();
            if (installer != null)
            {
                update = _appContext.updates.Select(m => m).Where(m => m.version == installer.cur_version && m.installer_ID == installer.ID).FirstOrDefault();

            }
            string path = update.update_location;//"wwwroot/updates/" + filename;
            var stream = new MemoryStream(System.IO.File.ReadAllBytes(path));
            int path_count = update.update_location.Split('\\').Count();
            var filename = update.update_location.Split('\\')[path_count - 1];
            string contentType = GetContenttype(filename);

            FileStreamResult result = new FileStreamResult(stream, new MediaTypeHeaderValue(contentType))
            {
                FileDownloadName = filename
            };
            return result;
        }

        public static string GetContenttype(string fileName)
        {
            var provider =
                new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
