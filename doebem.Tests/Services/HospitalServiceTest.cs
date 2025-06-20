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
    public class HospitalServiceTest
    {
        private readonly Mock<IHospitalRepository> _hospitalRepoMock;
        private readonly Mock<IDonationRepository> _donationRepoMock;
        private readonly HospitalService _hospitalService;

        public HospitalServiceTest()
        {
            _hospitalRepoMock = new Mock<IHospitalRepository>();
            _donationRepoMock = new Mock<IDonationRepository>();
            _hospitalService = new HospitalService(_hospitalRepoMock.Object, _donationRepoMock.Object);
        }

        [Fact]
        public async Task DeleteHospital_ShouldThrowException_WhenHospitalNotFound()
        {
            _hospitalRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Hospital)null);

            Func<Task> act = async () => await _hospitalService.DeleteHospital(Guid.NewGuid());

            await act.Should().ThrowAsync<Exception>().WithMessage("Hospital não foi encontrado!");
        }

        [Fact]
        public async Task DeleteHospital_ShouldSetHospitalIdToNullOnDonations_AndDeleteHospital()
        {
            var hospitalId = Guid.NewGuid();
            var hospital = new Hospital { Id = hospitalId };
            var donations = new List<Donation>
        {
            new Donation { Id = Guid.NewGuid(), HospitalId = hospitalId },
            new Donation { Id = Guid.NewGuid(), HospitalId = hospitalId }
        };

            _hospitalRepoMock.Setup(r => r.GetByIdAsync(hospitalId)).ReturnsAsync(hospital);
            _donationRepoMock.Setup(r => r.GetByHospitalIdAsync(hospitalId)).ReturnsAsync(donations);
            _donationRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Donation>())).Returns(Task.CompletedTask);
            _hospitalRepoMock.Setup(r => r.DeleteAsync(hospitalId)).Returns(Task.CompletedTask);

            var result = await _hospitalService.DeleteHospital(hospitalId);

            result.Should().BeTrue();
            foreach (var donation in donations)
            {
                donation.HospitalId.Should().BeNull();
            }

            _donationRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Donation>()), Times.Exactly(donations.Count));
            _hospitalRepoMock.Verify(r => r.DeleteAsync(hospitalId), Times.Once);
        }

        [Fact]
        public async Task RegisterHospital_ShouldThrowException_WhenCNESInvalid()
        {
            var invalidDto = new HospitalCreateDto { CNES = 123456 };

            Func<Task> act = async () => await _hospitalService.RegisterHospital(invalidDto);

            await act.Should().ThrowAsync<Exception>().WithMessage("CNES está inválido. Deve conter exatamente 7 dígitos");
        }

        [Fact]
        public async Task RegisterHospital_ShouldThrowException_WhenCNESAlreadyExists()
        {
            var dto = new HospitalCreateDto { CNES = 1234567 };
            var existingHospital = new Hospital { CNES = 1234567 };

            _hospitalRepoMock.Setup(r => r.GetByCnesAsync(dto.CNES)).ReturnsAsync(existingHospital);

            Func<Task> act = async () => await _hospitalService.RegisterHospital(dto);

            await act.Should().ThrowAsync<Exception>().WithMessage("CNES já cadastrado!");
        }

        [Fact]
        public async Task RegisterHospital_ShouldReturnNewHospitalId_WhenValid()
        {
            var dto = new HospitalCreateDto
            {
                Name = "Hospital Teste",
                CNES = 1234567,
                State = "SP",
                City = "São Paulo",
                Phone = "123456789",
                Description = "Teste"
            };

            _hospitalRepoMock.Setup(r => r.GetByCnesAsync(dto.CNES)).ReturnsAsync((Hospital)null);
            _hospitalRepoMock.Setup(r => r.AddAsync(It.IsAny<Hospital>())).Returns(Task.CompletedTask);

            var result = await _hospitalService.RegisterHospital(dto);

            result.Should().NotBeEmpty();
            _hospitalRepoMock.Verify(r => r.AddAsync(It.Is<Hospital>(h =>
                h.Name == dto.Name &&
                h.CNES == dto.CNES &&
                h.State == dto.State &&
                h.City == dto.City
            )), Times.Once);
        }
    }
}
