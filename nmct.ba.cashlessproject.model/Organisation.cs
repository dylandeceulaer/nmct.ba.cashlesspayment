using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Organisation
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Login is verplicht")]
        [MaxLength(50, ErrorMessage="Mag maximum 50 karakters bevatten.")]
        [MinLength(2, ErrorMessage="Moet minimum 2 karakters bevatten")]
        [DisplayName("Login")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [MaxLength(50, ErrorMessage = "Mag maximum 50 karakters bevatten.")]
        [MinLength(4, ErrorMessage = "Moet minimum 4 karakters bevatten")]
        [DisplayName("Wachtwoord")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Database naam is verplicht")]
        [MaxLength(50, ErrorMessage = "Mag maximum 50 karakters bevatten.")]
        [MinLength(2, ErrorMessage = "Moet minimum 2 karakters bevatten")]
        [DisplayName("Database naam")]
        public string DbName { get; set; }
        [Required(ErrorMessage = "Database Login is verplicht")]
        [MaxLength(50, ErrorMessage = "Mag maximum 50 karakters bevatten.")]
        [MinLength(2, ErrorMessage = "Moet minimum 2 karakters bevatten")]
        [DisplayName("Database Login")]
        public string DbLogin { get; set; }
        [Required(ErrorMessage = "Database wachtwoord is verplicht")]
        [MaxLength(50, ErrorMessage = "Mag maximum 50 karakters bevatten.")]
        [MinLength(4, ErrorMessage = "Moet minimum 4 karakters bevatten")]
        [DisplayName("Database wachtwoord")]
        public string DbPassword { get; set; }
        [Required(ErrorMessage = "Organisatienaam is verplicht")]
        [MaxLength(50, ErrorMessage = "Mag maximum 50 karakters bevatten.")]
        [MinLength(2, ErrorMessage = "Moet minimum 2 karakters bevatten")]
        [DisplayName("Organisatienaam")]
        public string OrganisationName { get; set; }
        [Required(ErrorMessage = "Adres is verplicht")]
        [MaxLength(100, ErrorMessage = "Mag maximum 100 karakters bevatten.")]
        [MinLength(10, ErrorMessage = "Moet minimum 10 karakters bevatten")]
        [DisplayName("Adres")]
        public string Address { get; set; }
        [MaxLength(50, ErrorMessage = "Mag maximum 50 karakters bevatten.")]
        [MinLength(6, ErrorMessage = "Moet minimum 6 karakters bevatten")]
        [Required(ErrorMessage = "Email is verplicht")]
        [DisplayName("Email")]
        public string Email { get; set; }
        [MaxLength(15, ErrorMessage = "Mag maximum 15 karakters bevatten.")]
        [MinLength(2, ErrorMessage = "Moet minimum 2 karakters bevatten")]
        [Required(ErrorMessage = "Telefoonnumer is verplicht")]
        [DisplayName("Telefoonnummer")]
        public string Phone { get; set; }
    }
}
