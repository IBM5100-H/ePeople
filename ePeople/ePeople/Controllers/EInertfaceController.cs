using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using System.IO;
using System.Text;
using System.Collections;

namespace ePeople.Controllers
{
    public class EInertfaceController : Controller
    {
        // GET: EInertface
        EpeopleEntities db = new EpeopleEntities();
        public ActionResult Index()
        {

            return Json("");
        }

        public ActionResult GetArticlesTitle()
        {
            var article = db.Article;

            return Json(article, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetArticlesDetail(string ArticleId)
        {
            var article = db.Article.Find(Convert.ToInt32(ArticleId));
            //var article = db.Article.Find(1);
            string path1 = Server.MapPath("/");
            string path = path1 + article.ArticlePath;
            StreamReader sr = new StreamReader(path, Encoding.UTF8);
            StringBuilder art = new StringBuilder();
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                art.Append(line.ToString());
            }
            Hashtable ht = new Hashtable();
            ht["ArticleTitle"] = article.ArticleTitle;
            ht["ArticleDate"] = article.ArticleDate;
            ht["ArticleDetail"] = art.ToString();

            return Json(ht,JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPicture(string PictureTypeId)
        {
            int PictureTypeIdd = Convert.ToInt32(PictureTypeId);
            var picture= db.Picture.Where(e=>e.PictureTypeId== PictureTypeIdd);
            Hashtable ht = new Hashtable();
            string url = Request.Url.AbsoluteUri;
            string urlPath = url.Substring(0, url.IndexOf('/',8) );
            ht["type"] = "Index";
            ht["urlPath"] = urlPath;
            ht["picture"] = picture;
            return Json(ht,JsonRequestBehavior.AllowGet);
        }
    }
}