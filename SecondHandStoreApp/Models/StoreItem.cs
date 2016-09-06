using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondHandStoreApp.Models
{
    public class StoreItem
    {
        public StoreItem()
        {
            Images = new List<MyImage>();
            HelperImagePaths = new List<string>();
        }
        public int ID { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsApproved { get; set; }
        public bool IsFinished { get; set; }
        public bool isSold { get; set;}

        [Required]
        public string ItemName { get; set; }
        [Required]
        public int Price { get; set; }

        [Required]
        public Gender itemGender { get; set; }
        [Required]
        public Category category { get; set; }
        public SubcategoryClothes subcategoryClothes { get; set; }
        public SubcategoryBags subcategoryBags { get; set; }
        public SubcategoryShoes subcategoryShoes { get; set; }
        public SubcategoryAccessories subcategoryAccessories { get; set; }

        public virtual ICollection<MyImage> Images { get; set; }
        public List<string> HelperImagePaths { get; set; }
        public string Description { get; set; }
        public Condition condition { get; set; }
        public Material material { get; set; }
        public Size? size { get; set; }
        public ShoeSize? shoeSize { get; set; }
        public double? heelSize { get; set; }
        public string  Brand { get; set; }

        public int? length { get; set; }
        public int? width { get; set; }

        public virtual int? SellerId { get; set; }
        public virtual Seller seller { get; set; }

        public virtual ICollection<MyUser> UsersCarts { get; set; }

        public virtual int? orderId { get; set; }
        public virtual Delivery order { get; set; }

    }

    public enum Gender
    {
        Male,
        Female 
    }

    public enum Category
    {
        Clothes,
        Bags,
        Shoes,
        Accessories
    }

    public enum SubcategoryClothes
    {
        Blazers,
        Dresses,
        JacketAndCoat,
        Jeans,
        Jumpsuit,
        Knitwear,
        Shorts,
        Skirts,
        Suits,
        Tops,
        Trousers,
        Beachwear
    }

    public enum SubcategoryBags
    {
        Backpacks,
        Wallets,
        Bag,

    }

    public enum SubcategoryShoes
    {
        Trainers,
        Sandals,
        Heels,
        Boots,
        Shoes,
    }

    public enum SubcategoryAccessories
    {
        Belts,
        Glasses,
        Hats,
        Scarves,
        Sunglasses,
        Watches,
        MobilePhoneCases,
        Jewellery
    }

    public enum Condition
    {
        LikeNew,
        VeryGood,
        Good,
        Acceptable
    }
    public enum Colour
    {
        White,
        Black,
        Gray,
        Brown,
        Beige,
        Cream,
        Red,
        Bordeaux,
        Pink, 
        Violet,
        Orange,
        Nude,
        Blue,
        Turquoise,
        Petrol,
        Green,
        Olive,
        Khaki,
        Taupe,
        Yellow,
        Gold,
        Silvery,
        Pattern,
        BlackAndWhite,
        Other
    }


    public enum Material
    {
        Cotton, Flax, Wool, Ramie, Silk, Linen,
        Denim, Leather, Fur, Nylon, Polyesters, Spandex, Other
    }

    public enum Size
    {
        XXS, XS, S, M, L, XL, XXL, XXXL,
        EU32, EU34, EU38, EU40, EU42
    }

    public enum ShoeSize
    {
        EU36, EU37, EU38, EU39, EU40, EU41, EU42, EU43, EU44, EU45, EU46, EU47,
        US4, US45, US5, US55, US6, US65, US7, US75, US8, US85, US9, US95, US10, US105, US11, US115, US12
    }
}
