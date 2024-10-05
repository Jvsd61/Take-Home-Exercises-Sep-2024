using BookSystem;
using FluentAssertions;
using System;
using Xunit;

namespace TestProject1
{
    public class UnitTest1
    {
        // Test 1: Successfully create a book with no reviews
        [Fact]
        public void CreateBook_WithNoReviews()
        {
            // Arrange
            var author = new Author("Ralph", "Waldo Ellison", "http://ralphwaldoellison.com", "New York", "USA");
            var book = new Book("978-0-7653-8669-4", "Invisible Man", 1952, author, GenreType.Fiction);

            // Act & Assert
            book.Should().NotBeNull();
            book.Reviews.Should().BeEmpty();
        }

        // Test 2: Successfully create a book with reviews
        [Fact]
        public void CreateBook_WithReviews()
        {
            // Arrange
            var author = new Author("Ralph", "Waldo Ellison", "http://ralphwaldoellison.com", "New York", "USA");
            var book = new Book("978-0-1234-5678-9", "Another Great Book", 2021, author, GenreType.Fantasy);
            book.AddReview(new Review("978-0-1234-5678-9", new Reviewer("Alice", "Wonder", "alice@wonder.com", null), RatingType.Buy, "Loved it!"));
            book.AddReview(new Review("978-0-1234-5678-9", new Reviewer("Bob", "Builder", "bob@builder.com", null), RatingType.Recommend, "Great for kids."));
            book.AddReview(new Review("978-0-1234-5678-9", new Reviewer("Charlie", "Brown", "charlie@brown.com", null), RatingType.Avoid, "Not my cup of tea."));

            // Act & Assert
            book.Reviews.Should().HaveCount(3);
        }

        // Test 3: Fail to create a book with missing required fields
        [Fact]
        public void Fail_WhenMissingRequiredFields()
        {
            // Arrange
            Action act = () => new Book(null, null, 0, null, GenreType.Fiction);

            // Act & Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*ISBN cannot be null*")
                .And.ParamName.Should().Be("isbn");

            // Also check for Title
            act = () => new Book("978-0-7653-8669-4", null, 2020, null, GenreType.Fiction);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*Title cannot be null*")
                .And.ParamName.Should().Be("title");

            // Check for Author
            act = () => new Book("978-0-7653-8669-4", "Great Book", 2020, null, GenreType.Fiction);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*Author is required*")
                .And.ParamName.Should().Be("author");
        }

        // Test 4: Successfully change the publication year
        [Fact]
        public void ChangePublicationYear_Success()
        {
            // Arrange
            var author = new Author("Ralph", "Waldo Ellison", "http://ralphwaldoellison.com", "New York", "USA");
            var book = new Book("978-0-7653-8669-4", "Invisible Man", 1952, author, GenreType.Fiction);
            int newYear = 2021;

        }

        // Test 5: Fail if the publication year is not a positive number
        [Fact]
        public void Fail_WhenPublicationYearIsNotPositive()
        {
            // Arrange
            var author = new Author("Ralph", "Waldo Ellison", "http://ralphwaldoellison.com", "New York", "USA");
            Action act = () => new Book("978-0-7653-8669-4", "Invisible Man", -1952, author, GenreType.Fiction);

            // Act & Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Publish year must be a positive, non-zero whole number.")
                .And.ParamName.Should().Be("publishYear");
        }

        // Test 6: Add a review successfully
        [Fact]
        public void AddReview_Success()
        {
            // Arrange
            var author = new Author("Ralph", "Waldo Ellison", "http://ralphwaldoellison.com", "New York", "USA");
            var book = new Book("978-0-7653-8669-4", "Invisible Man", 1952, author, GenreType.Fiction);
            var reviewer = new Reviewer("Alice", "Wonder", "alice@wonder.com", null);
            var review = new Review("978-0-7653-8669-4", reviewer, RatingType.Buy, "Loved it!");

            // Act
            book.AddReview(review);

            // Assert
            book.Reviews.Should().ContainSingle().Which.Should().Be(review);
        }

        // Test 7: Fail to add review with missing ISBN
        [Fact]
        public void Fail_WhenAddingReviewWithNullISBN()
        {
            // Arrange
            var author = new Author("Ralph", "Waldo Ellison", "http://ralphwaldoellison.com", "New York", "USA");
            var book = new Book("978-0-7653-8669-4", "Invisible Man", 1952, author, GenreType.Fiction);
            var reviewer = new Reviewer("Alice", "Wonder", "alice@wonder.com", null);
            var review = new Review(null, reviewer, RatingType.Buy, "Loved it!");

            // Act
            Action act = () => book.AddReview(review);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("ISBN cannot be null, empty, or whitespace.")
                .And.ParamName.Should().Be("isbn");
        }

        // Test 8: Fail to add review when the review's ISBN does not match
        [Fact]
        public void Fail_WhenAddingReviewWithMismatchedISBN()
        {
            // Arrange
            var author = new Author("Ralph", "Waldo Ellison", "http://ralphwaldoellison.com", "New York", "USA");
            var book = new Book("978-0-7653-8669-4", "Invisible Man", 1952, author, GenreType.Fiction);
            var reviewer = new Reviewer("Alice", "Wonder", "alice@wonder.com", null);
            var review = new Review("978-1-2345-6789-0", reviewer, RatingType.Buy, "Loved it!");

            // Act
            Action act = () => book.AddReview(review);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Review ISBN 978-1-2345-6789-0 does not match Book ISBN 978-0-7653-8669-4.")
                .And.ParamName.Should().Be("review.ISBN");
        }

        // Test 9: Fail to add review when the reviewer already has a review
        [Fact]
        public void Fail_WhenReviewerAlreadyHasReview()
        {
            // Arrange
            var author = new Author("Ralph", "Waldo Ellison", "http://ralphwaldoellison.com", "New York", "USA");
            var book = new Book("978-0-7653-8669-4", "Invisible Man", 1952, author, GenreType.Fiction);
            var reviewer = new Reviewer("Alice", "Wonder", "alice@wonder.com", null);
            var review1 = new Review("978-0-7653-8669-4", reviewer, RatingType.Buy, "Loved it!");
            var review2 = new Review("978-0-7653-8669-4", reviewer, RatingType.Recommend, "Great read.");

            // Act
            book.AddReview(review1);
            Action act = () => book.AddReview(review2);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Reviewer Alice Wonder has already submitted a review.")
                .And.ParamName.Should().Be("review");
        }
    }
}
