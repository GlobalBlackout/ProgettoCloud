﻿using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using CloudSite.Models.User;

namespace CloudSite.Models.ConvalidationUserAuth
{
    public class ConvalidationUser
    {
        private string _userName;
        private string _userEmail;
        private string _userPassword;

        public ConvalidationUser(UserModel user)
        {
            _userName = user.UserName;
            _userEmail = user.UserEmail;
            _userPassword = user.UserPassword;
        }

        public bool isTheUserHaveValidParametres()
        {
            if (IsTheUserHaveValidUsername() &&
                IsTheUserHaveValidEmail() &&
                IsTheUserHaveValidPassword())
                return true;
            else
                return false;
        }

        public bool IsTheUserHaveValidUsername()
        {
            string regUsername = @"^(?=[A-Za-z0-9])(?!.*[._()\[\]-]{2})[A-Za-z0-9._()\[\]-]{3,20}$";
            return Regex.IsMatch(_userName, regUsername);
        }

        public bool IsTheUserHaveValidPassword()
        {
            string regPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";
            return Regex.IsMatch(_userPassword, regPassword);
        }

        public bool IsTheUserHaveValidEmail()
        {
            EmailAddressAttribute emailChecker = new EmailAddressAttribute();
            return emailChecker.IsValid(_userEmail);
        }

        public string CryptUserPassword(string userPassword)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(userPassword);
            data = new SHA256Managed().ComputeHash(data);
            string hash = System.Text.Encoding.ASCII.GetString(data);
            
            return hash;
        }

        public bool CheckPasswordIsTheSame(string passwordToCheck, string passwordDB)
        {
            return CryptUserPassword(passwordToCheck) == passwordDB;
        }
    }
}