﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace mywebsite.Models.Products
{
    public class Keyboard
    {
        [Key] // Первинний ключ
        [ForeignKey("Product")] // Зв'язок із таблицею Product
        public int Id { get; set; }

        public Product Product { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Layout { get; set; }
        [Required]
        public bool RGB { get; set; }
        [Required]
        public bool HotSwap {  get; set; }
        [Required]
        public string Connection {  get; set; }
        [Required]
        public string KeycapsProfile { get; set; }
        [Required]
        public string CaseMaterial {  get; set; }
        [Required]
        public string Mount {  get; set; }
        [Required]
        public string SwitchType { get; set; }
    }
}
