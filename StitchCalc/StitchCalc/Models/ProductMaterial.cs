using System;
using Realms;

namespace StitchCalc.Models
{
	public class ProductMaterial : RealmObject, IGuidId
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
		public string StringId { get; set; } = Guid.NewGuid().ToString();

        public Product Product { get; set; }
        public string StringMaterialId { get; set; }
        public string Name { get; set; }
		public double Price { get; set; }
		public double Width { get; set; }
		public double Length { get; set; }


		public Guid Id { get { return Guid.Parse(StringId); } set { StringId = value.ToString(); } }
		public Guid MaterialId { get { return Guid.Parse(StringMaterialId); } set { StringMaterialId = value.ToString(); } }
	}
}
