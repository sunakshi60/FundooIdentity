using System.Text.Encodings.Web;

namespace Fundoo.Infrastructure.Email.Templates;

public static class PasswordResetEmailTemplate
{
    public static string Build(string email, string resetToken)
    {
        var safeEmail = HtmlEncoder.Default.Encode(email);
        var safeToken = HtmlEncoder.Default.Encode(resetToken);

        return $"""
            <p>We received a request to reset the password for the Fundoo account associated with {safeEmail}.</p>
            <p>Use this reset token in the Fundoo password reset request:</p>
            <p><code>{safeToken}</code></p>
            <p>If you did not request a password reset, you can ignore this email.</p>
            """;
    }
}
