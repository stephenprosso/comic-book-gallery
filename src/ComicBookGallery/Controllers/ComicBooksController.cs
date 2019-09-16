using ComicBookGallery.Data;
using ComicBookGallery.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComicBookGallery.Controllers
{
    public class ComicBooksController : Controller
    {
        private ComicBookRepository _comicBookRepository = null;

        //we need an instance of our respository.
        //add a constructor and instanciate our respoitory  within it
        //remember constructors are special methods that are called when an instance of our class is being instantiated
        //we use constructors to initialize instance members which is what we are doing our with respoitory field
        //constructier can be identitfied by thier lack of return type in their name which matches the class name. 
        public ComicBooksController()
        {
            _comicBookRepository = new ComicBookRepository();
        }

        public ActionResult Detail(int id)
        {
            var comicBook = _comicBookRepository.GetComicBook(id);


           //strongly typed view passing the comic book model instance to the view
           //A strongly typed view is an MVC view that is associtated with a speficis type, a strongly typed view exposes the model instance to intstance model property
            return View(comicBook);

        }

        ComicBookGallery.Models.RexEntities db = new ComicBookGallery.Models.RexEntities();

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            var model = db.ORDER_DETAIL;
            return PartialView("~/Views/Home/_GridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] ComicBookGallery.Models.ORDER_DETAIL item)
        {
            var model = db.ORDER_DETAIL;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Views/Home/_GridViewPartial.cshtml", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] ComicBookGallery.Models.ORDER_DETAIL item)
        {
            var model = db.ORDER_DETAIL;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.Order_Number == item.Order_Number);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Views/Home/_GridViewPartial.cshtml", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete(System.String Order_Number)
        {
            var model = db.ORDER_DETAIL;
            if (Order_Number != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.Order_Number == Order_Number);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Home/_GridViewPartial.cshtml", model.ToList());
        }
    }
}