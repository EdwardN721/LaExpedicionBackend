using FluentValidation;
using LaExpedicion.Application.DTOs.Peticion;

namespace LaExpedicion.Application.Validations;

public class UsuarioValidator : AbstractValidator<CrearUsuarioDto>
{
    public UsuarioValidator()
    {
        RuleFor(u => u.Nombre)
            .NotEmpty().WithMessage("Nombre no puede estar vacio")
            .MinimumLength(3).WithMessage("El nombre es demasiado pequeño.")
            .MaximumLength(20).WithMessage("El nombre es demasiado largo.");
        
        RuleFor(u => u.PrimerApellido)
            .NotEmpty().WithMessage("El primer apellido no puede estar vacio")
            .MinimumLength(3).WithMessage("El primer apellido es demasiado pequeño.")
            .MaximumLength(20).WithMessage("El primer apellido es demasiado largo.");
        
        RuleFor(u => u.SegundoApellido)
            .MaximumLength(20).WithMessage("El segundo apellido es demasiado largo.");
        
        RuleFor(u => u.Correo)
            .NotEmpty().WithMessage("El correo no puede estar vacio")
            .EmailAddress().WithMessage("El formato del correo no es válido.");
        
        RuleFor(u => u.Telefono)
            .MaximumLength(10).WithMessage("El Telefono es demasiado largo.");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("El password no puede estar vacio")
            .MinimumLength(8).WithMessage("El password debe tener al menos 8 caracteres.");
        
    }
}