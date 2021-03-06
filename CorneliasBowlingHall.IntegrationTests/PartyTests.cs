﻿using System;
using System.Collections.Generic;
using System.Text;
using CorneliasBowlinghall.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CorneliasBowlinghall.EFDatabase;
using CorneliasBowlinghall.System;
using CorneliasBowlinghall.Interfaces;
using System.Linq;

namespace CorneliasBowlingHall.IntegrationTests
{
    public class PartyTests
    {
        private ApplicationDbContext _context;
        private IBowlingRepository _bowlingRepository;

        public PartyTests()
        {
            var randomDatabaseName = Guid.NewGuid();

            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionBuilder.UseSqlServer($"Server=(localdb)\\MSSQLLocalDB;Database={randomDatabaseName};Trusted_Connection=True;");
            _context = new ApplicationDbContext(optionBuilder.Options);

            _bowlingRepository = new BowlingRepository(_context);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            CompetitionFaker.GenerateFakeCompetitions(_bowlingRepository);
        }

        [Fact]
        public void Can_Create_Party()
        {
            var system = new BowlingSystem(_bowlingRepository);

            system.CreateParty("Bertil Sandrasson", "451007-7835");

            var foundParty = _context.Parties.Where(p => p.Name.Contains("Bertil Sandrasson")).FirstOrDefault();

            Assert.Equal("Bertil Sandrasson", foundParty.Name);
        }

        [Fact]
        public void Can_Search_Party()
        {
            var system = new BowlingSystem(_bowlingRepository);

            var party = system.FindParty("winner2017").FirstOrDefault();

            Assert.Contains("winner2017", party.Name);
        }
    }
}
