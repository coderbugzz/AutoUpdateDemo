using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoUpdateDemo.Models
{
    public class Installer_Update
    {
        [Key]
        public int ID { get; set; }
        public int installer_ID { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string version { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string update_location { get; set; }
        public DateTime Date { get; set; }
    }
}
