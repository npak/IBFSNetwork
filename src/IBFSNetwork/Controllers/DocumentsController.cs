using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Data;
using IBFSNetwork.Models.AlertViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace IBFSNetwork.Controllers
{
    //[Authorize(Roles = "admin")]

    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private IHostingEnvironment _environment;

        private List<string> FilesPath= new List<string>();


        public DocumentsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var ApplicationDbContext = _context.Documents;
            return View(await ApplicationDbContext.ToListAsync());
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.SingleOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create(int alertId)
        {
            ViewBag.AlertId = alertId;
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DocumentId,Content,Contentype,DocName,DocTypeId")] Document document, ICollection<IFormFile> files)
        {
            if (files.Count > 0)
            {
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        
                        using (var fileStream = new   FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                        {
                            
                            await file.CopyToAsync(fileStream);
                            //using (var binaryReader = new BinaryReader(fileStream))
                                //= binaryReader.ReadBytes((int)file.Length);
                            Stream stream = file.OpenReadStream();
                            BinaryReader reader = new BinaryReader(stream);
                            document.Content = reader.ReadBytes((int)file.Length);
                            document.Contentype = file.ContentType;
                        }

                    }
                }
            }
            else
            {
                //ModelState.AddModelError("Content", "File is requred");
            }
            if (ModelState.IsValid)
            {
                
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(document);
        }

        [HttpPost]
        public IActionResult UploadFilesAjax()
        {
            // read list of previous uploaded files
            //int alertid = Convert.ToInt32(Request.Form["AlertId"]);

            int cnt = Convert.ToInt32( Request.Form["filecnt"].ToString());
            int ind = -1;
            List<UploadDocumentTemplate> uploadDocs = new List<UploadDocumentTemplate>();
            UploadDocumentTemplate upDoc;
            for (int i=0; i<cnt;i++)
            {
                ind = i;
                upDoc = new UploadDocumentTemplate();
                upDoc.LiID = "li_" + i.ToString();
                upDoc.HrefID = "ahref_" + i.ToString();
                upDoc.HrefClass = "aLink";
                upDoc.DocumentPaths=( Request.Form["file" + i.ToString()]);
                upDoc.ContentId = "con_" + i.ToString();
                upDoc.ContentType = Request.Form["con" + i.ToString()];
                upDoc.DocumentHtmlId = "docid_" + i.ToString();
                upDoc.DocumentId = Request.Form["docid" + i.ToString()];
                upDoc.UserForder = HttpContext.User.Identity.Name;
                uploadDocs.Add(upDoc);
            }

            // read new uploaded file(s)  <input type="hidden" asp-for="Documents[1].DocName" value="aa1" />
            long size = 0;
            var files = Request.Form.Files;
            var uploads = Path.Combine(_environment.WebRootPath, "uploads\\" + HttpContext.User.Identity.Name);
            
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            foreach (var file in files)
            {
                //filename = hostingEnv.WebRootPath + $@"\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(Path.Combine(uploads, file.FileName)))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                //doc = new DocumentList();
                upDoc = new UploadDocumentTemplate();
                ind++;
                upDoc.LiID = "li_" + ind.ToString();
                upDoc.HrefID = "ahref_" + ind.ToString();
                upDoc.HrefClass = "aLink";
                upDoc.DocumentPaths = file.FileName.Trim().Replace("\r","").Replace("\n","");
                upDoc.ContentId = "con_" + ind.ToString();
                upDoc.ContentType = file.ContentType;
                upDoc.DocumentHtmlId = "docid_" + ind.ToString();
                upDoc.DocumentId = "0";
                upDoc.UserForder = HttpContext.User.Identity.Name;
                uploadDocs.Add(upDoc);

                //doc.Type = file.ContentType;
                
            }
            return PartialView("pIndex", uploadDocs);
        }

        [HttpPost]
        public IActionResult Save()
        {

            // read list of previous uploaded files
            int cnt = Convert.ToInt32(Request.Form["filecnt"].ToString());

            int ind = 0;
            List<UploadDocumentTemplate> uploadDocs = new List<UploadDocumentTemplate>();
            UploadDocumentTemplate upDoc;

            var uploads = Path.Combine(_environment.WebRootPath, "uploads\\" + HttpContext.User.Identity.Name);
            //  (new FileInfo(uploads)).Directory.Create();
            int fraudsterId = Convert.ToInt32(Request.Form["FraudsterId"]);

            try
            {
                for (int i = 0; i < cnt; i++)
                {
                    ind = i;
                    //read file anf save

                    // Read the file and convert it to Byte Array
                    string filePath = Path.Combine(uploads, Request.Form["file" + i.ToString()]);
                    string filename = Path.GetFileName(filePath);

                    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    br.Dispose();
                    fs.Dispose();
                    Document doc = new Document();
                    doc.DocName = filename;
                    doc.Content = bytes;
                    doc.FraudsterId = fraudsterId;
                    doc.Contentype = Request.Form["con" + i.ToString()];

                    _context.Documents.Add(doc);
                }

                _context.SaveChanges();

                Codes.Utils.DeleteDirectory(Path.Combine(_environment.WebRootPath, "uploads\\" + HttpContext.User.Identity.Name));
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

            }


            for (int i = 0; i < cnt; i++)
            {
                ind = i;
                upDoc = new UploadDocumentTemplate();
                upDoc.LiID = "li_" + i.ToString();
                upDoc.HrefID = "ahref_" + i.ToString();
                upDoc.HrefClass = "aLink";
                upDoc.DocumentPaths = Request.Form["file" + i.ToString()];
                upDoc.ContentId = "con_" + i.ToString(); ;
                upDoc.ContentType = Request.Form["con" + i.ToString()];
                uploadDocs.Add(upDoc);
            }


                return PartialView("pIndex", uploadDocs);
        }

        public FileResult DownloadFile(int id)
        {
            var doc = _context.Documents.Where(d => d.DocumentId == id).SingleOrDefault();

            HttpContext.Response.ContentType = doc.Contentype;
            FileContentResult result = new FileContentResult((byte[])doc.Content, doc.Contentype.Trim())
            {
                FileDownloadName = doc.DocName.Trim()
            };

            return result;
        }

        public ActionResult DownloadfromServer(string fileName)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads\\" + HttpContext.User.Identity.Name);
            string filePath = Path.Combine(uploads, fileName);

            byte[] fileBytes = GetFile(filePath);
            
            return File(fileBytes,Codes.Utils.GetMimeType(Path.GetExtension(filePath)), fileName);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        public ActionResult GetImage(int id)
        {
            var doc = _context.Documents.Where(d => d.DocumentId == id).SingleOrDefault();

            byte[] imageData = doc.Content;

            //instead of what augi wrote using the binarystreamresult this was the closest thing i found so i am assuming that this is what it evolved into 
            return new FileStreamResult(new System.IO.MemoryStream(imageData), "image/jpeg");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.SingleOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Documents.SingleOrDefaultAsync(m => m.DocumentId == id);
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.DocumentId == id);
        }
    }
}
