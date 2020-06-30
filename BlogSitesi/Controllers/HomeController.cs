using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSitesi.Models;
using PagedList;
using PagedList.Mvc;

namespace BlogSitesi.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        mvcblogDB db = new mvcblogDB();
        public ActionResult Index(int Page=1)
        {
            var makale = db.Makales.OrderByDescending(m=>m.makaleId).ToPagedList(Page,5);


            return View(makale);
        }

        public ActionResult KategoriMakale(int id) {

            var makaleler = db.Makales.Where(m => m.Kategori.kategoriId == id).ToList();
            return View(makaleler);

        }
        public ActionResult MakaleDetay(int id) {
            var makale = db.Makales.Where(m => m.makaleId == id).SingleOrDefault();
            if (makale == null) {

                return HttpNotFound();
            }

            return View(makale);
        }
        public ActionResult Hakkimda() {

            return View();


        }
        public ActionResult Iletisim()
        {

            return View();

        }
        public ActionResult KategoriPartial()
        {

            return View(db.Kategoris.ToList());

        }

        public JsonResult YorumYap(string yorum, int makaleId) {

            var uyeId = Session["uyeId"];
            if (yorum != null) {

                db.Yorums.Add(new Yorum { uyeId = Convert.ToInt32(uyeId), makaleId = makaleId, icerik = yorum, tarih = DateTime.Now });
                db.SaveChanges();
            }

            return Json(false,JsonRequestBehavior.AllowGet);
        }
        public ActionResult YorumSil(int id) {

            var uyeId = Session["uyeId"];
            var yorum = db.Yorums.Where(y => y.yorumId == id).SingleOrDefault();
            var makale = db.Makales.Where(m => m.makaleId == yorum.makaleId).SingleOrDefault();
            if (yorum.uyeId == Convert.ToInt32(uyeId))
            {

                db.Yorums.Remove(yorum);
                db.SaveChanges();
                return RedirectToAction("MakaleDetay", "Home", new { id = makale.makaleId });
            }
            else {

                return HttpNotFound();
            }

            
        }
        public ActionResult OkunmaArttir(int MakaleId ) {

            var makale = db.Makales.Where(m => m.makaleId == MakaleId).SingleOrDefault();
            makale.okunma += 1;
            db.SaveChanges();
            return View();
        }
    }
    
}