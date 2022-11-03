using Library.Data;
using Library.Data.Users;
using Library.Mapping;
using Library.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace AculocityPracticalInterview.Controllers
{
    /// <summary>
    /// Basic controller for front end use
    /// </summary>
    public class UsersController : Controller
    {
        private readonly UsersAPIController _usersAPIController;
        private readonly IMapper<UsersDto, UsersModel> _usersMapper;

        public UsersController(IDataRepository<UsersDto> usersDataRepository, IMapper<UsersDto, UsersModel> usersMapper, UsersAPIController usersAPIController)
        {
            _usersMapper = usersMapper;
            _usersAPIController = usersAPIController;
        }

        // GET: Users
        public async Task<IActionResult> Index(string sortOrder, int direction, string searchValue)
        {
            IEnumerable<UsersDto>? users = (await _usersAPIController.GetUsers()).Value;
            if (users != null)
            {
                #region Search Region
#pragma warning disable CS8602
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    ViewBag.Search = searchValue;
                    users = users.Where(
                        model => model.Name.ToLower().Contains(searchValue) ||
                        model.Surname.ToLower().Contains(searchValue) ||
                        model.Email.ToLower().Contains(searchValue) ||
                        model.Department.ToLower().Contains(searchValue) ||
                        model.SockColour.ToLower().Contains(searchValue));
                }
#pragma warning restore CS8602 
                #endregion

                #region Sorting Region
                //Algorithm can be improved with a beter data structure
                //Could move to different function
                //Should be fine for now, but cleaner code requires moving to another function.
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    ViewBag.Sort = sortOrder;
                    switch (sortOrder)
                    {
                        case "Name":
                            if (direction >= 0)
                            {
                                users = users.OrderBy(model => model.Name);
                                ViewBag.Name = 1;
                            }
                            else
                            {
                                users = users.OrderByDescending(model => model.Name);
                                ViewBag.Name = -1;
                            }
                            break;
                        case "Surname":
                            if (direction >= 0)
                            {
                                users = users.OrderBy(model => model.Surname);
                                ViewBag.Surname = 1;
                            }
                            else
                            {
                                users = users.OrderByDescending(model => model.Surname);
                                ViewBag.Surname = -1;
                            }
                            break;
                        case "Email":
                            if (direction >= 0)
                            {
                                users = users.OrderBy(model => model.Email);
                                ViewBag.Email = 1;
                            }
                            else
                            {
                                users = users.OrderByDescending(model => model.Email);
                                ViewBag.Email = -1;
                            }
                            break;
                        case "Department":
                            if (direction >= 0)
                            {
                                users = users.OrderBy(model => model.Department);
                                ViewBag.Department = 1;
                            }
                            else
                            {
                                users = users.OrderByDescending(model => model.Department);
                                ViewBag.Department = -1;
                            }
                            break;
                        case "CreatedOn":
                            if (direction >= 0)
                            {
                                users = users.OrderBy(model => model.CreatedOn);
                                ViewBag.CreatedOn = 1;
                            }
                            else
                            {
                                users = users.OrderByDescending(model => model.CreatedOn);
                                ViewBag.CreatedOn = -1;
                            }
                            break;
                        case "SockColour":
                            if (direction >= 0)
                            {
                                users = users.OrderBy(model => model.SockColour);
                                ViewBag.SockColour = 1;
                            }
                            else
                            {
                                users = users.OrderByDescending(model => model.SockColour);
                                ViewBag.SockColour = -1;
                            }
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }
            return View(users);
        }

        /// <summary>
        /// Should be implemented to view the details of the user
        /// Out of scope
        /// </summary>
        /// <param name="id">Id of user to view</param>
        /// <returns>New view with the details page</returns>
        [Library.Utils.TODO]
        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _usersAPIController.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Email,Department,CreatedOn,SockColour")] UsersModel users)
        {
            if (ModelState.IsValid)
            {
                await _usersAPIController.InsertUser(_usersMapper.MapForward(users));

                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }
    }
}
