using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rpmDEL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace rpmDEL.Controllers
{
    public class ApanelController : Controller
    {
        private ApplicationContext db;
        private IWebHostEnvironment _app;
        public HttpContext _httpContext;
        public ApanelController(ApplicationContext context, IWebHostEnvironment app)
        {
            db = context;
            _app = app;
        }

        public async Task<IActionResult> RequestProfileEdit(int? id, string oldlogin)
        {
            if (id != null)
            {
                user user = await db._user.FirstOrDefaultAsync(predicate => predicate._ID == id);
                if (user != null)
                {
                    ViewBag.oldlogin = oldlogin;
                    return View(user);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> RequestProfileEdit(user curruser, IFormFile file, string oldlogin, string oldimagepath)
        {
            if (ModelState.IsValid)
            {
                FileModel newFile = null;
                if (db._user.AsNoTracking().FirstOrDefault(u => u._login == curruser._login) == null || curruser._login == oldlogin)
                {
                    if (file != null)
                    {
                        string path = "/files/" + file.FileName;
                        using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        newFile = new FileModel
                        {
                            _name = file.FileName,
                            _path = path
                        };
                        db.files.Add(newFile);
                    }
                    if (newFile != null)
                        curruser._imgpath = newFile._path;
                    else
                        curruser._imgpath = oldimagepath;

                    db._user.Update(curruser);
                    await db.SaveChangesAsync();
                    return RedirectToAction("main", "home");
                }
                else
                {
                    ViewBag.oldlogin = oldlogin;
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует!");
                }
            }
            curruser._imgpath = oldimagepath;
            return View(curruser);
        }

        [HttpGet]
        [ActionName("RequestProfileDelete")]
        public async Task<IActionResult> ConfirmRequestProfileDelete(int? id)
        {
            if (id != null)
            {
                user user = await db._user.FirstOrDefaultAsync(predicate => predicate._ID == id);
                if (user != null)
                {
                    return View(user);
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> RequestProfileDelete(int? id)
        {
            if (id != null)
            {
                user user = await db._user.FirstOrDefaultAsync(predicate => predicate._ID == id);
                if (user != null)
                {
                    db._user.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("main", "home");
                }
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult RequestProfileAdd()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestProfileAdd(AddUserModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                    user user = await db._user.FirstOrDefaultAsync(u => u._login == model._login);
                    if (user == null)
                    {
                        FileModel newFile = null;

                        if (file != null)
                        {
                            string path = "/files/" + file.FileName;
                            using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                            newFile = new FileModel
                            {
                                _name = file.FileName,
                                _path = path
                            };
                            db.files.Add(newFile);
                        }

                        user = new user { _login = model._login, _pass = model._pass, _EMail = model._EMail, _Name = model._Name, _Fam = model._Fam, _Surname = model._Surname, _regday = DateTime.Now };

                        if (newFile != null)
                            user._imgpath = newFile._path;
                        else
                            user._imgpath = "/images/Icon.jpg";


                        role userRole = await db.roles.FirstOrDefaultAsync(r => r.Name == "user");
                        if (userRole != null)
                            user.Role = userRole;

                        db._user.Add(user);
                        await db.SaveChangesAsync();
                        return RedirectToAction("main", "home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь с таким логином уже существует!");
                        return View(model);
                    }
            }
            ModelState.AddModelError("", "Не удалось подтвердить пароль!");
            return View(model);
        }
        public IActionResult RoleTable()
        {
            var roles = db.roles.ToList();

            return View(roles);
        }

        public async Task<IActionResult> RoleEdit(int? id)
        {
            if (id != null && (id != 1 || id != 2))
            {
                role role = await db.roles.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (role != null)
                {
                    return View(role);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> RoleEdit(role role)
        {
            db.roles.Update(role);
            await db.SaveChangesAsync();
            return RedirectToAction("RoleTable", "apanel");
        }

        public async Task<IActionResult> RoleDelete(int? id)
        {
            if (id != null && (id != 1 || id != 2))
            {
                role role = await db.roles.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (role != null)
                {
                    return View(role);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> RoleDelete(role role)
        {
            db.roles.Remove(role);
            await db.SaveChangesAsync();
            return RedirectToAction("RoleTable", "apanel");
        }

        public IActionResult RoleAdd()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleAdd(role role)
        {
            db.roles.Add(role);
            await db.SaveChangesAsync();
            return RedirectToAction("RoleTable", "apanel");
        }

        public IActionResult PostTable()
        {
            var post = db.posts.ToList();

            return View(post);
        }

        public IActionResult RequestPostAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RequestPostAdd(Post post, IFormFile file1, IFormFile file2, IFormFile file3, IFormFile file4, IFormFile file5)
        {
            var user = db._user.AsNoTracking().FirstOrDefaultAsync(p => p._ID == post.UserID).Result;

            if (user != null)
            {

                FileModel newFile1 = null;
                FileModel newFile2 = null;
                FileModel newFile3 = null;
                FileModel newFile4 = null;
                FileModel newFile5 = null;

                if (file1 != null)
                {
                    string path = "/files/" + file1.FileName;
                    using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                    {
                        await file1.CopyToAsync(fileStream);
                    }
                    newFile1 = new FileModel
                    {
                        _name = file1.FileName,
                        _path = path
                    };
                    db.files.Add(newFile1);
                }

                if (file2 != null)
                {
                    string path = "/files/" + file2.FileName;
                    using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                    {
                        await file2.CopyToAsync(fileStream);
                    }
                    newFile2 = new FileModel
                    {
                        _name = file2.FileName,
                        _path = path
                    };
                    db.files.Add(newFile2);
                }

                if (file3 != null)
                {
                    string path = "/files/" + file3.FileName;
                    using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                    {
                        await file3.CopyToAsync(fileStream);
                    }
                    newFile3 = new FileModel
                    {
                        _name = file3.FileName,
                        _path = path
                    };
                    db.files.Add(newFile3);
                }

                if (file4 != null)
                {
                    string path = "/files/" + file4.FileName;
                    using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                    {
                        await file4.CopyToAsync(fileStream);
                    }
                    newFile4 = new FileModel
                    {
                        _name = file4.FileName,
                        _path = path
                    };
                    db.files.Add(newFile4);
                }

                if (file5 != null)
                {
                    string path = "/files/" + file5.FileName;
                    using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                    {
                        await file5.CopyToAsync(fileStream);
                    }
                    newFile5 = new FileModel
                    {
                        _name = file5.FileName,
                        _path = path
                    };
                    db.files.Add(newFile5);
                }

                if (newFile1 != null)
                    post.img1 = newFile1._path;
                else
                    post.img1 = null;

                if (newFile2 != null)
                    post.img2 = newFile2._path;
                else
                    post.img2 = null;

                if (newFile3 != null)
                    post.img3 = newFile3._path;
                else
                    post.img3 = null;

                if (newFile4 != null)
                    post.img4 = newFile4._path;
                else
                    post.img4 = null;

                if (newFile5 != null)
                    post.img5 = newFile5._path;
                else
                    post.img5 = null;

                db.posts.Add(post);
                await db.SaveChangesAsync();
                return RedirectToAction("posttable", "apanel");
            }
            else
            {
                ModelState.AddModelError("", "Данного пользователя не существует!");
                return View(post);
            }
        }

        public async Task<IActionResult> RequestPostEdit(int? id)
        {
            if (id != null)
            {
                Post post = await db.posts.FirstOrDefaultAsync(predicate => predicate.id == id);
                if (post != null)
                {
                    ViewBag.oldimagepath1 = post.img1;
                    ViewBag.oldimagepath2 = post.img2;
                    ViewBag.oldimagepath3 = post.img3;
                    ViewBag.oldimagepath4 = post.img4;
                    ViewBag.oldimagepath5 = post.img5;
                    return View(post);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> RequestPostEdit(Post newpost, IFormFile file1, IFormFile file2, IFormFile file3, IFormFile file4, IFormFile file5, string oldimagepath1, string oldimagepath2, string oldimagepath3, string oldimagepath4, string oldimagepath5, string cb1, string cb2, string cb3, string cb4, string cb5)
        {
            if (ModelState.IsValid)
            {
                var user = db._user.AsNoTracking().FirstOrDefaultAsync(p => p._ID == newpost.UserID).Result;

                if (user != null)
                {
                    FileModel newFile1 = null;
                    FileModel newFile2 = null;
                    FileModel newFile3 = null;
                    FileModel newFile4 = null;
                    FileModel newFile5 = null;

                    if (file1 != null)
                    {
                        string path = "/files/" + file1.FileName;
                        using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                        {
                            await file1.CopyToAsync(fileStream);
                        }
                        newFile1 = new FileModel
                        {
                            _name = file1.FileName,
                            _path = path
                        };
                        db.files.Add(newFile1);
                    }

                    if (file2 != null)
                    {
                        string path = "/files/" + file2.FileName;
                        using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                        {
                            await file2.CopyToAsync(fileStream);
                        }
                        newFile2 = new FileModel
                        {
                            _name = file2.FileName,
                            _path = path
                        };
                        db.files.Add(newFile2);
                    }

                    if (file3 != null)
                    {
                        string path = "/files/" + file3.FileName;
                        using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                        {
                            await file3.CopyToAsync(fileStream);
                        }
                        newFile3 = new FileModel
                        {
                            _name = file3.FileName,
                            _path = path
                        };
                        db.files.Add(newFile3);
                    }

                    if (file4 != null)
                    {
                        string path = "/files/" + file4.FileName;
                        using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                        {
                            await file4.CopyToAsync(fileStream);
                        }
                        newFile4 = new FileModel
                        {
                            _name = file4.FileName,
                            _path = path
                        };
                        db.files.Add(newFile4);
                    }

                    if (file5 != null)
                    {
                        string path = "/files/" + file5.FileName;
                        using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                        {
                            await file5.CopyToAsync(fileStream);
                        }
                        newFile5 = new FileModel
                        {
                            _name = file5.FileName,
                            _path = path
                        };
                        db.files.Add(newFile5);
                    }

                    if (newFile1 != null)
                        newpost.img1 = newFile1._path;
                    else
                        newpost.img1 = oldimagepath1;

                    if (newFile2 != null)
                        newpost.img2 = newFile2._path;
                    else
                        newpost.img2 = oldimagepath2;

                    if (newFile3 != null)
                        newpost.img3 = newFile3._path;
                    else
                        newpost.img3 = oldimagepath3;

                    if (newFile4 != null)
                        newpost.img4 = newFile4._path;
                    else
                        newpost.img4 = oldimagepath4;

                    if (newFile5 != null)
                        newpost.img5 = newFile5._path;
                    else
                        newpost.img5 = oldimagepath5;

                    if (cb1 != null) newpost.img1 = null;
                    if (cb2 != null) newpost.img2 = null;
                    if (cb3 != null) newpost.img3 = null;
                    if (cb4 != null) newpost.img4 = null;
                    if (cb5 != null) newpost.img5 = null;


                    db.posts.Update(newpost);
                    await db.SaveChangesAsync();
                    return RedirectToAction("posttable", "apanel");
                }
                else
                {
                    ViewBag.oldimagepath1 = oldimagepath1;
                    ViewBag.oldimagepath2 = oldimagepath2;
                    ViewBag.oldimagepath3 = oldimagepath3;
                    ViewBag.oldimagepath4 = oldimagepath4;
                    ViewBag.oldimagepath5 = oldimagepath5;
                    newpost.img1 = oldimagepath1;
                    newpost.img2 = oldimagepath2;
                    newpost.img3 = oldimagepath3;
                    newpost.img4 = oldimagepath4;
                    newpost.img5 = oldimagepath5;
                    ModelState.AddModelError("", "Данного пользователя не существует");
                }
            }
            return View(newpost);
        }

        public async Task<IActionResult> RequestPostDelete(int? id)
        {
            if (id != null)
            {
                Post post = await db.posts.FirstOrDefaultAsync(predicate => predicate.id == id);
                if (post != null)
                {
                    return View(post);
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> RequestDeletePost(int? id)
        {
            if (id != null)
            {
                Post post = await db.posts.FirstOrDefaultAsync(predicate => predicate.id == id);
                if (post != null)
                {
                    db.posts.Remove(post);
                    await db.SaveChangesAsync();
                    return RedirectToAction("posttable", "apanel");
                }
            }
            return NotFound();
        }
    }
}
