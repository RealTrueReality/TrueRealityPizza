using Microsoft.AspNetCore.Components.Forms;

namespace TrueRealityPizza.Shared;

public interface IManagePizzaRepository {
    Task<List<PizzaSpecial?>> GetPizzaSpecialsAsync();
    Task<PizzaSpecial> GetPizzaSpecialByIdAsync(int id);
    Task AddPizzaSpecialAsync(PizzaSpecial? pizza);
    Task UpdatePizzaSpecialAsync(PizzaSpecial? pizza);
    Task DeletePizzaSpecialAsync(int pizzaId);
    Task<string> UploadImageAsync(IBrowserFile imageFile, string pizzaName, bool isEdit, string existingImageUrl = "");
}