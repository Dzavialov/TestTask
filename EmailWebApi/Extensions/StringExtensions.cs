using System.ComponentModel.DataAnnotations;

namespace EmailWebApi.Extensions
{
    public static class StringEntensions
    {
        public static bool IsValidEmail(this string email)
        {
            var emailValidation = new EmailAddressAttribute();
            return emailValidation.IsValid(email);
        }
    }
}
