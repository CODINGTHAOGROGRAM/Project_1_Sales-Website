using asm.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace asm.Services.FoodSvc
{
    public class FoodService : IFoodService
    {
        private readonly DataContext _dataContext;
        public FoodService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public string CreateFood(FoodItem item)
        {
            try
            {
                _dataContext.FoodItems.Add(item);
                _dataContext.SaveChanges();
                return "Create Successful";
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        public int EditFoodItem(FoodItem foodItem)
        {
            int ret = 0;
            try
            {
                FoodItem _food = _dataContext.FoodItems.FirstOrDefault(f => f.FoodId == foodItem.FoodId);
                if(_food == null)
                {
                    return 0;
                }
                _food.FoodId = foodItem.FoodId;
                _food.FoodName = foodItem.FoodName;
                _food.Price = foodItem.Price;
                _food.KindFood = foodItem.KindFood;
                _food.Img = foodItem.Img;
                _food.Description = foodItem.Description;
                _food.Status = foodItem.Status;
                _dataContext.Update(_food);
                _dataContext.SaveChanges();
                ret = foodItem.FoodId;
            }
            catch
            {
                ret = 0;
            }
            return ret;

        }

        public FoodItem GetFoodItemByID(int id)
        {
            try
            {
                return _dataContext.FoodItems.FirstOrDefault(f => f.FoodId == id);
            }catch (Exception ex)
            {
                return new FoodItem();
            }
        }

        public List<FoodItem> GetListFoodItems()
        {
            try
            {
                return _dataContext.FoodItems.ToList();
            }catch(Exception ex)
            {
                return new List<FoodItem>();
            }
        }

       
    }
}
