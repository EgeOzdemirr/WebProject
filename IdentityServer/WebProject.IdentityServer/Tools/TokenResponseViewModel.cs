﻿using Microsoft.VisualBasic;
using System;

namespace WebProject.IdentityServer.Tools
{
	public class TokenResponseViewModel
	{
		public TokenResponseViewModel(string token, DateTime expireDate)
		{
			Token = token;
			ExpireDate = expireDate;
		}
		public string Token { get; set; }
		public DateTime ExpireDate { get; set; }
    }
}
