using System.ComponentModel.DataAnnotations;

namespace POIApplication.Entities
{
    public class Point
    {
        public int Id { get; set; }
        [Required(ErrorMessage= "İsim boş olamaz")]
        [StringLength(100, ErrorMessage ="İsim maximum 100 karakter olmalı")]
        public string Name { get; set; }
        [Required(ErrorMessage = "X koordinati girilmeli")]
        [Range(-180, 180, ErrorMessage ="X koordinatı için verilen değer aralık dışı")]
        public int X { get; set; }
        [Required(ErrorMessage = "Y koordinati girilmeli")]
        [Range(-90, 90, ErrorMessage = "Y koordinatı için verilen değer aralık dışı")]
        public int Y { get; set; }
    }
}
