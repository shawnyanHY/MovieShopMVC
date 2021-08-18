using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ServiceInterfaces
{
    public interface ICurrentUserService
    {
        public int UserId { get; }
        public bool IsAuthenticated { get; }
        public string Email { get; }
        public string FullName { get; }
        public bool IsAdmin { get; }
        public bool IsSuperAdmin { get; }
        IEnumerable<string> Roles { get; }
    }
}
