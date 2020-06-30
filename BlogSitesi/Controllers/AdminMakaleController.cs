using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSitesi.Models;
using System.Web.Helpers;
using System.IO;

namespace BlogSitesi.Controllers
{
    public class AdminMakaleController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: AdminMakale
        public ActionResult Index()
        {
            var makales = db.Makales.ToList();
            return View(makales);
        }

        // GET: AdminMakale/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminMakale/Create
        public ActionResult Create()
        {
            ViewBag.kategoriID = new SelectList(db.Kategoris, "kategoriId", "kategoriAdi");
            return View();
        }

        // POST: AdminMakale/Create
        [HttpPost]
        public ActionResult Create(Makale makale, string etiketler, HttpPostedFileBase foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    WebImage img = new WebImage(foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(800, 300);
                    img.Save("~/Uploads/MakaleFoto/" + newfoto);
                    makale.foto = "~/Uploads/MakaleFoto/" + newfoto;


                }
                if (etiketler != null)
                {

                    String[] etiketdizi = etiketler.Split(',');
                    foreach (var i in etiketdizi)
                    {

                        var yenietiket = new Etiket { etiketAdi = i };
                        db.Etikets.Add(yenietiket);
                        makale.Etikets.Add(yenietiket);
                    }

                }
                makale.uyeId = Convert.ToInt32(Session["uyeId"]);
                db.Makales.Add(makale);
                db.SaveChanges();
                return RedirectToAction("Index");


                // TODO: Add insert logic here


            }
            return View(makale);
        }

        // GET: AdminMakale/Edit/5
        public ActionResult Edit(int id)
        {
            var makale = db.Makales.Where(n => n.makaleId == id).SingleOrDefault();
            if (makale == null)
            {

                return HttpNotFound();
            }
            ViewBag.kategoriId = new SelectList(db.Kategoris, "kategoriId", "kategoriAdi", makale.kategoriId);
            return View(makale);
        }

        // POST: AdminMakale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase foto, Makale makale)
        {
            try
            {
                var makales = db.Makales.Where(n => n.makaleId == id).SingleOrDefault();
                if (foto != null)
                {

                    if (System.IO.File.Exists(Server.MapPath(makales.foto)))
                    {

                        System.IO.File.Delete(Server.MapPath(makales.foto));


                    }
                    WebImage img = new WebImage(foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(foto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(800, 300);
                    img.Save("~/Uploads/MakaleFoto/" + newfoto);

                    makale.foto = "~/Uploads/MakaleFoto/" + newfoto;
                    makales.baslik = makale.baslik;
                    makales.icerik = makale.icerik;
                    makales.kategoriId = makale.kategoriId;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
               
            }
            catch
            {
                ViewBag.KategoriId = new SelectList(db.Kategoris, "KategoriId", "KategoriAdi", makale.kategoriId);
                return View(makale);
            }
        }

        // GET: AdminMakale/Delete/5
        public ActionResult Delete(int id)
        {
            var makale = db.Makales.Where(n => n.makaleId == id).SingleOrDefault();
            if (makale == null)
            {

                return HttpNotFound();
            }
           
            return View(makale);

          
        }

        // POST: AdminMakale/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var makales = db.Makales.Where(n => n.makaleId == id).SingleOrDefault();
                if (makales == null)
                {

                    return HttpNotFound();
                }
                //Makalenin yorumlar resimler ve etiketleri vs vr . Bunlarında silinmesi lazım

                if (System.IO.File.Exists(Server.MapPath(makales.foto)))
                {

                    System.IO.File.Delete(Server.MapPath(makales.foto));


                }
                foreach (var i in makales.Yorums.ToList()) {

                    db.Yorums.Remove(i);
                }
                foreach (var i in makales.Etikets.ToList()) {

                    db.Etikets.Remove(i);
                }
                db.Makales.Remove(makales);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
