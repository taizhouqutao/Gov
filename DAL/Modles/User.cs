using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
    public class User : Basic
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("UserName")]
        public required string UserName { get; set; }

        [Column("PassWord")]
        public required string PassWord { get; set; }

        [Column("RealName")]
        public required string RealName { get; set; }

        [Column("UserEmail")]
        public required string UserEmail{ get; set; }

        [Column("UserPost")]
        public string? UserPost{ get; set; }

        [Column("UserHead")]
        public string? UserHead{ get; set; }
    }
}
