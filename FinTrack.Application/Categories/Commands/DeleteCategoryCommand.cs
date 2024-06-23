﻿using FinTrack.Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Categories.Commands
{
    public class DeleteCategoryCommand : IRequest<CategoryDto>
    {
        public DeleteCategoryCommand(int categoryId)
        {
            CategoryId = categoryId;
        }
        [Required(ErrorMessage = "Category Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
        public int CategoryId { get; set; }
    }
}
