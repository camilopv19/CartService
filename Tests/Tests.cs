using BLL;
using Carts.Controllers;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests
{

    public class CartsControllerTests
    {
        private readonly Mock<ICartService> _cartServiceMock;
        public CartsControllerTests()
        {
            _cartServiceMock = new Mock<ICartService>();
        }
        [Fact]
        public void Get_CartExists_ReturnsOk()
        {
            // Arrange
            var cartController = new CartsController(_cartServiceMock.Object);
            var expectedCart = new Cart { Id = "cartId" };

            _cartServiceMock.Setup(service => service.GetCart(It.IsAny<string>()))
                .Returns(new Cart { Id = "cartId" });

            // Act
            var result = cartController.Get("cartId");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedItem = Assert.IsType<Cart>(okResult.Value);

            // Compare the returned with the expected values/properties
            Assert.Equal(expectedCart.Items, returnedItem.Items); 
            Assert.Equal(expectedCart.Id, returnedItem.Id); 
        }

        [Fact]
        public void Get_CartDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            string anyCartId= Guid.NewGuid().ToString();
            _cartServiceMock.Setup(service => service.GetCart(anyCartId)).Returns((Cart)null);

            // Act
            var cartController = new CartsController(_cartServiceMock.Object);
            var result = cartController.Get(anyCartId);
            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void AddItem_ValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            string anyCartId = Guid.NewGuid().ToString();
            var cartServiceMock = new Mock<ICartService>();
            var item = new Item { CartId = anyCartId };

            cartServiceMock.Setup(service => service.Insert(It.IsAny<string>(), It.IsAny<Item>()))
                .Returns(1);
            cartServiceMock.Setup(service => service.GetCart(It.IsAny<string>()))
                .Returns(new Cart { Id = anyCartId });

            // Act
            var cartController = new CartsController(cartServiceMock.Object);
            var result = cartController.AddItem(anyCartId, item);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
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
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public void Delete_ItemExists_ReturnsOk()
        {
            // Arrange
            var cartServiceMock = new Mock<ICartService>();
            var cartController = new CartsController(cartServiceMock.Object);

            cartServiceMock.Setup(service => service.Delete(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(1);

            // Act
            var result = cartController.Delete("cartId", 1);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
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
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetItemsFromCartV2_ReturnsItems()
        {
            // Arrange
            var cartId = "sampleCartId";
            var expectedItems = new List<Item> { /* create some test items */ };

            _cartServiceMock.Setup(service => service.GetItemsFromCart(cartId)).Returns(expectedItems);

            var controller = new Carts2Controller(_cartServiceMock.Object);

            // Act
            var result = controller.Get(cartId);

            // Assert
            Assert.Equal(expectedItems, result);
        }

        [Fact]
        public void GetItemsFromCartV2_EmptyCart_ReturnsEmptyList()
        {
            // Arrange
            var cartId = "emptyCartId";
            var emptyCart = new Cart { Id = cartId, Items = new List<Item>() };

            _cartServiceMock.Setup(service => service.GetItemsFromCart(cartId)).Returns(emptyCart.Items);

            var controller = new Carts2Controller(_cartServiceMock.Object);

            // Act
            var result = controller.Get(cartId);

            // Assert
            Assert.Empty(result);
        }
    }

}