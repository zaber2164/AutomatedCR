using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomatedCR.DbFile;
using Newtonsoft.Json;

namespace AutomatedCR.Controllers
{
    public class CourseController : Controller
    {
        private readonly AutomatedCrConnection dbEntities = new AutomatedCrConnection();
        // GET: Course
        public ActionResult Course()
        {
            try
            {
                List<Course> CourseList =
                    dbEntities.Courses.ToList();
                if (CourseList.Count>0)
                {
                    return View(CourseList);
                }
                else
                {
                    return View(new List<Course>());
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult GetCourseById(long Id)
        {
            try
            {
                Course crs = dbEntities.Courses.Where(e => e.CourseId == Id).FirstOrDefault();

                String data = JsonConvert.SerializeObject(crs, Formatting.None);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Add(Course Data)
        {
            try
            {
                //Int64 userid = Convert.ToInt32(Session["UserId"]);
                if (Data != null)
                {
                    dbEntities.Courses.Add(Data);
                    dbEntities.SaveChanges();
                    int id = Data.CourseId;

                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Data not found.", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult Update(Course Data)
        {
            try
            {
                //Int64 userid = Convert.ToInt32(Session["UserId"]);
                if (Data != null)
                {
                    var course = dbEntities.Courses.FirstOrDefault(e => e.CourseId == Data.CourseId);

                    course.Title = Data.Title;
                    course.TeacherId = Data.TeacherId;
                    course.Time = Data.Time;
                    course.Location = Data.Location;
                    course.Semester = Data.Semester;

                    dbEntities.SaveChanges();

                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Data not found.", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult UpdateTableData()
        {
            try
            {
                List<Course> courseList =
                    dbEntities.Courses.ToList();

                return Json(courseList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}