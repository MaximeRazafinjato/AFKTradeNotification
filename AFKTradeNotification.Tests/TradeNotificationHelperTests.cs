using AFKTradeNotification.Helpers;

namespace AFKTradeNotification.Tests
{
    public class TradeNotificationHelperTests
    {
        [Theory]
        [InlineData("@From vervekk: Hi, I would like to buy your Fracturing Orb listed for 5.3 divine in Settlers (stash tab \"￥\"; position: left 5, top 11)", "vervekk")]
        [InlineData("@From vervekk: Hi, I would like to buy your 2 Fracturing Orb listed for 5.3 divine in Settlers (stash tab \"￥\"; position: left 5, top 11)", "vervekk")]
        [InlineData("@From 칼구르_호롱: Hi, I would like to buy your Fracturing Orb listed for 5.3 divine in Settlers (stash tab \"￥\"; position: left 5, top 11)", "칼구르_호롱")]
        public void GetUserName_WhenTradeStringIsValid_ShouldReturnUsername(string tradeString, string expectedUsername)
        {
            // Given
            var expected = expectedUsername;

            // When
            var result = TradeNotificationHelper.GetUserName(tradeString);

            // Then
            result.Should().Be(expectedUsername);
        }

        [Theory]
        [InlineData("@From vervekk: Hi, I would like to buy your Fracturing Orb listed for 5.3 divine in Settlers (stash tab \"￥\"; position: left 5, top 11)", "Fracturing Orb", "5.3 divine")]
        [InlineData("@From vervekk: Hi, I would like to buy your 2 Fracturing Orb listed for 5.3 divine in Settlers (stash tab \"￥\"; position: left 5, top 11)", "2 Fracturing Orb", "5.3 divine")]
        [InlineData("@From 칼구르_호롱: Hi, I would like to buy your Fracturing Orb listed for 5.3 divine in Settlers (stash tab \"￥\"; position: left 5, top 11)", "Fracturing Orb", "5.3 divine")]
        public void GetItemAndPrice_WhenTradeStringIsValid_ShouldReturnItemAndPrice(string tradeString, string expectedItem, string expectedPrice)
        {
            // Given

            // When
            var (resultItem, resultPrice) = TradeNotificationHelper.GetItemAndPrice(tradeString);

            // Then
            resultItem.Should().Be(expectedItem);
            resultPrice.Should().Be(expectedPrice);
        }
    }
}