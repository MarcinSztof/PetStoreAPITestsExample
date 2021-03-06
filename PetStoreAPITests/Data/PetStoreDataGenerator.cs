﻿using APIClient.Models;
using System;
using System.Collections.Generic;

namespace PetStoreAPITests.Data
{
    public static class PetStoreDataGenerator
    {
        public static Pet GetBasicPet() => new Pet() 
            {
                Id = DateGenerationUtils.GetRandomInt32(100000, 999999),
                Name = DateGenerationUtils.GetRandomAlphabeticString(7),
                Category = GetCategory(1),
                PhotoUrls = new List<string>(),
                Tags = new List<Tag>() { new Tag() { Id = 1, Name = "Exotic" } },
                Status = PetStatus.Available
            };

        public static Pet GetBasicPet(PetStatus status)
        {
            var pet = GetBasicPet();
            pet.Status = status;
            return pet;
        }

        public static Category GetCategory(int id) => id switch
            {
                1 => new Category() { Id = 1, Name = "Birds"},
                2 => new Category() { Id = 2, Name = "Fishes"},
                _ => new Category() { Id = 3, Name = "Rodents"}
            };

        public static Order GetBasicOrder() => new Order()
        {
            Id = DateGenerationUtils.GetRandomInt32(100000, 999999),
            PetId = DateGenerationUtils.GetRandomInt32(100000, 999999),
            Quantity = 1,
            ShipDate = DateTime.Now.ToUniversalTime().Date.AddDays(7),
            Status = OrderStatus.Placed,
            Complete = false
        };
    }
}
