using System;
using Realms;

namespace StitchCalc.Models
{
	public class ProductMaterial : RealmObject
    {
		public ProductMaterial() { }

		public ProductMaterial(Material material)
		{
            MaterialId = material.Id;
			Name = material.Name;
			Price = material.Price;
			Width = material.Width;
		}



        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid MaterialId { get; set; }
        public string Name { get; set; }
		public double Price { get; set; }
		public double Width { get; set; }
		public double Length { get; set; }
	}
}
