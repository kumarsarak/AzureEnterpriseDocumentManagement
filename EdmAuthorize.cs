using System; 
using System.Collections.Generic; 
using System.Configuration; 
using System.Diagnostics; 
using System.Linq; 
using System.Web; 
using System.Web.Mvc; 
using System.Web.Security;


namespace WebRole1
{
    public class EdmAuthorize : AuthorizeAttribute
    {
        /*private readonly string[] allowedroles;
        public EdmAuthorize(params string[] roles)  
        {
            this.allowedroles = roles;  
        }  

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            System.Security.Claims.ClaimsIdentity claimsIdentity = httpContext.User.Identity as System.Security.Claims.ClaimsIdentity;
            //get all roles of user from claims token
            String userRoles = claimsIdentity.Claims.First(x => x.Type.ToLower().Contains("group")).Value;
            String upn = claimsIdentity.Claims.First(x => x.Type.ToLower().Contains("upn")).Value;
            //compare the available roles to roles of the user
            foreach (var role in allowedroles)
            {
                string[] rolegroups = ConfigurationManager.AppSettings[role].Split(',').Select(s => s.Trim()).ToArray();
                foreach (String rolegroup in rolegroups)
                {
                    if ((userRoles.Contains(rolegroup))
                    || (upn.Contains(rolegroup)))
                    {
                        authorize = true;
                    }
                }
  
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Home/Unauthorized");
        }  */


    }
}