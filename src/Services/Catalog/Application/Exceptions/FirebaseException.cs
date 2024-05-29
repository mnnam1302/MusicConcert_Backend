using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions;

public static class FirebaseException
{
    public class FireBaseAuthenticateException : BadRequestException
    {
        public FireBaseAuthenticateException()
            : base("The firebase authentication was failed.")
        {
        }
    }
}
