using FluentValidation;
using NotePool.Application.ViewModels.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Validators.Notes
{
    public class CreateNoteValidator : AbstractValidator<VM_Create_Note>
    {
        public CreateNoteValidator() 
        {
            RuleFor(n => n.Title)
                .NotEmpty()
                .NotNull()
                     .WithMessage("Not Adını Boş Geçmeyiniz")
                .MaximumLength(100)
                .MinimumLength(5)
                      .WithMessage("Not Adını 5-100 Arası Giriniz");

            RuleFor(n => n.Description)
                .MaximumLength(500)
                     .WithMessage("Açıklama 500 Karakteri Geçemez");

            RuleFor(n => n.Tags)
                .MaximumLength(150)
                     .WithMessage("Etiketler 150 Karakteri Geçemez");

            RuleFor(n => n.CourseId)
                .NotEmpty().WithMessage("Ders Seçiniz");
            RuleFor(n => n.InstitutionId)
                .NotEmpty().WithMessage("Kurum Seçiniz");
            RuleFor(n => n.UserId)
                .NotEmpty().WithMessage("Kullanıcı Bilgisi Eksik Olamaz");
        }
    }
}
