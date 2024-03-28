using HubFile.Data;
using HubFile.FileUploadService;
using HubFile.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HubFile.Controllers
{
    public class FileController : Controller
    {
        private readonly HubFileContext _context;
        private readonly IFileUploadService _fileUploadService;

        public FileController(HubFileContext context, IFileUploadService fileUploadService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
        }

        // GET: File
        public async Task<IActionResult> Index()
        {
            return View(await _context.FileModel.ToListAsync());
        }

        // GET: File/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileModel = await _context.FileModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileModel == null)
            {
                return NotFound();
            }

            return View(fileModel);
        }

        // GET: File/Upload
        public IActionResult Upload()
        {
            return View();
        }

        // GET: File/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a file to upload.");
                return View();
            }

            var filePath = await _fileUploadService.UploadFileAsync(file);

            var fileModel = new FileModel
            {
                Id = Guid.NewGuid(),
                Name = file.FileName,
                Path = filePath,
                Size = file.Length.ToString(),
                Extension = Path.GetExtension(file.FileName)
            };

            _context.Add(fileModel);
            await _context.SaveChangesAsync();

            // Return json object to indicate success and redirect to index page
            return Json(new { redirectUrl = Url.Action("Index") });
        }
        // GET: File/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileModel = await _context.FileModel.FindAsync(id);
            if (fileModel == null)
            {
                return NotFound();
            }
            return View(fileModel);
        }

        // POST: File/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Path,Size,Extension")] FileModel fileModel)
        {
            if (id != fileModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fileModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileModelExists(fileModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fileModel);
        }

        // GET: File/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileModel = await _context.FileModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileModel == null)
            {
                return NotFound();
            }

            return View(fileModel);
        }

        // POST: File/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fileModel = await _context.FileModel.FindAsync(id);
            if (fileModel != null)
            {
                _fileUploadService.DeleteFile(fileModel.Path);
                _context.FileModel.Remove(fileModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileModelExists(Guid id)
        {
            return _context.FileModel.Any(e => e.Id == id);
        }
    }
}
