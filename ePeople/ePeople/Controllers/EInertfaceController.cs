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
using System.Net;
using Newtonsoft.Json.Linq;

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

            return Json(ht, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="PictureTypeId">图片类型 1选手照片 2首页轮播 3参赛证 </param>
        /// <returns></returns>
        public ActionResult GetPicture(string PictureTypeId)
        {
            int PictureTypeIdd = Convert.ToInt32(PictureTypeId);
            var picture = db.Picture.Where(e => e.PictureTypeId == PictureTypeIdd);
            Hashtable ht = new Hashtable();
            string url = Request.Url.AbsoluteUri;
            string urlPath = url.Substring(0, url.IndexOf('/', 8));
            ht["type"] = "Index";
            ht["urlPath"] = urlPath;
            ht["picture"] = picture;
            return Json(ht, JsonRequestBehavior.AllowGet);
        }

        //获取学生列表
        public ActionResult GetSchool()
        {
            var school = db.School;
            Hashtable ht = new Hashtable();
            ht["message"] = "success";
            ht["schoolList"] = school;
            return Json(ht, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="PerformanceName1"></param>
        /// <returns></returns>
        public ActionResult AddPlayer(string obj, string PerformanceName1)
        {
            SignUpMV playerMV = JsonConvert.DeserializeObject<SignUpMV>(obj);
            Player player = new Player();
            string performanceName1 = PerformanceName1.Replace("/", "").Replace("[", "").Replace("]", "").Trim('"');
            DateTime today = DateTime.Now;
            Hashtable ht = new Hashtable();
            player.PlayerBirthday = today.AddYears(-playerMV.PlayerBirthday);
            player.PlayerSex = playerMV.PlayerSex;
            player.PlayerDeclaration = playerMV.PlayerDeclaration;
            player.PlayerIdCard = playerMV.PlayerIdCard;
            player.PlayerName = playerMV.PlayerName;
            player.PlayerPrize = playerMV.PlayerPrize;
            player.SchoolId = playerMV.SchoolId;
            player.PlayerVotes = 0;
            db.Player.Add(player);
            int flag = db.SaveChanges();
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
                if (!String.IsNullOrEmpty(PerformanceName1))
                {
                    performanceMXes.Add(new PerformanceMX()
                    {
                        PerformanceName = performanceName1,
                        PlayerId = playerId
                    });
                }
                if (!String.IsNullOrEmpty(playerMV.PerformanceName2))
                {
                    performanceMXes.Add(new PerformanceMX()
                    {
                        PerformanceName = playerMV.PerformanceName2,
                        PlayerId = playerId
                    });
                }
                if (!String.IsNullOrEmpty(playerMV.PerformanceName3))
                {
                    performanceMXes.Add(new PerformanceMX()
                    {
                        PerformanceName = playerMV.PerformanceName3,
                        PlayerId = playerId

                    });
                }
                db.PerformanceMX.AddRange(performanceMXes);

                db.SaveChanges();

                ht["message"] = "success";
                ht["PlayerId"] = playerId;
                return Json(ht, JsonRequestBehavior.AllowGet);
            }
            ht["message"] = "fail";
            return Json(ht, JsonRequestBehavior.AllowGet);

        }

        //生成参赛证
        public ActionResult CreateCertificate(string PlayerId)
        {
            int playerid = Convert.ToInt32(PlayerId);
            List<V_certificate> player = new List<V_certificate>();
            player.AddRange(db.V_certificate.Where(e => e.PlayerId == playerid));
            Hashtable ht = new Hashtable();

            DateTime brith = Convert.ToDateTime(player[0].PlayerBirthday);
            int age = DateTime.Now.Year - brith.Year;
            ht["age"] = age;


            ht["player"] = player;
            return Json(ht, JsonRequestBehavior.AllowGet);

           
        }

        //获取所有选手信息
        public ActionResult GetPlayer(string number, string size, string name)
        {
            int Number = Convert.ToInt32(number);
            int Size = Convert.ToInt32(size);
            var list = db.Player;
            int total = list.Count();
            var lists = list.OrderBy(e => e.PlayerId).Skip(Size * (Number - 1)).Take(Size);


            Hashtable ht = new Hashtable();
            ht["pageNum"] = Number;
            ht["pageSize"] = Size;
            ht["size"] = Size;
            ht["list"] = lists;
            ht["pages"] = (total + Size - 1) / Size;
            ht["total"] = total;

            return Json(ht, JsonRequestBehavior.AllowGet);
        }

        //openid
        public ActionResult GetOpenId(string code)
        {
            Hashtable ht = new Hashtable();
            string appid = "wx6cfb73c5eb45daa2";
            string secret = "c68351e72cc59a9c06aa5591ab0073a5";
            string apiurl = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiurl);
            request.Method = "GET";
            request.Timeout = 5000;


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadToEnd();
                var jsonobj = (JObject)JsonConvert.DeserializeObject(result);
                string _openid = jsonobj["openid"].ToString();
                ht["openid"] = _openid;
                ht["session_key"] = jsonobj["session_key"].ToString();

            }
            return Json(ht, JsonRequestBehavior.AllowGet);
        }
        //投票
        public ActionResult Vote()
        {
            Hashtable ht = new Hashtable();
            string playerId = Request["playerId"];
            string voterOpenId = Request["voterOpenId"];
            string voterName = Request["voterName"];
            var isNullVoter = db.Voter.Where(e => e.VoterOpenId == voterOpenId).FirstOrDefault();
            int n = 0;
            Voter voter = new Voter()
            {
                VoterName = voterName,
                VoterOpenId = voterOpenId,
                VoterVotes=5
            };

            //是否已存在用户
            if (isNullVoter == null)
            {
                db.Voter.Add(voter);
                n = db.SaveChanges();
                if (n > 0)
                {
                    var player = db.Player.Find(Convert.ToInt32(playerId));
                    player.PlayerVotes += 1;

                    voter.VoterVotes -= 1;

                    VoterMX voterMX = new VoterMX()
                    {
                        VoterId = voter.VoterId,
                        PlayerId = Convert.ToInt32(playerId),
                        VoterMXTime = FetchDBDateTime()

                    };
                    db.VoterMX.Add(voterMX);
;
                    db.SaveChanges();
                }

            }
            else
            {
                var voters = db.Voter.Where(e => e.VoterOpenId == voterOpenId).FirstOrDefault();
                if(voters.VoterVotes>0)
                {
                    var player = db.Player.Find(Convert.ToInt32(playerId));
                    player.PlayerVotes += 1;

                    voters.VoterVotes -= 1;
                    VoterMX voterMX = new VoterMX()
                    {
                        VoterId = voters.VoterId,
                        PlayerId = Convert.ToInt32(playerId),
                        VoterMXTime = FetchDBDateTime()
                    };
                    db.VoterMX.Add(voterMX);
                    db.SaveChanges();
                    ht["message"] = "投票成功";
                }
                else
                {
                    ht["message"] = "超出投票次数";
                }
           

            }





           
            return Json(ht, JsonRequestBehavior.AllowGet);
        }


        #region FetchDBDateTime Function              
        /// <summary>
        /// 获取数据库的当前时间
        /// </summary>
        /// <returns></returns>
        public DateTime FetchDBDateTime()
        {
            var now = db.Database.SqlQuery<DateTime?>("SELECT GetDate()").First();
            if (now == null)
            {
                now = new DateTime(1900, 1, 1, 0, 0, 0);
            }
            return now.Value;
        }
        #endregion

        public ActionResult UploadAvatarUrl()
        {
            var photo = Request.Form["avatar"];

            return Content("");
        }

    }
}