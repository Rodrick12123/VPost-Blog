﻿namespace Blog.Models.ViewModels
{
    public class ViewUsers
    {
        public List<User> Users { get; set; }
        public string UserName { get; set; }
        public  string Email { get; set; }
        public string Password { get; set; }
        public bool AdminCheckbox { get; set; }
    }
}
