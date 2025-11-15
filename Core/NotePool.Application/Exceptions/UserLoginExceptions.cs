using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Exceptions
{
    public class UserLoginExceptions : Exception
    {
        public UserLoginExceptions(): base("Kullanıcı Adı, Mail veya Şifre Hatalı") 
        {

        }

        public UserLoginExceptions(string? message) : base(message)
        {

        }

        public UserLoginExceptions(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
