using BLL;
using Carts.Controllers;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{

    public class CartsControllerTests
    {
        [Fact]
        public void Get_CartExists_ReturnsOk()
        {
            // Arrange
            var cartServiceMock = new Mock<ICartService>();
            var cartController = new CartsController(cartServiceMock.Object);

            cartServiceMock.Setup(service => service.GetCart(It.IsAny<string>()))
                .Returns(new Cart { Id = "cartId" });

            // Act
            var result = cartController.Get("cartId");

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Get_CartDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var cartServiceMock = new Mock<ICartService>();
            var cartController = new CartsController(cartServiceMock.Object);

            cartServiceMock.Setup(service => service.GetCart(It.IsAny<string>()))
                .Returns((Cart)null);

            // Act
            var result = cartController.Get("nonExistentCartId");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void AddItem_ValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var cartServiceMock = new Mock<ICartService>();
            var cartController = new CartsController(cartServiceMock.Object);
            var item = new Item { CartId = "cartId" };

            cartServiceMock.Setup(service => service.Insert(It.IsAny<string>(), It.IsAny<Item>()))
                .Returns(1);

            cartServiceMock.Setup(service => service.GetCart(It.IsAny<string>()))
                .Returns(new Cart { Id = "cartId" });

            // Act
            var result = cartController.AddItem("cartId", item);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void AddItem_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var cartServiceMock = new Mock<ICartService>();
            var cartController = new CartsController(cartServiceMock.Object);
            var item = new Item();

            cartServiceMock.Setup(service => service.Insert(It.IsAny<string>(), It.IsAny<Item>()))
                .Returns(0);

            // Act
            var result = cartController.AddItem("cartId", item);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_ItemExists_ReturnsNoContent()
        {
            // Arrange
            var cartServiceMock = new Mock<ICartService>();
            var cartController = new CartsController(cartServiceMock.Object);

            cartServiceMock.Setup(service => service.Delete(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(1);

            // Act
            var result = cartController.Delete("cartId", 1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_ItemDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var cartServiceMock = new Mock<ICartService>();
            var cartController = new CartsController(cartServiceMock.Object);

            cartServiceMock.Setup(service => service.Delete(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(0);

            // Act
            var result = cartController.Delete("cartId", 1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        //[Fact]
        //public void Get_ReturnsCarts()
        //{
        //    // Arrange
        //    var cartServiceMock = new Mock<ICartService>();
        //    cartServiceMock.Setup(service => service.GetCart("testId") .Returns(new Cart() { 
        //            Id = Guid.NewGuid().ToString(), 
        //            Items = new List<Item>()
        //        });
        //    var controller = new CartsController(cartServiceMock.Object);

        //    // Act
        //    var result = controller.Get();

        //    // Assert
        //    var okResult = Assert.IsAssignableFrom<IEnumerable<Item>>(result);
        //    var items = Assert.IsAssignableFrom<IEnumerable<Item>>(okResult);
        //    Assert.Empty(items);
        //}

        //[Fact]
        //public void Get_ReturnsCart_WhenValidIdProvided()
        //{
        //    // Arrange
        //    var cartServiceMock = new Mock<ICartService>();
        //    cartServiceMock.Setup(service => service.GetCart(It.IsAny<string>())).Returns(new Item());
        //    var controller = new CartsController(cartServiceMock.Object);

        //    // Act
        //    var result = controller.Get("1"); // Provide a valid ID

        //    // Assert
        //    Assert.IsType<ActionResult<Item>>(result);
        //}

        //[Fact]
        //public void Get_ReturnsNotFound_WhenInvalidIdProvided()
        //{
        //    // Arrange
        //    var cartServiceMock = new Mock<ICartService>();
        //    cartServiceMock.Setup(service => service.GetCart(It.IsAny<string>())).Returns(null as Item);
        //    var controller = new CartsController(cartServiceMock.Object);

        //    // Act
        //    var result = controller.Get("999"); // Provide an invalid ID

        //    // Assert
        //    Assert.IsType<NotFoundResult>(result.Result);
        //}

        //[Fact]
        //public void Get_ReturnsOkResult_WhenItemFound()
        //{
        //    // Arrange
        //    var cartServiceMock = new Mock<ICartService>();
        //    var controller = new CartsController(cartServiceMock.Object);
        //    var itemId = "1"; // Sample item ID for testing
        //    var itemToReturn = new Item { Id = itemId, Name = "Test Item" };

        //    cartServiceMock.Setup(service => service.GetCart(itemId)).Returns(itemToReturn);

        //    // Act
        //    var result = controller.Get(itemId);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result.Result);
        //    var returnedItem = Assert.IsType<Item>(okResult.Value);
        //    Assert.Equal(itemToReturn, returnedItem); // Compare the returned item with the expected item
        //}
    }

}