using Microsoft.AspNetCore.Mvc;
using rpmDEL.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace rpmDEL.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        CookieOptions cookie = new CookieOptions();
        private IWebHostEnvironment _app;
        public HttpContext _httpContext;
        public HomeController(ApplicationContext context, IWebHostEnvironment app)
        {
            db = context;
            _app = app;
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            if (file != null)
            {
                string path = "/files/" + file.FileName;
                using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                FileModel newFile = new FileModel
                {
                    _name = file.FileName,
                    _path = path
                };
                db.files.Add(newFile);
                await db.SaveChangesAsync();
                return RedirectToAction("login");
            }
            return NotFound();
        }

        public IActionResult AddFile()
        {
            return View(db.files.ToList());
        }

        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Profile()
        {
            IQueryable<user> users = db._user;
            var item = await users.ToListAsync();

            IQueryable<Post> posts = db.posts;
            var item1 = await posts.ToListAsync();
            ProfileViewModel profileViewModel = new ProfileViewModel
            {
                users = item,
                posts = item1
            };

            return View(profileViewModel);
        }

        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> RequestProfile(int? id)
        {
            IQueryable<user> users = db._user;
            var item = await users.ToListAsync();

            IQueryable<Post> posts = db.posts;
            var item1 = await posts.ToListAsync();
            ProfileViewModel profileViewModel = new ProfileViewModel
            {
                users = item,
                posts = item1
            };

            ViewBag.ShowUserID = id;
            return View(profileViewModel);
        }

        public async Task<IActionResult> ProfileEdit(int? id, string oldimagepath)
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

        [HttpPost]
        public async Task<IActionResult> ProfileEdit(user curruser, IFormFile file, string oldlogin, string oldimagepath)
        {
            if (ModelState.IsValid)
            {
                FileModel newFile = null;
                if (db._user.AsNoTracking().FirstOrDefaultAsync(u => u._login == curruser._login) == null || curruser._login == oldlogin)
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
                    return RedirectToAction("profile", "home");
                }
                else
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует!");
            }
            curruser._imgpath = oldimagepath;
            return View(curruser);
        }

        public async Task<IActionResult> PostEdit(int? id)
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

        [HttpPost]
        public async Task<IActionResult> PostEdit(Post newpost, IFormFile file1, IFormFile file2, IFormFile file3, IFormFile file4, IFormFile file5, string oldimagepath1, string oldimagepath2, string oldimagepath3, string oldimagepath4, string oldimagepath5, string cb1, string cb2, string cb3, string cb4, string cb5)
        {
            if (ModelState.IsValid)
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
                return RedirectToAction("profile", "home");
            }
            return View(newpost);
        }

        public async Task<IActionResult> PostDelete(int? id)
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

        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id != null)
            {
                Post post = await db.posts.FirstOrDefaultAsync(predicate => predicate.id == id);
                if (post != null)
                {
                    db.posts.Remove(post);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Profile", "home");
                }
            }
            return NotFound();
        }

        public IActionResult PostAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostAdd(Post post, int? userid, IFormFile file1, IFormFile file2, IFormFile file3, IFormFile file4, IFormFile file5)
        {
            post.UserID = Convert.ToInt32(userid);

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
            return RedirectToAction("profile", "home");
        }

        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> main(int? Id, string lgn, string email, SortState sortOrder = SortState.IdAsc, int page = 1)
        {
            var allusers = db._user.ToList<user>();

                    IQueryable<user> users = db._user;
                    //Фильтрация или поиск
                    if (Id != null && Id > 0)
                    {
                        users = users.Where(p => p._ID == Id);
                    }

                    if (!String.IsNullOrEmpty(lgn))
                    {
                        users = users.Where(p => p._login.Contains(lgn));
                    }
                    //Сортировка
                    switch (sortOrder)
                    {
                        case SortState.IdAsc:
                            {
                                users = users.OrderBy(m => m._ID);
                                break;
                            }

                        case SortState.IdDesc:
                            {
                                users = users.OrderByDescending(m => m._ID);
                                break;
                            }

                        case SortState.EmailAsc:
                            {
                                users = users.OrderBy(m => m._EMail);
                                break;
                            }

                        case SortState.EmailDesc:
                            {
                                users = users.OrderByDescending(m => m._EMail);
                                break;
                            }


                        case SortState.LoginAsc:
                            {
                                users = users.OrderBy(m => m._login);
                                break;
                            }

                        case SortState.LoginDesc:
                            {
                                users = users.OrderByDescending(m => m._login);
                                break;
                            }
                        default:
                            {
                                users = users.OrderBy(m => m._ID);
                                break;
                            }
                    }

                    //Пагинация
                    int pagesize = 10;
                    var count = await users.CountAsync();
                    var item = await users.Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();

                    LoginViewModel loginViewModel = new LoginViewModel
                    {
                        PageViewModel = new PageViewModel(count, page, pagesize),
                        SortViewModel = new SortViewModel(sortOrder),
                        FilterViewModel = new FilterViewModel(Id, lgn, email),
                        users = item
                    };

                    return View(loginViewModel);
        }

        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> UserList(string lgn, string email, SortState sortOrder = SortState.IdAsc, int page = 1)
        {
            var allusers = db._user.ToList<user>();

            IQueryable<user> users = db._user;
            //Фильтрация или поиск

            if (!String.IsNullOrEmpty(lgn))
            {
                users = users.Where(p => p._login.Contains(lgn));
            }
            //Сортировка
            switch (sortOrder)
            {
                case SortState.EmailAsc:
                    {
                        users = users.OrderBy(m => m._EMail);
                        break;
                    }

                case SortState.EmailDesc:
                    {
                        users = users.OrderByDescending(m => m._EMail);
                        break;
                    }


                case SortState.LoginAsc:
                    {
                        users = users.OrderBy(m => m._login);
                        break;
                    }

                case SortState.LoginDesc:
                    {
                        users = users.OrderByDescending(m => m._login);
                        break;
                    }
                default:
                    {
                        users = users.OrderBy(m => m._ID);
                        break;
                    }
            }

            //Пагинация
            int pagesize = 10;
            var count = await users.CountAsync();
            var item = await users.Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();

            LoginViewModel loginViewModel = new LoginViewModel
            {
                PageViewModel = new PageViewModel(count, page, pagesize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(null, lgn, email),
                users = item
            };

            return View(loginViewModel);
        }
    }
}
