using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_gallery.Models;
using System.IO;


namespace web_gallery.Controllers
{
    public class ImageController : Controller
    {
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Image image)
        {
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
                string extension = Path.GetExtension(image.ImageFile.FileName);
                filename = filename + DateTime.Now.ToString("yymmssfff") + extension;


                image.ImagePath = "~/Image/" + filename;
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".gif" || extension.ToLower() == ".tiff" || extension.ToLower() == ".ico" || extension.ToLower() == ".bmp")
                  
                {
                    filename = Path.Combine(Server.MapPath("~/Image/"), filename);
                    image.ImageFile.SaveAs(filename);
                    using (DBmodel db = new DBmodel())
                    
                    {
                        db.Entry(image).State = System.Data.Entity.EntityState.Modified;
                        db.Images.Add(image);
                        db.SaveChanges();
                        TempData["msg"] = "Pictur updated";
                        return RedirectToAction("Add");
                    }
                    ModelState.Clear();

                }
                else
                {
                    ViewBag.msg = "Invalid File Type";
                }


                
            }
            else
            {
                
            }
            return View();
        }

        [HttpGet]
        public ActionResult Update(int? image)
        { 

            if(ModelState.IsValid)
            {
                if (image == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

                }
                using (DBmodel db = new DBmodel())

                {
                    var pictures = db.Images.Find(image);
                    if (pictures == null)
                    {
                        return HttpNotFound();
                    }
                    Session["imagePath"] = pictures.ImagePath;
                }

            }

            return View();

        }
        [HttpPost]

        public ActionResult Update(HttpPostedFileBase file, Image emp)
        {
            if(ModelState.IsValid)
            {
                if (file != null)
                {

                }
                else
                {
                    using (DBmodel db = new DBmodel())
                        emp.ImagePath = TempData["imagePath"].ToString();

                    using (DBmodel db = new DBmodel())
                    {
                        db.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                        if (db.SaveChanges() > 0)
                        {
                            TempData["msg"] = "Data Updated";
                            return RedirectToAction("index");
                        }

                    }
                    ModelState.Clear();





                }

            }
            return View();
        }

        public ActionResult Delete(int? image)
        {
            if (image == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            using (DBmodel db = new DBmodel())

            {
                var pictures = db.Images.Find(image);

                if (pictures == null)
                {
                    return HttpNotFound();
                }

                string currentImage = Request.MapPath(pictures.ImagePath);



                db.Entry(pictures).State = System.Data.Entity.EntityState.Deleted;
                if(db.SaveChanges() > 0)
                {
                    if(System.IO.File.Exists(currentImage))
                    {
                        System.IO.File.Delete(currentImage);
                    }
                    TempData["msg"] = "Data Deleted";
                    return RedirectToAction("Add");
                }
            }
           





            return View();

        }



        [HttpGet]
        public ActionResult View(int id)
        {
            Image image = new Image();
            using (DBmodel db = new DBmodel())
            {
                image = db.Images.Where(x => x.ImageId == id).FirstOrDefault();
            }
            return View(image);
        }

    }
}