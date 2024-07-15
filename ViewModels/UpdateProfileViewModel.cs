﻿namespace MyField.ViewModels
{
    public class UpdateProfileViewModel
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Email { get; set; }
        public IFormFile? ProfilePictureFile { get; set; }
    }
}
