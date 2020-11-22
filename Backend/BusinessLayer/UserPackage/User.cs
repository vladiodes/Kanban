using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    class User
    {

        private const int minPassLength = 5;
        private const int maxPassLength = 25;
        private const int minNum = 1;
        private const int minUpper = 1;
        private const int minLower = 1;
        //fields
        private string email;
        public string Email { get => email;
            set {
                if (!EmailVerify(value)) throw new ApplicationException("Email is illegal");
                email = value;
            }
        }

        private string nickname;
        public string Nickname { get => nickname;
            set
            {
                if (value == null || value.Equals(""))
                    throw new ApplicationException("Can't insert empty nickname");
                nickname = value;
            }
        }

        private string password;
        private string Password { get => password;
            set
            {
                if (!PassVerify(value)) throw new ApplicationException($"Illegal password, the password must contain {minPassLength}-{maxPassLength} characters and include {minUpper} uppercase, {minLower} lower case and {minNum} number");
                password = value;
            }
        }

        private int userId = -1;
        public int ID { get => userId; }

        private UserDAL dalUser;
        public UserDAL DalUser{get=> dalUser;}

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="nickname">nickname</param>
        /// <param name="password">password</param>
        public User(string email, string nickname, string password)
        {
            Email = email;
            Nickname = nickname;
            Password = password;
            dalUser = new UserDAL(email, nickname, password);
            userId = (int)dalUser.Id;
        }

        /// <summary>
        /// Sets the board id of this user
        /// </summary>
        /// <param name="boardid">Board id</param>
        public void setBoardId(int boardid)
        {
            DalUser.BoardId = boardid;
        }

        /// <summary>
        /// Simple constructor, will be used when getting info from database
        /// </summary>
        /// <param name="dalUser"></param>
        public User(UserDAL dalUser)
        {
            Email = dalUser.Email;
            Nickname = dalUser.Nickname;
            Password = dalUser.Password;
            this.dalUser = dalUser;
            this.userId = (int)dalUser.Id;
        }

        //methods

        /// <summary>
        /// Checks wheteher email is a valid email
        /// </summary>
        /// <param name="email">input email</param>
        /// <returns>True for valid email, else false</returns>
        private bool EmailVerify(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks whether password is valid
        /// </summary>
        /// <param name="password">input password</param>
        /// <returns>True for valid password, else false</returns>
        private bool PassVerify(string password)
        {
            bool isValidPassword = password != null && (password.Length >= minPassLength & password.Length <= maxPassLength);
            if (isValidPassword)
            {
                int lowerCase = 0;
                int upperCase = 0;
                int num = 0;

                for (int i = 0; i < password.Length & (lowerCase < minLower | upperCase < minUpper | num < minNum); i++)
                {
                    char current = password[i];
                    if (current >= 'A' && current <= 'Z') upperCase++;

                    else if (current >= 'a' && current <= 'z') lowerCase++;
                    else if (current >= '0' && current <= '9') num++;
                }
                isValidPassword = isValidPassword && (lowerCase >= minLower & upperCase >= minUpper & num >= minNum);
            }
            return isValidPassword;
        }

        /// <summary>
        /// Checks if the password provided matches to the actual user's password
        /// </summary>
        /// <param name="password">password provided by 3rd party</param>
        /// <returns>true - password matches else returns false</returns>
        public bool Login(string password) { return this.Password.Equals(password); }
    }
}
