﻿using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<CategoryDto>
    {
        public CreateCategoryCommand(string title, int iconId, TransactionType type)
        {
            Title = title;
            IconId = iconId;
            Type = type;
        }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(20, ErrorMessage = "Title cannot exceed 20 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Icon Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Icon Id must be greater than 0")]
        public int IconId { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public TransactionType Type { get; set; }
    }
}
