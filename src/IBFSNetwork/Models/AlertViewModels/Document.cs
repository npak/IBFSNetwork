using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IBFSNetwork.Models.AlertViewModels
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        //[Required]
        //public int AlertId { get; set; }

        [Required]
        public int FraudsterId { get; set; }

        [Required]
        [Display(Name = "Doc Name")]
        [MaxLength(255)]
        public string DocName { get; set; }

        [MaxLength(100)]
        public string Contentype { get; set; }

        public byte[] Content { get; set; }

        //public Alert Alert { get; set; }
        public Fraudster Fraudster { get; set; }
    }

    //public class DocumentType
    //{
    //    [Key]
    //    public int DocTypeId { get; set; }
    //    [MaxLength(128)]
    //    public string DocTypeName { get; set; }
    //}

    public class AlerDocument
    { 
        [Key]
        public int AlertDocId { get; set; }
        public Document Document { get; set; }
        public Alert Alert { get; set; }
    }

    public class UploadDocumentTemplate
    {
        public string  DocumentHtmlId { get; set; }
        public string DocumentId { get; set; }
        public string DocumentPaths { get; set; }
        public string LiID { get; set; }
        public string HrefID { get; set; }
        public string HrefClass { get; set; }
        public string ContentId { get; set; }

        public string ContentType { get; set; }
        public string UserForder { get; set; }
    }

    public class UploadDocument
    {
        [Required]
        [Display(Name = "Doc Name")]
        [MaxLength(255)]
        public string DocName { get; set; }

        public List<string> DocumentPaths { get; set; }

    }

}
