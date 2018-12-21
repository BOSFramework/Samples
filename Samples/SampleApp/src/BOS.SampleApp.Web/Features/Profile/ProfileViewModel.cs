using BOS.SampleApp.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BOS.SampleApp.Web.Features.Profile
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
        public string DisplayName { get; set; }
        public DateTimeOffset? Birthdate { get; set; }

        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public string BirthDateString { get; set; }

        public string Gender { get; set; }
        public string Email { get; set; }
        public string EmailRecord { get; set; }
    }
}
