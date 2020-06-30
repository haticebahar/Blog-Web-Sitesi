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
    public class UyeController : Controller
    {
        // GET: Uye
        mvcblogDB db = new mvcblogDB();
        public ActionResult Index(int id)
        {
            var uye = db.Uyes.Where(u => u.uyeId == id).SingleOrDefault();
            if (Convert.ToInt32(Session["uyeId"]) != uye.uyeId) {

                return HttpNotFound();
            }
            return View(uye);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Uye uye)
        {
            var login = db.Uyes.Where(u => u.kullaniciAdi == uye.kullaniciAdi).SingleOrDefault();
            if (login.kullaniciAdi == uye.kullaniciAdi && login.Email == uye.Email && login.Sifre == uye.Sifre)
            {
                Session["uyeId"] = login.uyeId;
                Session["kullaniciAdi"] = login.kullaniciAdi;
                Session["yetkiId"] = login.yetkiId;
                return RedirectToAction("Index", "Home");

            }
            else
            {

                return View();
            }
        }
        public ActionResult Logout()  {

            Session["uyeId"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
            }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Uye uye , HttpPostedFileBase foto)
        {
            if (ModelState.IsValid) {
                if (foto != null)
                {

                    WebImage img = new WebImage(foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/UyeFoto/" + newfoto);
                    uye.foto = "~/Uploads/UyeFoto/" + newfoto;

                    uye.yetkiId = 2;
                    db.Uyes.Add(uye);
                    db.SaveChanges();
                    Session["uyeId"] = uye.uyeId;
                    Session["kullaniciAdi"] = uye.kullaniciAdi;
                    return RedirectToAction("Index", "Home");

                }
                else {
                    ModelState.AddModelError("Fotoğraf", "Fotoğraf seçiniz");
                }
            }
            return View(uye);
        }
        public ActionResult Edit(int id) {

            var uye = db.Uyes.Where(u => u.uyeId == id).SingleOrDefault();
            if (Convert.ToInt32(Session["uyeId"]) != uye.uyeId)
            {
                return HttpNotFound();
            }
            
            return View(uye);
        }
        [HttpPost]
        public ActionResult Edit(Uye uye,HttpPostedFileBase foto, int id)
        {
            if (ModelState.IsValid) {
                var uyes = db.Uyes.Where(u => u.uyeId == id).SingleOrDefault();
                if (foto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(uye.foto)))
                    {

                        System.IO.File.Delete(Server.MapPath(uyes.foto));


                    }
                    WebImage img = new WebImage(foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(foto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/UyeFoto/" + newfoto);
                    uyes.foto = "~/Uploads/UyeFoto/" + newfoto;
                }
                    
                    uyes.AdSoyad = uye.AdSoyad;
                    uyes.kullaniciAdi = uye.kullaniciAdi;
                    uyes.Sifre = uye.Sifre;
                    uyes.Email = uye.Email;
                    db.SaveChanges();
                    Session["kullaniciAdi"] = uye.kullaniciAdi;

                    return RedirectToAction("Index", "Home",new { id=uyes.uyeId});
                
            }
            return View();
        }
    }
}