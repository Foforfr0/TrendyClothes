using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Utilities
{
    public  static class Constants
    {
        public static readonly string DEFAULT_PROFILE_PIC_PATH = "/Resources/Images/default-profile-pic.png";

        public static readonly long MAX_IMAGE_SIZE = 5 * 1024 * 1024;
        public static readonly int MAX_LENGTH_PHONENUMBER = 10;
        public static readonly int MAX_LENGTH_NAMES = 32;
        public static readonly int MAX_LENGTH_EMAIL = 32;
        public static readonly int MAX_LENGTH_ADDRESSNAME = 64;
        public static readonly int MAX_LENGTH_CITY = 16;
        public static readonly int MAX_LENGTH_ZIPCODE = 5;
        public static readonly int MAX_LENGTH_USERNAME = 16;
        public static readonly int MAX_LENGTH_PASSWORD = 24;
        public static readonly int MAX_LENGTH_DESCRIPTION = 128;
        public static readonly int MAX_LENGTH_MONETARY_VALUE = 9;
        public static readonly decimal MAX_MONETARY_VALUE = 9999.99m;

        public static readonly string IMAGE_FILE_FILTER = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

        public static readonly int UNIT_ID_PIECES = 3;
        public static readonly string MONETARY_DECIMAL_PATTERN = $@"^(\d{{0,3}})(\.\d{{0,3}})?$";

        #region STRING-PATTERNS
        public static readonly string NUMERIC_PATTERN = @"[^0-9]";
        public static readonly string ALPHANUMERIC_PATTERN = @"[^a-zA-Z0-9]";
        public static readonly string NAMES_PATTERN = @"[^A-Za-zÁÉÍÓÚÜÑáéíóúüñ.' -]";
        public static readonly string GENERAL_TEXT_PATTERN = @"[^A-Za-zÁÉÍÓÚÜÑáéíóúüñ0-9\s\-\.,()/#]+$";
        public static readonly string EMAIL_ALLOWED_CHARS_PATTERN = @"[^a-zA-Z0-9@._\-+]";
        public static readonly string SAFE_PASSWORD_PATTERN = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{6,}$";
        public static readonly string EMAIL_FORMAT_PATTERN = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public static readonly string MONETARY_VALUE_PATTERN = @"[^$0-9,.]";
        public static readonly string QUANTITY_VALUE_PATTERN = @"[^0-9,.]";
        #endregion
    }
}
