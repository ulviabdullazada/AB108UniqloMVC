﻿namespace AB108Uniqlo.Models
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
