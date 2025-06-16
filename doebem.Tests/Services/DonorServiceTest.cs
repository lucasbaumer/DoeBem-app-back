using doeBem.Application.DTOS;
using doeBem.Application.Services;
using doeBem.Core.Entities;
using doeBem.Core.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace doeBem.Tests.Services
{
    public class DonorServiceTest
    {
        private Mock<IDonorRepository> _donorRepoMock;
        private Mock<UserManager<IdentityUser>> _userManagerMock;

        public DonorServiceTest()
        {
            _donorRepoMock = new Mock<IDonorRepository>();

            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        private DonorService CreateService()
        {
            return new DonorService(_donorRepoMock.Object, _userManagerMock.Object);
        }

        [Fact]
        public async Task GetByIdWithDonationAsync_ShouldReturnDonorWithDonations_WhenDonorExists()
        {
            var id = Guid.NewGuid();
            var donor = new Donor
            {
                Id = id,
                Name = "Ana",
                Cpf = "11122233344",
                Email = "ana@email.com",
                Phone = "111111111",
                DateOfBirth = DateTime.Parse("1970-01-01"),
                Donations = new List<Donation>
                {
                    new Donation
                    {
                        Id = Guid.NewGuid(),
                        Value = 150,
                        Date = DateTime.Today,
                        HospitalId = Guid.NewGuid(),
                        Hospital = new Hospital { Name = "Hospital X" }
                    }
                }
            };

            _donorRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(donor);

            var service = CreateService();
            var result = await service.GetByIdWithDonationAsync(id);

            result.Should().NotBeNull();
            result.Name.Should().Be("Ana");
            result.Donations.Should().HaveCount(1);
            result.Donations.First().HospitalName.Should().Be("Hospital X");
        }

        [Fact]
        public async Task GetByIdWithDonationAsync_ShouldReturnNull_WhenDonorDoesNotExist()
        {
            _donorRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Donor)null);

            var service = CreateService();
            var result = await service.GetByIdWithDonationAsync(Guid.NewGuid());

            result.Should().BeNull();
        }

        [Fact]
        public async Task RegisterDonor_ShouldThrowException_WhenEmailAlreadyExists()
        {
            var dto = new DonorCreateDTO
            {
                Email = "email@teste.com",
                Password = "Senha@123",
                Cpf = "12345678901",
                DateOfBirth = "1990-01-01",
                Name = "Teste",
                Phone = "123456789"
            };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email))
                .ReturnsAsync(new IdentityUser());

            var service = CreateService();

            Func<Task> act = async () => await service.RegisterDonor(dto);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Email já cadastrado");
        }

        [Fact]
        public async Task RegisterDonor_ShouldThrowException_WhenCpfAlreadyExists()
        {
            var dto = new DonorCreateDTO
            {
                Email = "novo@email.com",
                Password = "Senha@123",
                Cpf = "12345678901",
                DateOfBirth = "1990-01-01",
                Name = "Teste",
                Phone = "123456789"
            };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser)null);

            _donorRepoMock.Setup(r => r.GetByCpfAsync(dto.Cpf))
                .ReturnsAsync(new Donor());

            var service = CreateService();

            Func<Task> act = async () => await service.RegisterDonor(dto);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Cpf já cadastrado!");
        }

        [Fact]
        public async Task RegisterDonor_ShouldThrowException_WhenDateOfBirthIsInvalid()
        {
            var dto = new DonorCreateDTO
            {
                Email = "novo@email.com",
                Password = "Senha@123",
                Cpf = "12345678901",
                DateOfBirth = "data-invalida",
                Name = "Teste",
                Phone = "123456789"
            };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser)null);

            _donorRepoMock.Setup(r => r.GetByCpfAsync(dto.Cpf))
                .ReturnsAsync((Donor)null);

            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(u => u.DeleteAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var service = CreateService();

            Func<Task> act = async () => await service.RegisterDonor(dto);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Erro ao cadastrar doador!!Data de nascimento inválida, formato esperado yyyy-MM-dd");
        }

        [Fact]
        public async Task RegisterDonor_ShouldReturnId_WhenSuccessful()
        {
            var dto = new DonorCreateDTO
            {
                Email = "novo@email.com",
                Password = "Senha@123",
                Cpf = "12345678901",
                DateOfBirth = "1990-01-01",
                Name = "Teste",
                Phone = "123456789"
            };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser)null);

            _donorRepoMock.Setup(r => r.GetByCpfAsync(dto.Cpf))
                .ReturnsAsync((Donor)null);

            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<IdentityUser>(), "donor"))
                .ReturnsAsync(IdentityResult.Success);

            _donorRepoMock.Setup(r => r.AddAsync(It.IsAny<Donor>()))
                .Returns(Task.CompletedTask);

            var service = CreateService();

            var result = await service.RegisterDonor(dto);

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task RegisterDonor_ShouldThrowException_WhenUserCreationFails()
        {
            var dto = new DonorCreateDTO
            {
                Email = "novo@email.com",
                Password = "Senha@123",
                Cpf = "12345678901",
                DateOfBirth = "1990-01-01",
                Name = "Teste",
                Phone = "123456789"
            };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser)null);

            _donorRepoMock.Setup(r => r.GetByCpfAsync(dto.Cpf))
                .ReturnsAsync((Donor)null);

            var failedResult = IdentityResult.Failed(new IdentityError { Description = "Erro de criação" });
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), dto.Password))
                .ReturnsAsync(failedResult);

            var service = CreateService();

            Func<Task> act = async () => await service.RegisterDonor(dto);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Erro ao criar usuário: Erro de criação");
        }
    }
}
