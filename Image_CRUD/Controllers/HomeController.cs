using Image_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Image_CRUD.Controllers
{
    public class HomeController : Controller
    {
        db_crudEntities db = new db_crudEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult AddImage()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult AddImage(Image img)
        {

            string FileName = Path.GetFileNameWithoutExtension(img.ImageFile.FileName);
            string Extension = Path.GetExtension(img.ImageFile.FileName);

            FileName = DateTime.Now.ToString("yyyyMMddhhmmss") + Extension;
            img.ImagePath = "~/Image/" + FileName;
            FileName = Path.Combine(Server.MapPath("~/Image/"), FileName);
            img.ImageFile.SaveAs(FileName);

            db.Images.Add(img);
            db.SaveChanges();

            TempData["Message"] = "Upload successful!";

            //ViewBag.Message = string.Format("Hello Success");
            //ViewData["Message"] = "Success";

            ModelState.Clear();

            return View();
        }

        
        public ActionResult ListView()
        {

            return View(db.Images.ToList());
        }

        public ActionResult Edit(int? id)
        {
            
            var title = db.Images.Find(id);

            TempData["imgPath"] = title.ImagePath;
            return View(title);
        }

        [HttpPost]
        public ActionResult Edit(Image img)
        {

            if (img != null)
            {
                string FileName = Path.GetFileNameWithoutExtension(img.ImageFile.FileName);
                string Extension = Path.GetExtension(img.ImageFile.FileName);

                FileName = DateTime.Now.ToString("yyyyMMddhhmmss") + Extension;
                img.ImagePath = "~/Image/" + FileName;
                FileName = Path.Combine(Server.MapPath("~/Image/"), FileName);
                //img.ImageFile.SaveAs(FileName);

                //db.Images.Add(img);
                db.Entry(img).State = EntityState.Modified;
                string oldImgPath = Request.MapPath(TempData["imgPath"].ToString());



                if (db.SaveChanges() > 0)
                {
                    img.ImageFile.SaveAs(FileName);
                    if (System.IO.File.Exists(oldImgPath))
                    {
                        System.IO.File.Delete(oldImgPath);
                    }
                    TempData["msg"] = "Data Updated";
                    return RedirectToAction("ListView");
                }
                //db.SaveChanges();
                

                //TempData["Message"] = "Upload successful!";
            }
            else
            {
                img.ImagePath = TempData["imgPath"].ToString();
                db.Entry(img).State = EntityState.Modified;

                if (db.SaveChanges()>0)
                {
                    TempData["msg"] = "Data Updated";
                    return RedirectToAction("ListView");
                }

            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            var dlt = db.Images.Find(id);
            string currentImg = Request.MapPath(dlt.ImagePath);
            db.Entry(dlt).State = EntityState.Deleted;

            if (db.SaveChanges() > 0)
            {
                if (System.IO.File.Exists(currentImg))
                {
                    System.IO.File.Delete(currentImg);
                }
                TempData["msg"] = "Data Deleted";
                return RedirectToAction("ListView");
            }

            return View();
        }

    }
}