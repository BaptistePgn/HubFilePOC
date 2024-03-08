using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HubFile.Data;
using HubFile.Models;
using HubFile.FileUploadService;

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
        public async Task<IActionResult> Create(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            var filePath = await _fileUploadService.UploadFileAsync(file);

            var fileModel = new FileModel
            {
                Name = file.FileName,
                Path = filePath,
                Size = file.Length.ToString(),
                Extension = file.ContentType
            };

            return View(fileModel);
        }

        // POST: File/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save([Bind("Id,Name,Path,Size,Extension")] FileModel fileModel)
        {
            if (ModelState.IsValid)
            {
                fileModel.Id = Guid.NewGuid();
                _context.Add(fileModel);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Create));
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
