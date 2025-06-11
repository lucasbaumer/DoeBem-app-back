using Castle.DynamicProxy.Generators;
using doeBem.Application.DTOS;
using doeBem.Application.Services;
using doeBem.Core.Entities;
using doeBem.Core.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Tests.Services
{
    public class DonationServicesTest
    {
        private readonly DonationService _donationService;
        private readonly Mock<IDonationRepository> _donationRepositoryMock;

        public DonationServicesTest()
        {
            _donationRepositoryMock = new Mock<IDonationRepository>();
            _donationService = new DonationService(_donationRepositoryMock.Object );
        }

        [Fact]
        public async Task RegisterDonation_ShouldReturnDonationId_WhenDataIsValid()
        {
            var donationDto = new DonationCreateDTO
            {
                Value = 100,
                Date = "2025-05-05",
                DonorId = Guid.NewGuid(),
                HospitalId = Guid.NewGuid()
            };

            var result = await _donationService.RegisterDonation( donationDto );

            result.Should().NotBeEmpty();
            _donationRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Donation>()), Times.Once);
        }

        [Fact]
        public async Task RegisterDonation_ShouldThrowException_WhenDataIsInvalid()
        {
            var donationDto = new DonationCreateDTO
            {
                Value = 100,
                Date = "invalid-date",
                DonorId = Guid.NewGuid(),
                HospitalId = Guid.NewGuid()
            };

            Func<Task> action = async () => await _donationService.RegisterDonation(donationDto);

            await action.Should().ThrowAsync<Exception>()
                .WithMessage("Data da Doação Inválida");
        }

        [Fact]
        public async Task DeleteDonation_ShouldCallRepository_WhenIdIsValid()
        {
            var donationId = Guid.NewGuid();

            var result = await _donationService.DeleteDonation(donationId);

            result.Should().BeTrue();
            _donationRepositoryMock.Verify(repo => repo.DeleteAsync(donationId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDonaiton_WhenDonationExists()
        {
            var donationId = Guid.NewGuid();
            _donationRepositoryMock.Setup(repo => repo.GetByIdAsync(donationId))
                .ReturnsAsync(new Donation { Id = donationId, Value = 100, Date = DateTime.Now });

            var result = await _donationService.GetByIdAsync(donationId);

            result.Should().NotBeNull();
            result.Id.Should().Be(donationId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowException_WhenDonationDoesNotExist()
        {
            var donationId = Guid.NewGuid();
            _donationRepositoryMock.Setup(repo => repo.GetByIdAsync(donationId))
                .ReturnsAsync((Donation)null);

            Func<Task> action = async () => await _donationService.GetByIdAsync(donationId);

            await action.Should().ThrowAsync<Exception>()
                .WithMessage("Nenhuma doação encontrada!");
        }

        [Fact]
        public async Task UpdateDonation_ShouldReturnTrue_WhenDonationExists()
        {
            var donationId = Guid.NewGuid();
            _donationRepositoryMock.Setup(repo => repo.GetByIdAsync(donationId))
                .ReturnsAsync(new Donation { Id = donationId });

            var updateDto = new DonationUpdateDTO
            {
                Value = 150,
                Date = "2024-06-01",
                DonorId = Guid.NewGuid(),
                HospitalId = Guid.NewGuid()
            };

            var result = await _donationService.UpdateDonation(donationId, updateDto);

            result.Should().BeTrue();
            _donationRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Donation>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDonation_ShouldThrowException_WhenDonationDoesNotExist()
        {
            var donationId = Guid.NewGuid();
            _donationRepositoryMock.Setup(repo => repo.GetByIdAsync(donationId))
                .ReturnsAsync((Donation)null);

            var updateDto = new DonationUpdateDTO
            {
                Value = 150,
                Date = "2024-06-01",
                DonorId = Guid.NewGuid(),
                HospitalId = Guid.NewGuid()
            };

            Func<Task> action = async () => await _donationService.UpdateDonation(donationId, updateDto);

            await action.Should().ThrowAsync<Exception>()
                .WithMessage("Doação não encontrada!");
        }

        [Fact]
        public async Task UpdateDonation_ShouldThrowException_WhenDateIsInvalid()
        {
            var donationId = Guid.NewGuid();
            _donationRepositoryMock.Setup(repo => repo.GetByIdAsync(donationId))
                .ReturnsAsync(new Donation { Id = donationId });

            var updateDto = new DonationUpdateDTO
            {
                Value = 150,
                Date = "invalid-date",
                DonorId = Guid.NewGuid(),
                HospitalId = Guid.NewGuid()
            };

            Func<Task> action = async () => await _donationService.UpdateDonation(donationId, updateDto);

            await action.Should().ThrowAsync<Exception>()
                .WithMessage("Data de nascimento inválida");
        }
    }
}
