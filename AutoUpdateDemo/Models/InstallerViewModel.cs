using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoUpdateDemo.Models
{
    public class InstallerViewModel
    {
        public int ID { get; set; }
        public string System { get; set; }
        public string cur_version { get; set; }
        public string location { get; set; }
        public IFormFile attachment { get; set; }
        public DateTime date { get; set; }
        public List<Installer_Info> installers { get; set; }
    }
}
