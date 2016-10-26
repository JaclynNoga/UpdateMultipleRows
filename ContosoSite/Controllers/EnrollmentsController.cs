using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoSite.Models;

namespace ContosoSite.Controllers
{
    public class EnrollmentsController : Controller
    {
        private ContosoUniversityDataEntities db = new ContosoUniversityDataEntities();

        // GET: Enrollments
        public ActionResult Index()
        {
            var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Student);
            return View(enrollments.ToList());
        }

        // GET: Enrollments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentID,Grade,CourseID,StudentID")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        //GET: Enrollments/Edit
        [HttpGet]
        public ActionResult Edit()
        {
            List<Enrollment> model = new List<Enrollment>();
            using (ContosoUniversityDataEntities dc = new ContosoUniversityDataEntities())
            {
                model = dc.Enrollments.ToList();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName");
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(List<Enrollment> list)
        {
            if (ModelState.IsValid)
            {
                using (ContosoUniversityDataEntities dc = new ContosoUniversityDataEntities())
                {
                    foreach(var i in list)
                    {
                        var c = dc.Enrollments.Where(a => a.EnrollmentID.Equals(i.EnrollmentID)).FirstOrDefault();
                        if (c != null)
                        {
                            c.Grade = i.Grade;
                            c.CourseID = i.CourseID;
                            c.StudentID = i.StudentID;
                        }
                    }
                    dc.SaveChanges();
                }
                ViewBag.Message = "Successfully Updated.";
                return View(list);
            }
            else
            {
                ViewBag.Message = "Failed! Please try again.";
                return View(list);
            }
        }

        // GET: Enrollments/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Enrollment enrollment = db.Enrollments.Find(id);
        //    if (enrollment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
        //    ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName", enrollment.StudentID);
        //    return View(enrollment);
        //}

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "EnrollmentID,Grade,CourseID,StudentID")] Enrollment enrollment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(enrollment).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
        //    ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName", enrollment.StudentID);
        //    return View(enrollment);
        //}

        // GET: Enrollments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
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
