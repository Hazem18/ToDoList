using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class MyListController : Controller
    {
        public ApplicationDbContext context { get; set; } = new ApplicationDbContext();
        public IActionResult Index()
        {
            
            if (Request.Cookies.ContainsKey("Added"))
            {
                ViewBag.UserName = Request.Cookies["Added"];
            }
            else
            {
                ViewBag.UserName = "Guest";
            }
            var list = context.Lists.ToList();
            return View(list);
        }


        public IActionResult AddNew()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNew(MyList list , IFormFile PdfUrl)
        {
            var fileName = UploadImg(PdfUrl);
            if (fileName != null)
            {
                list.PdfUrl = fileName;
            }
            else
            {
                list.PdfUrl = " ";
            }
            context.Lists.Add(list);
            context.SaveChanges();
            TempData["Added"] = "List Added successfully";
            return RedirectToAction("Index");
        }

        public IActionResult SaveUserName()
        {

            return View();
        }
        [HttpPost]
        public IActionResult SaveUserName(string UserName)
        {
            CookieOptions options = new CookieOptions();
            options.Secure = true;
            options.Expires = DateTimeOffset.Now.AddDays(1);
            Response.Cookies.Append("Added", UserName, options);
            return RedirectToAction("Index");
        }

       
        public IActionResult Edit(int Id)
        { 
            var list = context.Lists.Find(Id);
            return View(list); 
        }

        [HttpPost]
        public IActionResult Edit(MyList list)
        {
            var existingList = context.Lists.Find(list.Id);

            if (existingList != null)
            { 
                list.PdfUrl = existingList.PdfUrl;
                context.Entry(existingList).CurrentValues.SetValues(list);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int Id)
        {
            context.Lists.Remove(new MyList() { Id = Id});
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        private string? UploadImg(IFormFile PdfUrl)
        {
            // save in wwwroot
            if (PdfUrl.Length > 0)
            {
                var fileName = PdfUrl.FileName; 
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    PdfUrl.CopyTo(stream);
                }
                return PdfUrl.FileName;
            }
            return null;
        }
    }
}
