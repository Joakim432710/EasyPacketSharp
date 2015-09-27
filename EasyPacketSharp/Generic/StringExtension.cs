using System;
using System.Linq;
using EasyPacketSharp.Exceptions;

namespace EasyPacketSharp.Generic
{
    public static class StringExtension
    {
        public static readonly char[] AddressSeparators = { '.', ':' };

        public static readonly string[] ProtocolSeparators = { "://" };

        public static readonly char[] HostPortSeparators = { ':', ',' };

        private static readonly char[] BaseCharacters =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 
            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V'
        };

        private static readonly char[] Base32Chars = //RFC 4648
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z', '2', '3', '4', '5',
            '6', '7'
        };

        private static readonly char[] Base64Chars = //RFC2045, Using '+' and '/' according to RFC3548
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
            'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', '+', '/' //Replace + and / with - and _ to match "Base 64 Encoding with URL and Filename Safe Alphabe"
        };

        /// <summary>
        ///     Converts <paramref name="s"/> to a string of base <paramref name="toBase"/> using base <paramref name="fromBase"/>
        /// </summary>
        /// <remarks>
        ///     To do this base conversion the following scheme is used:
        ///     Base  2 - 16: Hexadecimal Scheme (0-9 then A-F)
        ///     Base 17 - 32: RFC4648 Base32 Scheme
        ///     Base 33 - 64: RFC4648 Base64 Scheme
        ///     <see cref="http://tools.ietf.org/html/rfc4648"/>
        /// </remarks>
        /// <param name="s">A string containing numbers to convert</param>
        /// <param name="fromBase">The base of <paramref name="s"/> to convert from</param>
        /// <param name="toBase">The base to convert <paramref name="s"/> to</param>
        /// <exception cref="Exceptions.InvalidCharacterException">
        ///     Thrown when <paramref name="s"/> contains a character not in the standard character set for base <paramref name="fromBase"/>
        ///     For character standards see remarks
        /// </exception>
        /// <returns><paramref name="s"/> converted into base <paramref name="toBase"/></returns>
        public static string ToBase(this string s, uint fromBase, uint toBase)
        {
            var cpy = s;
            var ret = string.Empty;

            char[] fromBuf, toBuf;
            if (fromBase < 17)
                fromBuf = BaseCharacters;
            else if (fromBase < 33)
                fromBuf = Base32Chars;
            else
                fromBuf = Base64Chars;
            if (toBase < 17)
                toBuf = BaseCharacters;
            else if (toBase < 33)
                toBuf = Base32Chars;
            else
                toBuf = Base64Chars;
            var tmp = new char[fromBase];
            Array.Copy(fromBuf, tmp, fromBase);
            fromBuf = tmp; 
            

            if(cpy.Any(c => !fromBuf.Contains(c))) throw new InvalidCharacterException($"The passed string contains an invalid character not part of the standard for base {fromBase}");

            //long longVal = 0;
            //for (var iii = 0; iii < cpy.Length; ++iii)
            //    longVal += Array.IndexOf(fromBuf, cpy[iii]) * (long)Math.Pow(fromBase, cpy.Length - iii - 1);
            var longVal = cpy.Select((t, iii) => Array.IndexOf(fromBuf, t)*(long) Math.Pow(fromBase, cpy.Length - iii - 1)).Sum();

            long curVal = 1;
            while (longVal > curVal)
                curVal *= toBase;
            curVal /= toBase;

            while (curVal != 1)
            {
                long val = 0;
                if (longVal >= curVal)
                {
                    while (longVal >= curVal)
                    { 
                        ++val;
                        longVal -= curVal;
                    }
                    ret += toBuf[val];
                }
                else
                    ret += toBuf[0];
                curVal /= toBase;
            }
            ret += toBuf[longVal];
            return ret;
        }

        public static string ParseIPFromBase(this string ip, uint numberBase)
        {
            var ret = string.Empty;
            var done = false;
            foreach (var separator in AddressSeparators)
            {
                var finalSplit = ip.Split(separator);
                if (done) throw new ParseHostException($"Invalid separator combination detected in {ip}, found multiple separators");

                for (var iii = 0; iii < finalSplit.Length; ++iii)
                {
                    done = true;
                    if (iii != 0)
                        ret += separator;
                    ret += finalSplit[iii].ToBase(numberBase, 10); //TODO: IPv6 doesn't use base 10, what do?
                }
            }
            return ret;
        }
    }
}
