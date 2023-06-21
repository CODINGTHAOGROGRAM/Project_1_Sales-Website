using asm.Models;

namespace asm.Services.FoodSvc
{
    public interface IFoodService
    {
        public List<FoodItem> GetListFoodItems();
        public FoodItem GetFoodItemByID(int id);
        public int EditFoodItem( FoodItem foodItem);
        public string CreateFood(FoodItem item);
    }
}
