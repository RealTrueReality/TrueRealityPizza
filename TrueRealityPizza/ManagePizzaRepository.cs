using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TrueRealityPizza
{
    public class ManagePizzaRepository : IManagePizzaRepository
    {
        private readonly PizzaStoreContext _context;
        private readonly IWebHostEnvironment _environment;

        public ManagePizzaRepository(PizzaStoreContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<List<PizzaSpecial?>> GetPizzaSpecialsAsync()
        {
            return await _context.Specials.ToListAsync();
        }

        public async Task<PizzaSpecial> GetPizzaSpecialByIdAsync(int id)
        {
            return await _context.Specials.FindAsync(id);
        }

        public async Task AddPizzaSpecialAsync(PizzaSpecial? pizza)
        {
            _context.Specials.Add(pizza);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePizzaSpecialAsync(PizzaSpecial? pizza)
        {
            _context.Specials.Update(pizza);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePizzaSpecialAsync(int pizzaId)
        {
            var pizza = await _context.Specials.FindAsync(pizzaId);
            if (pizza != null)
            {
                _context.Specials.Remove(pizza);
                await _context.SaveChangesAsync();
            }
        }
        
        // 实现图片上传方法
        public async Task<string> UploadImageAsync(IBrowserFile imageFile, string pizzaName, bool isEdit, string existingImageUrl = "")
        {
            // 确定图片上传的目录
            var pizzasFolder = Path.Combine(_environment.WebRootPath, "img", "pizzas");

            // 确保目录存在
            if (!Directory.Exists(pizzasFolder))
            {
                Directory.CreateDirectory(pizzasFolder);
            }

            // 获取上传图片的扩展名
            var extension = Path.GetExtension(imageFile.Name);
            if (string.IsNullOrEmpty(extension))
            {
                throw new ArgumentException("Invalid image file extension");
            }

            // 生成新文件名（使用披萨名）
            var newFileName = $"{pizzaName}{extension}";
            var newFilePath = Path.Combine(pizzasFolder, newFileName);

            // 如果是编辑模式且现有图片存在，则覆盖现有图片
            if (isEdit && !string.IsNullOrEmpty(existingImageUrl))
            {
                var existingFilePath = Path.Combine(_environment.WebRootPath, existingImageUrl.Replace("/", "\\"));
                if (File.Exists(existingFilePath))
                {
                    File.Delete(existingFilePath);  // 删除现有图片
                }
            }
            else if (File.Exists(newFilePath))
            {
                // 添加新披萨时如果文件名冲突，生成唯一文件名
                newFileName = $"{pizzaName}_{Guid.NewGuid()}{extension}";
                newFilePath = Path.Combine(pizzasFolder, newFileName);
            }

            // 使用 FileStream 保存文件
            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await imageFile.OpenReadStream().CopyToAsync(stream);
            }

            // 返回图片的相对路径
            return $"/img/pizzas/{newFileName}";
        }
    }
}
