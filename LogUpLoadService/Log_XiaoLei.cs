using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LogUpLoadService
{
    public class Log_XiaoLei
    {
        [Key]
        [Required]
        [Display(Name = "Id", Description = "")]
        [Column("Id", Order = 0, TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "LogTime", Description = "")]
        [Column("LogTime", Order = 0, TypeName = "DateTime")]
        public DateTime? LogTime { get; set; }

        [Required]
        [Display(Name = "Content", Description = "")]
        [Column("Content", Order = 20, TypeName = "nvarchar")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "CreateTime", Description = "")]
        [Column("CreateTime", Order = 0, TypeName = "DateTime")]
        public DateTime CreateTime { get; set; }
     
    }
}
