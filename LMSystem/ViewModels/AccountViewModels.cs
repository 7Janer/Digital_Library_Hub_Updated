using System.ComponentModel.DataAnnotations;

namespace LMSystem.ViewModels;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Username or Email")]
    public string? UsernameOrEmail { get; set; }

    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}

public class RegisterViewModel
{
    [Required, StringLength(100)]
    [Display(Name = "Full Name")]
    public string? FullName { get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required, StringLength(50)]
    public string? Username { get; set; }

    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    [Display(Name = "Confirm Password")]
    public string? ConfirmPassword { get; set; }
}

public class ForgotPasswordViewModel
{
    [Required, EmailAddress]
    public string? Email { get; set; }
}

public class ResetPasswordViewModel
{
    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Token { get; set; }

    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    [Display(Name = "Confirm Password")]
    public string? ConfirmPassword { get; set; }
}

public class ProfileViewModel
{
    [Required, StringLength(100)]
    [Display(Name = "Full Name")]
    public string? FullName { get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }
}

public class ChangePasswordViewModel
{
    [Required, DataType(DataType.Password)]
    [Display(Name = "Current Password")]
    public string? CurrentPassword { get; set; }

    [Required, DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string? NewPassword { get; set; }

    [Required, DataType(DataType.Password), Compare(nameof(NewPassword))]
    [Display(Name = "Confirm New Password")]
    public string? ConfirmPassword { get; set; }
}

public class UserAdminViewModel
{
    public required LMSystem.Models.ApplicationUser User { get; set; }
    public string Role { get; set; } = "Member";
}
