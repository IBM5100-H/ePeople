using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using System.IO;
using System.Text;
using System.Collections;
using Newtonsoft.Json;

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

        //获取文章标题
        public ActionResult GetArticlesTitle()
        {
            var article = db.Article;

            return Json(article, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="ArticleId">文章id</param>
        /// <returns></returns>
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
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="PictureTypeId">图片类型 1选手照片 2首页轮播 3参赛证 </param>
        /// <returns></returns>
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

        //获取学习列表
        public ActionResult GetSchool()
        {
            var school = db.School;
            Hashtable ht = new Hashtable();
            ht["message"] = "success";
            ht["schoolList"] = school;
            return Json(ht, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddPlayer(string obj, string PerformanceName1)
        {
            SignUpMV playerMV = JsonConvert.DeserializeObject<SignUpMV>(obj);
            Player player = new Player();
            string performanceName1 = PerformanceName1.Replace("//","").Replace("[","").Replace("]","");
            DateTime today = DateTime.Now;

            player.PlayerBirthday =today.AddYears(-playerMV.PlayerBirthday);
            player.PlayerSex = playerMV.PlayerSex;
            player.PlayerDeclaration = playerMV.PlayerDeclaration;
            player.PlayerIdCard = playerMV.PlayerIdCard;
            player.PlayerName = playerMV.PlayerName;
            player.PlayerPrize = playerMV.PlayerPrize;
            player.SchoolId = playerMV.SchoolId;            
            db.Player.Add(player);
            int flag= db.SaveChanges();
            int playerId;
            if (flag > 0)
            {
                playerId = player.PlayerId;
                Parent parent = new Parent();
                parent.ParentName = playerMV.ParentName;
                parent.ParentEmail = playerMV.ParentEmail;
                parent.PlayerId = playerId;
                db.Parent.Add(parent);

                List<PerformanceMX> performanceMXes = new List<PerformanceMX>();
                if (!String.IsNullOrEmpty(PerformanceName1 ))
                {
                    performanceMXes.Add(new PerformanceMX()
                    {
                        PerformanceName = performanceName1,
                        PlayerId=playerId
                    });                
                }
                if (!String.IsNullOrEmpty(playerMV.PerformanceName2 ))
                {
                    performanceMXes.Add(new PerformanceMX()
                    {
                        PerformanceName = playerMV.PerformanceName2,
                        PlayerId = playerId
                    });
                }
                if (!String.IsNullOrEmpty( playerMV.PerformanceName3 ))
                {
                    performanceMXes.Add(new PerformanceMX()
                    {
                        PerformanceName = playerMV.PerformanceName3,
                        PlayerId = playerId

                    });
                }
                db.PerformanceMX.AddRange(performanceMXes);

                db.SaveChanges();
            }
            return Content("success");
        }

        //生成才赛证图片
        public ActionResult CreateCertificate(string PlayerId)
        {
            var player = db.V_certificate.Where(e =>e.PlayerId== 10);

            //string selectPerformanceSql = "select PerformanceName from V_certificate where PlayerId=10";
            return Json(player, JsonRequestBehavior.AllowGet);
            
            //return Content("");
        }
        public ActionResult UploadAvatarUrl()
        {
            var photo = Request.Form["avatar"];

            return Content("");
        }

    }
}