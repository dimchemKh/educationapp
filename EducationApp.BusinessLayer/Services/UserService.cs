using System.Threading.Tasks;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EducationApp.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userRepository = userRepository;
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync()
        {
            ApplicationUser user = new ApplicationUser();

            IdentityResult result = await _userManager.CreateAsync()
            
        }
    }
}
