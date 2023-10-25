using System.Security.Claims;

namespace Marina.UI.Models
{
    public class MyExtension
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public MyExtension(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string SetDbName()
        {
            var user = _contextAccessor.HttpContext.User.Identities;///FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var name = user.FirstOrDefault().Name;

            //ClaimsPrincipal User = new ClaimsPrincipal();
            //ClaimsPrincipal currentUser = User;
            //ClaimsIdentity currentIdentity = currentUser.Identity as ClaimsIdentity;
            //string firstName = currentIdentity.Name;
            //string lastName = currentIdentity.FindFirst("LastName")?.Value;
            string str = "";
            var date = DateTime.Now.ToString("yyyy/MM/dd/HH:mm");
            var dateStr = date.Replace("/", "").Replace(":", "");
            //var firstName = user.Claims.FirstOrDefault(c => c.Type == "FirstName");
            //var lastName = user.Claims.FirstOrDefault(c => c.Type == "LastName");
            str = $"{dateStr}_{name}";
            return str;

        }
    }
}
