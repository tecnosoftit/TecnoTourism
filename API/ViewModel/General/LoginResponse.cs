using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.General
{
    public class LoginResponse
    {
        public User User { get; set; }
        public List<Roles> Roles { get; set; }
        public bool Valid { get; set; }
    }
}
