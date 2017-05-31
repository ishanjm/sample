using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Model.SystemModel;
using Model.MasterData;
using Service.MasterDataService;

namespace App.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Profile(string ID)
        {
            UserModel UserData = new UserModel();
            if (string.IsNullOrEmpty(ID))
            {
              UserData = UserService.Fetch(UserIdentity.UserID);
            }
            else
            {
                UserData = UserService.Fetch(ID);
            }
            return View(UserData);
        }
        public ActionResult UsersList()
        {
            ViewBag.PageTitle = "User List";
            if (Request.Cookies["UserID"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }

            if (UserIdentity.UserGroup != UserGroup.Admin && UserIdentity.UserGroup != UserGroup.StaffMember)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            return View();
        }
        public ActionResult ManageUser(string id, string mode)
        {
            if (Request.Cookies["UserID"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            switch (mode)
            {
                case "New":

                    if (UserIdentity.UserGroup != UserGroup.Admin)
                    {
                        return RedirectToAction("AccessDenied", "Error");
                    }
                    ViewBag.PageTitle = "Create User";
                    ViewBag.PageType = "Create User";
                    break;
                case "Edit":
                    if (UserIdentity.UserGroup != UserGroup.Admin)
                    {
                        return RedirectToAction("AccessDenied", "Error");
                    }
                    ViewBag.PageTitle = "Edit User";
                    ViewBag.PageType = "Edit User";
                    break;
                case "View":
                    ViewBag.PageTitle = "View User";
                    ViewBag.PageType = "View User";
                    break;
                case "Delete":
                    if (UserIdentity.UserGroup != UserGroup.Admin)
                    {
                        return RedirectToAction("AccessDenied", "Error");
                    }
                    ViewBag.PageTitle = "Delete";
                    ViewBag.PageType = "Delete";
                    break;
                default:
                    if (UserIdentity.UserGroup != UserGroup.Admin)
                    {
                        return RedirectToAction("AccessDenied", "Error");
                    }
                    ViewBag.PageTitle = "User List";
                    return RedirectToAction("UsersList");
            }

            UserModel User = new UserModel();
            User.ServiceStartDate = DateTime.Now;

            if (string.Equals(mode, "View") && !string.IsNullOrEmpty(id))
            {
                User = UserService.Fetch(id);
            }
            else if (string.Equals(mode, "Edit") && !string.IsNullOrEmpty(id))
            {
                User = UserService.Fetch(id);
            }
            else if (string.Equals(mode, "Delete"))
            {
                User.ID = id;
                UserService.Delete(User);
                return RedirectToAction("UsersList");
            }
            ViewBag.Status = User.Status.ToString();
            return View(User);
        }
        public string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }
        [HttpPost]
        public ActionResult ManageUser(UserModel User, FormCollection col)
        {
            if (Request.Cookies["UserID"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            string Status = Request.Form["IsActive"];
            string mode = Request.Form["FormMode"];
            switch (mode)
            {
                case "Create User":
                    ViewBag.PageTitle = "Create User";
                    ViewBag.PageType = "Create User";
                    if (UserIdentity.UserGroup != UserGroup.Admin)
                    {
                        return RedirectToAction("AccessDenied", "Error");
                    }
                    break;
                case "Edit User":
                    if (UserIdentity.UserGroup != UserGroup.Admin)
                    {
                        return RedirectToAction("AccessDenied", "Error");
                    }
                    ViewBag.PageTitle = "Edit User";
                    ViewBag.PageType = "Edit User";
                    break;
                case "View File":
                    ViewBag.PageTitle = "View User";
                    ViewBag.PageType = "View User";
                    break;
                default:
                    ViewBag.PageTitle = "File List";
                    return RedirectToAction("UsersList");
            }
            if (string.Equals(mode, "Create User"))
            {
                if (Status == "on")
                {
                //    User.Status = BaseModule.Models.SystemData.Status.Active;
                }
                else if (Status == null)
                {
                //    User.Status = BaseModule.Models.SystemData.Status.Deleted;
                }
                if (ModelState.IsValid && ValidatePrefix(User.OrphanPrefix,User.UserID))
                {
                    try
                    {
                        HttpPostedFileBase ProfileImage = Request.Files[0];
                        if (ProfileImage.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(ProfileImage.FileName);
                            if (extension != ".jpg" && extension != ".JPG" && extension != ".jpeg" && extension != ".JPEG" && extension != ".png" && extension != ".PNG")
                            {
                                ModelState.AddModelError(string.Empty, "Only JPEG or PNG files are allowed for Profile Image");
                                return View(User);
                            }
                            String path = Path.Combine(Server.MapPath("~/UserContent/ProfileImages/"), (User.FirstName + " " + User.LastName));
                          //  string SaveLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserContent\\ProfileImages\\" + User.FirstName + " " + User.LastName); ;
                            string SaveLocationDB = "UserContent\\ProfileImages\\" + User.FirstName + " " + User.LastName;
                            if (System.IO.Directory.Exists(path) == false)
                            {
                                System.IO.Directory.CreateDirectory(path);
                            }
                            DateTime dtNow = DateTime.Now;
                            string NewFileName = Convert.ToString(dtNow.Year) + Convert.ToString(dtNow.Month) + Convert.ToString(dtNow.Day) + Convert.ToString(dtNow.Hour) + Convert.ToString(dtNow.Minute) + Convert.ToString(dtNow.Second) + Convert.ToString(dtNow.Millisecond);
                            path = path + "\\" + NewFileName + extension;
                            SaveLocationDB = SaveLocationDB + "\\" + NewFileName + extension;

                            User.ProfileImage = SaveLocationDB;

                            extension = ProfileImage.FileName.Split('.')[1];
                            if (extension == "jpg")
                            {
                                extension = "jpeg";
                            }
                            //string ImageText = Request.Form["ImageData"];
                            //string convert = ImageText.Replace("data:image/" + extension + ";base64,", String.Empty);
                            //Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(convert));
                            //MemoryStream ms = new MemoryStream(bitmapData, 0, bitmapData.Length);

                            // Convert byte[] to Image
                           // ms.Write(bitmapData, 0, bitmapData.Length);
                           //  System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                           //  var file = image;

                            // Convert byte[] to Image
                           // ms.Write(bitmapData, 0, bitmapData.Length);
                            // System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                            //image.Save(SaveLocation);

                            //FileStream fs = new FileStream(path, FileMode.Create);
                            //BinaryWriter bw = new BinaryWriter(fs);
                            //try
                            //{
                            //    bw.Write(bitmapData);
                            //}
                            //finally
                            //{
                            //    fs.Close();
                            //    bw.Close();
                            //}
                            ProfileImage.SaveAs(path);

                          //  image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        UserService.Create(User);
                       Session["SuccessMsg"] = "Saved Successfully";
                    }
                    catch (Exception e)
                    {
                        Session["SuccessMsg"] = e.Message;
                        ModelState.AddModelError("Error", e.Message);
                        return View(User);
                    }
                }
                else
                {
                    return View(User);
                }
            }

            else if (string.Equals(mode, "Edit User") )
            {
                if (Status == "on")
                {
                  //  User.Status = BaseModule.Models.SystemData.Status.Active;
                }
                else if (Status == null)
                {
                  //  User.Status = BaseModule.Models.SystemData.Status.Deleted;
                }
                if (ModelState.IsValid && ValidatePrefix(User.OrphanPrefix, User.UserID))
                {
                    try
                    {
                        HttpPostedFileBase ProfileImage = Request.Files[0];
                        if (Request.Files.Count > 0)
                        {
                            String SaveLocation = Path.Combine(Server.MapPath("~/UserContent/ProfileImages/"), (User.FirstName + " " + User.LastName));
                          //  string SaveLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserContent\\ProfileImages\\" + User.FirstName + " " + User.LastName); ;
                            string SaveLocationDB = "UserContent\\ProfileImages\\" + User.FirstName +" " +User.LastName;
                            try
                            {
                                if (ProfileImage.ContentLength > 0)
                                {
                                    System.IO.Directory.Delete(SaveLocation, true);
                                }
                            }
                            catch
                            {

                            }

                            if (ProfileImage.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(ProfileImage.FileName);
                                if (extension != ".jpg" && extension != ".JPG" && extension != ".jpeg" && extension != ".JPEG" && extension != ".png" && extension != ".PNG")
                                {
                                    ModelState.AddModelError(string.Empty, "Only JPEG or PNG files are allowed for Profile Image");
                                    return View(User);
                                }
                                if (System.IO.Directory.Exists(SaveLocation) == false)
                                {
                                    System.IO.Directory.CreateDirectory(SaveLocation);
                                }
                                DateTime dtNow = DateTime.Now;
                                string NewFileName = Convert.ToString(dtNow.Year) + Convert.ToString(dtNow.Month) + Convert.ToString(dtNow.Day) + Convert.ToString(dtNow.Hour) + Convert.ToString(dtNow.Minute) + Convert.ToString(dtNow.Second) + Convert.ToString(dtNow.Millisecond);
                                SaveLocation = SaveLocation + "\\" + NewFileName + extension;
                                SaveLocationDB = SaveLocationDB + "\\" + NewFileName + extension;

                                User.ProfileImage = SaveLocationDB;

                                extension = ProfileImage.FileName.Split('.')[1];
                                if (extension == "jpg")
                                {
                                    extension = "jpeg";
                                }
                               // string ImageText = Request.Form["ImageData"];
                               // string convert = ImageText.Replace("data:image/" + extension + ";base64,", String.Empty);
                               // Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(convert));
                               // MemoryStream ms = new MemoryStream(bitmapData, 0, bitmapData.Length);

                               // // Convert byte[] to Image
                               // ms.Write(bitmapData, 0, bitmapData.Length);
                               //// System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                               // //image.Save(SaveLocation);

                               // FileStream fs = new FileStream(SaveLocation, FileMode.Create);
                               // BinaryWriter bw = new BinaryWriter(fs);
                               // try
                               // {
                               //     bw.Write(bitmapData);
                               // }
                               // finally
                               // {
                               //     fs.Close();
                               //     bw.Close();
                               // }

                                ProfileImage.SaveAs(SaveLocation);
                            }
                           
                        }

                        UserService.Update(User);
                        Session["SuccessMsg"] = "Saved Successfully";
                    }
                    catch (Exception e)
                    {
                        Session["SuccessMsg"] = e.Message;
                        ModelState.AddModelError("Error", e.Message);
                        return View(User);
                    }
                }
                else
                {
                    ViewBag.HasError = "True";
                    ViewBag.ErrorMsg = "Not Saved,Sponser Prefix Duplicated";
                    return View(User);
                }
            }
            return RedirectToAction("UsersList", "Users");
        }

        private bool ValidatePrefix(string Prefix,string UserID)
        {
            List<UserModel> UserList = new List<UserModel>();
            if (!string.IsNullOrEmpty(UserID))
            {
                UserList = UserService.SearchSponserList(" OrphanPrefix='" + Prefix + "' AND UserID !='" + UserID + "' ");
            }
            else
            {
                UserList = UserService.SearchSponserList(" OrphanPrefix='" + Prefix + "'");
            }
            if (UserList.Count>0)
            {
                //Session["SuccessMsg"] = true;
                //ModelState.AddModelError("Error", "Orphan prefix already in the system");
                return false;
            }
            else
            {
                return true;
            }
        }
        public ActionResult ListUsersJson(jQueryDataTableParamModel param)
        {
            // Sorting parameters
            var iSortCol = int.Parse(Request.Params["iSortCol_0"]);
            var iSortDir = Request.Params["sSortDir_0"];
            string sortColmn = string.Empty;
            string search = param.sSearch;
            string SortExpression = string.Empty;

            switch (iSortCol)
            {
                case 1:
                    sortColmn = "concat(FirstName,' ',LastName)";
                    break;
                case 2:
                    sortColmn = "Email";
                    break;
                case 3:
                    sortColmn = "UserGroup";
                    break;
                case 4:
                    sortColmn = "OrphanPrefix";
                    break;
            }

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            string orderByClause = string.Empty;

            if (Request.Params["bSortable_" + iSortCol] == "true")
            {
                sb.Append(sortColmn);
                sb.Append(" ");
                sb.Append(iSortDir);
                orderByClause = sb.ToString();
                orderByClause = "ORDER BY " + orderByClause + ", concat(FirstName,' ',LastName)";
            }

            var Name = Convert.ToString(Regex.Replace(Request["sSearch_2"], @"[']", String.Empty));
            var Email = Convert.ToString(Regex.Replace(Request["sSearch_3"], @"[']", String.Empty));
            var UserGroup = Convert.ToString(Regex.Replace(Request["sSearch_4"], @"[']", String.Empty));
            var Status = Convert.ToString(Regex.Replace(Request["sSearch_5"], @"[']", String.Empty));

            int TotalDBResultCount = 0;
            List<UserModel> UserList = null;
            SearchCondition condition = new SearchCondition();

            try
            {
                if (orderByClause == "")
                    condition.SortExpression = "order by concat(FirstName,' ',LastName)";
                else
                    condition.SortExpression = orderByClause;

                condition.RecordStart = param.iDisplayStart + 1;
                condition.RecordEnd = condition.RecordStart - 1 + param.iDisplayLength;

                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    ////search by Main Search
                    condition.searchCond = string.Format(@" FirstName like %{0}%' and LastName like %{0}%' or Email like '%{0}%' or UserGroup like '%{0}%' or OrphanPrefix like '%{0}%' ", Regex.Replace(param.sSearch, @"[']", String.Empty));
                }

                ////search by colmns
                condition.searchCondColFilter = string.Format(@"(( FirstName like '%{0}%' OR LastName like '%{0}%') and Email like '%{1}%' and UserGroup like '%{2}%' and OrphanPrefix like '%{3}%')", Name, Email, UserGroup, Status);

                UserList = UserService.Search(condition).Values.ToList();
                TotalDBResultCount = UserList.Count;

            }
            catch (UnauthorizedAccessException unExp)
            {

            }
            catch (Exception Exp)
            {

            }

            if (UserList != null)
            {
                var result = from User in UserList
                             select new object[] { User.UserID, User.FirstName + " " + User.LastName, User.Email, User.UserGroup.ToString(), User.OrphanPrefix, "Action" };

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = result.Count(),//Total filterd from DB Records
                    iTotalDisplayRecords = condition.TotalCount, //Total DB Records
                    aaData = result
                },
                JsonRequestBehavior.AllowGet);
            }
            else return null;
        }
    }
}