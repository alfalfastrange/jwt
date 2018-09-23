using System.Web.Http;
using Jwt.Api.Filters;
using Jwt.Api.ViewModels;
using Jwt.Entity.Entities;
using Jwt.Repository.Interfaces;
using Jwt.Service.Api.Session;

namespace Jwt.Api.Controllers
{
    public class ProfilesController : ApiController
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ISessionService _sessionService;

        public ProfilesController(IProfileRepository profileRepository,
                                  ISessionService sessionService)
        {
            _profileRepository = profileRepository;
            _sessionService = sessionService;
        }

        [HttpGet]
        [Authorize]
        [Route("v1/profiles")]
        public IHttpActionResult Get()
        {
            Profile profile = _profileRepository.GetById(_sessionService.GetAuthenticatedProfileId(Request));
            var model = new ProfileViewModel
            {
                Username = profile.Username,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                FullName = profile.FullName,
                ProfileType = profile.ProfileTypeId.ToString(),
                ProfileStatusType = profile.ProfileStatusTypeId.ToString(),
            };
            return Ok(model);
        }

        [HttpGet]
        [AdminAuthorize]
        [Route("v1/profiles/secure")]
        public IHttpActionResult GetSensitiveData()
        {
            Profile profile = _profileRepository.GetById(_sessionService.GetAuthenticatedProfileId(Request));
            return Ok(profile);
        }
    }
}