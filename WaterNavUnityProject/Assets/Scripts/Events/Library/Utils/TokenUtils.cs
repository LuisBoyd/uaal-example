using System;
using Events.Library.Models;

namespace Events.Library.Utils
{
    public class TokenUtils: ITokenUtils
    {
        public Token GenerateNewToken()
        {
            return new Token() { TokenId = Guid.NewGuid().ToString() };
        }
    }
}