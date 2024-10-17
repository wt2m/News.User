﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities.Abstract;

namespace UserService.Domain.Entities
{
    public class User : PersistentData
    {
        
        public string Email { get; private set; }
        public string FullName { get; private set; }
        public string UserName { get; set; }
        public bool IsActive { get; private set; }

        public User(string email, string fullName, string userName)
        {
            
            Email = email;
            FullName = fullName;
            UserName = userName;
            IsActive = false;
        }

        public void ActivateUser()
        {
            IsActive = true;
        }
    }
}