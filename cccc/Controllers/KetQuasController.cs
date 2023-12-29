using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cccc.Models;

namespace cccc.Controllers
{
    public class KetQuasController : Controller
    {
        private DataContext db = new DataContext();

        // GET: KetQuas
        public ActionResult Index1()
        {
            var ketQuas = db.KetQuas.Include(k => k.NhanVien);
            //tim thấp nhất

            var ketQuaMin = ketQuas.OrderBy(k => k.SoLuong * k.DonGia).FirstOrDefault();
            ViewBag.ketQuaMin = ketQuaMin;

            return View(ketQuas.ToList());
        }
        public ActionResult Index( string search, string sort,int soluong = 20,int MaNV = 0)
        {
            var ketQuas = db.KetQuas.Include(k => k.NhanVien);
            //tim kiem textbox
            if(!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                ketQuas = ketQuas.Where(k => k.NhanVien.HoTenNV.Trim().ToLower().Contains(search));
            }
            //combobox
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTenNV");
            if(MaNV != 0 )
            {
                ketQuas = ketQuas.Where(k => k.MaNV == MaNV);
            }
            //sort
            if (sort == "tt_asc")
            {
                ViewBag.TTsort = "tt_desc";
            }
            else ViewBag.TTsort = "tt_asc";
            switch(sort)
            {
                case "tt_desc":
                    ketQuas = ketQuas.Where(k=>k.SoLuong >= soluong).OrderByDescending(k => k.DonGia);
                    break;
                case "tt_asc":
                    ketQuas = ketQuas.Where(k => k.SoLuong >= soluong).OrderBy(k => k.DonGia);
                    break;            
            }    

            return View(ketQuas.ToList());
        }

        // GET: KetQuas/Details/5
        public ActionResult Details(int? id, string id2)
        {
            if (id == null || id2 ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KetQua ketQua = db.KetQuas.Find(id, id2);
            if (ketQua == null)
            {
                return HttpNotFound();
            }
            return View(ketQua);
        }

        // GET: KetQuas/Create
        public ActionResult Create()
        {
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTenNV");
            return View();
        }

        // POST: KetQuas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,TenSp,DonGia,SoLuong")] KetQua ketQua)
        {
            if (ModelState.IsValid)
            {
                db.KetQuas.Add(ketQua);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTenNV", ketQua.MaNV);
            return View(ketQua);
        }

        // GET: KetQuas/Edit/5
        public ActionResult Edit(int? id, string id2)
        {
            if (id == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KetQua ketQua = db.KetQuas.Find(id, id2);
            if (ketQua == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTenNV", ketQua.MaNV);
            return View(ketQua);
        }

        // POST: KetQuas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,TenSp,DonGia,SoLuong")] KetQua ketQua)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ketQua).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTenNV", ketQua.MaNV);
            return View(ketQua);
        }

        // GET: KetQuas/Delete/5
        public ActionResult Delete(int? id, string id2)
        {
            if (id == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KetQua ketQua = db.KetQuas.Find(id, id2);
            if (ketQua == null)
            {
                return HttpNotFound();
            }
            return View(ketQua);
        }

        // POST: KetQuas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string id2)
        {
            KetQua ketQua = db.KetQuas.Find(id, id2);
            db.KetQuas.Remove(ketQua);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
