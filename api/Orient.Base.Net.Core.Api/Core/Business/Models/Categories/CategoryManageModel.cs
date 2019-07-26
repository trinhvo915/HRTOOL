using Orient.Base.Net.Core.Api.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Categories
{
    public class CategoryManageModel
    {
        [Required]
        public string Name { get; set; }

        public void SetDateToModel(Category category)
        {
            category.Name = Name;
        }
    }
}
