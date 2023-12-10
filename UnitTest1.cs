using Xunit;
using FluentAssertions;
using CalculatorTRPO;
namespace TestProject1
{
    public class UnitTest1
    {
        public Parser parser;
        public UnitTest1()
        {
            this.parser = new Parser();
        }

        [Theory]
        [InlineData("1+5", "6")]
        [InlineData("1+5*(6+1)", "36")]
        [InlineData("5*7/2", "17,5")]
        [InlineData("5*7+(5)", "40")]
        [InlineData("5*7+", "35")]

        public void Parser_ShouldReturnResult(string left, string right)
        {
            var result = parser.StartParsing(left);
            result.Should().Be(right);
        }
        [Fact]
        public void Parser_ShouldReturnCurrentNumber_IfUserTransferOneNumber()
        {
            var result = parser.StartParsing("1");
            result.Should().Be("1");

        }
        [Fact]
        public void Parser_ShouldReturnZero_IfUserTransferEmptyString()
        {
            parser.Invoking(x => x.StartParsing("")).
                Should().Throw<System.ArgumentNullException>();
        }
        [Theory]
        [InlineData("2+(", "Error")]
        [InlineData("2+)", "Error")]
        [InlineData("2+()", "Error")]

        public void Parser_ShouldReturnErrorString_IfUserTryingCalculateNumberAndBracket(string left, string right)
        {
            var result = parser.StartParsing(left);
            result.Should().Be(right);
        }

        [Theory]
        [InlineData("2()", "Error")]
        [InlineData("()2", "Error")]

        public void Parser_ShouldReturnErrorString_IfUserTryingCalculateNumberNearBracket(string left, string right)
        {
            var result = parser.StartParsing(left);
            result.Should().Be(right);
        }
        [Theory]
        [InlineData("2+)3+5(", "Error")]
        [InlineData("2+)(", "Error")]
        public void Parser_ShouldReturnErrorString_IfUserReverseBracketsAndTryingCalculate(string left, string right)
        {
            var result = parser.StartParsing(left);
            result.Should().Be(right);
        }
        [Theory]
        [InlineData("2+(0,5)", "2,5")]
        [InlineData("2*2,6", "5,2")]
        public void Parser_ShouldReturnDoubleNumber_IfUserWritingDot(string left, string right)
        {
            var result = parser.StartParsing(left);
            result.Should().Be(right);

        }
    }
}