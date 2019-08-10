using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceW.Data;
using FinanceW.Models;
using FinanceW.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Collections.Generic;
using System;

namespace FinanceW.Controllers
{
    public class UsersClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersClaimsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ApplicationUsers
        public async Task<IActionResult> Index()
        {
            //Verificar acceso de usuario
            var result = UserClaimAccess();
            if (result != String.Empty)
            {
                return View(result);
            }

            List<UserClaimsViewModel> userclaimsView = new List<UserClaimsViewModel>();

            foreach (var uc in _context.UserClaims)
            {
                var _user = await _userManager.FindByIdAsync(uc.UserId);
                var ucv = new UserClaimsViewModel { Id = uc.Id, ClaimType = uc.ClaimType, UserName = _user.UserName, UserId = _user.Id };
                userclaimsView.Add(ucv);
            }

            return View(userclaimsView.ToList());
        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var uc = await _context.UserClaims.SingleOrDefaultAsync(u => u.Id == id);

            if (uc == null)
            {
                return NotFound();
            }

            var _user = await _userManager.FindByIdAsync(uc.UserId);

            return View(new UserClaimsViewModel { Id = uc.Id, UserName = _user.UserName, UserId = _user.Id, ClaimType = uc.ClaimType, ClaimValue = uc.ClaimValue });
        }

        // GET: ApplicationUsers/Create
        public IActionResult Create()
        {
            IList<ApplicationUser> list = _context.ApplicationUser.ToList();
            return View(new UserClaimsViewModel { UserList = list });
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaimType,ClaimValue,UserId")]  UserClaimsViewModel userclaims)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var _user = await _userManager.FindByIdAsync(userclaims.UserId);
                    var _claim = new Claim(userclaims.ClaimType, userclaims.ClaimValue);

                    await _userManager.AddClaimAsync(_user, _claim);
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            IList<ApplicationUser> list = _context.ApplicationUser.ToList();
            return View(new UserClaimsViewModel { UserList = list });
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var uc = await _context.UserClaims.SingleOrDefaultAsync(u => u.Id == id);

            if (uc == null)
            {
                return NotFound();
            }

            var _user = await _userManager.FindByIdAsync(uc.UserId);

            return View(new UserClaimsViewModel { Id = uc.Id, UserName = _user.UserName, UserId = _user.Id, ClaimType = uc.ClaimType, ClaimValue = uc.ClaimValue });
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClaimType,ClaimValue,UserId")] UserClaimsViewModel userclaims)
        {
            if (id != userclaims.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var _claim = await _context.UserClaims.SingleOrDefaultAsync(m => m.Id == id);
                    var _claimOld = new Claim(_claim.ClaimType, _claim.ClaimValue);

                    var _user = await _userManager.FindByIdAsync(userclaims.UserId);
                    var _claimNew = new Claim(userclaims.ClaimType, userclaims.ClaimValue);

                    //await _userManager.AddClaimAsync(user, _claim);

                    await _userManager.ReplaceClaimAsync(_user, _claimOld, _claimNew);
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            return View(userclaims);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var uc = await _context.UserClaims.SingleOrDefaultAsync(u => u.Id == id);

            if (uc == null)
            {
                return NotFound();
            }

            var _user = await _userManager.FindByIdAsync(uc.UserId);

            return View(new UserClaimsViewModel { Id = uc.Id, UserName = _user.UserName, UserId = _user.Id, ClaimType = uc.ClaimType, ClaimValue = uc.ClaimValue });
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var _uClaim = await _context.UserClaims.SingleOrDefaultAsync(m => m.Id == id);
            _context.UserClaims.Remove(_uClaim);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }

        private String UserClaimAccess()
        {
            if (!(User.Claims.Any(c => c.Type == "ADMIN" && c.Value == "X")))
            {
                return "~/Views/Shared/AccessDenied.cshtml";
            }

            return String.Empty;
        }
    }
}
