﻿namespace QuantSignalServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public bool IsVIP { get; set; }
    }
}