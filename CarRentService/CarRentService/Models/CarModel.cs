using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentService.Models
{
    
    public class CarModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Rezerwacja")]
        public string Reservation { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DisplayName("Marka")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z ]*$", ErrorMessage = "Nazwa samochodu nie może zawierać znaków specjalnych, cyfr i musi zaczynać się od dużej litery")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [RegularExpression(@"^[A-Za-z0-9\s ]*$", ErrorMessage = "Niepoprawna nazwa modelu samochodu")]
        public string Model { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DisplayName("Rok produkcji")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Rok produkcji musi składać się z 4 znaków")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Rok może zawierać tylko cyfry")]
        public string YearOfProduction { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DisplayName("Moc (hp)")]
        [StringLength(4, MinimumLength = 2, ErrorMessage = "Moc może mieć maksymalnie 4, a minimum 2 znaki ")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Moc może zawierać tylko cyfry")]
        public string Power { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DisplayName("Cena (zł)")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Cena może zawierać tylko cyfry")]
        public string Price { get; set; }

      


       



    }
}
