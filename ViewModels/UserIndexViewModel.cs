// ViewModels/UserIndexViewModel.cs
using OIMRF.Models;
using System.Collections.Generic;

namespace OIMRF.ViewModels
{
    public class UserIndexViewModel
    {
        public List<User> Users { get; set; }
        public CreateUserViewModel NewUser { get; set; }
    }
}
