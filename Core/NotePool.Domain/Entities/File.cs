using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NotePool.Domain.Entities.Common;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotePool.Domain.Entities
{
    public class File : BaseEntity
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Storage {  get; set; }

        [NotMapped]
        public override DateTime UpdatedDate { get => base.UpdatedDate; set => base.UpdatedDate = value;}
    }
}
