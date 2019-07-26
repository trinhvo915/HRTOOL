using Orient.Base.Net.Core.Api.Core.Entities;
using System;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Categories
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {

        }

        public CategoryViewModel(Category category) : this()
        {
            if (category != null)
            {
                Id = category.Id;
                Name = category.Name;
            }
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
