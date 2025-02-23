﻿using CustomerRecognitionService.Entities.DTOs;

namespace CustomerRecognitionService.Entities
{
    public class Customer
    {
        public  int Id { get; set; }
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
        public  string? Email { get; set; }
        public  string? PhoneNumber { get; set; }
        public  string? Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsMerged { get; set; }

    }
}
