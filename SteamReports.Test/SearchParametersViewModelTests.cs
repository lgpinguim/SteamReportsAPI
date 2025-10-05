using SteamReports.Application.ViewModels;

namespace SteamReports.Test
{
    public class SearchParametersViewModelTests
    {
        [Fact]
        public void IsDateFilterValid_BothDatesNull_ReturnsTrue()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                StartDate = null,
                EndDate = null
            };

            // Act
            var result = searchParams.IsDateFilterValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDateFilterValid_BothDatesProvided_ReturnsTrue()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                StartDate = DateTime.UtcNow.AddDays(-7),
                EndDate = DateTime.UtcNow
            };

            // Act
            var result = searchParams.IsDateFilterValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDateFilterValid_OnlyStartDateProvided_ReturnsFalse()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                StartDate = DateTime.UtcNow,
                EndDate = null
            };

            // Act
            var result = searchParams.IsDateFilterValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDateFilterValid_OnlyEndDateProvided_ReturnsFalse()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                StartDate = null,
                EndDate = DateTime.UtcNow
            };

            // Act
            var result = searchParams.IsDateFilterValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDateFilterValid_EndDateBeforeStartDate_ReturnsFalse()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(-1)
            };

            // Act
            var result = searchParams.IsDateFilterValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDateFilterValid_SameDates_ReturnsTrue()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var searchParams = new SearchParametersViewModel
            {
                StartDate = now,
                EndDate = now
            };

            // Act
            var result = searchParams.IsDateFilterValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasDateFilter_BothDatesNull_ReturnsFalse()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                StartDate = null,
                EndDate = null
            };

            // Act
            var result = searchParams.HasDateFilter();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasDateFilter_OnlyStartDateProvided_ReturnsTrue()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                StartDate = DateTime.UtcNow,
                EndDate = null
            };

            // Act
            var result = searchParams.HasDateFilter();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasDateFilter_OnlyEndDateProvided_ReturnsTrue()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                StartDate = null,
                EndDate = DateTime.UtcNow
            };

            // Act
            var result = searchParams.HasDateFilter();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasDateFilter_BothDatesProvided_ReturnsTrue()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                StartDate = DateTime.UtcNow.AddDays(-7),
                EndDate = DateTime.UtcNow
            };

            // Act
            var result = searchParams.HasDateFilter();

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(null, null, false, true)]  // No dates - valid, no filter
        [InlineData("2024-01-01", "2024-01-31", true, true)]  // Valid range
        [InlineData("2024-01-31", "2024-01-01", true, false)]  // Invalid range
        [InlineData("2024-01-01", null, true, false)]  // Only start
        [InlineData(null, "2024-01-31", true, false)]  // Only end
        public void DateFilterValidation_VariousScenarios_WorksCorrectly(
            string? startDateStr, 
            string? endDateStr, 
            bool expectedHasFilter, 
            bool expectedIsValid)
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                StartDate = string.IsNullOrEmpty(startDateStr) ? null : DateTime.Parse(startDateStr),
                EndDate = string.IsNullOrEmpty(endDateStr) ? null : DateTime.Parse(endDateStr)
            };

            // Act & Assert
            Assert.Equal(expectedHasFilter, searchParams.HasDateFilter());
            Assert.Equal(expectedIsValid, searchParams.IsDateFilterValid());
        }
    }
}