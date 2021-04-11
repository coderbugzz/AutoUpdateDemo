using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoUpdateDemo.Models
{
    public class UpdateViewModel
    {
        public int ID { get; set; }
        public int installer_ID { get; set; }
        public string version { get; set; }
        public string update_location { get; set; }
        public IFormFile attachment { get; set; }
        public DateTime Date { get; set; }

        public List<Installer_Update> updates { get; set; }
    }
}
