using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Exceptions
{
    public class UserCreateExceptions : Exception
    {
        public UserCreateExceptions() : base("Kullanıcı oluşturulurken hata oluştu")
        {
            
        }
        public UserCreateExceptions(string message) : base(message)
        {

        }
        public UserCreateExceptions(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
