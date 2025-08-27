namespace Common.Helpers
{
    public static class PasswordEncryption
    {
        public static string EncryptedPassword(string? password)
        {
            if (password == null)
            {
                return "Please Enter Password inorder to encrypt it.";
            }
            var EncryptPassword = BCrypt.Net.BCrypt.HashPassword(password);
            if (EncryptPassword != null)
            {
                return EncryptPassword;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
