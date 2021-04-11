using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoUpdateDemo.Models
{
    public class Installer_Info
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string System { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string cur_version { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string location { get; set; }
        public DateTime date { get; set; }
    }
}
