using EmailWebApi.Controllers;
using EmailWebApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmailApp.Test
{
    public class FileControllerTest
    {
        private readonly FileController _controller;
        private readonly Mock<IAzureBlobService> _service;
        public FileControllerTest()
        {
            _service = new Mock<IAzureBlobService>();
            _controller = new FileController(_service.Object);
        }

        [Fact]
        public async Task Upload_ValidData_ReturnsOkResult()
        {
            //arrange
            var file = new Mock<IFormFile>();
            file.Setup(f => f.FileName).Returns("validfile.docx");
            var email = "valid@email.com";

            //act
            var result = await _controller.Upload(file.Object, email);

            //assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Upload_InvalidEmail_ReturnsBadRequest()
        {
            //arrange
            var file = new Mock<IFormFile>();
            file.Setup(f => f.FileName).Returns("validfile.docx");
            var email = "invalid-email";

            //act
            var result = await _controller.Upload(file.Object, email);

            //assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Upload_InvalidFileExtension_ReturnsBadRequest()
        {
            //arrange
            var file = new Mock<IFormFile>();
            file.Setup(f => f.FileName).Returns("invalidfile.pdf");
            var email = "valid@email.com";

            //act
            var result = await _controller.Upload(file.Object, email);

            //assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}